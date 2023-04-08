using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Produtos
{

    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;

        public EditModel(AppDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment = null)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public string Mensagem { get; set; }

        [BindProperty]
        public Produto Produto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Produto = _context.Produtos.First(x => x.Id == id);
                return Page();
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro: {ex.Message}";
                ViewData["erro"] = true;
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync(int id, IFormFile? fotoProduto)
        {
            try
            {
                var produtoDb = _context.Produtos.First(x => x.Id == id);
                produtoDb.Preco = Produto.Preco;
                produtoDb.Descricao = Produto.Descricao;
                produtoDb.Nome = Produto.Nome;

                if (fotoProduto != null)
                {
                    var pathArquivo = SalvarArquivo(fotoProduto, produtoDb.PathImagem);
                    if (pathArquivo != "")
                        produtoDb.PathImagem = pathArquivo;
                }

                _context.Produtos.Update(produtoDb);
                await _context.SaveChangesAsync();
                ViewData["sucesso"] = true;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["erro"] = true;
                Mensagem = $"Erro: {ex.Message}";
                return Page();
            }
        }

        private string SalvarArquivo(IFormFile file, string pathAtual)
        {
            string caminho_WebRoot = _appEnvironment.WebRootPath;
            string path, url;
            path = caminho_WebRoot + "\\ImagensProdutos";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }

            path += $"\\{file.FileName}";
            url = $"\\ImagensProdutos\\{file.FileName}";

            if (url == pathAtual)
                return "";

            //copia o arquivo para o local de destino original
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                return url;
            }
        }
    }
}
