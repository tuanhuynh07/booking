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
    
    public partial class ACCOUNT
    {
        public decimal USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_FULL_NAME { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_VALID_ADMIN { get; set; }
        public Nullable<bool> USER_IS_ADMIN { get; set; }
        public Nullable<System.DateTime> USER_CREATEDATE { get; set; }
        public Nullable<decimal> USER_CREATEBY { get; set; }
        public Nullable<bool> USER_ALLOW_CATEGORY { get; set; }
        public Nullable<bool> USER_ALLOW_ARTICLE { get; set; }
        public Nullable<bool> USER_ALLOW_MEDIA { get; set; }
        public Nullable<bool> USER_ALLOW_USER { get; set; }
        public Nullable<decimal> ROLE_ID { get; set; }
        public Nullable<bool> USER_ACTIVED { get; set; }
    
        public virtual ROLE ROLE { get; set; }
    }
}