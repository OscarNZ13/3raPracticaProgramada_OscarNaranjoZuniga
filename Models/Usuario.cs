using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3raPracticaProgramada_OscarNaranjoZuniga.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public required string NombreUsuario { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public required string Contrasena { get; set; }

        public ICollection<Comentarios> Comentarios { get; set; } = new List<Comentarios>();
    }
}
