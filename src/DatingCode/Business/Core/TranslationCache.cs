using DatingCode.BusinessModels.Core;
using DatingCode.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DatingCode.Business.Core
{
    internal class TranslationCache
    {
        public TranslationCache()
        {
            _translationsById = new ConcurrentDictionary<int, IEnumerable<TranslationInfo>>();
            _translationByIdAndLocale = new ConcurrentDictionary<Tuple<int, int>, TranslationInfo>();
            _translationByIdAndLocaleLock = new object();
            _allTranslations = new List<TranslationInfo>();
            _allTranslationsLock = new object();
        }

        private ConcurrentDictionary<int, IEnumerable<TranslationInfo>> _translationsById;
        private ConcurrentDictionary<Tuple<int, int>, TranslationInfo> _translationByIdAndLocale;
        private object _translationByIdAndLocaleLock;
        private List<TranslationInfo> _allTranslations;
        private bool _allTranslationsSet;
        private object _allTranslationsLock;

        internal Maybe<TranslationInfo> GetTranslation(int objectId, int localeId)
        {
            lock (_translationByIdAndLocaleLock)
            {
                if (_translationByIdAndLocale.TryGetValue(new Tuple<int, int>(objectId, localeId), out var translation))
                    return Maybe<TranslationInfo>.NewSuccess(translation);

                return Maybe<TranslationInfo>.NewFailure($"Can't find all for ID: '{objectId}' and Locale: '{localeId}'.");
            }
        }

        internal Maybe<IEnumerable<TranslationInfo>> GetTranslations(int objectId)
        {
            if (_translationsById.TryGetValue(objectId, out var translations))
                return Maybe<IEnumerable<TranslationInfo>>.NewSuccess(translations);

            return Maybe<IEnumerable<TranslationInfo>>.NewFailure($"Can't find all for ID: '{objectId}'.");
        }

        internal void SetTranslations(int objectId, IEnumerable<TranslationInfo> translations)
        {
            _translationsById.AddOrUpdate(objectId, translations, (id, transls) => translations);
        }

        internal void SetTranslation(int objectId, int localeId, TranslationInfo translation)
        {
            lock (_translationByIdAndLocaleLock)
            {
                _translationByIdAndLocale.AddOrUpdate(new Tuple<int, int>(objectId, localeId), translation, (tpl, trns) => translation);
            }
        }

        internal Maybe<IEnumerable<TranslationInfo>> GetAllTranslations()
        {
            lock (_allTranslationsLock)
            {
                if (_allTranslationsSet)
                    return Maybe<IEnumerable<TranslationInfo>>.NewSuccess(_allTranslations);

                return Maybe<IEnumerable<TranslationInfo>>.NewFailure($"All are not set in cache.");
            }
        }

        internal void SetAllTranslations(IEnumerable<TranslationInfo> translations)
        {
            lock (_allTranslationsLock)
            {
                _allTranslations.Clear();
                _allTranslations.AddRange(translations);
                _allTranslationsSet = true;
            }

            lock (_translationByIdAndLocaleLock)
            {
                foreach (var translation in translations)
                    SetTranslation(objectId: translation.ObjectId, localeId: translation.LocaleId, translation: translation);
            }
        }

        internal void InvalidateTranslation(int objectId)
        {
            _translationsById.TryRemove(objectId, out _);

            lock (_allTranslationsLock)
            {
                _allTranslations.Clear();
                _allTranslationsSet = false;
            }

            lock (_translationByIdAndLocaleLock)
            {
                var keysToRemove = _translationByIdAndLocale.Keys.Where(x => x.Item1 == objectId);
                foreach (var key in keysToRemove)
                    _translationByIdAndLocale.Remove(key, out _);
            }
        }
    }
}
