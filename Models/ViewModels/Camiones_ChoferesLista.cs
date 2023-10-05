using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportesMVC.Models.ViewModels
{
    public class Camiones_ChoferesLista
    {
        public int id_Camion { get; set; }
        public string matricula { get; set; }
        public string tipo_Camion { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public double capacidad { get; set; }
        public double kilometraje { get; set; }
        public string url_Foto { get; set; }
        public bool disponibilidad { get; set; }
        public Nullable<int> Chofer_ID { get; set; }
        public string Nombre_chofer { get; set; }
    }
}