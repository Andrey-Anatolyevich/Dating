using System;
using AutoMapper;
using Dating.Models;
using DatingCode.Business.Core;
using DatingCode.Business.Geo;
using DatingCode.BusinessModels.Auth;
using DatingCode.Config;
using DatingCode.Core;
using DatingCode.Extensions;
using DatingCode.Infrastructure.Di;
using DatingCode.Session;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Dating.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            _placesService = DiProxy.Get<IPlacesService>();
            _sessionOperator = DiProxy.Get<SessionOperator>();
            _mapper = DiProxy.Get<IMapper>();
            _localeService = DiProxy.Get<ILocalizationService>();
            _localizer = DiProxy.Get<ILocalizer>();

            var config = DiProxy.Get<ConfigValuesCollection>();
            _userFilesUrlPrefix = config.GetUserFilesUrlPrefix();

            var contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        private readonly string _userFilesUrlPrefix;
        protected IMapper _mapper;
        protected IPlacesService _placesService;
        protected SessionOperator _sessionOperator;
        protected ILocalizationService _localeService;
        protected ILocalizer _localizer;
        private JsonSerializerSettings _jsonSerializerSettings;

        public BaseViewModel GetBaseModel()
        {
            var model = new BaseViewModel();
            FillBaseModel(model);
            return model;
        }

        public void FillBaseModel(BaseViewModel model)
        {
            model.Localizer = _localizer;
            model.CurrentLocale = _localeService.GetLocale(Consts.LocaleCodeEn).Value;

            var maybeUser = GetMaybeMyUser();
            model.CurrentUser = maybeUser;
            if (maybeUser != null && maybeUser.Success)
            {
                model.ShowAdminPanel = maybeUser.Value.IsAdmin;
                model.CurrentLocale = maybeUser.Value.Locale;
            }
            else
            {
                model.ShowAdminPanel = false;
            }

            if (model.CurrentUser == null)
                model.CurrentUser = Maybe<UserInfo>.NewFailure("User is not set.");
        }

        protected Maybe<UserInfo> GetMaybeMyUser()
        {
            var maybeUser = HttpContext.User.GetMyUser();
            return maybeUser;
        }

        protected UserInfo GetMyUser()
        {
            var maybeUser = GetMaybeMyUser();
            if (!maybeUser.Success)
                throw new Exception("Can't get user.");

            return maybeUser.Value;
        }

        protected string GetPartialPicUrl(string[] pathParts)
        {
            return $"{_userFilesUrlPrefix}/{ string.Join("/", pathParts)}";
        }

        internal protected string ToJson(object objectToSerialize)
        {
            var result = JsonConvert.SerializeObject(objectToSerialize, _jsonSerializerSettings);
            return result;
        }
    }
}
