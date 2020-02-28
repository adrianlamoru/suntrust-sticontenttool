//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace st1001.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offer
    {
        public Offer()
        {
            this.LayoutDetails = new HashSet<LayoutDetail>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string ContentIDs { get; set; }
        public bool HasProject { get; set; }
        public string CreateByExternal { get; set; }
        public bool Deleted { get; set; }
        public int OfferType { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ICollection<LayoutDetail> LayoutDetails { get; set; }
    }
}
