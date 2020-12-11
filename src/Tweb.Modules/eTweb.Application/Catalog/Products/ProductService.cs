using eTweb.Application.Dtos;
using eTweb.Data.EF;
using eTweb.Data.Entities;
using eTweb.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Microsoft.EntityFrameworkCore;
using eTweb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using eTweb.Application.Common;
using System.Net.Http.Headers;
using eTweb.ViewModels.Catalog.ProductImages;

namespace eTweb.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly eTwebDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(
            eTwebDbContext context,
            IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                IsFeatured = request.IsFeatured,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
            };

            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                     new ProductImage()
                     {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                     }
                };
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);

            if (product == null || productTranslations == null) throw new eTwebException($"Cannot find a product with id: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoTitle = request.SeoTitle;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) throw new eTwebException($"Cannot find a product with id: {productId}");

            product.Price = newPrice;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) throw new eTwebException($"Cannot find a product with id: {productId}");

            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eTwebException($"Cannit find a product with id: {productId}");

            // Delete images
            var images = _context.ProductImages.Where(pi => pi.ProductId == productId);
            if (images.Count() > 0)
            {
                foreach (var image in images)
                {
                    await _storageService.DeleteFileAsync(image.ImagePath);
                }
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(pt => pt.Id == productId && pt.LanguageId == languageId);

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                DateCreated = product.DateCreated,
                Name = productTranslation != null ? productTranslation.Name : null,
                Description = productTranslation != null ? productTranslation.Description : null,
                Details = productTranslation != null ? productTranslation.Details : null,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                LanguageId = productTranslation != null ? productTranslation.LanguageId : null,
            };

            return productViewModel;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // 1. Select join. Note: using System.Linq
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            // 2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }

            // 3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new ProductViewModel()
                    {
                        Id = x.p.Id,
                        Price = x.p.Price,
                        OriginalPrice = x.p.OriginalPrice,
                        Stock = x.p.Stock,
                        ViewCount = x.p.ViewCount,
                        DateCreated = x.p.DateCreated,
                        Name = x.pt.Name,
                        Description = x.pt.Description,
                        Details = x.pt.Details,
                        SeoAlias = x.pt.SeoAlias,
                        SeoDescription = x.pt.SeoDescription,
                        SeoTitle = x.pt.SeoTitle
                    })
                    .ToListAsync();

            // 4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return pagedResult;
        }

        // private methods
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Add(productImage);

            await _context.SaveChangesAsync();

            return productImage.Id;
        }

        public async Task<int> UpdateImage(int productId, int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
                throw new eTwebException($"Cannot find a ProductImage with id: {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Update(productImage);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveImage(int productId, int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
                throw new eTwebException($"Cannot find a Product Image with id: {imageId}");

            _context.ProductImages.Remove(productImage);

            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
                throw new eTwebException($"Cannot find a product image with id: {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Id = productImage.Id,
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                FileSize = productImage.FileSize,
                ImagePath = productImage.ImagePath,
                IsDefault = productImage.IsDefault,
                ProductId = productImage.ProductId,
                SortOrder = productImage.SortOrder
            };

            return viewModel;
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .Select(x => new ProductImageViewModel()
                {
                    Id = x.Id,
                    Caption = x.Caption,
                    DateCreated = x.DateCreated,
                    FileSize = x.FileSize,
                    ImagePath = x.ImagePath,
                    IsDefault = x.IsDefault,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder
                })
                .ToListAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            // 1. Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            // 2. Filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // 3. Paging
            var totalRecord = await query.CountAsync();
            var data = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    SeoDescription = x.pt.SeoDescription,
                    Details = x.pt.Details,
                    Name = x.pt.Name,
                    LanguageId = x.pt.LanguageId,
                    SeoAlias = x.pt.SeoAlias,
                    SeoTitle = x.pt.SeoTitle
                })
                .ToListAsync();

            // 4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                Items = data,
                TotalRecord = totalRecord
            };

            return pagedResult;
        }
    }
}