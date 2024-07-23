using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3raPracticaProgramada_OscarNaranjoZuniga.Models
{
    public class Comentarios
    {
        [Key]
        public int ComentarioId { get; set; }
        public int UsuarioId { get; set; }

        public required string Comentario { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        public required Usuario Usuario { get; set; } 
    }
}
