using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TransportesMVC.Models.ViewModels
{
    public class CamionDTO
    {
        public int id_Camion { get; set; }
        [Required]
        [Display(Name = "Matricula")]
        public string matricula { get; set; }
        [Required]
        [Display(Name = "Tipo Camión")]
        public string tipo_Camion { get; set; }
        [Required]
        [Display(Name = "Marca")]
        public string marca { get; set; }
        [Display(Name = "Modelo")]
        public string modelo { get; set; }
        [Required]
        [Display(Name = "Capacidad")]
        public double capacidad { get; set; }
        [Display(Name = "Kilometraje")]
        public double kilometraje { get; set; }
        [DataType(DataType.ImageUrl)]
        public string url_Foto { get; set; }
        public bool disponibilidad { get; set; }
        public Nullable<int> Chofer_ID { get; set; }
    }
}