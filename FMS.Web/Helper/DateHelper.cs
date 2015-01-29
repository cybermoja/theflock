using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMS.Web.Helper.Extensions;

namespace FMS.Web.Helper
{
    public class DateHelper
    {
        public static int GetWeekNow()
        {
            return DateTime.Now.GetCurrentWeek();
        }
    }
}