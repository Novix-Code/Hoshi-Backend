using Hoshi.Data.LookupSeeders;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.DashboardModels.StatisticsModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.ServiceModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.AdminModels;
using Hoshi.Models.UserModels.Resets;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hoshi.Data
{
    public class HoshiDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public HoshiDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Loop through all foreign key relationships
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableForeignKey foreignKey in entityType.GetForeignKeys())
                {
                    // Set DeleteBehavior to NoAction for all foreign keys
                    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
                }
            }

            // Add Main Data Seeders:
            LibyanCitiesSeeder.SeedLibyanCities(modelBuilder);
            ServiceModelsSeeder.SeedAllHomeServicesData(modelBuilder);
            FeesSeeder.SeedFees(modelBuilder);

            // Add Views
            modelBuilder.Entity<OverViewPage>().HasNoKey().ToView("OverviewView");
            modelBuilder.Entity<clientPageModel>().HasNoKey().ToView("ClientPageView4");
            modelBuilder.Entity<NewClient>().HasNoKey().ToView("NewClientView");
            modelBuilder.Entity<NewClient>().HasNoKey().ToView("AllClientView");
            modelBuilder.Entity<NewClient>().HasNoKey().ToView("SuspendedUser");
            modelBuilder.Entity<ClientDetailsModelView>().HasNoKey().ToView("ClientDetailsView");
            modelBuilder.Entity<WorkerDetailsViewModel>().HasNoKey().ToView("WorkerDetailsView");
            modelBuilder.Entity<CitiesViewModel>().HasNoKey().ToView("CitiesgetView");
            modelBuilder.Entity<orderViewModelDetails>().HasNoKey().ToView("OrdersGetView");
            modelBuilder.Entity<clientPageModel>().HasNoKey().ToView("WorkerPageView");
            modelBuilder.Entity<NewClient>().HasNoKey().ToView("NewWorkerView");
            modelBuilder.Entity<NewClient>().HasNoKey().ToView("AllWorkertView");
            modelBuilder.Entity<NewClient>().HasNoKey().ToView("SuspendedWorker");
            modelBuilder.Entity<JobViewModel>().HasNoKey().ToView("JobView");
            modelBuilder.Entity<PortfolioViewModel>().HasNoKey().ToView("PortfolioView");
        }





        /// <summary>
        /// this is View Models
        /// </summary>
        public DbSet<OverViewPage> OverviewView { get; set; }
        public DbSet<clientPageModel> ClientPageView4 { get; set; }
        public DbSet<NewClient> NewClientView { get; set; }
        public DbSet<NewClient> AllClientView { get; set; }
        public DbSet<NewClient> SuspendedUser { get; set; }
        public DbSet<ClientDetailsModelView> ClientDetailsView { get; set; }
        public DbSet<WorkerDetailsViewModel> WorkerDetailsView { get; set; }
        public DbSet<CitiesViewModel> CitiesgetView { get; set; }
        public DbSet<orderViewModelDetails> OrdersGetView { get; set; }
        public DbSet<clientPageModel> WorkerPageView { get; set; }
        public DbSet<NewClient> NewWorkerView { get; set; }
        public DbSet<NewClient> AllWorkertView { get; set; }
        public DbSet<NewClient> SuspendedWorker { get; set; }
        public DbSet<JobViewModel> JobView { get; set; }
        public DbSet<PortfolioViewModel> PortfolioView { get; set; }




        //---------------------------------------------------------------
        //--------------------------User Models--------------------------
        //---------------------------------------------------------------
        // Global User Models
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<SuspendedUser> SuspendedUsers { get; set; }
        public DbSet<SuspendReason> SuspendReasons { get; set; }
        public DbSet<UserCollectionAlert> UserCollectionAlerts { get; set; }

        // Client Models
        public DbSet<ClientSpecification> ClientSpecifications { get; set; }

        // Worker Models
        public DbSet<WorkerSpecification> WorkerSpecifications { get; set; }
        public DbSet<WorkerService> WorkerServices { get; set; }
        public DbSet<WorkerPortfolio> WorkerPortfolios { get; set; }
        public DbSet<WorkerRejection> WorkerRejections { get; set; }
        public DbSet<WorkerWallet> WorkerWallets { get; set; }
        public DbSet<WorkerWalletHistory> WorkerWalletHistories { get; set; }
        public DbSet<WorkerPaymentHistroy> WorkerPaymentHistroys { get; set; }

        // Admin Models
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionPage> PermissionPages { get; set; }
        public DbSet<AdminPage> AdminPages { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

        //-----------------------------------------------------------------
        //--------------------------System Models--------------------------
        //-----------------------------------------------------------------
        // Service Models
        public DbSet<Job> Jobs { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<JobService> JobServices { get; set; }

        // Order Models
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderImage> OrderImages { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistory { get; set; }
        public DbSet<OrderVisit> OrderVisits { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        // Promotion Models
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionService> PromotionServices { get; set; }
        public DbSet<PromotionTaken> PromotionsTaken { get; set; }

        // Global Models
        public DbSet<City> Cities { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }

        //--------------------------------------------------------------
        //--------------------------Dashboard Models--------------------------
        //--------------------------------------------------------------
        // Main Models
        public DbSet<CompanyRevenue> CompanyRevenues { get; set; }
        public DbSet<TermsAndCondetions> TermsAndCondetions { get; set; }
        public DbSet<AdminNotification> AdminNotifications { get; set; }
        public DbSet<AdminNotificationFlag> AdminNotificationFlags { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<ArchiveSettings> ArchiveSettings { get; set; }

        // Statistics Models
        public DbSet<NumericalStatistics> NumericalStatistics { get; set; }
        public DbSet<NumericalStatisticsValue> NumericalStatisticsValues { get; set; }
        public DbSet<CustomerGrowthRate> CustomerGrowthRates { get; set; }
        public DbSet<IncomeGrowthRate> IncomeGrowthRates { get; set; }
        public DbSet<OrderComplaetionRate> OrderComplaetionRates { get; set; }
        public DbSet<CategoryRequestRate> CategoryRequestRates { get; set; }
        public DbSet<ServiceRequestRate> ServiceRequestRates { get; set; }
        public DbSet<ComplaintSolvingRate> ComplaintSolvingRates { get; set; }
    }
}
