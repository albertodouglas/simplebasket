using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Produtos
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Produto Produto { get; set; }

        public string Mensagem { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Produto = _context.Produtos.FirstOrDefault(x => x.Id == id);
            if (Produto == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == 0 || id == null)
                {
                    return NotFound();
                }

                Produto = await _context.Produtos.FindAsync(id);
                if (Produto != null)
                {
                    _context.Produtos.Remove(Produto);
                    await _context.SaveChangesAsync();
                }

                ViewData["sucesso"] = true;
                return Page();
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                ViewData["erro"] = true;
                return Page();
            }
        }
    }
}
