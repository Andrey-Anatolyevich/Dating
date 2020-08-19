using AutoMapper;
using DatingCode.Business.Basics;
using DatingCode.Business.Core;
using DatingCode.Business.Geo;
using DatingCode.Business.Dating;
using DatingCode.Business.Media;
using DatingCode.Business.Users;
using DatingCode.Config;
using DatingCode.Session;
using System;
using System.Collections.Generic;

namespace DatingCode.Infrastructure.Di
{
    public class DiProxy
    {
        static DiProxy()
        {
            _elements = new Dictionary<Type, object>();
        }

        public DiProxy(
            IPlacesService placesService
            , SessionOperator sessionOperator
            , IObjectTypesService objectTypesService
            , IObjectsService objectsService
            , ILocalizationService localeService
            , IMapper mapper
            , ILocalizer localizer
            , IAdsService adsService
            , IFilesService filesService
            , ConfigValuesCollection configValues
            , IUserInfoService userInfoService)
        {
            _elements.Add(typeof(IPlacesService), placesService);
            _elements.Add(typeof(SessionOperator), sessionOperator);
            _elements.Add(typeof(IObjectTypesService), objectTypesService);
            _elements.Add(typeof(IObjectsService), objectsService);
            _elements.Add(typeof(ILocalizationService), localeService);
            _elements.Add(typeof(IMapper), mapper);
            _elements.Add(typeof(ILocalizer), localizer);
            _elements.Add(typeof(IAdsService), adsService);
            _elements.Add(typeof(IFilesService), filesService);
            _elements.Add(typeof(ConfigValuesCollection), configValues);
            _elements.Add(typeof(IUserInfoService), userInfoService);
        }

        private static Dictionary<Type, object> _elements;

        public static T Get<T>()
        {
            var type = typeof(T);
            var resultObject = _elements[type];
            var result = (T)resultObject;
            return result;
        }
    }
}
