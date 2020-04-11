using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.ViewModels
{
   public class PagedViewModel
    {
        public IEnumerable<QuestionShortViewModel> Questions { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
