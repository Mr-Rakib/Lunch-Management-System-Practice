
using LMS.Models;
using LMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;


namespace LMS.Controllers
{
    [Authorize(Roles="admin,subadmin,user")]
    public class AdminController : Controller
    {

        AdminService adminService = new AdminService();
        OrderService orderService = new OrderService();
        
        public ActionResult Index( int? page)
        {
            if (isOnlyUser())
            {
                return RedirectToAction("Order");
            }
            else
            {
                int pageSize = 20;
                int pageNumber = (page ?? 1);

                List<SignUp> allUserInfo = adminService.findAllInfo();
                List<UsersRole> users = adminService.findRoles();
                TempData["totalUser"] = users.FindAll(u => u.userRole == "user").Count();
                TempData["totalSubAdmin"] = users.FindAll(u => u.userRole == "subadmin").Count();
                TempData["totalAdmin"] = users.FindAll(u => u.userRole == "admin").Count();
                
                return View(allUserInfo.ToPagedList(pageNumber, pageSize));
               
            }
        }

        private bool isOnlyUser()
        {
            var role = adminService.findRoles().FindAll(
                        u => u.username == User.Identity.Name).ToList();

            var id = adminService.findAllInfoByEmail(User.Identity.Name).id;

            var isuser = adminService.findRoleByID(id, "user");

            if (isuser != null && role.Count() == 1 && isuser.userRole == "user")
            {
                return true;
            }
            else return false;
        }

