using DTC_BE.Models;
using DTC_BE.Entities;

namespace DTC_BE.CodeBase
{
    public static class Dictionary
    {

        public static List<SelectListItem> GetListQuarter()
        {
            List<SelectListItem> lstQuarter = [];
            for (int quarter = 1; quarter <= 4; quarter++)
            {
                lstQuarter.Add(new SelectListItem { Value = quarter.ToString(), Text = $"Quý {quarter}" });
            }
            return lstQuarter;
        }

        public static List<SelectListItem> GetListMonth()
        {
            List<SelectListItem> lstMonth = [];
            for (int month = 1; month <= 12; month++)
            {
                lstMonth.Add(new SelectListItem { Value = month.ToString(), Text = $"Tháng {month}" });
            }
            return lstMonth;
        }

    }
}
