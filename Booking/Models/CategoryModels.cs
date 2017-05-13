using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models
{
    public class CategoryModels
    {
        private DB_BOOKINGEntities db = null;

        public List<CATEGORY> listAll()
        {
            return db.CATEGORies.OrderByDescending(m => m.CATEGORY_CREATEDATE).ToList();
        }
        public List<CATEGORY> listAll(int take)
        {
            return db.CATEGORies.OrderByDescending(m => m.CATEGORY_CREATEDATE).Take(take).ToList();
        }
        public List<CATEGORY> listCategoryParent()
        {
            return db.CATEGORies.Where(m => m.CATEGORY_PARENT_ID == null).OrderByDescending(m => m.CATEGORY_CREATEDATE).ToList();
        }
        public List<CATEGORY> listCategoryChild(decimal id)
        {
            return db.CATEGORies.Where(m => m.CATEGORY_PARENT_ID == id).OrderByDescending(m => m.CATEGORY_CREATEDATE).ToList();
        }
        public List<CATEGORY> listCategoryChild(decimal id, int take)
        {
            return db.CATEGORies.Where(m => m.CATEGORY_PARENT_ID == id).OrderByDescending(m => m.CATEGORY_CREATEDATE).Take(take).ToList();
        }
        public CATEGORY getCategoryById(decimal id)
        {
            return db.CATEGORies.Find(id);
        }
        public CATEGORY getCategoryByAlias(string alias)
        {
            return db.CATEGORies.Where(m => m.CATEGORY_ALIAS == alias).FirstOrDefault();
        }
        public bool haveChildCategory(decimal id)
        {
            if (db.CATEGORies.Where(m => m.CATEGORY_PARENT_ID == id).Count() > 0) return true;
            else return false;
        }
        public bool haveParentCategory(decimal id)
        {
            if (db.CATEGORies.Where(m => m.CATEGORY_PARENT_ID != null && m.CATEGORY_ID == id).Count() > 0) return true;
            else return false;
        }
    }
}