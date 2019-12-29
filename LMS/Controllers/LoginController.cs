using LMS.Models;
using LMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LMS.Controllers
{

     [AllowAnonymous]
    public class LoginController : Controller
    {
       
        LoginService loginservice= new LoginService();
        

        public ActionResult Login()
        {
            if (! User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            return RedirectToAction("Index","Admin");
          
        }

        

        [HttpPost]
        public ActionResult Login(string username , string password)
        {
            Login login = new Login(username, password);
            if (loginservice.findByLogin(login))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                TempData["loginSucc"] = "Welcome " + username;
                
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["loginErr"] = "Invalid Username or Password ! Please put all valid";
                return View();
            }
        }

        // GET: Login/Sign up
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Login/Sign up
        [HttpPost]
        public ActionResult SignUp(string name, string email, string contact, string address, string password, string confirmPassword)
        {
            SignUp user = new SignUp(name, email, contact, address, password, confirmPassword);

            if (validateUser(user))
            {
                if (loginservice.save(user))
                {
                    TempData["createSucc"] = "User Created Succesfully ";
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    TempData["createErr"] = "Failed ! Email already exist";
                    return View();
                }
            }
            else
            {
                TempData["createErr"] = "Faild To add user!";
                return View();
            }
        }

        private bool validateUser(SignUp user)
        {
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");


            if (user.name.All(c => !char.IsDigit(c)))
            {
                if (regex.IsMatch(user.email.Trim()))
                {
                    if (user.contact.Length == 11 && user.contact.All(c => char.IsDigit(c)))
                    {
                        if (isValidPassword(user.password))
                        {
                            if (user.password == user.confirmPassword)
                            {
                                return true;
                            }
                            else
                            {
                                TempData["confirmPassErr"] = "Password didnot match";
                                return false;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        TempData["contractErr"] = "invalid contract";
                        return false;
                    }
                }
                else
                {
                    TempData["emailErr"] = "invalid email";
                    return false;
                }
            }
            else
            {
                TempData["nameErr"] = "invalid name";
                return false;
            }
        }

        private bool isValidPassword(string password)
        {
            if (password.Length > 6)
            {
                if (password.Any(Char.IsUpper) && password.Any(ch => !Char.IsLetterOrDigit(ch)))
                {
                    return true;
                }
                else
                {
                    TempData["passwordErr"] = "Password must contain a special and Upper-Case character";
                    return false;
                }
            }
            else
            {
                TempData["passwordErr"] = "Password Length must be more than 6 character";
                return false;
            }
        }


        public ActionResult Logout()
        {

            ModelState.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Login");
        }

    }
}
