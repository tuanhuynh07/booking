﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class HOTEL
    {
        public HOTEL()
        {
            this.ROOMs = new HashSet<ROOM>();
            this.TRANSLATION_HOTEL = new List<TRANSLATION_HOTEL>();
        }
    
        public decimal? HOTEL_ID { get; set; }
        [Required]
        [DisplayName("Tên Khách Sạn")]
        public string HOTEL_NAME { get; set; }
        [Required]
        [DisplayName("Địa chỉ")]
        public string HOTEL_ADDRESS { get; set; }
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> HOTEL_CREATEDATE { get; set; }
        [DisplayName("Mô tả ngắn")]
        public string HOTEL_DESCRIPTION { get; set; }
        [DisplayName("Mô tả ngắn")]
        public string HOTEL_BRIEF { get; set; }
        public Nullable<int> NUMBER_RATING { get; set; }
        public Nullable<int> TOTAL_RATING { get; set; }
        public Nullable<int> TOTAL_ROOM { get; set; }
        public Nullable<int> PRICE_GENERAL { get; set; }
        public Nullable<int> HOTEL_STAR { get; set; }
        public Nullable<short> HOTEL_LEVEL { get; set; }
        public Nullable<bool> HOTEL_STATUS { get; set; }
        public string HOTEL_IMAGE { get; set; }
        public string MEDIA_ARRAY { get; set; }
        public string HOTEL_CHECKIN { get; set; }
        public string HOTEL_MAP { get; set; }
        public string HOTEL_ALIAS { get; set; }
    
        public virtual ICollection<ROOM> ROOMs { get; set; }
        public virtual List<TRANSLATION_HOTEL> TRANSLATION_HOTEL { get; set; }
    }
}
