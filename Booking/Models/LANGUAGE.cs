//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Booking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LANGUAGE
    {
        public LANGUAGE()
        {
            this.TRANSLATION_ARTICLE = new HashSet<TRANSLATION_ARTICLE>();
            this.TRANSLATION_CATEGORY = new HashSet<TRANSLATION_CATEGORY>();
            this.TRANSLATION_CUSTOMER = new HashSet<TRANSLATION_CUSTOMER>();
            this.TRANSLATION_HOTEL = new HashSet<TRANSLATION_HOTEL>();
            this.TRANSLATIONs = new HashSet<TRANSLATION>();
        }
    
        public decimal LANGUAGE_ID { get; set; }
        public string LANGUAGE_NAME { get; set; }
        public string LANGUAGE_CODE { get; set; }
    
        public virtual ICollection<TRANSLATION_ARTICLE> TRANSLATION_ARTICLE { get; set; }
        public virtual ICollection<TRANSLATION_CATEGORY> TRANSLATION_CATEGORY { get; set; }
        public virtual ICollection<TRANSLATION_CUSTOMER> TRANSLATION_CUSTOMER { get; set; }
        public virtual ICollection<TRANSLATION_HOTEL> TRANSLATION_HOTEL { get; set; }
        public virtual ICollection<TRANSLATION> TRANSLATIONs { get; set; }
        public virtual TRANSLATION_ROOM TRANSLATION_ROOM { get; set; }
    }
}
