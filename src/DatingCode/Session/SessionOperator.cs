using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Users;
using DatingCode.Config;
using DatingCode.Core;
using Microsoft.AspNetCore.Http;
using System;

namespace DatingCode.Session
{
    public class SessionOperator
    {
        public SessionOperator(Consts consts)
        {
            _sessionUserIdKey = consts.SessionUserIdKey;
            _sessionLocationIdKey = consts.SessionLocationIdKey;
            _sessionLocaleIdKey = consts.SessionLocaleIdKey;
        }

        private readonly string _sessionUserIdKey;
        private readonly string _sessionLocationIdKey;
        private readonly string _sessionLocaleIdKey;

        public Maybe<UserInfo> CurrentUser_Get(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));


            var context = httpContext as DefaultHttpContext;
            var userContainer = context.User as UserContainer;
            if (userContainer == null)
                return Maybe<UserInfo>.NewFailure("Not logged in.");

            var result = Maybe<UserInfo>.NewFromValue(userContainer.MyUser, "Not logged in.");
            return result;
        }

        public void CurrentUser_Set(HttpContext httpContext, UserContainer userContainer)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));
            if (userContainer == null)
                throw new ArgumentNullException(nameof(userContainer));


            var context = httpContext as DefaultHttpContext;
            context.User = userContainer;

            var userIdString = userContainer.MyUser.Id.ToString();
            context.Session.SetString(_sessionUserIdKey, userIdString);
        }

        public void CurrentUser_Clear(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));


            var context = httpContext as DefaultHttpContext;
            context.User = null;
            context.Session.Remove(_sessionUserIdKey);
        }

        public void ChosenLocationId_Set(HttpContext httpContext, int locationId)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var context = httpContext as DefaultHttpContext;
            context.Session.SetInt32(_sessionLocationIdKey, locationId);
        }

        public Maybe<int> ChosenLocationId_Get(HttpContext httpContext)
        {
            if (httpContext == null)
                return Maybe<int>.NewFailure($"{nameof(httpContext)} is NULL.");

            var context = httpContext as DefaultHttpContext;
            var nLocationId = context.Session.GetInt32(_sessionLocationIdKey);
            if (nLocationId.HasValue)
                return Maybe<int>.NewSuccess(nLocationId.Value);
            else
                return Maybe<int>.NewFailure("Value is not found in session.");
        }

        public void ChosenLocaleId_Set(HttpContext httpContext, int localeId)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var context = httpContext as DefaultHttpContext;
            context.Session.SetInt32(_sessionLocaleIdKey, localeId);
        }

        public Maybe<int> ChosenLocaleId_Get(HttpContext httpContext)
        {
            if (httpContext == null)
                return Maybe<int>.NewFailure($"{nameof(httpContext)} is NULL.");

            var context = httpContext as DefaultHttpContext;
            var nLocaleId = context.Session.GetInt32(_sessionLocaleIdKey);
            if (nLocaleId.HasValue)
                return Maybe<int>.NewSuccess(nLocaleId.Value);
            else
                return Maybe<int>.NewFailure($"Value is not found in session by ke: '{_sessionLocaleIdKey}'.");
        }
    }
}
