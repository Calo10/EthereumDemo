using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class ModifyUserModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public string UserLastName { get; set; }
        public int Profile { get; set; }
    }
}