using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Login.Models;
using Login.Repository;
using Login.Services;

namespace Login.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]

        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
            {
                return NotFound(new { message = "Usuario ou senha invalidos" });
            }
            
            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonymus")]
        [AllowAnonymous]
        public string Anonymus() => "Anonimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Authenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "Manager")]
        public string Manager() => "Gerente";

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee, manager")]
        public string Employee() => "Funcionario";


    }
}