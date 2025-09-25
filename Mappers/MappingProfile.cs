using AutoMapper;
using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerRejectionDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.RolePermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.RoleDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionPageDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.AdminPageDTOs;
using Hoshi.Models.UserModels.AdminModels;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ServiceRequestRateDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.OrderComplaetionRateDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsValueDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.IncomeGrowthRateDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CustomerGrowthRateDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ComplaintSolvingRateDTOs;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CategoryRequestRateDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;
using Hoshi.DTOs.UserDTOs.UserOTPDTOs;
using Hoshi.DTOs.UserDTOs.UserCollectionAlertDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.SuspendReasonDTOs;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.Models.UserModels;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.ServiceDTOs.JobServiceDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.Models.ServiceModels;
using Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionServiceDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.Models.PromotionModels;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.DTOs.GlobalDTOs.UserNotificationDTOs;
using Hoshi.DTOs.GlobalDTOs.RateDTOs;
using Hoshi.DTOs.GlobalDTOs.NotificationTypeDTOs;
using Hoshi.DTOs.GlobalDTOs.FeeDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.Models.GlobalModels;
using Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs;
using Hoshi.DTOs.DashboardDTOs.CompanyRevenueDTOs;
using Hoshi.DTOs.DashboardDTOs.ArchiveSettingsDTOs;
using Hoshi.DTOs.DashboardDTOs.ArchiveDTOs;
using Hoshi.DTOs.DashboardDTOs.AdminNotificationDTOs;
using Hoshi.Models.DashboardModels;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Models.ViewModels;

