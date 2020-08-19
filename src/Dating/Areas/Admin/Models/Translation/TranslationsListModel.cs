using Dating.Models;
using DatingCode.BusinessModels.Core;
using DatingCode.Core;
using System.Collections.Generic;

namespace Dating.Areas.Admin.Models.Translation
{
    public class TranslationsListModel : BaseViewModel
    {
        public Result<IEnumerable<TranslationInfo>> Translations;
    }
}
