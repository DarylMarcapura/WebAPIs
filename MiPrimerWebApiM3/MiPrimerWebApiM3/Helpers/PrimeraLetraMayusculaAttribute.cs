using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Helpers
{
    //validacion por atributo
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        //sobreescribir un metodo
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe de ser en mayuscula");
            }
            return ValidationResult.Success;
            //return base.IsValid(value, validationContext);
        }
    }
}
