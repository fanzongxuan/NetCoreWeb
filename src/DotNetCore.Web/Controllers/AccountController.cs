﻿using Microsoft.AspNetCore.Mvc;
using DotNetCore.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using DotNetCore.Core.Domain.UserInfos;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using DotNetCore.Core.Extensions;
using DotNetCore.Service.Accounts;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCore.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (ModelState.IsValid)
            {
                var user = new Account { UserName = model.Username };
                var result = _accountService.Register(user, model.Password);

                if (result.Succeeded)
                {
                    _accountService.LoginWithUserNameAndPwd(model.Username, model.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ErrorNotification(string.Join("|", result.Errors.Select(x => x.Description)));
                }
            }
            else
            {
                ErrorNotification(string.Join("|", ModelState.Errors()));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (ModelState.IsValid)
            {
                var result = _accountService.LoginWithUserNameAndPwd(model.Username,
                   model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ErrorNotification("用户名或密码错误！");
            }
            else
            {
                ErrorNotification(string.Join("|", ModelState.Errors()));
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Logout()
        {
            _accountService.LoginOut();
            return RedirectToAction("Index", "Home");
        }
    }
}