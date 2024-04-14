using Contracts.Request;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Extensions.Cache;
using ShredStorePresentation.Services.ProductServices;
using ShredStorePresentation.Services.UserService;

namespace ShredStorePresentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserHttpService _userHttpService;
        private readonly IProductHttpService _productHttpService;
        private readonly ILogger<UserController> _logger;
        private readonly string ErrorMessage = "An error has occurred.";
        public UserController(IUserHttpService userHttpService, IProductHttpService productHttpService, ILogger<UserController> logger)
        {
            _userHttpService = userHttpService;
            _productHttpService = productHttpService;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login() => View();
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
                        _logger.LogInformation(LogMessages.LogLoginMessage(), [loggedUser.Name, DateTime.Now.ToString(), loggedUser.Id]);
                        return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
                    }
                    else
                    {
                        ViewBag.Message = "Wrong Password / User does not exists";
                        _logger.LogInformation(LogMessages.LogFailedLoginMessage(), [DateTime.Now.ToString()]);
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ErrorMessage;
                   _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            SetSessionInfo();
            return RedirectToAction(nameof(Index), ControllerExtensions.ControllerName<HomeController>());
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
                   _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
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
        public async Task<IActionResult> EditAccount(int id)
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);
            var user = await _userHttpService.GetById(id);
            return View(user.MapToUpdateUserRequest());
        }
        [HttpPost]
        public async Task<IActionResult> EditAccount(UpdateUserRequest userEdit)
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);

            try
            {
                if (userEdit.Id <= 0)
                {
                    ViewBag.Message = "User doesn't exitst. Please send this to admin.";
                    return View();
                }
                await _userHttpService.EditUser(userEdit);
                SetSessionInfo(await _userHttpService.GetById(userEdit.Id));
                return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
            }
            catch (Exception ex)
            {
                ViewBag.Message = ErrorMessage;
               _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
                return View();

            }
        }
        [HttpGet]
        public IActionResult ChangePassword() => View();        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginUserRequest userLogin)
        {
            userLogin.Email = HttpContext.Session.GetString(SessionKeys.GetSessionKeyEmail());
            if (userLogin.Email is not null && userLogin.Password is not null)
            {
                try
                {
                    var loggedUser = await _userHttpService.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32(SessionKeys.GetSessionKeyId()).Value;
                        if (loggedUser.Id == sessionId)
                        {
                            ResetPasswordUserRequest request = new ResetPasswordUserRequest
                            {
                                Password = userLogin.Password,
                                Email = userLogin.Email
                            };
                            return View(nameof(NewPassword), request);
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ErrorMessage;
                   _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
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
                    return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
                }
                else
                {
                    ViewBag.Message = "Invalid Email.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ErrorMessage;
               _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
                return View();
            }
        }
        [HttpGet]
        public IActionResult DeleteAccount() => View();
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(LoginUserRequest userLogin)
        {
            
            try
            {
                userLogin.Email = HttpContext.Session.GetString(SessionKeys.GetSessionKeyEmail());
                if (userLogin.Email is not null && userLogin.Password is not null)
                {
                    var loggedUser = await _userHttpService.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32(SessionKeys.GetSessionKeyId()).Value;
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

                ViewBag.Message = ErrorMessage;
               _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
                return View();
            }

            return View();
        }
        [HttpGet]
        public IActionResult NoAccount()
        {
            ViewBag.Message = "Please create an account to add to cart.";
            return View(nameof(Login));
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
        private List<string> GetRoles()
        {
            List<string> Role = ["Shop", "Customer"];
            return Role;
        }
        private void SetSessionInfo()
        {
            _logger.LogInformation(LogMessages.LogLogoutMessage(), [HttpContext.Session.GetString(SessionKeys.GetSessionKeyName()),
                HttpContext.Session.GetInt32(SessionKeys.GetSessionKeyId()) ,DateTime.Now.ToString()]);

            HttpContext.Session.SetString(SessionKeys.GetSessionKeyName(), "");
            HttpContext.Session.SetInt32(SessionKeys.GetSessionKeyId(), 0);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyEmail(), "");
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyRole(), "");
        }
        private void SetSessionInfo(UserResponse user)
        {
            HttpContext.Session.SetInt32(SessionKeys.GetSessionKeyId(), user.Id);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyName(), user.Name);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyEmail(), user.Email);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyRole(), user.Role);
        }
        

    }
}
