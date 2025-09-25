using Hoshi.Models.DashboardModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.TermsAndCondetionsSeeders
{
    public static class TermsAndCondetionsSeeder
    {
        private static DateTime createdAt = new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc);

        public static void Seeder(ModelBuilder modelBuilder)
        {
            List<TermsAndCondetions> terms = new();

#if DEBUG
            string clientContent = File.ReadAllText(@"Data/TermsAndCondetionsSeeders/ClientTermsAndCondetions.txt");
#else
            string clientContent = "";
#endif

            terms.Add(new TermsAndCondetions
            {
                Id = 1,
                Title = "اتفاقية الاستخدام للعملاء",
                ForClient = true,
                Content = clientContent,
                CreatedAt = createdAt,
            });

#if DEBUG
            string workerContent = File.ReadAllText(@"Data/TermsAndCondetionsSeeders/WorkerTermsAndCondetions.txt");
#else
            string workerContent = "";
#endif

            terms.Add(new TermsAndCondetions
            {
                Id = 2,
                Title = "اتفاقية الاستخدام للعمال",
                ForClient = false,
                Content = workerContent,
                CreatedAt = createdAt,
            });

            modelBuilder.Entity<TermsAndCondetions>().HasData(terms);
        }
    }
}
