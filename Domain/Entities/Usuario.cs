using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayAttribute(Name = "Nome")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")] 
        public string Nome { get; set; }


        [DisplayAttribute(Name = "e-Mail")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [EmailAddress(ErrorMessage = "e-Mail inválido")]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [DisplayAttribute(Name = "Senha")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string Senha { get; set; }

    }
}
