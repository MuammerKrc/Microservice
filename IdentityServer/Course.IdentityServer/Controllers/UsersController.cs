using Course.IdentityServer.Dtos;
using Course.IdentityServer.Models;
using Course.Shared.ControllerHelper;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Course.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UsersController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
         
        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateDto userCreateDto)
        {
            var user = new ApplicationUser()
            {
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email,
                City = userCreateDto.City
            };
            IdentityResult result = await _userManager.CreateAsync(user, userCreateDto.Password);
            if(!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(i=>i.Description).ToList(), 400));
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.Sub).Value;
            if(userId==null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user==null)
            {
                return BadRequest();
            }
            return Ok(new { Id = user.Id, Username = user.UserName, Email = user.Email, City = user.City });
        }
    }
}
