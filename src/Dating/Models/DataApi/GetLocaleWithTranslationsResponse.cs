using System.Collections.Generic;

namespace Dating.Models.DataApi
{
    public class GetLocaleWithTranslationsResponse
    {
        public int LocaleId;
        public List<TranslationInfoModel> Translations;
    }
}
