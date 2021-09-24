using Course.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Shared.ControllerHelper
{
    public class CustomBaseController:ControllerBase
    {
        public IActionResult QQReturnObject<T>(Response<T> response)
        {
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}