namespace Hoshi.Mappers
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// AutoMapper profile for mapping between entities and DTOs.
        /// Includes generic helpers to reduce repetitive mapping configuration.
        /// </summary>
        public MappingProfile()
        {

			GenericCreateTimestampedMaps<WorkerWalletHistory, WorkerWalletHistoryPostDTO, WorkerWalletHistoryPutDTO, WorkerWalletHistoryGetDTO>();

			GenericCreateTimestampedMaps<WorkerWallet, WorkerWalletPostDTO, WorkerWalletPutDTO, WorkerWalletGetDTO>();

			GenericCreateTimestampedMaps<WorkerSpecification, WorkerSpecificationPostDTO, WorkerSpecificationPutDTO, WorkerSpecificationGetDTO>();

			GenericCreateTimestampedMaps<WorkerRejection, WorkerRejectionPostDTO, WorkerRejectionPutDTO, WorkerRejectionGetDTO>();

			GenericCreateTimestampedMaps<WorkerPortfolio, WorkerPortfolioPostDTO, WorkerPortfolioPutDTO, WorkerPortfolioGetDTO>();

			GenericCreateTimestampedMaps<WorkerPaymentHistroy, WorkerPaymentHistroyPostDTO, WorkerPaymentHistroyPutDTO, WorkerPaymentHistroyGetDTO>();

			GenericCreateTimestampedMaps<UserPermission, UserPermissionPostDTO, UserPermissionPutDTO, UserPermissionGetDTO>();

			GenericCreateTimestampedMaps<RolePermission, RolePermissionPostDTO, RolePermissionPutDTO, RolePermissionGetDTO>();

			GenericCreateBasicMaps<Role, RolePostDTO, RolePutDTO, RoleGetDTO>();

			GenericCreateTimestampedMaps<PermissionPage, PermissionPagePostDTO, PermissionPagePutDTO, PermissionPageGetDTO>();

			GenericCreateTimestampedMaps<Permission, PermissionPostDTO, PermissionPutDTO, PermissionGetDTO>();

			GenericCreateTimestampedMaps<Page, PagePostDTO, PagePutDTO, PageGetDTO>();

			GenericCreateTimestampedMaps<AdminPage, AdminPagePostDTO, AdminPagePutDTO, AdminPageGetDTO>();

			GenericCreateBasicMaps<ServiceRequestRate, ServiceRequestRatePostDTO, ServiceRequestRatePutDTO, ServiceRequestRateGetDTO>();

			GenericCreateBasicMaps<OrderComplaetionRate, OrderComplaetionRatePostDTO, OrderComplaetionRatePutDTO, OrderComplaetionRateGetDTO>();

			GenericCreateBasicMaps<NumericalStatisticsValue, NumericalStatisticsValuePostDTO, NumericalStatisticsValuePutDTO, NumericalStatisticsValueGetDTO>();

			GenericCreateBasicMaps<NumericalStatistics, NumericalStatisticsPostDTO, NumericalStatisticsPutDTO, NumericalStatisticsGetDTO>();

			GenericCreateBasicMaps<IncomeGrowthRate, IncomeGrowthRatePostDTO, IncomeGrowthRatePutDTO, IncomeGrowthRateGetDTO>();

			GenericCreateBasicMaps<CustomerGrowthRate, CustomerGrowthRatePostDTO, CustomerGrowthRatePutDTO, CustomerGrowthRateGetDTO>();

			GenericCreateBasicMaps<ComplaintSolvingRate, ComplaintSolvingRatePostDTO, ComplaintSolvingRatePutDTO, ComplaintSolvingRateGetDTO>();

			GenericCreateBasicMaps<CategoryRequestRate, CategoryRequestRatePostDTO, CategoryRequestRatePutDTO, CategoryRequestRateGetDTO>();

			GenericCreateTimestampedMaps<UserOTP, UserOTPPostDTO, UserOTPPutDTO, UserOTPGetDTO>();

			GenericCreateTimestampedMaps<UserCollectionAlert, UserCollectionAlertPostDTO, UserCollectionAlertPutDTO, UserCollectionAlertGetDTO>();

			GenericCreateBasicMaps<User, UserPostDTO, UserPutDTO, UserGetDTO>();

			GenericCreateTimestampedMaps<SuspendReason, SuspendReasonPostDTO, SuspendReasonPutDTO, SuspendReasonGetDTO>();

			GenericCreateTimestampedMaps<SuspendedUser, SuspendedUserPostDTO, SuspendedUserPutDTO, SuspendedUserGetDTO>();

			GenericCreateTimestampedMaps<ClientSpecification, ClientSpecificationPostDTO, ClientSpecificationPutDTO, ClientSpecificationGetDTO>();

			GenericCreateTimestampedMaps<ServiceCategory, ServiceCategoryPostDTO, ServiceCategoryPutDTO, ServiceCategoryGetDTO>();

			GenericCreateTimestampedMaps<Service, ServicePostDTO, ServicePutDTO, ServiceGetDTO>();

			GenericCreateTimestampedMaps<JobService, JobServicePostDTO, JobServicePutDTO, JobServiceGetDTO>();

			GenericCreateTimestampedMaps<Job, JobPostDTO, JobPutDTO, JobGetDTO>();

			GenericCreateTimestampedMaps<PromotionTaken, PromotionTakenPostDTO, PromotionTakenPutDTO, PromotionTakenGetDTO>();

			GenericCreateBasicMaps<PromotionService, PromotionServicePostDTO, PromotionServicePutDTO, PromotionServiceGetDTO>();

			GenericCreateTimestampedMaps<Promotion, PromotionPostDTO, PromotionPutDTO, PromotionGetDTO>();

			GenericCreateTimestampedMaps<OrderVisit, OrderVisitPostDTO, OrderVisitPutDTO, OrderVisitGetDTO>();

			GenericCreateBasicMaps<OrderStatusHistory, OrderStatusHistoryPostDTO, OrderStatusHistoryPutDTO, OrderStatusHistoryGetDTO>();

			GenericCreateBasicMaps<OrderImage, OrderImagePostDTO, OrderImagePutDTO, OrderImageGetDTO>();

			GenericCreateTimestampedMaps<Order, OrderPostDTO, OrderPutDTO, OrderGetDTO>();

			GenericCreateTimestampedMaps<Offer, OfferPostDTO, OfferPutDTO, OfferGetDTO>();

			GenericCreateTimestampedMaps<Invoice, InvoicePostDTO, InvoicePutDTO, InvoiceGetDTO>();

			GenericCreateTimestampedMaps<UserNotification, UserNotificationPostDTO, UserNotificationPutDTO, UserNotificationGetDTO>();

			GenericCreateTimestampedMaps<Rate, RatePostDTO, RatePutDTO, RateGetDTO>();

			GenericCreateTimestampedMaps<NotificationType, NotificationTypePostDTO, NotificationTypePutDTO, NotificationTypeGetDTO>();

			GenericCreateTimestampedMaps<Fee, FeePostDTO, FeePutDTO, FeeGetDTO>();

			GenericCreateTimestampedMaps<ComplaintType, ComplaintTypePostDTO, ComplaintTypePutDTO, ComplaintTypeGetDTO>();

			GenericCreateTimestampedMaps<Complaint, ComplaintPostDTO, ComplaintPutDTO, ComplaintGetDTO>();

			GenericCreateTimestampedMaps<City, CityPostDTO, CityPutDTO, CityGetDTO>();

			GenericCreateTimestampedMaps<TermsAndCondetions, TermsAndCondetionsPostDTO, TermsAndCondetionsPutDTO, TermsAndCondetionsGetDTO>();

			GenericCreateBasicMaps<CompanyRevenue, CompanyRevenuePostDTO, CompanyRevenuePutDTO, CompanyRevenueGetDTO>();

			GenericCreateTimestampedMaps<ArchiveSettings, ArchiveSettingsPostDTO, ArchiveSettingsPutDTO, ArchiveSettingsGetDTO>();

			GenericCreateBasicMaps<Archive, ArchivePostDTO, ArchivePutDTO, ArchiveGetDTO>();

			GenericCreateBasicMaps<AdminNotification, AdminNotificationPostDTO, AdminNotificationPutDTO, AdminNotificationGetDTO>();

			CreateMap<UserPostDTO, ApplicationUserRegisterRequestDto>()
				.ForMember(dest => dest.Password, opt => opt.MapFrom(_ => "Hoshi@00"));

			CreateMap<Order, OrderGetAllDto>().ReverseMap();
			
			CreateMap<ClientSpecification, ClientDataDto>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User!.UserName)).ReverseMap();	

            CreateMap<Service, ServiceDataDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ServiceCategory!.CategoryName)).ReverseMap();

            CreateMap<Order, SubmittedOrderDetailsDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ImagesUrl, opt => opt.MapFrom(src => src.OrderImages!.Select(img => img.ImageURL)));

			// Basic DTO mappers
			//CreateMap<BasicDTO, >().ReverseMap();

			CreateMap<UserBasicDTO, User>().ReverseMap();

			CreateMap<OrderBasicDTO, Order>().ReverseMap();

			CreateMap<OfferBasicDTO, Offer>().ReverseMap();

			CreateMap<ServiceBasicDTO, Service>().ReverseMap();

			CreateMap<ServiceCategoryBasicDTO, ServiceCategory>().ReverseMap();

			CreateMap<WorkerPortfolioBasicDTO, WorkerPortfolio>().ReverseMap();

			CreateMap<DashbordWorkerDetailsDTO, WorkerSpecification>().ReverseMap();

            // Handel enums in mappers
            CreateMap<FeePostDTO, Fee>()
                .ForMember(d => d.FeeType, s => s.MapFrom(s => s.FeeType.ToString()))
                // Add current date time whithin creating a new Row
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => DateTime.Now));

            CreateMap<OrderPostDTO, Order>()
                // Add current date time whithin creating a new Row
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => DateTime.Now))
                .ForMember(d => d.ServicingDateTime, s => s.MapFrom(s => s.ServicingDateTime.ToUniversalTime()));

            CreateMap<PromotionPostDTO, Promotion>()
                .ForMember(d => d.PromotionFor, s => s.MapFrom(s => s.PromotionFor.ToString()))
                .ForMember(d => d.StartDate, s => s.MapFrom(s => s.StartDate.Value.ToUniversalTime()))
                .ForMember(d => d.EndDate, s => s.MapFrom(s => s.EndDate.Value.ToUniversalTime()));

            CreateMap<PromotionPutDTO, Promotion>()
                .ForMember(d => d.PromotionFor, s => s.MapFrom(s => s.PromotionFor.ToString()))
                // Add current date time whithin updating a new Row
                .ForMember(d => d.ModifiedAt, s => s.MapFrom(s => DateTime.Now))
                .ForMember(d => d.StartDate, s => s.MapFrom(s => s.StartDate.Value.ToUniversalTime()))
                .ForMember(d => d.EndDate, s => s.MapFrom(s => s.EndDate.Value.ToUniversalTime()))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, context) =>
                {
                    if (srcMember == null) return false;

                    var type = srcMember.GetType();
                    return !(type.IsValueType && Activator.CreateInstance(type)?.Equals(srcMember) == true);
                }));

            // View models mappers: used to handel the requierd mapping in GenericFSPService
            CreateMap<NewClientViewModel, NewClientViewModel>();
            CreateMap<AllClientModelForView, AllClientModelForView>();
            CreateMap<SuspendedUserModelForView, SuspendedUserModelForView>();

            CreateMap<NewWorkerModelForView, NewWorkerModelForView>();
            CreateMap<AllWorkersModelForView, AllWorkersModelForView>();
            CreateMap<SuspendedWorkerModelForView, SuspendedWorkerModelForView>();

            // Suggested improvement (projection-friendly mappings for queries):
            // CreateMap<Order, OrderSearchResultDto>()
            //     .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiveName))
            //     .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName));
        }

        
        /// <summary>
        /// A generic function to create defualt maps for basic models.
        /// </summary>
        /// <typeparam name="T">A generic Base Model that inhreit from IBaseModel.</typeparam>
        /// <typeparam name="PostDto">Model Post DTO.</typeparam>
        /// <typeparam name="PutDto">Model Put DTO.</typeparam>
        /// <typeparam name="GetDto">Model Get DTO.</typeparam>
        private void GenericCreateBasicMaps<T, PostDto, PutDto, GetDto>() where T : IBaseModel
        {
            CreateMap<PostDto, T>();

            CreateMap<PutDto, T>()
                // Don't map the Null props in PutDTO and get its values from source model.
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, context) =>
                {
                    if (srcMember == null) return false;

                    var type = srcMember.GetType();
                    return !(type.IsValueType && Activator.CreateInstance(type)?.Equals(srcMember) == true);
                }));

            CreateMap<GetDto, T>().ReverseMap();

            CreateMap<GetDto, List<T>>().ReverseMap();
            // Suggested improvement (avoid List mappings to prevent accidental materialization):
            // Remove the List<> mapping above and map collections explicitly where needed.
        }

        /// <summary>
        /// A generic function to create defualt maps for models that has Timestamp props.
        /// </summary>
        /// <typeparam name="T">A generic Base Model that inhreit from TimestampedModel to enshure that it include the CreatedAt and ModifiedAt Properities.</typeparam>
        /// <typeparam name="PostDto">Model Post DTO.</typeparam>
        /// <typeparam name="PutDto">Model Put DTO.</typeparam>
        /// <typeparam name="GetDto">Model Get DTO.</typeparam>
        private void GenericCreateTimestampedMaps<T, PostDto, PutDto, GetDto>() where T : TimestampedModel
        {
            CreateMap<PostDto, T>()
                // Add current date time whithin creating a new Row
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => DateTime.Now));

            CreateMap<PutDto, T>()
                // Add current date time whithin updating a new Row
                .ForMember(d => d.ModifiedAt, s => s.MapFrom(s => DateTime.Now))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, context) =>
                {
                    if (srcMember == null) return false;

                    var type = srcMember.GetType();
                    return !(type.IsValueType && Activator.CreateInstance(type)?.Equals(srcMember) == true);
                }));

            CreateMap<GetDto, T>().ReverseMap();

            CreateMap<GetDto, List<T>>().ReverseMap();
            // Suggested improvement (use UtcNow for server consistency):
            // .ForMember(d => d.CreatedAt, s => s.MapFrom(s => DateTime.UtcNow));
            // .ForMember(d => d.ModifiedAt, s => s.MapFrom(s => DateTime.UtcNow));
        }
    }
}
