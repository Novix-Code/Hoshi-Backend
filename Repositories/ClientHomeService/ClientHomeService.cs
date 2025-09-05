using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ClientDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.ServiceModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Hoshi.Repositories.ClientHomeService
{
    /// <summary>
    /// Builds client home data including non-taken promotions and active services by category.
    /// </summary>
    public class ClientHomeService : IClientHomeService
    {
        private readonly HoshiDbContext _context;
        public ClientHomeService(HoshiDbContext context)
        {
            _context = context;
      
        }
        /// <summary>
        /// For all clients, compute available promotions (not yet taken) and active services per category.
        /// </summary>
        public async Task<ResultDTO<List<GetAllHomeServiceDTO>>> GetAllClientWithServiceAsync()
        {
            // 1. Get all relative Services and promotions
            var allClients = await _context.Users
                .Where(u => u.UserType == "Client")
                .ToListAsync();

            var clientPromotions = await _context.Promotions
                .Where(p => p.PromotionFor == Enums.PromotionFor.Client)
                .ToListAsync();

            var allCategories = await _context.ServiceCategories
                .Include(c => c.Services)
                .ToListAsync();

            var allClientPromotionsTaken = await _context.PromotionsTaken
                .Where(p => allClients.Select(c => c.Id).Contains(p.UserId))
                .ToListAsync();
            
            var resultList = new List<GetAllHomeServiceDTO>();
            foreach (var client in allClients)
            {
                // 2. Get each Client promotions and services
                var takenPromotionIds = allClientPromotionsTaken
                    .Where(p => p.UserId == client.Id)
                    .Select(p => p.PromotionId)
                    .ToHashSet(); 

                var nonTakenPromotions = clientPromotions
                    .Where(p => !takenPromotionIds.Contains(p.Id))
                    .ToList();

                var targetActiveCategories = allCategories.Select(category => new TargetActiveCategory
                {
                    Title = category.CategoryName,
                    ActiveService = category.Services
                        .Where(s => !s.IsDeleted)
                        .ToList()
                }).ToList();
                // 3. Build Results
                var clientHome = new ClientHomeDto
                {
                    Promotions = nonTakenPromotions,
                    ActiveCategoriesServices = targetActiveCategories
                };

                resultList.Add(new GetAllHomeServiceDTO
                {
                    ClienId = client.Id,
                    ClienName = client.UserName,
                    DetailsAboutServices = clientHome
                });
            }



            return ResultDTO<List<GetAllHomeServiceDTO>>.Success(resultList);

        }

        /// <summary>
        /// For a specific client, compute available promotions (not yet taken) and active services per category.
        /// </summary>
        public async Task<ResultDTO<ClientHomeDto>> GetClientWithServiceById(int ClientId)
        {
            // 1. Get all relative Services and promotions
            var takenPromotionIds = await _context.PromotionsTaken
                .Where(p => p.UserId == ClientId)
                .Select(p => p.PromotionId)
                .ToListAsync();

            var clientPromotions = await _context.Promotions
                .Where(p => p.PromotionFor == Enums.PromotionFor.Client)
                .ToListAsync();

            var nonTakenPromotions = clientPromotions
                .Where(p => !takenPromotionIds.Contains(p.Id))
                .ToList();

            var allCategories = await _context.ServiceCategories
                .Include(c => c.Services)
                .ToListAsync();

            var targetActiveCategories = allCategories.Select(category => new TargetActiveCategory
            {
                Title = category.CategoryName,
                ActiveService = category.Services
                    .Where(service => !service.IsDeleted)
                    .ToList()
            }).ToList();

            //3. Build the result DTO
            var result = new ClientHomeDto
            {
                Promotions = nonTakenPromotions,
                ActiveCategoriesServices = targetActiveCategories
            };

            return ResultDTO<ClientHomeDto>.Success(result);
        }

    }
}
