namespace Domain.Entities
{
    public class Carrinho
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ProdutoId { get; set; }
        public Usuario? Usuario { get; set; }
        public Produto? Produto { get; set; }
    }
}
