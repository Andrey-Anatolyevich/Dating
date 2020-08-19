using DatingCode.BusinessModels.Core;
using DatingCode.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DatingCode.Business.Core
{
    internal class LocaleCache
    {
        public LocaleCache()
        {
            _allLocales = new List<LocaleInfo>();
            _allLocalesLock = new object();
            _localesById = new ConcurrentDictionary<int, LocaleInfo>();
            _localesByCode = new ConcurrentDictionary<string, LocaleInfo>();
        }

        private ConcurrentDictionary<int, LocaleInfo> _localesById;
        private ConcurrentDictionary<string, LocaleInfo> _localesByCode;
        private List<LocaleInfo> _allLocales;
        private bool _allLocalesSet;
        private object _allLocalesLock;

        internal Maybe<LocaleInfo> GetLocaleById(int id)
        {
            if (_localesById.TryGetValue(id, out var locale))
                return Maybe<LocaleInfo>.NewSuccess(locale);

            return Maybe<LocaleInfo>.NewFailure($"Nothing in cache by ID: '{id}'.");
        }

        internal Maybe<IEnumerable<LocaleInfo>> GetAllLocales()
        {
            lock (_allLocalesLock)
            {
                if (_allLocalesSet)
                    return Maybe<IEnumerable<LocaleInfo>>.NewSuccess(_allLocales);

                return Maybe<IEnumerable<LocaleInfo>>.NewFailure("All is not set.");
            }
        }

        internal void SetAllLocales(IEnumerable<LocaleInfo> locales)
        {
            lock (_allLocalesLock)
            {
                _allLocales.Clear();
                _allLocales.AddRange(locales);
                _allLocalesSet = true;
            }
        }

        internal Maybe<LocaleInfo> GetLocaleByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Maybe<LocaleInfo>.NewFailure($"Parameter '{nameof(code)}' is NULL / Empty / Whitespace.");

            if (_localesByCode.TryGetValue(code, out var locale))
                return Maybe<LocaleInfo>.NewSuccess(locale);

            return Maybe<LocaleInfo>.NewFailure($"Nothing in cache by Code: '{code}'.");
        }

        internal void SetLocale(LocaleInfo locale)
        {
            _localesById.AddOrUpdate(locale.Id, locale, (id, loc) => locale);
            _localesByCode.AddOrUpdate(locale.Code, locale, (id, loc) => locale);
        }
    }
}
