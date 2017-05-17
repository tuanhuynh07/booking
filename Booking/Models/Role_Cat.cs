using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models
{
    public class Role_Cat
    {
        public Role_Cat()
        {
            this.ROLEs = new HashSet<ROLE>();
            this.CATEGORies = new HashSet<CATEGORY>();
        }
        public decimal ROLE_ID { get; set; }
        public decimal CATEGORY_ID { get; set; }
        public virtual ICollection<ROLE> ROLEs { get; set; }
        public virtual ICollection<CATEGORY> CATEGORies { get; set; }
    }
}