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
    
    public partial class LayoutDetail
    {
        public int ID { get; set; }
        public int OfferID { get; set; }
        public string ContentID { get; set; }
        public string Note { get; set; }
    
        public virtual Offer Offer { get; set; }
    }
}
