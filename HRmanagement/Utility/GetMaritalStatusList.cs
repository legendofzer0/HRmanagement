using HRmanagement.Data.enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRmanagement.Utility
{
    public class GetMaritalStatusList
    {
        public static List<SelectListItem> GetMaritalStatus()
        {
            return Enum.GetValues(typeof(MaritialStatus))
               .Cast<MaritialStatus>()
               .Select(e => new SelectListItem
               {
                   Value = e.ToString(),
                   Text = e.ToString()
               }).ToList();
        }
    }
}