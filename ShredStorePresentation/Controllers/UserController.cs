using Contracts.Request;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStorePresentation.Services.ProductServices;
using ShredStorePresentation.Services.UserService;

namespace ShredStorePresentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserHttpService _userHttpService;
        private readonly IProductHttpService _productHttpService;
        public const string SessionKeyName = "_Name";
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";
        public const string SessionKeyEmail = "_Email";
        public UserController(IUserHttpService userHttpService, IProductHttpService productHttpService)
        {
            _userHttpService = userHttpService;
            _productHttpService = productHttpService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserRequest userLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loggedUser = await _userHttpService.Login(userLogin);
                    if (loggedUser != null && loggedUser.Id != 0)
                    {
                        SetSessionInfo(loggedUser);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Wrong Password / User does not exists";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    //_utilityClass.GetLog().Error(ex, "Exception caught at Login action in UserOperationsController.");
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.SetString(SessionKeyName, "");
            HttpContext.Session.SetInt32(SessionKeyId, 0);
            HttpContext.Session.SetString(SessionKeyRole, "");
            return RedirectToAction(nameof(Index), "Home");
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateUserRequest userData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userHttpService.Create(userData);
                    //await _emailSender.SendEmailAsync(userData.Email, 2);
                    return RedirectToAction(nameof(Login));


                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error occurred while registering.";
                    //_utilityClass.GetLog().Error(ex, "Exception caught at CreateAccount action in UserOperationsController.");
                    return View();

                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserDetails(int Id, CancellationToken token)
        {

            var selected = await _userHttpService.GetById(Id);
            ViewBag.UserProducts = await _productHttpService.GetAllByUserId(Id, token);
            return View(selected);
        }
        [HttpGet]
        public IActionResult EditAccount()
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditAccount(UpdateUserRequest userEdit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userEdit.Id <= 0)
                    {
                        ViewBag.Message = "User doesn't exitst. Please send this to admin.";
                        return View();
                    }
                    await _userHttpService.EditUser(userEdit);
                    return RedirectToAction("SetSessionInfo", "ShredStore", userEdit);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    //_utilityClass.GetLog().Error(ex, "Exception caught at EditAccount action in UserOperationsController.");
                    return View();

                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginUserRequest userLogin)
        {
            userLogin.Email = HttpContext.Session.GetString("_Email");
            if (userLogin.Email != null && userLogin.Password != null)
            {
                try
                {
                    var loggedUser = await _userHttpService.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32("_Id").Value;
                        if (loggedUser.Id == sessionId)
                        {
                            ResetPasswordUserRequest request = new ResetPasswordUserRequest
                            {
                                Password = userLogin.Password,
                                Email = userLogin.Email
                            };
                            return View("NewPassword", request);
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    //_utilityClass.GetLog().Error(ex, "Exception caught at ChangePassword action in UserOperationsController.");
                    return View();
                    throw;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(ResetPasswordUserRequest request)
        {
            try
            {
                bool ok = await _userHttpService.ResetUserPassword(request);
                if (ok)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Invalid Email.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error has occurred.";
                //_utilityClass.GetLog().Error(ex, "Exception caught at NewPassword action in UserOperationsController.");
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAccount() => View();
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(LoginUserRequest userLogin)
        {
            userLogin.Email = HttpContext.Session.GetString("_Email");
            try
            {
                if (userLogin.Email != null && userLogin.Password != null)
                {
                    var loggedUser = await _userHttpService.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32("_Id").Value;
                        if (loggedUser.Id == sessionId)
                        {
                            await _userHttpService.Delete(sessionId);
                            return RedirectToAction(nameof(Logout));
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {

                ViewBag.Message = "An error has occurred.";
                //_utilityClass.GetLog().Error(ex, "Exception caught at DeleteAccount action in UserOperationsController.");
                return View();
            }

            return View();
        }
        [HttpGet]
        public IActionResult NoAccount()
        {
            ViewBag.Message = "Please create an account to add to cart.";
            return View("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            IEnumerable<UserResponse> users = await _userHttpService.GetAll();
            return View(users);
        }
        
        [HttpGet]
        public async Task<IActionResult> AdminProducts(CancellationToken token)
        {
            IEnumerable<ProductResponse> users = await _productHttpService.GetAll(token);
            return View(users);
        }

        //[HttpGet]
        //public async Task<IActionResult> ForgotPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(string Email)
        //{
        //    if (_utilityClass.IsEmailValid(Email))
        //    {
        //        bool res = await _user.CheckEmail(Email);
        //        if (res)
        //        {
        //            try
        //            {
        //                var randomUser = _userFactory.CreateUser(Email);
        //                bool ok = await _user.ResetUserPassword(randomUser);
        //                if (!ok)
        //                {
        //                    ViewBag.Message = "Invalid Email.";
        //                    return View();

        //                }
        //                await _emailSender.SendEmailAsync(Email, 1, randomUser);
        //                return RedirectToAction("Index", "ShredStore");
        //            }
        //            catch (Exception ex)
        //            {
        //                ViewBag.Message = "An error has occurred.";
        //                _utilityClass.GetLog().Error(ex, "Exception caught at PasswordReset action in UserOperationsController.");
        //                return View();
        //                throw;
        //            }

        //        }
        //        else
        //        {
        //            ViewBag.Message = "Invalid Email.";
        //            return View();
        //        }
        //    }
        //    return View();
        //}
        private List<string> GetRoles()
        {
            List<string> Role = ["Shop", "Customer"];
            return Role;
        }
        private void SetSessionInfo(UserResponse user)
        {
            HttpContext.Session.SetInt32(SessionKeyId, user.Id);
            HttpContext.Session.SetString(SessionKeyName, user.Name);
            HttpContext.Session.SetString(SessionKeyEmail, user.Email);
            HttpContext.Session.SetString(SessionKeyRole, user.Role);
        }
        
    }
}
