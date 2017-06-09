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
    
    public partial class CATEGORY
    {
        public CATEGORY()
        {
            this.ARTICLEs = new HashSet<ARTICLE>();
            this.ROLEs = new HashSet<ROLE>();
        }
    
        public decimal CATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public Nullable<short> CATEGORY_ORDER { get; set; }
        public Nullable<bool> CATEGORY_IS_SHOW_MENU { get; set; }
        public Nullable<bool> CATEGORY_IS_SHOW_FOOTER { get; set; }
        public string CATEGORY_LINK { get; set; }
        public string CATEGORY_ALIAS { get; set; }
        public Nullable<System.DateTime> CATEGORY_CREATEDATE { get; set; }
        public string CATEGORY_CREATEBY { get; set; }
        public Nullable<decimal> CATEGORY_PARENT_ID { get; set; }
        public string CATEGORY_IMAGE { get; set; }
        public Nullable<bool> CATEGORY_ISMEDIA { get; set; }
        public Nullable<short> CATEGORY_AREA_HOME { get; set; }
        public Nullable<decimal> NAME_TRANSLATION_ID { get; set; }
    
        public virtual ICollection<ARTICLE> ARTICLEs { get; set; }
        public virtual TRANSLATION_CATEGORY TRANSLATION_CATEGORY { get; set; }
        public virtual ICollection<ROLE> ROLEs { get; set; }
    }
}
