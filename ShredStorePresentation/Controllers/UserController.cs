using Contracts.Request;
using Contracts.Response.JwtResponses;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStorePresentation.Extensions;
using ShredStorePresentation.Extensions.Cache;
using ShredStorePresentation.Services.JtwServices;
using ShredStorePresentation.Services.ProductServices;
using ShredStorePresentation.Services.UserService;


namespace ShredStorePresentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserHttpService _userHttpService;
        private readonly IProductHttpService _productHttpService;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtGenerationService _jwtService;
        private const int cookieExpireTime = 15;
        private readonly string ErrorMessage = "An error has occurred.";
        public UserController(IUserHttpService userHttpService, IProductHttpService productHttpService, ILogger<UserController> logger, IJwtGenerationService jwtService)
        {
            _userHttpService = userHttpService;
            _productHttpService = productHttpService;
            _logger = logger;
            _jwtService = jwtService;
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
                        JwtGenerateResponse token = await _jwtService.GenerateToken(loggedUser);

                        SetSessionInfo(loggedUser, token);

                        _logger.LogInformation(LogMessages.LogLoginMessage(), [loggedUser.Name, DateTime.Now.ToString(), loggedUser.Id]);
                        return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
                    }

                    ViewBag.Message = "Wrong Password / User does not exists";
                    _logger.LogInformation(LogMessages.LogFailedLoginMessage(), [DateTime.Now.ToString()]);
                    return View();

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
        public async Task<IActionResult> UserDetails(int Id)
        {

            string? token = Request.Cookies[Constants.TokenName];
            if (token is not null)
            {
                var selected = await _userHttpService.GetById(Id, token);

                ViewBag.UserProducts = Enumerable.Empty<ProductResponse>();

                if (selected.Role != Role.Customer)
                    ViewBag.UserProducts = await _productHttpService.GetAllByUserId(Id, token);

                return View(selected);
            }
            return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());

        }
        [HttpGet]
        public async Task<IActionResult> EditAccount(int id)
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);

            string? token = Request.Cookies[Constants.TokenName];

            if (token is not null)
            {
                var user = await _userHttpService.GetById(id, token);
                return View(user.MapToUpdateUserRequest());
            }
            return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
        }
        [HttpPost]
        public async Task<IActionResult> EditAccount(UpdateUserRequest userEdit)
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);

            try
            {
                string? token = Request.Cookies[Constants.TokenName];

                if (token is not null)
                {
                    if (userEdit.Id <= 0)
                    {
                        ViewBag.Message = "User doesn't exitst. Please send this to admin.";
                        return View();
                    }
                    UserResponse userResponse = await _userHttpService.EditUser(userEdit, token);

                    JwtGenerateResponse newToken = await _jwtService.GenerateToken(userResponse);

                    SetSessionInfo(userResponse, newToken);

                    return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
                }
                throw new UnauthorizedAccessException();
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
        public async Task<IActionResult> ChangePassword(ResetPasswordUserRequest request)
        {
            string? token = Request.Cookies[Constants.TokenName];
            try
            {
                if (token is not null)
                {
                    request.Email = HttpContext.Session.GetString(SessionKeys.GetSessionKeyEmail());
                    if (request.Email is not null && request.Password is not null)
                    {
                        LoginUserRequest loginRequest = new LoginUserRequest
                        {
                            Email = request.Email,
                            Password = request.Password
                        };
                        var loggedUser = await _userHttpService.Login(loginRequest);

                        if (loggedUser != null)
                        {
                            int sessionId = HttpContext.Session.GetInt32(SessionKeys.GetSessionKeyId()).Value;
                            if (loggedUser.Id == sessionId)
                            {
                                bool ok = await _userHttpService.ResetUserPassword(request, token);

                                if (ok)
                                    return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());

                                ViewBag.Message = "Invalid Email.";
                                return View();
                            }
                        }
                        return View();
                    }
                }
                throw new UnauthorizedAccessException();
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
                string? token = Request.Cookies[Constants.TokenName];

                if (token is not null)
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
                                await _userHttpService.Delete(sessionId, token);
                                return RedirectToAction(nameof(Logout));
                            }
                        }
                        else
                        {
                            return View();
                        }
                    }
                }
                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {

                ViewBag.Message = ErrorMessage;
                _logger.LogError(ex, LogMessages.LogErrorMessage(), [ControllerExtensions.ControllerName<HomeController>(), ex.Message, DateTime.Now.ToString()]);
                return View();
            }
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
            string? token = Request.Cookies[Constants.TokenName];

            if (token is not null)
            {
                IEnumerable<UserResponse> users = await _userHttpService.GetAll(token);
                return View(users);
            }
            return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());

        }

        [HttpGet]
        public async Task<IActionResult> AdminProducts()
        {
            string? token = Request.Cookies[Constants.TokenName];

            if (token is not null)
            {
                IEnumerable<ProductResponse> users = await _productHttpService.GetAll();
                return View(users);
            }
            return RedirectToAction(ControllerExtensions.IndexActionName(), ControllerExtensions.ControllerName<HomeController>());
            
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
        private void SetSessionInfo(UserResponse user, JwtGenerateResponse response)
        {
            HttpContext.Response.Cookies.Append("token", response.Token, new CookieOptions { Expires = DateTime.Now.AddMinutes(cookieExpireTime) });
            HttpContext.Session.SetInt32(SessionKeys.GetSessionKeyId(), user.Id);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyName(), user.Name);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyEmail(), user.Email);
            HttpContext.Session.SetString(SessionKeys.GetSessionKeyRole(), user.Role);
        }

       
    }
}
