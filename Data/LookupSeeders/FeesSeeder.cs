using Hoshi.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.LookupSeeders
{
    public static class FeesSeeder
    {
        private static DateTime createdAt = new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc);

        public static void SeedFees(ModelBuilder modelBuilder)
        {

            List<Fee> fees = new List<Fee>() 
            {
                new Fee()
                {
                    Id = 1,
                    MainFees = 25.0,
                    MinFees = 25.0,
                    MaxFees = 200.0,
                    FeeType = Enums.FeeType.CommissionFee,
                    CreatedAt = createdAt,
                },
                new Fee()
                {
                    Id = 2,
                    MainFees = 15.0,
                    MinFees = 15.0,
                    MaxFees = 180.0,
                    FeeType = Enums.FeeType.VisitingFee,
                    CreatedAt = createdAt,
                },
                new Fee()
                {
                    Id = 3,
                    MainFees = 18.0,
                    MinFees = 20.0,
                    MaxFees = 300.0,
                    FeeType = Enums.FeeType.CancellationFee,
                    CreatedAt = createdAt,
                },
                new Fee()
                {
                    Id = 4,
                    MainFees = 0.0,
                    MinFees = 0.0,
                    MaxFees = 250.0,
                    FeeType = Enums.FeeType.ClientIndebtednessFee,
                    CreatedAt = createdAt,
                },
                new Fee()
                {
                    Id = 5,
                    MainFees = 0.0,
                    MinFees = 0.0,
                    MaxFees = 200.0,
                    FeeType = Enums.FeeType.WorkerIndebtednessFee,
                    CreatedAt = createdAt,
                },
            };

            modelBuilder.Entity<Fee>().HasData(fees);
        }
    }
}
