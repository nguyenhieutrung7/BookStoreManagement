//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SiteOwner
    {
        public int SiteOwnerID { get; set; }
        public string SiteOwnerName { get; set; }
        public string SiteOwnerAddress { get; set; }
        public string SiteOwnerEmail { get; set; }
        public string SiteOwnerPhoneNumber { get; set; }
        public int GenderID { get; set; }
        public int AccountID { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
