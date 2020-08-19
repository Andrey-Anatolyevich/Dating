using Dating.Areas.Admin.Models.Users;
using Dating.Models.Shared;
using DatingCode.Business.Users;
using DatingCode.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dating.Areas.Admin.Controllers
{
    [Auth(allowedClaims: "admin")]
    public class UsersController : AdminBaseController
    {
        public UsersController(IUserInfoService userService)
            : base()
        {
            _userService = userService;
        }

        private IUserInfoService _userService;

        public IActionResult AllUsersJson()
        {
            var allUsers = _userService.GetAll();
            var jsonUsers = _mapper.Map<List<UserInfoJson>>(allUsers);
            var result = new ApiData<List<UserInfoJson>>() { data = jsonUsers };
            var serializedResult = ToJson(result);
            return Ok(serializedResult);
        }
    }
}
