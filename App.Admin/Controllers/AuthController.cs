﻿using App.Admin.Models;
using App.Data.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAddress = "http://localhost:5005/api/Users";
        private readonly string _apiRoleAddress = "http://localhost:5005/api/Roles";
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]


        public async Task<IActionResult> Register(User newUser, string password2)
        {
            try
            {
                if (newUser.Password == password2)
                {
                    var users = await _httpClient.GetFromJsonAsync<List<User>>(_apiAddress);
                    var user = users.FirstOrDefault(u => u.Email == newUser.Email);

                    if (user is not null)
                        ModelState.AddModelError("", "This Email Has Already Been Registered!");

                    var roles = await _httpClient.GetFromJsonAsync<List<Role>>(_apiRoleAddress);

                    // Burada, doktor, kullanıcı ve admin rollerinden birini seçelim.
                    Role selectedRole = SelectRoleToAssign(roles);

                    if (selectedRole == null)
                    {
                        ModelState.AddModelError("", "No valid role found to assign to the new user.");
                        return View(newUser);
                    }

                    newUser.RoleId = selectedRole.Id; // Eğer Id kullanıyorsanız RoleId'ye atayın, ya da RoleName'e göre yapıyorsanız RoleName'e atayın.

                    var add = await _httpClient.PostAsJsonAsync(_apiAddress, newUser);

                    if (add.IsSuccessStatusCode)
                        return RedirectToAction("Index", "Home");
                    else
                        ModelState.AddModelError("", "An error occurred while registering the user.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(newUser);
        }

        // Belirtilen üç rol (doktor, kullanıcı ve admin) arasından birini seçen yardımcı fonksiyon.
        private Role SelectRoleToAssign(List<Role> roles)
        {

            // Varsayılan olarak, doktor rolünü döndürelim:
            return roles.FirstOrDefault(r => r.RoleName == "doktor");
        }







        public IActionResult Login([FromQuery] string? redirectUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var users = await _httpClient.GetFromJsonAsync<List<User>>(_apiAddress);
            var account = users?.FirstOrDefault(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
            if (account == null)
            {

                ModelState.AddModelError("", "Login Failed!");

                return View(loginModel);
            }
            else
            {

                if (account.RoleId == 1)
                {
                    var userAccess = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Role, account.Role.RoleName)

                    };

                    var userIdentity = new ClaimsIdentity(userAccess, "Login");


                    ClaimsPrincipal claimsPrincipal = new(userIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    return Redirect("/Main");
                }
                else
                {
                    ModelState.AddModelError("", "You have not permitted!");
                    return View(loginModel);
                }


            }
       













        }
    }
}