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
    
    public partial class TRANSLATION_ROOM
    {
        public decimal? ID { get; set; }
        public decimal LANGUAGE_ID { get; set; }
        public string ROOM_NAME { get; set; }
        public decimal ROOM_ID { get; set; }
    
        public virtual LANGUAGE LANGUAGE { get; set; }
        public virtual ROOM ROOM { get; set; }
    }
}
