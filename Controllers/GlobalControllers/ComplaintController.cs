using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using Hoshi.Models.OrderModels;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.Models.GlobalModels;

namespace Hoshi.Controllers.GlobalControllers.ComplaintControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        Complaint, 
        ComplaintGetDTO, 
        ComplaintPostDTO, 
        ComplaintPutDTO>
    {
        public ComplaintController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Complaint, 
                ComplaintGetDTO, 
                ComplaintPostDTO, 
                ComplaintPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Complaint, 
                ComplaintGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.Client)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.Worker)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.City)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.Service)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(Complaint.ComplaintType)}",
				$"{nameof(Complaint.Order)}.{nameof(Order.OrderStatusHistory)}",
				$"{nameof(Complaint.User)}",
			];
        }
    }
}
