using HRmanagement.Data.enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRmanagement.Utility
{
    public class GetEmployementType
    {
        public static List<SelectListItem> GetEmployementTypeList()
        {
            return Enum.GetValues(typeof(EmpTypes))
                .Cast<EmpTypes>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
        }
    }
}