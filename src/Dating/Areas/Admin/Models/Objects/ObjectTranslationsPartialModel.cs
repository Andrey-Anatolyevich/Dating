using DatingCode.BusinessModels.Basics;
using DatingCode.BusinessModels.Core;
using System.Collections.Generic;

namespace Dating.Areas.Admin.Models.Objects
{
    public class ObjectTranslationsPartialModel
    {
        public ObjectItem Object;
        public IEnumerable<LocaleInfo> Locales;
        public IEnumerable<TranslationInfo> Translations;
    }
}
