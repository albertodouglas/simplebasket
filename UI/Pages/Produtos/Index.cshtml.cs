using Domain;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Identity;
using UI.Helpers;

namespace UI.Pages.Produtos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostRecords(DataTableAjaxPostModel model)
        {
            try
            {
                var produtos = _context.Produtos.Select(x => x);
                int totalRecords = produtos.Count();

                if (!string.IsNullOrEmpty(model.search.value))
                {
                    var busca = model.search.value.ToLower();
                    produtos = produtos.Where(x => x.Nome.ToLower().Contains(busca));
                }

                if (model.order != null)
                {
                    var sortBy = model.columns[model.order[0].column].data;
                    var sortDir = model.order[0].dir.ToLower();
                    produtos = produtos.OrderBy(sortBy + " " + sortDir);
                }
                else
                {
                    produtos = produtos.OrderBy(x => x.Nome);
                }

                var result = await produtos
                    .Skip(model.start)
                    .Take(model.length)
                    .ToArrayAsync();

                return new JsonResult(new
                {
                    draw = model.draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = result.Count() != totalRecords ? produtos.Count() : totalRecords,
                    data = result
                });

            }
            catch
            {
                return new JsonResult(new
                {
                    model.draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = ""
                });
            }
        }

        public async Task<JsonResult> OnPostListarCarrinho()
        {
            try
            {
                int userId = int.Parse(User.Identity.GetId());
                var carrinho = _context.Carrinho
                    .Include(x => x.Produto)
                    .Where(x => x.UsuarioId == userId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Produto.Nome,
                        x.Produto.Preco,
                        Quantidade = 1,
                    });
                return new JsonResult(new { result = carrinho });

            }
            catch(Exception ex) 
            {
                return new JsonResult(new
                {
                    result = $"Erro: {ex.Message}"
                });
            }
        }

        public async Task<JsonResult> OnPostAdicionarCarrinho(int produtoId)
        {
            try
            {
                int userId = int.Parse(User.Identity.GetId());

                var jaAdicionado = _context.Carrinho.Any(x => x.ProdutoId == produtoId);
                if (jaAdicionado)
                {
                    return new JsonResult(new { result = "Produto já adicionado ao carrinho" });
                }

                Carrinho c = new();
                c.UsuarioId = userId;
                c.ProdutoId = produtoId;
                _context.Carrinho.Add(c);
                await _context.SaveChangesAsync();
                return new JsonResult(new { result = "OK" });
            }
            catch(Exception ex)
            {
                return new JsonResult(new { result = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostExcluirCarrinho(int idCarrinho)
        {
            try
            {
                var c = await _context.Carrinho.FindAsync(idCarrinho);
                if (c != null)
                {
                    _context.Carrinho.Remove(c);
                    await _context.SaveChangesAsync();
                }
                return new JsonResult(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { result = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostComprar()
        {
            try
            {
                int userId = int.Parse(User.Identity.GetId());

                Pedido pedido = new Pedido();
                pedido.DataPedido = DateTime.Now;
                pedido.UsuarioId = userId;
                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                var carrinho = _context.Carrinho.Where(x=>x.UsuarioId == userId).ToList();
                foreach (var c in carrinho)
                {
                    ItemPedido ip = new ItemPedido();
                    ip.PedidoId = pedido.Id;
                    ip.ProdutoId = c.ProdutoId;
                    ip.Quantidade = 1;
                    _context.ItensPedidos.Add(ip);
                }

                _context.Carrinho.RemoveRange(carrinho);

                await _context.SaveChangesAsync();

                return new JsonResult(new { result = "OK" });
            }
            catch(Exception ex)
            {
                return new JsonResult(new { result = $"Erro: {ex.Message}" });
            }
        }
    }

    public class CarrinhoUser
    {
        public int UserId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
    }
}
