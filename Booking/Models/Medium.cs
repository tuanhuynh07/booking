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
    
    public partial class Medium
    {
        public decimal MEDIA_ID { get; set; }
        public string MEDIA_TITLE { get; set; }
        public string MEDIA_IMAGE_PATH { get; set; }
        public string MEDIA_PATH_VIDEO { get; set; }
        public string MEDIA_EMBED_VIDEO { get; set; }
        public Nullable<int> MEDIA_ORDER { get; set; }
        public Nullable<System.DateTime> MEDIA_CREATEDATE { get; set; }
        public string MEDIA_CREATEBY { get; set; }
        public Nullable<bool> MEDIA_DISABLE { get; set; }
        public string MEDIA_ALIAS { get; set; }
        public Nullable<decimal> CATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_ALIAS { get; set; }
    }
}