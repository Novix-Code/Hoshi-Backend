using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Repositories.ClientHomeService;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.ClientOrderService
{
    public class ClientOrderService : IClientOrderService
    {
        private readonly HoshiDbContext _Context;
        private readonly IMapper _mapper;
        private readonly IClientHomeService _clientHomeService;
        public ClientOrderService(HoshiDbContext context, IMapper mapper, IClientHomeService clientHomeService)
        {
            _Context = context;
            _mapper = mapper;
            _clientHomeService = clientHomeService;
        }
        public async Task<ResultDTO<string>> AddOrderAsync(OrderPostDTO dto)
        {
            var orderMapper = _mapper.Map<Order>(dto);
            _Context.Orders.Add(orderMapper);
            await _Context.SaveChangesAsync();

            var histOrder = new OrderStatusHistoryPostDTO
            {
                CreatedAt = DateTime.Now,
                OrderId   = orderMapper.Id,
                OrderStatus = orderMapper.OrderStatus
            };
            var histMapper = _mapper.Map<OrderStatusHistory>(histOrder);

            var invoiceOrder = new InvoicePostDTO
            {
                 OrderId = orderMapper.Id,
                 CancellationFee = 0,
                 ClientIndebtednessFee = 0,
                 ClientPromotionFee = 0,
                 ClientTotalPrice = 0,
                 CommissionFee = 0,
                 OrderPrice = 0,
                 VisitingFee = 0,
                 WorkerPromotionFee = 0,
                 WorkerTotalPrice = 0,
            };
            var invoicMapper = _mapper.Map<Invoice>(invoiceOrder);

            var clientPromotionNonTaken = await _clientHomeService.GetByIdServiceAsync(dto.ClientId);
            var slectedPromotionId = clientPromotionNonTaken.Data.Promotions.Select(p=>p.Id).FirstOrDefault(); 
            
            //add offer
            




            var promotionOrder = new PromotionTakenPostDTO
            {
                UserId = dto.ClientId , 
                OrderId = orderMapper.Id , 
                OfferId = await _Context.Offers.Where(p=>p.OrderId == orderMapper.Id).Select(p=>p.Id).FirstOrDefaultAsync(),
                PromotionId = slectedPromotionId
            };
            var promotionMapper = _mapper.Map<PromotionTaken>(promotionOrder);


            //_Context.PromotionsTaken.Add(promotionMapper);
            //await _Context.SaveChangesAsync();

            _Context.Invoices.Add(invoicMapper);
            await _Context.SaveChangesAsync();

            _Context.OrderStatusHistory.Add(histMapper);    

            await _Context.SaveChangesAsync();

            return ResultDTO<string>.Success(orderMapper.Id.ToString());
             
        }

        public Task<ResultDTO<OrderGetAllDto>> GetAllClientsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
