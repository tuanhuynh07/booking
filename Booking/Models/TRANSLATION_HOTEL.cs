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
    
    public partial class TRANSLATION_HOTEL
    {
        public decimal? ID { get; set; }
        public decimal LANGUAGE_ID { get; set; }
        public string NAME { get; set; }
        public decimal HOTEL_ID { get; set; }
        public string BRIEF { get; set; }
        public string ADDRESS { get; set; }
        public string DESCRIPTION { get; set; }
    
        public virtual HOTEL HOTEL { get; set; }
        public virtual LANGUAGE LANGUAGE { get; set; }
    }
}
