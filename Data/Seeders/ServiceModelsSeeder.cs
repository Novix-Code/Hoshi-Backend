using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.Seeders
{
    public static class ServiceModelsSeeder
    {
        // Combined seeder method
        public static void SeedAllHomeServicesData(ModelBuilder modelBuilder)
        {
            SeedServiceCategories(modelBuilder);
            SeedServices(modelBuilder);
            SeedJobs(modelBuilder);
            SeedJobServices(modelBuilder);
        }

        private static void SeedServiceCategories(ModelBuilder modelBuilder)
        {
            var categories = new List<ServiceCategory>
            {
                new ServiceCategory
                {
                    Id = 1,
                    CategoryName = "خدمات التنظيف",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 2,
                    CategoryName = "الصيانة والإصلاح",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 3,
                    CategoryName = "الخدمات الكهربائية",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 4,
                    CategoryName = "السباكة",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 5,
                    CategoryName = "التكييف والتبريد",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 6,
                    CategoryName = "الدهان والديكور",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 7,
                    CategoryName = "النجارة والأثاث",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 8,
                    CategoryName = "البستنة والحدائق",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 9,
                    CategoryName = "خدمات السيارات",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 10,
                    CategoryName = "التوصيل والنقل",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 11,
                    CategoryName = "الخدمات التقنية",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new ServiceCategory
                {
                    Id = 12,
                    CategoryName = "الأمن والحراسة",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                }
            };

            modelBuilder.Entity<ServiceCategory>().HasData(categories);
        }

        private static void SeedServices(ModelBuilder modelBuilder)
        {
            var services = new List<Service>
            {
                // خدمات التنظيف (Category 1)
                new Service
                {
                    Id = 1,
                    ServiveName = "تنظيف المنازل الشامل",
                    ImageURL = "/images/services/house-cleaning.jpg",
                    ServiceCategoryId = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 2,
                    ServiveName = "تنظيف النوافذ والزجاج",
                    ImageURL = "/images/services/window-cleaning.jpg",
                    ServiceCategoryId = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 3,
                    ServiveName = "تنظيف السجاد والموكيت",
                    ImageURL = "/images/services/carpet-cleaning.jpg",
                    ServiceCategoryId = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 4,
                    ServiveName = "تنظيف المطابخ والحمامات",
                    ImageURL = "/images/services/kitchen-bathroom-cleaning.jpg",
                    ServiceCategoryId = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 5,
                    ServiveName = "تنظيف الأثاث والمفروشات",
                    ImageURL = "/images/services/furniture-cleaning.jpg",
                    ServiceCategoryId = 1,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // الصيانة والإصلاح (Category 2)
                new Service
                {
                    Id = 6,
                    ServiveName = "صيانة الأجهزة المنزلية",
                    ImageURL = "/images/services/appliance-repair.jpg",
                    ServiceCategoryId = 2,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 7,
                    ServiveName = "إصلاح الأبواب والنوافذ",
                    ImageURL = "/images/services/door-window-repair.jpg",
                    ServiceCategoryId = 2,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 8,
                    ServiveName = "صيانة المولدات الكهربائية",
                    ImageURL = "/images/services/generator-maintenance.jpg",
                    ServiceCategoryId = 2,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 9,
                    ServiveName = "إصلاح البلاط والأرضيات",
                    ImageURL = "/images/services/tile-repair.jpg",
                    ServiceCategoryId = 2,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // الخدمات الكهربائية (Category 3)
                new Service
                {
                    Id = 10,
                    ServiveName = "تركيب الكهرباء المنزلية",
                    ImageURL = "/images/services/electrical-installation.jpg",
                    ServiceCategoryId = 3,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 11,
                    ServiveName = "إصلاح الأعطال الكهربائية",
                    ImageURL = "/images/services/electrical-repair.jpg",
                    ServiceCategoryId = 3,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 12,
                    ServiveName = "تركيب الإنارة والثريات",
                    ImageURL = "/images/services/lighting-installation.jpg",
                    ServiceCategoryId = 3,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 13,
                    ServiveName = "تركيب أنظمة الأمان",
                    ImageURL = "/images/services/security-systems.jpg",
                    ServiceCategoryId = 3,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // السباكة (Category 4)
                new Service
                {
                    Id = 14,
                    ServiveName = "إصلاح تسريبات المياه",
                    ImageURL = "/images/services/plumbing-leak-repair.jpg",
                    ServiceCategoryId = 4,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 15,
                    ServiveName = "تركيب وصيانة الحنفيات",
                    ImageURL = "/images/services/faucet-installation.jpg",
                    ServiceCategoryId = 4,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 16,
                    ServiveName = "تسليك المجاري والأنابيب",
                    ImageURL = "/images/services/drain-cleaning.jpg",
                    ServiceCategoryId = 4,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 17,
                    ServiveName = "تركيب خزانات المياه",
                    ImageURL = "/images/services/water-tank-installation.jpg",
                    ServiceCategoryId = 4,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // التكييف والتبريد (Category 5)
                new Service
                {
                    Id = 18,
                    ServiveName = "تركيب أجهزة التكييف",
                    ImageURL = "/images/services/ac-installation.jpg",
                    ServiceCategoryId = 5,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 19,
                    ServiveName = "صيانة وتنظيف المكيفات",
                    ImageURL = "/images/services/ac-maintenance.jpg",
                    ServiceCategoryId = 5,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 20,
                    ServiveName = "إصلاح الثلاجات والمجمدات",
                    ImageURL = "/images/services/refrigerator-repair.jpg",
                    ServiceCategoryId = 5,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // الدهان والديكور (Category 6)
                new Service
                {
                    Id = 21,
                    ServiveName = "دهان الجدران الداخلية",
                    ImageURL = "/images/services/interior-painting.jpg",
                    ServiceCategoryId = 6,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 22,
                    ServiveName = "دهان الواجهات الخارجية",
                    ImageURL = "/images/services/exterior-painting.jpg",
                    ServiceCategoryId = 6,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 23,
                    ServiveName = "تركيب ورق الجدران",
                    ImageURL = "/images/services/wallpaper-installation.jpg",
                    ServiceCategoryId = 6,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 24,
                    ServiveName = "الديكور والتصميم الداخلي",
                    ImageURL = "/images/services/interior-design.jpg",
                    ServiceCategoryId = 6,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // النجارة والأثاث (Category 7)
                new Service
                {
                    Id = 25,
                    ServiveName = "تفصيل وتركيب الأثاث",
                    ImageURL = "/images/services/furniture-making.jpg",
                    ServiceCategoryId = 7,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 26,
                    ServiveName = "إصلاح الأثاث المكسور",
                    ImageURL = "/images/services/furniture-repair.jpg",
                    ServiceCategoryId = 7,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 27,
                    ServiveName = "تركيب الأرفف والخزائن",
                    ImageURL = "/images/services/shelving-installation.jpg",
                    ServiceCategoryId = 7,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // البستنة والحدائق (Category 8)
                new Service
                {
                    Id = 28,
                    ServiveName = "تنسيق وتصميم الحدائق",
                    ImageURL = "/images/services/garden-design.jpg",
                    ServiceCategoryId = 8,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 29,
                    ServiveName = "قص وتهذيب الأشجار",
                    ImageURL = "/images/services/tree-trimming.jpg",
                    ServiceCategoryId = 8,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 30,
                    ServiveName = "زراعة النباتات والورود",
                    ImageURL = "/images/services/planting.jpg",
                    ServiceCategoryId = 8,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // خدمات السيارات (Category 9)
                new Service
                {
                    Id = 31,
                    ServiveName = "غسيل وتنظيف السيارات",
                    ImageURL = "/images/services/car-washing.jpg",
                    ServiceCategoryId = 9,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 32,
                    ServiveName = "صيانة السيارات المنزلية",
                    ImageURL = "/images/services/car-maintenance.jpg",
                    ServiceCategoryId = 9,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 33,
                    ServiveName = "تغيير إطارات السيارات",
                    ImageURL = "/images/services/tire-change.jpg",
                    ServiceCategoryId = 9,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // التوصيل والنقل (Category 10)
                new Service
                {
                    Id = 34,
                    ServiveName = "توصيل الطلبات والمشتريات",
                    ImageURL = "/images/services/delivery.jpg",
                    ServiceCategoryId = 10,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 35,
                    ServiveName = "نقل الأثاث والعفش",
                    ImageURL = "/images/services/furniture-moving.jpg",
                    ServiceCategoryId = 10,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 36,
                    ServiveName = "خدمات النقل الشخصي",
                    ImageURL = "/images/services/personal-transport.jpg",
                    ServiceCategoryId = 10,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // الخدمات التقنية (Category 11)
                new Service
                {
                    Id = 37,
                    ServiveName = "تركيب وصيانة الكمبيوتر",
                    ImageURL = "/images/services/computer-repair.jpg",
                    ServiceCategoryId = 11,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 38,
                    ServiveName = "تركيب شبكات الإنترنت",
                    ImageURL = "/images/services/internet-installation.jpg",
                    ServiceCategoryId = 11,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 39,
                    ServiveName = "برمجة وتطوير المواقع",
                    ImageURL = "/images/services/web-development.jpg",
                    ServiceCategoryId = 11,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },

                // الأمن والحراسة (Category 12)
                new Service
                {
                    Id = 40,
                    ServiveName = "خدمات الحراسة المنزلية",
                    ImageURL = "/images/services/home-security.jpg",
                    ServiceCategoryId = 12,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Service
                {
                    Id = 41,
                    ServiveName = "تركيب كاميرات المراقبة",
                    ImageURL = "/images/services/security-cameras.jpg",
                    ServiceCategoryId = 12,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                }
            };

            modelBuilder.Entity<Service>().HasData(services);
        }

        private static void SeedJobs(ModelBuilder modelBuilder)
        {
            var jobs = new List<Job>
            {
                new Job
                {
                    Id = 1,
                    JobTitle = "عامل تنظيف",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 2,
                    JobTitle = "فني صيانة عامة",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 3,
                    JobTitle = "كهربائي",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 4,
                    JobTitle = "سباك",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 5,
                    JobTitle = "فني تكييف وتبريد",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 6,
                    JobTitle = "دهان",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 7,
                    JobTitle = "نجار",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 8,
                    JobTitle = "مهندس ديكور",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 9,
                    JobTitle = "بستاني",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 10,
                    JobTitle = "ميكانيكي سيارات",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 11,
                    JobTitle = "سائق توصيل",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 12,
                    JobTitle = "فني كمبيوتر",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 13,
                    JobTitle = "مبرمج",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 14,
                    JobTitle = "حارس أمن",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 15,
                    JobTitle = "فني أنظمة أمان",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 16,
                    JobTitle = "عامل نقل",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 17,
                    JobTitle = "مصمم حدائق",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                },
                new Job
                {
                    Id = 18,
                    JobTitle = "فني أجهزة منزلية",
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                }
            };

            modelBuilder.Entity<Job>().HasData(jobs);
        }

        private static void SeedJobServices(ModelBuilder modelBuilder)
        {
            var jobServices = new List<JobService>
            {
                // عامل تنظيف (Job 1) - Cleaning Services (1-5)
                new JobService { Id = 1, JobId = 1, ServiceId = 1, CreatedAt = DateTime.Now },
                new JobService { Id = 2, JobId = 1, ServiceId = 2, CreatedAt = DateTime.Now },
                new JobService { Id = 3, JobId = 1, ServiceId = 3, CreatedAt = DateTime.Now },
                new JobService { Id = 4, JobId = 1, ServiceId = 4, CreatedAt = DateTime.Now },
                new JobService { Id = 5, JobId = 1, ServiceId = 5, CreatedAt = DateTime.Now },

                // فني صيانة عامة (Job 2) - General Maintenance (6-9)
                new JobService { Id = 6, JobId = 2, ServiceId = 6, CreatedAt = DateTime.Now },
                new JobService { Id = 7, JobId = 2, ServiceId = 7, CreatedAt = DateTime.Now },
                new JobService { Id = 8, JobId = 2, ServiceId = 8, CreatedAt = DateTime.Now },
                new JobService { Id = 9, JobId = 2, ServiceId = 9, CreatedAt = DateTime.Now },

                // كهربائي (Job 3) - Electrical Services (10-13)
                new JobService { Id = 10, JobId = 3, ServiceId = 10, CreatedAt = DateTime.Now },
                new JobService { Id = 11, JobId = 3, ServiceId = 11, CreatedAt = DateTime.Now },
                new JobService { Id = 12, JobId = 3, ServiceId = 12, CreatedAt = DateTime.Now },
                new JobService { Id = 13, JobId = 3, ServiceId = 13, CreatedAt = DateTime.Now },

                // سباك (Job 4) - Plumbing Services (14-17)
                new JobService { Id = 14, JobId = 4, ServiceId = 14, CreatedAt = DateTime.Now },
                new JobService { Id = 15, JobId = 4, ServiceId = 15, CreatedAt = DateTime.Now },
                new JobService { Id = 16, JobId = 4, ServiceId = 16, CreatedAt = DateTime.Now },
                new JobService { Id = 17, JobId = 4, ServiceId = 17, CreatedAt = DateTime.Now },

                // فني تكييف وتبريد (Job 5) - AC & Cooling (18-20)
                new JobService { Id = 18, JobId = 5, ServiceId = 18, CreatedAt = DateTime.Now },
                new JobService { Id = 19, JobId = 5, ServiceId = 19, CreatedAt = DateTime.Now },
                new JobService { Id = 20, JobId = 5, ServiceId = 20, CreatedAt = DateTime.Now },

                // دهان (Job 6) - Painting Services (21-23)
                new JobService { Id = 21, JobId = 6, ServiceId = 21, CreatedAt = DateTime.Now },
                new JobService { Id = 22, JobId = 6, ServiceId = 22, CreatedAt = DateTime.Now },
                new JobService { Id = 23, JobId = 6, ServiceId = 23, CreatedAt = DateTime.Now },

                // نجار (Job 7) - Carpentry Services (25-27)
                new JobService { Id = 24, JobId = 7, ServiceId = 25, CreatedAt = DateTime.Now },
                new JobService { Id = 25, JobId = 7, ServiceId = 26, CreatedAt = DateTime.Now },
                new JobService { Id = 26, JobId = 7, ServiceId = 27, CreatedAt = DateTime.Now },

                // مهندس ديكور (Job 8) - Interior Design (24)
                new JobService { Id = 27, JobId = 8, ServiceId = 24, CreatedAt = DateTime.Now },
                new JobService { Id = 28, JobId = 8, ServiceId = 23, CreatedAt = DateTime.Now },

                // بستاني (Job 9) - Gardening Services (28-30)
                new JobService { Id = 29, JobId = 9, ServiceId = 28, CreatedAt = DateTime.Now },
                new JobService { Id = 30, JobId = 9, ServiceId = 29, CreatedAt = DateTime.Now },
                new JobService { Id = 31, JobId = 9, ServiceId = 30, CreatedAt = DateTime.Now },

                // ميكانيكي سيارات (Job 10) - Car Services (31-33)
                new JobService { Id = 32, JobId = 10, ServiceId = 31, CreatedAt = DateTime.Now },
                new JobService { Id = 33, JobId = 10, ServiceId = 32, CreatedAt = DateTime.Now },
                new JobService { Id = 34, JobId = 10, ServiceId = 33, CreatedAt = DateTime.Now },

                // سائق توصيل (Job 11) - Delivery Services (34, 36)
                new JobService { Id = 35, JobId = 11, ServiceId = 34, CreatedAt = DateTime.Now },
                new JobService { Id = 36, JobId = 11, ServiceId = 36, CreatedAt = DateTime.Now },

                // فني كمبيوتر (Job 12) - Computer Services (37, 38)
                new JobService { Id = 37, JobId = 12, ServiceId = 37, CreatedAt = DateTime.Now },
                new JobService { Id = 38, JobId = 12, ServiceId = 38, CreatedAt = DateTime.Now },

                // مبرمج (Job 13) - Programming Service (39)
                new JobService { Id = 39, JobId = 13, ServiceId = 39, CreatedAt = DateTime.Now },

                // حارس أمن (Job 14) - Security Service (40)
                new JobService { Id = 40, JobId = 14, ServiceId = 40, CreatedAt = DateTime.Now },

                // فني أنظمة أمان (Job 15) - Security Systems (13, 41)
                new JobService { Id = 41, JobId = 15, ServiceId = 13, CreatedAt = DateTime.Now },
                new JobService { Id = 42, JobId = 15, ServiceId = 41, CreatedAt = DateTime.Now },

                // عامل نقل (Job 16) - Moving Service (35)
                new JobService { Id = 43, JobId = 16, ServiceId = 35, CreatedAt = DateTime.Now },

                // مصمم حدائق (Job 17) - Garden Design (28)
                new JobService { Id = 44, JobId = 17, ServiceId = 28, CreatedAt = DateTime.Now },

                // فني أجهزة منزلية (Job 18) - Appliance Repair (6, 20)
                new JobService { Id = 45, JobId = 18, ServiceId = 6, CreatedAt = DateTime.Now },
                new JobService { Id = 46, JobId = 18, ServiceId = 20, CreatedAt = DateTime.Now }
            };

            modelBuilder.Entity<JobService>().HasData(jobServices);
        }
    }
}
