using MiPrimerWebApiM3.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Entities
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        ////no se debe de pasar de los 10 caracteres
        //[StringLength(10,ErrorMessage ="El campo Nonbre debe de tener {1} caracteres o menos")]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        //[Range(18,20, ErrorMessage ="la edad debe de ser entre 18 y 20")]
        //public int Edad { get; set; }
        //[CreditCard]
        //public string CreditCard { get; set; }
        //[Url]
        //public string Url { get; set; }
        public List<Libro> Libros { get; set; }

        //validaciones por modelo, se limita al modelo es dificil reutilizar
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraletra = Nombre[0].ToString();

                if (primeraletra != primeraletra.ToUpper())
                {
                    yield return new ValidationResult("la primera letra debe de ser en mayuscula", new string[] {
                        nameof(Nombre)
                    });
                }
            }
        }
    }
}
