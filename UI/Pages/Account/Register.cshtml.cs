using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UsuarioViewModel UsuarioViewModel { get; set; }

        public string Mensagem { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UsuarioViewModel.Senha != UsuarioViewModel.ConfirmaSenha)
            {
                ViewData["erro"] = true;
                Mensagem = "Senhas não conferem";
                return Page();
            }

            var userExist = _context.Usuarios.Any(x => x.Email.ToLower() == UsuarioViewModel.Email.ToLower() || x.Nome.ToLower() == UsuarioViewModel.Nome.ToLower());
            if (userExist)
            {
                ViewData["erro"] = true;
                Mensagem = "Já existe este e-mail e/ou usuário cadastrado no sistema";
                return Page();
            }

            try
            {
                Usuario usu = new();
                usu.Email = UsuarioViewModel.Email;
                usu.Nome = UsuarioViewModel.Nome;
                usu.Senha = UsuarioViewModel.Senha;
                _context.Usuarios.Add(usu);
                await _context.SaveChangesAsync();

                ViewData["sucesso"] = true;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["erro"] = true;
                Mensagem = ex.Message;
                return Page();
            }
        }
    }
}
