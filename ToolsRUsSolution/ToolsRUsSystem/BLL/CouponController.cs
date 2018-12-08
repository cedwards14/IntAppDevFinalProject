using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ToolsRUs.Data.Entities;
using ToolsRUsSystem.DAL;
using System.ComponentModel;
using ToolsRUs.Data.POCOs;
#endregion

namespace ToolsRUsSystem.BLL
{
    public class CouponController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Coupon Coupon_GetByCouponIDValue(string couponIDValue)
        {
            using (var context = new ToolsRUsContext())
            {
                //var results = from x in context.Coupons
                //              where x.CouponIDValue == couponIDValue
                //              select x;
                Coupon coupon = context.Coupons.Where(x => x.CouponIDValue.Equals(couponIDValue)).Select(x => x).FirstOrDefault();
                return coupon;
            }

        }
    }
}
