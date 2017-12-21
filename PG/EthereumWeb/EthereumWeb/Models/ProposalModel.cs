using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class ProposalModel
    {
        [Display (Name ="Código de propuesta")]
        public string ContracEthereumProposal { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display (Name ="Nombre de Propuesta")]
        public string ProposalName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display (Name ="Opción de Votos")]
        public EnumSecurityType SecurityType { get; set; } // Publico / Privado

        [Required(ErrorMessage = "Campo requerido")]
        public EnumQuestionType QuestionType { get; set; } //Referendo 
        public int QuestionTypeText { get; set; } //Referendo 


        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display (Name ="Fecha de Inicio")]
        public DateTime InitialDate { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display (Name ="Fecha de Final")]
        public DateTime FinalDate { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display (Name ="Permite de Busqueda")]
        public int AdvancedSearch { get; set; } //Permite ver avances

        [Required(ErrorMessage = "Campo requerido")]
        [Display (Name ="Descripción")]
        public string Description { get; set; }

        public List<OptionModel> Options { get; set; }

        [Display (Name ="Ya voto")]
        public bool IsVoted { get; set; }

        [Display (Name ="Cantidad mínima seleccionada")]
        public int MinimumQuantitySelected { get; set; }

        [Display(Name = "Cantidad máxima seleccionada")]
        public int MaximumQuantitySelected { get; set; }

        public string UserCreator { get; set; }

        public bool IsMine { get; set; }

        public IList<string> SelectedOptions { get; set; }
    }
}