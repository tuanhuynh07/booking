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
        public string CUSTOMER_FULLNAME { get; set; }
        public decimal ROOM_ID { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public System.DateTime DATE_BEGIN { get; set; }
        public Nullable<System.DateTime> DATE_FINISH { get; set; }
        public Nullable<short> NUMBER_NIGHT { get; set; }
        public Nullable<System.DateTime> DATE_BOOKING { get; set; }
        public Nullable<bool> PAY_STATUS { get; set; }
        public string PAY_STATUS_SCRIPT { get; set; }
        public string PAY_TYPE { get; set; }
        public string PAY_INFORMATION { get; set; }
        public string PAY_CODE { get; set; }
        public Nullable<System.DateTime> PAY_DATE { get; set; }
        public Nullable<System.DateTime> PAY_LIMIT { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual ROOM ROOM { get; set; }
    }
}