        public ActionResult Details(int id)
        {
            return View(adminService.findAllInfoById(id));
        }
        [Authorize(Roles ="admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string name , string email, string contact, string address, string password , string confirmPassword)
        {
            SignUp user = new SignUp (name, email, contact,address, password, confirmPassword);

            if (validateUser(user))
            {
                if (adminService.save(user))
                {
                    TempData["createSucc"] = "User Created Succesfully ";
                    return RedirectToAction("AllUsers");
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
        [Authorize(Roles = "admin,subadmin")]
        public ActionResult AllUsers(int? page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;

            if (User.IsInRole("admin"))
            {
                return View(adminService.findAllInfo().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                

                List<UsersRole> userRole = adminService.findRoles();

                var all = adminService.findAllInfo().Where(u => userRole.Any(y => y.userID == u.id && y.userRole == "user"));
                return View(all.ToPagedList(pageNumber , pageSize));
            }
            
        }

        
        public ActionResult Edit(int id)
        {
            return View(adminService.findAllInfoById(id));
        }
        [HttpPost]
        public ActionResult Edit(int id, string name, string email, string contact, string address, string password, string confirmPassword)
        {
            SignUp user = new SignUp(name, email, contact, address, password, confirmPassword);
            if (validateUser(user) && adminService.Update(id,user))
            {
                TempData["editSucc"] = "User Updated Succesfully";
                return RedirectToAction("AllUsers" ,"Admin");

            }else
            {
                TempData["editErr"] = "Failed To Update User";
                return View();
            }
        }
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            return View(adminService.findAllInfoById(id));
        }
        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, string name, string email, string contact, string address, string password)
        {
            if (adminService.deleteById(id))
            {
                TempData["deleteSucc"] = "User Deleted Succesfully";
               
            }
            else
            {
                TempData["deleteErr"] = "Failed To Update User";
            }
            return RedirectToAction("AllUsers");
        }

        //--------------------------------------------------------CRUD PART ENDS USERS-----------------------------------------------------------//
        [Authorize(Roles = "admin,subadmin")]
        public ActionResult SeeRoles()
        {
            return View(adminService.findRoles());
        }
        [Authorize(Roles = "admin")]
        public ActionResult UpdateRole(int id, string userRole)
        {
            var u = adminService.findRoleByID(id, userRole);
            UsersRole user = new UsersRole(u.userID, u.fullName, u.username,u.userRole,u.userRole);
            return View(user);
        }
        [HttpPost]
        public ActionResult UpdateRole(int id, string fullName, string username , string userCurrentRole ,string userRole)
        {
            /////Fix the bug
            UsersRole user = new UsersRole(id,fullName,username,userCurrentRole,userRole);
            if (adminService.SetRole(user))
            {
                TempData["updateRoleSucc"] = fullName + "  Role Updated Succesfully";
                return RedirectToAction("SeeRoles");
            }
            else
            {
                TempData["updateRoleErr"] = fullName + " Already is a " + userRole + " ! Please Set another or exit";
                return View(user);
            }
        }
        [Authorize(Roles = "admin")]
        public ActionResult AddRole(int id, string userRole)
        {
            return View(adminService.findRoleByID(id, userRole));
        }
        [HttpPost]
        public ActionResult AddRole(int id, string fullName, string username, string userRole)
        {
            UsersRole user = new UsersRole(id, fullName, username, userRole);
            if (adminService.AddRole(user))
            {
                TempData["updateRoleSucc"] = fullName + "  Role Added Succesfully";
                return RedirectToAction("SeeRoles"); 
            }
            else
            {
                TempData["addRoleErr"] = fullName + " Already is a "+userRole+" ! Please Set another or exit";
                return View(user);
            }
        }
        [Authorize(Roles = "admin")]
        public ActionResult DeleteRole(int id, string userRole)
        {
           
            if (adminService.DeleteRole(id,userRole))
            {
                TempData["deleteRoleErr"] = "ID "+id + " with role "+userRole+" Deleted Succesfully";
            }
            else
            {
                TempData["deleteRoleErr"] = "ID " + id + " has only 1 role left ! You could not delete it.";
            }
            return RedirectToAction("SeeRoles");
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
                            if(user.password == user.confirmPassword)
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
            if(password.Length > 6)
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

        ///--------------------------------------------------Admins and Sub-Admins Action's are ended------------------------------------------------//

        public ActionResult MyProfile()
        {
            return View(adminService.findAllInfo().Find(u => u.email == User.Identity.Name));
        }

        public ActionResult Order()
        {
            SignUp user = adminService.findAllInfoByEmail(User.Identity.Name);
            OrderViewModel orders = new OrderViewModel(
                orderService.findOrderInfoById(User.Identity.Name).FindAll(u => u.orderDate.Month == DateTime.Now.Month),
                orderService.findOrderInfo().FindAll(u => u.orderDate.Month == DateTime.Now.Month),
                new Order(user.id, user.name, DateTime.Now.Date, 1),
                orderService.getPerHeadMealCost()
                );

            SetAllCostingData();
            



            return View(orders);
        }

        private void SetAllCostingData()
        {
            int costperMeal = orderService.getPerMealCost().Find(x=> x.modifyDate < DateTime.Now).costPerMeal;
            int mealcount = orderService.findOrderInfoById(User.Identity.Name).FindAll(u => u.orderDate.Month == DateTime.Now.Date.Month && u.orderDate.Year == DateTime.Now.Date.Year).Sum(u => u.quantity);

            TempData["totalMyOrder"] = mealcount;
            TempData["totalOrderwithCost"] = mealcount * costperMeal;

            TempData["totalOrderMonth"] = orderService.findOrderInfo()

                .FindAll(u => u.orderDate.Month == DateTime.Now.Date.Month && u.orderDate.Year == DateTime.Now.Date.Year)
                .Sum(u => u.quantity);

            TempData["totalOrderToday"] = orderService.findOrderInfo()
                                            .FindAll(u => u.orderDate == DateTime.Now.Date)
                                            .Sum(u => u.quantity);

            TempData["totalOrder"] = orderService.findOrderInfo()
                                    .Sum(u=> u.quantity);

            @TempData["totalOrderMonthCost"]= orderService.findOrderInfo()
               .FindAll(u => u.orderDate.Month == DateTime.Now.Date.Month && u.orderDate.Year == DateTime.Now.Date.Year)
               .Sum(u => u.quantity)* costperMeal;
        }

        [HttpPost]
        public ActionResult Order(int userID ,string name, DateTime orderDate, int quantity)
        {
            if (quantity > 0)
            {
                if (orderService.setOrderInfoById(userID, orderDate, quantity))
                {
                    TempData["orderSucc"] = "Order Place Succesfully";
                }
                else
                {
                    TempData["orderErr"] = "You Have Already Order Today . If you want to change please click edit " +
                                            " or delete to cancel your lunch";
                }
            }
            else
            {
                TempData["orderQErr"] = "You have to order atlest 1 luch";
            }
            return RedirectToAction("Order");
        }

        public ActionResult EditOrder(int id,DateTime date)
        {
            SignUp user = adminService.findAllInfoById(id);
            OrderViewModel orders = new OrderViewModel(
                orderService.findOrderInfoById(User.Identity.Name),
                orderService.findOrderInfo(),
                orderService.findOrderInfoByUserId(id).Find(u => u.orderDate == DateTime.Now.Date),
                orderService.getPerHeadMealCost()
                );
            SetAllCostingData();
            return View(orders);
        }

        [HttpPost]
        public ActionResult EditOrder(int userID, string name, DateTime orderDate, int quantity)
        {
            if (quantity > 0)
            {
                if (orderService.updateOrderInfoById(userID, orderDate, quantity))
                {
                    TempData["orderSucc"] = "Order Updated Succesfully";
                }
                else
                {
                    TempData["orderErr"] = "You Have Already Order Today . If you want to change please click edit " +
                                            " or delete to cancel your lunch";
                }
            }
            else
            {
                TempData["orderQErr"] = "You have to order atlest 1 luch";
            }
            return RedirectToAction("Order");
        }

        
        public ActionResult DeleteOrder(int id, DateTime date)
        {
            if(orderService.deleteOrderByID(id, date))
            {
                TempData["orderSucc"] = "Order Deleted Succesfully";
            }
            else
            {
                TempData["orderErr"] = "Failed To Delete";
            }
            return RedirectToAction("Order");
        }


        public ActionResult LunchOrderHistory()
        {
            List<Order> orderList = new List<Order>();

            if (isOnlyUser())
            {
                orderList = orderService.findOrderInfo().FindAll(u => u.userID == adminService.findAllInfoByEmail(User.Identity.Name).id);
            }
            else
            {
                orderList = orderService.findOrderInfo();
            }

            OrderViewModel data = new OrderViewModel(

                   orderList,
                    new Order(adminService.findAllInfoByEmail(User.Identity.Name).id, DateTime.Today.Date, DateTime.Today.Date)
                );

            int costperMeal = orderService.getPerMealCost().Find(x => x.modifyDate < DateTime.Now).costPerMeal;
            @TempData["IndividualCost"] = orderList.Sum(u => u.quantity) * costperMeal;
            @TempData["IndividualMeal"] = orderList.Sum(u => u.quantity);
            return View(data);
        }

        [HttpPost]
        public ActionResult LunchOrderHistory(int userID, DateTime date1, DateTime date2)
        {
            List<Order> orderList = new List<Order>();
            OrderViewModel data = new OrderViewModel(
                   orderList = orderService.SearchHistory(userID,date1,date2),
                    new Order(userID, date1, date2)
                );

            int costperMeal = orderService.getPerMealCost().Find(x => x.modifyDate < DateTime.Now).costPerMeal;
            @TempData["IndividualCost"] = orderList.Sum(u => u.quantity) * costperMeal;
            @TempData["IndividualMeal"] = orderList.Sum(u => u.quantity);

            return View(data);
           
        }

    }
}
