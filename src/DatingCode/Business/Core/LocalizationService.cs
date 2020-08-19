using AutoMapper;
using DatingCode.BusinessModels.Core;
using DatingCode.Core;
using DatingCode.Storage.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Core
{
    public class LocalizationService : ILocalizationService
    {
        public LocalizationService(IMapper mapper, ILocaleStorage localeStorage, ITranslationStorage translationStorage)
        {
            _mapper = mapper;
            _localeStorage = localeStorage;
            _translationStorage = translationStorage;
            _localeCache = new LocaleCache();
            _translationCache = new TranslationCache();
        }

        private IMapper _mapper;
        private ILocaleStorage _localeStorage;
        private ITranslationStorage _translationStorage;
        private LocaleCache _localeCache;
        private TranslationCache _translationCache;

        public Result<IEnumerable<LocaleInfo>> GetAllLocales()
        {
            var maybeCacheLocales = _localeCache.GetAllLocales();
            if (maybeCacheLocales.Success)
                return maybeCacheLocales.ToResult();

            var maybeStorageLocales  = _localeStorage.GetAll();
            if (!maybeStorageLocales.Success)
                return Result<IEnumerable<LocaleInfo>>.NewFailure(maybeStorageLocales.ErrorMessage);

            var locales = _mapper.Map<IEnumerable<LocaleInfo>>(maybeStorageLocales.Value);
            _localeCache.SetAllLocales(locales);
            return Result<IEnumerable<LocaleInfo>>.NewSuccess(locales);
        }

        public Maybe<LocaleInfo> GetLocale(int id)
        {
            var maybeLocaleCached = _localeCache.GetLocaleById(id: id);
            if (maybeLocaleCached.Success)
                return maybeLocaleCached;

            var maybeStorageLocale = _localeStorage.Get(id: id);
            if (!maybeStorageLocale.Success)
                return Maybe<LocaleInfo>.NewFailure(maybeStorageLocale.ErrorMessage);

            var storageLocale = maybeStorageLocale.Value;
            var locale = _mapper.Map<LocaleInfo>(storageLocale);
            _localeCache.SetLocale(locale);
            return Maybe<LocaleInfo>.NewSuccess(locale);
        }

        public Maybe<LocaleInfo> GetLocale(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Maybe<LocaleInfo>.NewFailure($"'{code}' is NULL / Empty / Whitespace.");

            var maybeCacheLocale = _localeCache.GetLocaleByCode(code: code);
            if (maybeCacheLocale.Success)
                return maybeCacheLocale;

            var maybeStorageLocale = _localeStorage.Get(code: code);
            if (!maybeStorageLocale.Success)
                return Maybe<LocaleInfo>.NewFailure(maybeStorageLocale.ErrorMessage);

            var storageLocale = maybeStorageLocale.Value;
            var locale = _mapper.Map<LocaleInfo>(storageLocale);
            _localeCache.SetLocale(locale);
            return Maybe<LocaleInfo>.NewSuccess(locale);
        }

        public Result<IEnumerable<TranslationInfo>> GetAllTranslations()
        {
            var maybeAllTranslations = _translationCache.GetAllTranslations();
            if (maybeAllTranslations.Success)
                return maybeAllTranslations.ToResult();

            var maybeStorageTranslations = _translationStorage.GetAll();
            if (!maybeStorageTranslations.Success)
                return Result<IEnumerable<TranslationInfo>>.NewFailure(maybeStorageTranslations.ErrorMessage);

            var translations = _mapper.Map<IEnumerable<TranslationInfo>>(maybeStorageTranslations.Value);
            _translationCache.SetAllTranslations(translations: translations);
            return Result<IEnumerable<TranslationInfo>>.NewSuccess(translations);
        }

        public Result<IEnumerable<TranslationInfo>> GetTranslationsForObject(int objectId)
        {
            var maybeTranslationsCache = _translationCache.GetTranslations(objectId: objectId);
            if (maybeTranslationsCache.Success)
                return maybeTranslationsCache.ToResult();

            var maybeStorageTranslations = _translationStorage.GetForObject(objectId);
            if (!maybeStorageTranslations.Success)
                return Result<IEnumerable<TranslationInfo>>.NewFailure(maybeStorageTranslations.ErrorMessage);

            var translations = _mapper.Map<IEnumerable<TranslationInfo>>(maybeStorageTranslations.Value);
            _translationCache.SetTranslations(objectId: objectId, translations: translations);
            return Result<IEnumerable<TranslationInfo>>.NewSuccess(translations);
        }

        public Maybe<TranslationInfo> GetTranslationForObject(int objectId, int localeId)
        {
            var maybeTranslationCache = _translationCache.GetTranslation(objectId: objectId, localeId: localeId);
            if (maybeTranslationCache.Success)
                return maybeTranslationCache;

            var maybeStorageTranslation = _translationStorage.GetForObject(objectId: objectId, localeId: localeId);
            if (!maybeStorageTranslation.Success)
                return Maybe<TranslationInfo>.NewFailure($"No translation for Object: '{objectId}' and Locale: '{localeId}'.");

            var translation = _mapper.Map<TranslationInfo>(maybeStorageTranslation.Value);
            _translationCache.SetTranslation(objectId: objectId, localeId: localeId, translation: translation);
            return Maybe<TranslationInfo>.NewSuccess(translation);
        }

        public Result SetTranslation(int objectId, int localeId, string value)
        {
            _translationCache.InvalidateTranslation(objectId: objectId);

            var maybeStorageTranslations = _translationStorage.GetForObject(objectId);
            if (!maybeStorageTranslations.Success)
                return Result.NewFailure(maybeStorageTranslations.ErrorMessage);

            var foundTranslation = maybeStorageTranslations.Value
                .FirstOrDefault(x => x.ObjectId == objectId && x.LocaleId == localeId);
            if (foundTranslation == null)
            {
                var resultCreate = _translationStorage.Create(objectId, localeId, value);
                return resultCreate;
            }
            else
            {
                var resultSet = _translationStorage.SetValue(objectId, localeId, value);
                return resultSet;
            }
        }
    }
}
