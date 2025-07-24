using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.ServiceModels;

namespace Hoshi.DTOs.ClientDTOs
{
    public class ClientHomeDto
    {
        public List<Promotion> Promotions { get; set; }
        public List<TargetActiveCategory> ActiveCategoriesServices { get; set; }
    }

    public class TargetActiveCategory
    {
        public string Title { get; set; }
        public List<Service> ActiveService { get; set; }
    }
}
