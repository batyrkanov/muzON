﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MuzON.BLL.DTO;
using MuzON.BLL.Interfaces;
using MuzON.Domain.Identity;
using MuzON.Web.App_Start;
using MuzON.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MuzON.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUserService UserService) : base(UserService) { }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUsers()
        {
            var userDTOs = userService.GetUsers();

            return Json(new { data = Mapper.Map<IEnumerable<RegisterViewModel>>(userDTOs) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(Guid id)
        {
            var userDto = userService.GetUserById(id);
            var user = Mapper.Map<EditUserViewModel>(userDto);
            ViewBag.Role = userDto.Role;
            return PartialView("_Details", user);
        }

        public ActionResult Edit(Guid id)
        {
            var userDTO = userService.GetUserById(id);
            if (userDTO == null)
            {
                return Json(new { data = "userNotFound" }, JsonRequestBehavior.AllowGet);
            }
            var user = Mapper.Map<EditUserViewModel>(userDTO);
            ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Id", "Name", RoleManager.FindByName(userDTO.Role).Id);
            return PartialView("_Edit", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errorList = new List<string>();
                var userDto = Mapper.Map<EditUserViewModel, UserDTO>(model);
                var role = Guid.Parse(Request.Form["Roles"]);
                userDto.Role = RoleManager.FindById(role).Name;
                var result = await UpdateAsync(userDto);
                if (result.Succeeded)
                {
                    return Json(new { data = "success" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error);
                    }
                    return Json(new { model, errorMessage = errorList, JsonRequestBehavior.AllowGet });
                }
            }
            return Json(new { model, errorMessage = util.GetErrorList(ModelState.Values), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password
                };
                ClaimsIdentity claim = await Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return PartialView("_Registerpartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errorList = new List<string>();
                var userDto = Mapper.Map<RegisterViewModel, UserDTO>(model);
                userDto.Role = "user";

                var result = await CreateAsync(userDto);
                if (result.Succeeded)
                {
                    logger.InfoLog("Register",
                            "registered",
                            model.Email,
                            model.Id,
                            User.Identity.Name);
                    return Json(new { data = "success" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error);
                    }
                    return Json(new { model, errorMessage = errorList, JsonRequestBehavior.AllowGet });
                }

            }
            return Json(new { model, errorMessage = util.GetErrorList(ModelState.Values), JsonRequestBehavior.AllowGet });
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView("_ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                                new { UserId = user.Id, code }, protocol: Request.Url.Scheme);
                var result = await SendEmail(user.Email, "Reset Password",
                                    "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                if (result)
                    return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { model, errorMessage = util.GetErrorList(ModelState.Values) }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { model, errorMessage = util.GetErrorList(ModelState.Values), JsonRequestBehavior.AllowGet });
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Json(new { data = "userNotFound" });
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Json(new { data = "success" });
            }
            ModelState.AddModelError("", result.Errors.First());
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { model, errorMessage = util.GetErrorList(ModelState.Values) }, JsonRequestBehavior.AllowGet);
            }
            var result = await UserManager.ChangePasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    });
                }
                return Json(new { data = "success" });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return Json(new { model, errorMessage = util.GetErrorList(ModelState.Values) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(Guid id)
        {
            var user = userService.GetUserById(id);
            if (user == null)
                return HttpNotFound();
            return PartialView("_DeletePartial", Mapper.Map<RegisterViewModel>(user));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(Guid id)
        {
            userService.DeleteUser(id);
            return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
        }

        private async Task<bool> SendEmail(string userEmail, string messageSubject, string messageBody)
        {
            var sendState = false;
            var message = new MailMessage();
            message.To.Add(new MailAddress(userEmail));  // replace with valid value 
            message.From = new MailAddress("muzonfreemusik@gmail.com");  // replace with valid value
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "muzonfreemusik@gmail.com",  // replace with valid value
                    Password = "password"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                sendState = true;
            }
            return sendState;
        }

        private async Task<IdentityResult> CreateAsync(UserDTO userDTO)
        {
            User user = await UserManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                // if role "user" wasn't find in the table Roles, then create this
                Role role = await RoleManager.FindByNameAsync(userDTO.Role);
                if (role == null)
                {
                    role = new Role { Id = Guid.NewGuid(), Name = userDTO.Role };
                    var roleAddResult =
                        await RoleManager.CreateAsync(role);
                    if (roleAddResult.Errors.Count() > 0)
                        return IdentityResult.Failed("System can't create role, please tell about this to admin");
                }
                user = new User { Email = userDTO.Email, UserName = userDTO.Email };
                var result =
                    await UserManager.CreateAsync(user, userDTO.Password);
                if (result.Errors.Count() > 0)
                    return IdentityResult.Failed("Something wrong, please check all fields");
                //fill userRoles table
                await UserManager.AddToRoleAsync(user.Id, userDTO.Role);
                await userService.Save();
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed("User with same Email exist");
            }
        }

        private async Task<IdentityResult> UpdateAsync(UserDTO userDTO)
        {
            User user = UserManager.FindByEmail(userDTO.Email);
            var hash = user.PasswordHash;
            Mapper.Map(userDTO, user);
            user.PasswordHash = hash;
            // if role "user" wasn't find in the table Roles, then create this
            Role role = await RoleManager.FindByNameAsync(userDTO.Role);
            if (role == null)
            {
                role = new Role { Id = Guid.NewGuid(), Name = userDTO.Role };
                var roleAddResult =
                    await RoleManager.CreateAsync(role);
                if (roleAddResult.Errors.Count() > 0)
                    return IdentityResult.Failed("System can't create role, please tell about this to admin");
            }

            var result =
                await UserManager.UpdateAsync(user);
            if (result.Errors.Count() > 0)
                return IdentityResult.Failed("Something wrong, please check all fields");
            //get role name
            var existRoleId = UserManager.FindByEmail(user.Email).Roles.Select(x => x.RoleId).Single();
            await UserManager.RemoveFromRoleAsync(user.Id, RoleManager.FindById(existRoleId).Name);
            //fill userRoles table
            await UserManager.AddToRoleAsync(user.Id, userDTO.Role);
            await userService.Save();
            return IdentityResult.Success;
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
        {
            ClaimsIdentity claim = null;
            // find user 
            User user = await UserManager.FindAsync(userDTO.Email, userDTO.Password);
            // authorize him and return claims identity obj
            if (user != null)
                claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }
    }
}