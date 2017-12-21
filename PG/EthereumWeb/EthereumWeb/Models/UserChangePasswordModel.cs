using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class UserChangePasswordModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(20, ErrorMessage = "La contraseña debe tener de 5 a 20 caracteres", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(20, ErrorMessage = "La contraseña debe tener de 5 a 20 caracteres", MinimumLength = 5)]
        
        public string PasswordValidated { get; set; }

    }
}