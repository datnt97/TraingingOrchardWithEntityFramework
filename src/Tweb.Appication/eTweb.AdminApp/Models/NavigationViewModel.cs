using eTweb.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTweb.AdminApp.Models
{
    public class NavigationViewModel
    {
        public string CurrentLanguageId { get; set; }
        public List<LanguageViewModel> Languages { get; set; }
    }
}