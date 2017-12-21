using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class UserModel : HomeModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public string UserLastName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string RepitePassword { get; set; }

        public int Profile { get; set; }

        public bool IsProcessActive { get; set; }
        public EnumRegister IsFirstLogger { get; set; }
        public string Contract  { get; set; }

    }
}