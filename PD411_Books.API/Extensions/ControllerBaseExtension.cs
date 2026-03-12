using Microsoft.AspNetCore.Mvc;
using PD411_Books.BLL.Services;

namespace PD411_Books.API.Extensions
{
    public static class ControllerBaseExtension
    {
        public static IActionResult GetAction(this ControllerBase controller, ServiceResponse response)
        {
            if (response.Success)
            {
                return controller.Ok(response);
            }
            else
            {
                return controller.BadRequest(response);
            }
        }
    }
}
