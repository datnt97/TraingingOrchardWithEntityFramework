using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eTweb.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id { set; get; }

        [Display(Name = "Giá bán")]
        public decimal Price { set; get; }

        [Display(Name = "Giá nhập")]
        public decimal OriginalPrice { set; get; }

        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }

        [Display(Name = "View count")]
        public int ViewCount { set; get; }

        [Display(Name = "Ngày tọa")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string Description { set; get; }

        [Display(Name = "Chi tiết")]
        public string Details { set; get; }

        [Display(Name = "Seo alias")]
        public string SeoAlias { get; set; }

        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        [Display(Name = "Mã ngôn ngữ")]
        public string LanguageId { set; get; }
    }
}