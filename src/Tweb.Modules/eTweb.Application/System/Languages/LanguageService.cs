using eTweb.Data.EF;
using eTweb.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eTweb.ViewModels.Common;

namespace eTweb.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly eTwebDbContext _context;

        public LanguageService(eTwebDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            var languages = await _context.Languages
                .Select(x => new LanguageViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            if (languages.Count() == 0)
                return new ApiErrorResult<List<LanguageViewModel>>("Không tìm thấy ngôn ngữ.");
            return new ApiSuccessResult<List<LanguageViewModel>>(languages);
        }
    }
}