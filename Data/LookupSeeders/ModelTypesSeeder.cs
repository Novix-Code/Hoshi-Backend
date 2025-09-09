using Hoshi.Models.GlobalModels;
using Hoshi.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.LookupSeeders
{
    public static class ModelTypesSeeder
    {
        private static DateTime createdAt = new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc);

        public static void Seeder(ModelBuilder modelBuilder)
        {
            ComplaintTypeSeeder(modelBuilder);
            NotificationsTypeSeeder(modelBuilder);
        }

        private static void ComplaintTypeSeeder(ModelBuilder modelBuilder)
        {
            var types = new List<ComplaintType>
            {
                // Client Types
                new()
                {
                    Id = 1,
                    ForClient = true,
                    Type = "تأخر العامل",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 2,
                    ForClient = true,
                    Type = "آداء العامل ليس احترافيا",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 3,
                    ForClient = true,
                    Type = "المواد المستخدمة رديئة",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 4,
                    ForClient = true,
                    Type = "مشكلة في الدفع",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 5,
                    ForClient = true,
                    Type = "أسلوب غير لائق",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 6,
                    ForClient = true,
                    Type = "أخرى",
                    CreatedAt = createdAt,
                },

                // Worker Types
                new()
                {
                    Id = 7,
                    ForClient = false,
                    Type = "العميل غير موجود في الموعد",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 8,
                    ForClient = false,
                    Type = "أسلوب العميل غير لائق",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 9,
                    ForClient = false,
                    Type = "طلب العميل غير متوفر",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 10,
                    ForClient = false,
                    Type = "مشكلة في الدفع",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 11,
                    ForClient = false,
                    Type = "العميل طلب خدمات اضافية",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 12,
                    ForClient = false,
                    Type = "أخرى",
                    CreatedAt = createdAt,
                },
            };

            modelBuilder.Entity<ComplaintType>().HasData(types);
        }

        private static void NotificationsTypeSeeder(ModelBuilder modelBuilder)
        {
            var types = new List<NotificationType>
            {
                // Client Types
                new()
                {
                    Id = 1,
                    ForClient = true,
                    Type = "الطلب مقبول",
                    Title = "تم قبول طلبك. اضغط هنا للذهاب إلى تفاصيل الطلب.",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 2,
                    ForClient = true,
                    Type = "تم تأكيد الطلب",
                    Title = "تم تأكيد طلبك. اضغط هنا لاختيار احد العروض المقدمة من العمال.",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 3,
                    ForClient = true,
                    Type = "تم تعيين العامل",
                    Title = "تم قبول العرض المقدم من العامل. اضغط هنا لاستكمال الدفع.",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 4,
                    ForClient = true,
                    Type = "انتهاء الطلب",
                    Title = "تم انتهاء طلبك. اضغط هنا لتقييم مدى رضاك عن آداء العامل.",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 5,
                    ForClient = true,
                    Type = "الطلب ملغي",
                    Title = "تم إلغاء طلبك. اذا كنت مازلت تحتاج الخدمة برجاء انشاء طلب جديد.",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 6,
                    ForClient = true,
                    Type = "اشعار جديد",
                    Title = " تفاصيل الاشعار.",
                    CreatedAt = createdAt,
                },

                // Worker Types
                new()
                {
                    Id = 7,
                    ForClient = false,
                    Type = "العرض مقبول",
                    Title = "تم قبول عرضك على الطلب رقم <id># اضغط هنا للذهاب إلى تفاصيل الطلب.",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 8,
                    ForClient = false,
                    Type = "تم تعديل الرصيد",
                    Title = "تم إضافة مبلغ <price> دينار إلى محفظتك لدفع رسوم الطلب رقم <id>#. الذهاب الى المحفظة", 
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 9,
                    ForClient = false,
                    Type = "طلب خدمة جديد",
                    Title = "يتوافق الطلب رقم <id># مع خدماتك. اضغط هنا للاطلاع على التفاصيل ",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 10,
                    ForClient = false,
                    Type = "انتهاء الطلب",
                    Title = "تم انتهاء الطلب رقم <id># اضغط هنا لتقييم تجربتك مع العميل.", 
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 11,
                    ForClient = false,
                    Type = "الطلب ملغي",
                    Title = "تم إلغاء الطلب رقم <id># وإلغاء الموعد المسجل لتقديم الخدمة.", 
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 12,
                    ForClient = false,
                    Type = "اشعار جديد",
                    Title = "تفاصيل الاشعار.", 
                    CreatedAt = createdAt,
                },
            };

            modelBuilder.Entity<NotificationType>().HasData(types);
        }

        private static void SuspendReasonSeeder(ModelBuilder modelBuilder)
        {
            var types = new List<SuspendReason>
            {
                // Client Types
                new()
                {
                    Id = 1,
                    Reason = "مديونية",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 2,
                    Reason = "انتهاك اتفاقية الاستخدام",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 3,
                    Reason = "إلغاء متكرر",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 4,
                    Reason = "سوء معاملة",
                    CreatedAt = createdAt,
                },
            };

            modelBuilder.Entity<SuspendReason>().HasData(types);
        }


    }
}
