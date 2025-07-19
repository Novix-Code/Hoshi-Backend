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
    public class ClientHomeService : IClientHomeService
    {
        private readonly HoshiDbContext _context;
        private readonly GenericCRUDLibrary.GenericRepositories.GenericCRUDService.IGenericCRUDService<HoshiDbContext,PromotionTaken,PromotionTakenGetDTO,PromotionTakenPostDTO,PromotionTakenPutDTO> genericCRUDLibrary;
        public ClientHomeService(HoshiDbContext context, GenericCRUDLibrary.GenericRepositories.GenericCRUDService.IGenericCRUDService<HoshiDbContext, PromotionTaken, PromotionTakenGetDTO, PromotionTakenPostDTO, PromotionTakenPutDTO> genericCRUDLibrary)
        {
            _context = context;
            this.genericCRUDLibrary = genericCRUDLibrary;
        }
        public async Task<ResultDTO<List<GetAllHomeServiceDTO>>> GetAllServiceAsync()
        {
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
                var takenPromotionIds = allClientPromotionsTaken
                    .Where(p => p.UserId == client.Id)
                    .Select(p => p.PromotionId)
                    .ToHashSet(); 

                var nonTakenPromotions = clientPromotions
                    .Where(p => !takenPromotionIds.Contains(p.Id))
                    .ToList();

                var targetActiveCategories = allCategories.Select(category => new TargetActiveCategory
                {
                    Title = category.ServiveName,
                    ActiveService = category.Services
                        .Where(s => !s.IsDeleted)
                        .ToList()
                }).ToList();

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

        public async Task<ResultDTO<ClientHomeDto>> GetByIdServiceAsync(int Id)
        {
            // here related Promotions Taken
            var takenPromotionIds = await _context.PromotionsTaken
                .Where(p => p.UserId == Id)
                .Select(p => p.PromotionId)
                .ToListAsync();
            // all Client Promotion
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
                Title = category.ServiveName,
                ActiveService = category.Services
                    .Where(service => !service.IsDeleted)
                    .ToList()
            }).ToList();

            // Build the result DTO
            var result = new ClientHomeDto
            {
                Promotions = nonTakenPromotions,
                ActiveCategoriesServices = targetActiveCategories
            };

            return ResultDTO<ClientHomeDto>.Success(result);
        }

    }
}
