//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransportesMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Camiones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Camiones()
        {
            this.Rutas = new HashSet<Rutas>();
        }
    
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
    
        public virtual Choferes Choferes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rutas> Rutas { get; set; }
    }
}
