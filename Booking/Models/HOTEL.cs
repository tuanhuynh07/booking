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
    
    public partial class HOTEL
    {
        public HOTEL()
        {
            this.TRANSLATION_HOTEL = new List<TRANSLATION_HOTEL>();
            this.BOOKING_ROOM_DETAIL = new HashSet<BOOKING_ROOM_DETAIL>();
            this.ROOMs = new HashSet<ROOM>();
        }
    
        public decimal? HOTEL_ID { get; set; }
        public string HOTEL_NAME { get; set; }
        public string HOTEL_ADDRESS { get; set; }
        public Nullable<System.DateTime> HOTEL_CREATEDATE { get; set; }
        public string HOTEL_DESCRIPTION { get; set; }
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
    
        public virtual List<TRANSLATION_HOTEL> TRANSLATION_HOTEL { get; set; }
        public virtual ICollection<BOOKING_ROOM_DETAIL> BOOKING_ROOM_DETAIL { get; set; }
        public virtual ICollection<ROOM> ROOMs { get; set; }
    }
}
