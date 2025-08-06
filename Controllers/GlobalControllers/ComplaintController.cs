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
using Hoshi.Repositories.NotificationService;

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
        private readonly INotificationServiceHandler notificationServiceHandler;
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
,
            INotificationServiceHandler notificationServiceHandler) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

            includes = [
                $"{nameof(Complaint.ComplaintType)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.Client)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.Worker)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.City)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.Service)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.AppliedPromotion)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.OrderImages)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.OrderVisits)}",
                $"{nameof(Complaint.Order)}.{nameof(Order.OrderStatusHistory)}",
                $"{nameof(Complaint.User)}",
            ];
            this.notificationServiceHandler = notificationServiceHandler;
        }
        public override async Task<IActionResult> Add(ComplaintPostDTO postDTO)
        {
            await notificationServiceHandler.sendMessagetoAdmin($"شكوى جديده بخصوص الاوردر ,المرسل : {postDTO.UserId}", postDTO.OrderId);
            return await base.Add(postDTO);
        }
    }
}
