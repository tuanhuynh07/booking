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
    
    public partial class BOOKING_ROOM
    {
        public decimal CUSTOMER_ID { get; set; }
        public decimal ROOM_ID { get; set; }
        public System.DateTime DATE_BEGIN { get; set; }
        public Nullable<System.DateTime> DATE_FINISH { get; set; }
        public Nullable<short> NUMBER_NIGHT { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual ROOM ROOM { get; set; }
    }
}
