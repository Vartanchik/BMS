using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BMS.WebAPI.Utility.Extensions
{
    public static class ControllerExtension
    {
        public static int GetCurrentUserId(this ControllerBase controller)
        {
            return Convert.ToInt32(controller.User.Claims.Where(c => c.Type == "userId").First().Value);
        }
    }
}
