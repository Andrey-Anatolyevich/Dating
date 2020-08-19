using DatingCode.Business.Core;
using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Core;
using DatingCode.Core;

namespace Dating.Models
{
    public class BaseViewModel
    {
        public ILocalizer Localizer;

        public bool ShowAdminPanel;
        public Maybe<UserInfo> CurrentUser;
        public LocaleInfo CurrentLocale;

        public string GetLocaledString(string stringCode)
        {
            var result = Localizer.ForString(CurrentLocale.Id, stringCode);
            return result;
        }

        public string GetLocaledObject(int objectId)
        {
            var result = Localizer.ForObject(localeId: CurrentLocale.Id, objectId: objectId);
            return result;
        }
    }
}
