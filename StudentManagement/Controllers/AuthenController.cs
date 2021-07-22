using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Helpers;
using Microsoft.AspNetCore.Http;

namespace StudentManagement.Controllers
{
    public class AuthenController : Controller
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public AuthenController(IUserService userService,
           IMapper mapper,IConfiguration configuration)
        {
            _mapper = mapper;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Return view login
        /// </summary>
        //View Login
        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                var a = HttpContext.Session.GetString("Token");
                if (a != null)
                    _logger.Trace("Access View Login");
                return View();
                
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }
            
        }

        /// <summary>
        /// Return view register
        /// </summary>
        //View Register
        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                _logger.Trace("Access View Register");
                return View("Register");
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }

        }
        //View ChangePassword
        [Route("ChangePassword")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
           return View("ChangePassword");
        }
        /// <summary>
        /// Login with Email and Password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / Login
        ///     {
        ///        "email": "abc@gmail.com",
        ///        "password": "1111"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Login succesfully</response>
        /// <response code="400">Email or password is incorrect</response>          
        //Method Login
        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(Login model)
        {
            try {
                var user = _mapper.Map<User>(model);
                var a =_userService.Login(user);
                
                if (a == null)
                {
                    _logger.Error("Email or Password is incorrect");
                    return View();
                } 

                var claim = new[] {
               new Claim(JwtRegisteredClaimNames.Sub, user.Email)
                };
                var signinKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
                model.Token = new JwtSecurityTokenHandler().WriteToken(token);
                HttpContext.Session.SetString("Token", model.Token);
                _logger.Info("Login successfully with Token: " + model.Token);
                return Redirect("GetAllStudent");
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }            
        }
        /// <summary>
        /// Register new account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / Register
        ///     {
        ///        "username":"abc",
        ///        "email": "abc@gmail.com",
        ///        "password": "1111"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Create succesfully</response>
        /// <response code="400">If item is null</response>      
        [Route("Register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(Register model)
        {
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Register(user);
                return Redirect("Login");
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return View();
            }
        }
    }
        
    
}
