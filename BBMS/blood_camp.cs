//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BBMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class blood_camp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public blood_camp()
        {
            this.shifts = new HashSet<shift>();
        }
    
        public int blood_camp_id { get; set; }
        public int hospital_id { get; set; }
        public string driver_name { get; set; }
    
        public virtual hospital hospital { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<shift> shifts { get; set; }
    }
}