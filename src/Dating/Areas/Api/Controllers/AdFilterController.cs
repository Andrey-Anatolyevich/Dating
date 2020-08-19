using Dating.Areas.Api.Models.EscortFilter;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dating.Areas.Api.Controllers
{
    public class AdFilterController : ApiBaseController
    {
        [HttpGet]
        public IActionResult GetFilterAgeRange()
        {
            var signinResponseModel = new AgeOptionsResponseModel(18, 60);
            var result = ApiResponse<AgeOptionsResponseModel>.NewSuccess(signinResponseModel);
            var resultString = ToJson(result);
            return Ok(resultString);
        }
    }
}
