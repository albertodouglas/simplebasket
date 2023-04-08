using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Produtos
{
    public class CreateModel : PageModel
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _appEnvironment;
        private readonly AppDbContext _appContext;

        public CreateModel(AppDbContext appContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment appEnvironment)
        {
            _appContext = appContext;
            _appEnvironment = appEnvironment;
        }

        [BindProperty]
        public ProdutoViewModel ProdutoViewModel { get; set; }

        public string Mensagem { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fotoProduto)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Produto prod = new();
                prod.DataCriacao = DateTime.Now;
                prod.Preco = ProdutoViewModel.Preco;
                prod.Descricao = ProdutoViewModel.Descricao;
                prod.Nome = ProdutoViewModel.Nome;

                if (fotoProduto != null)
                {
                    prod.PathImagem = SalvarArquivo(fotoProduto);
                }
                _appContext.Produtos.Add(prod);
                await _appContext.SaveChangesAsync();
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

        private string SalvarArquivo(IFormFile file)
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

            //copia o arquivo para o local de destino original
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                return url;
            }
        }
    }
}
