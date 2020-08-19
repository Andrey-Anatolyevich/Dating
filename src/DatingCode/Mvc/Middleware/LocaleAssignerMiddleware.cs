using DatingCode.Business.Core;
using DatingCode.Config;
using DatingCode.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DatingCode.Mvc.Middleware
{
    public class LocaleAssignerMiddleware
    {
        public LocaleAssignerMiddleware(RequestDelegate next)
        {
            _next = next;

            _defaultLocaleCode = Consts.LocaleCodeEn;
        }

        private readonly RequestDelegate _next;

        private string _defaultLocaleCode;
        private bool _defaultLocaleIdLoaded;
        private int _defaultLocaleIdValue;

        public Task Invoke(HttpContext httpContext, ILocalizationService localeService, SessionOperator sessionOperator)
        {
            var context = httpContext as DefaultHttpContext;

            var maybeChosenLocaleId = sessionOperator.ChosenLocaleId_Get(httpContext);
            if (!maybeChosenLocaleId.Success)
            {
                var defaultLocaleId = GetDefaultLocaleId(localeService);
                sessionOperator.ChosenLocaleId_Set(httpContext, defaultLocaleId);
            }

            return _next(httpContext);
        }

        private int GetDefaultLocaleId(ILocalizationService localeService)
        {
            if (_defaultLocaleIdLoaded)
                return _defaultLocaleIdValue;


            var maybeLocale = localeService.GetLocale(code: _defaultLocaleCode);
            if (!maybeLocale.Success)
                throw new Exception(maybeLocale.ErrorMessage);

            _defaultLocaleIdValue = maybeLocale.Value.Id;
            _defaultLocaleIdLoaded = true;
            return _defaultLocaleIdValue;
        }
    }
}
