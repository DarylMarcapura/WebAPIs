using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class AutorDTO
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<LibroDTO> Books { get; set; }
    }
}
