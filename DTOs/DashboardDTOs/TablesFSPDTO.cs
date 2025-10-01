using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.DTOs.DashboardDTOs
{
    public class TablesFSPDTO
    {
        public List<FilteredSearchDTO>? Filters { get; set; }
        public int PageNumber { get; set; } = 1;
        public bool Ascending { get; set; } = true;
    }
}
