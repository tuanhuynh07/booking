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
    
    public partial class TRANSLATION_ROOM_TYPE
    {
        public decimal? ID { get; set; }
        public Nullable<decimal> ROOM_TYPE_ID { get; set; }
        public Nullable<decimal> LANGUAGE_ID { get; set; }
        [DisplayName("Loại Phòng")]
        [StringLength(200)]
        public string ROOM_TYPE_NAME { get; set; }
        public virtual LANGUAGE LANGUAGE { get; set; }
        public virtual ROOM_TYPE ROOM_TYPE { get; set; }
    }
}