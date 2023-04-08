using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Produto
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(50)]
        [Column(TypeName = "varchar(20)")]
        public string Nome { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "text")]
        public string Descricao { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Preco { get; set; }

        public DateTime DataCriacao { get; set; }

        [Display(Name = "Imagem")]
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string? PathImagem { get; set; }
    }
}
