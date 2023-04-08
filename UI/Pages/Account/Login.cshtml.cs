using Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Domain;

namespace UI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        public void OnGet()
        {
        }

        public string Mensagem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["aviso"] = true;
                Mensagem = "Informe nome do usuário e senha!";
                return Page();
            }

            try
            {
                var user = _context.Usuarios.FirstOrDefault(user => user.Nome.ToLower() == LoginViewModel.Usuario.ToLower() && user.Senha == LoginViewModel.Senha);
                if (user == null)
                {
                    Mensagem = "Usuário e/ou senha inválidos!";
                    ViewData["erro"] = true;
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim("Email", user.Email),
                    new Claim("Nome", user.Nome),
                    new Claim("UserId", user.Id.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    RedirectUri = "/",
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Redirect("/Produtos/Index");
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro ao tentar fazer o login do usuário {LoginViewModel.Usuario}: {ex.Message}";
                ViewData["erro"] = true;
                return Page();
            }

        }
    }
}
