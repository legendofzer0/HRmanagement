using HRmanagement.Data.enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRmanagement.Utility
{
    public class GetEmpStatusList
    {
        public static List<SelectListItem> GetEmpStatusLists()
        {
            return Enum.GetValues(typeof(EmpStatus))
                .Cast<EmpStatus>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
        }
    }
}