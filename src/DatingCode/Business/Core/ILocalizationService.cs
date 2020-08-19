using DatingCode.BusinessModels.Core;
using DatingCode.Core;
using System.Collections.Generic;

namespace DatingCode.Business.Core
{
    public interface ILocalizationService
    {
        Maybe<LocaleInfo> GetLocale(int id);
        Maybe<LocaleInfo> GetLocale(string code);
        Result<IEnumerable<LocaleInfo>> GetAllLocales();
        Result<IEnumerable<TranslationInfo>> GetAllTranslations();
        Result<IEnumerable<TranslationInfo>> GetTranslationsForObject(int objectId);
        Maybe<TranslationInfo> GetTranslationForObject(int objectId, int localeId);
        Result SetTranslation(int objectId, int localeId, string value);
    }
}
