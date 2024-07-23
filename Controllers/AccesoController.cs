using _3raPracticaProgramada_OscarNaranjoZuniga.Models;
using _3raPracticaProgramada_OscarNaranjoZuniga.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace _3raPracticaProgramada_OscarNaranjoZuniga.Controllers
{
    public class AccesoController : Controller
    {
        //Con esta variable del nuestro contexto es la que se va a utilizar para realizar acciones con la DB
        private readonly ApplicationDbContext _appDbContext;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AccesoController(ApplicationDbContext AppDbContext)
        {
            _appDbContext = AppDbContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario Usuario)
        {

            Usuario.Contrasena = _passwordHasher.HashPassword(Usuario, Usuario.Contrasena);

            //Se envia a la db
            await _appDbContext.Usuarios.AddAsync(Usuario);
            await _appDbContext.SaveChangesAsync();

            // Si se crea el ususario el id seria diferente de 0, por lo cual se habria registrado
            if (Usuario.UsuarioId != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewData["Mensaje"] = "No sé pudo crear el usuario";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            //Esto es por si uno ya esta autenticado y el tiempo de la sesion no ha caducado, entonces no aparecera el login de nuevo:
            if (User.Identity!.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Listas");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginVM Usuario)
        {

            var usuarioRecibido = await _appDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Email == Usuario.Email);

            if (usuarioRecibido == null)
            {
                ViewData["Mensaje"] = "Error al encontrar el usuario o el usuario está inactivo";
                return View();
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(usuarioRecibido, usuarioRecibido.Contrasena, Usuario.Contrasena);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                ViewData["Mensaje"] = "Contraseña incorrecta";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioRecibido.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuarioRecibido.NombreUsuario),
                new Claim(ClaimTypes.Email, usuarioRecibido.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties { AllowRefresh = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}