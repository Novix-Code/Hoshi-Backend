using Hoshi.Enums;
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
            SuspendReasonSeeder(modelBuilder);
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
                new()
                {
                    Id = 1,
                    ForClient = true,
                    Type = NotifType.Other.ToString(),
                    Title = "اشعار جديد",
                    CreatedAt = createdAt,
                },

                // Client Types
                new()
                {
                    Id = 2,
                    ForClient = true,
                    Type = NotifType.Acceptance.ToString(),
                    Title = "الطلب مقبول",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 3,
                    ForClient = true,
                    Type = NotifType.Confirmation.ToString(),
                    Title = "تم تأكيد الطلب",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 4,
                    ForClient = true,
                    Type = NotifType.Assignment.ToString(),
                    Title = "تم تعيين العامل",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 5,
                    ForClient = true,
                    Type = NotifType.Completion.ToString(),
                    Title = "انتهاء الطلب",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 6,
                    ForClient = true,
                    Type = NotifType.Cancellation.ToString(),
                    Title = "الطلب ملغي",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 7,
                    ForClient = true,
                    Type = NotifType.Cancellation.ToString(),
                    Title = "عامل ألغى الموعد",
                    CreatedAt = createdAt,
                },

                // Worker Types
                new()
                {
                    Id = 8,
                    ForClient = false,
                    Type = NotifType.Acceptance.ToString(),
                    Title = "العرض مقبول",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 9,
                    ForClient = false,
                    Type = NotifType.Confirmation.ToString(),
                    Title = "تم تعديل الرصيد",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 10,
                    ForClient = false,
                    Type = NotifType.Assignment.ToString(),
                    Title = "طلب خدمة جديد",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 11,
                    ForClient = false,
                    Type = NotifType.Completion.ToString(),
                    Title = "انتهاء الطلب",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 12,
                    ForClient = false,
                    Type = NotifType.Cancellation.ToString(),
                    Title = "العرض مرفوض",
                    CreatedAt = createdAt,
                },
                new()
                {
                    Id = 13,
                    ForClient = false,
                    Type = NotifType.Cancellation.ToString(),
                    Title = "الطلب ملغي",
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
