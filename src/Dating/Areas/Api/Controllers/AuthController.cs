using Dating.Areas.Api.Models.Auth;
using Dating.Models;
using DatingCode.Business.Users;
using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Users;
using DatingCode.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Api.Controllers
{
    public class AuthController : ApiBaseController
    {
        public AuthController(AuthService userService)
            : base()
        {
            _userService = userService;
        }

        private AuthService _userService;

        [HttpPost]
        public IActionResult SignIn(SignInRequestModel signInRequest)
        {
            if (signInRequest == null
                || string.IsNullOrWhiteSpace(signInRequest.Login)
                || string.IsNullOrWhiteSpace(signInRequest.Password))
            {
                return Ok(ToJson(ApiResponse.NewFail("InvalidCredentials")));
            }

            var mbUser = _userService.UserLoginRequest(login: signInRequest.Login, pass: signInRequest.Password);
            if (!mbUser.Success)
                return Ok(ToJson(ApiResponse.NewFail("InvalidCredentials")));

            var userContainer = new UserContainer(mbUser.Value);
            _sessionOperator.CurrentUser_Set(HttpContext, userContainer);

            var signinResponseModel = GetSigninResponseModel(mbUser.Value);
            var result = ApiResponse<SignInResponseModel>.NewSuccess(signinResponseModel);
            return Ok(ToJson(result));
        }

        [HttpGet]
        [HttpPost]
        public IActionResult GetCurrentUser()
        {
            var mbUser = _sessionOperator.CurrentUser_Get(HttpContext);
            if (!mbUser.Success)
                return Json(ApiResponse.NewFail("Not logged in."));

            var signinResponseModel = GetSigninResponseModel(mbUser.Value);
            var result = ApiResponse<SignInResponseModel>.NewSuccess(signinResponseModel);
            return Ok(ToJson(result));
        }

        [Auth]
        [HttpPost]
        public IActionResult SignOut()
        {
            _sessionOperator.CurrentUser_Clear(HttpContext);
            var result = ApiResponse.NewSuccess();
            return Ok(ToJson(result));
        }

        private SignInResponseModel GetSigninResponseModel(UserInfo user)
        {
            var result = _mapper.Map<SignInResponseModel>(user);
            return result;
        }
    }
}
