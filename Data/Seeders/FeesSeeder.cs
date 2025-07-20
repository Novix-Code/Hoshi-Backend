using Hoshi.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.Seeders
{
    public static class FeesSeeder
    {
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
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 2,
                    MainFees = 15.0,
                    MinFees = 15.0,
                    MaxFees = 180.0,
                    FeeType = Enums.FeeType.VisitingFee,
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 3,
                    MainFees = 18.0,
                    MinFees = 20.0,
                    MaxFees = 300.0,
                    FeeType = Enums.FeeType.CancellationFee,
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 4,
                    MainFees = 0.0,
                    MinFees = 0.0,
                    MaxFees = 250.0,
                    FeeType = Enums.FeeType.ClientIndebtednessFee,
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 5,
                    MainFees = 10.0,
                    MinFees = 00.0,
                    MaxFees = 00.0,
                    FeeType = Enums.FeeType.WorkerIndebtednessFee,
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 6,
                    MainFees = 18.0,
                    MinFees = 19.0,
                    MaxFees = 210.0,
                    FeeType = Enums.FeeType.CommissionFee,
                    IsSpecial = true,
                    ServiceId = 1,
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 7,
                    MainFees = 12.0,
                    MinFees = 15.0,
                    MaxFees = 175.0,
                    FeeType = Enums.FeeType.VisitingFee,
                    IsSpecial = true,
                    ServiceId = 8,
                    CreatedAt = DateTime.Now,
                },
                new Fee()
                {
                    Id = 8,
                    MainFees = 9.0,
                    MinFees = 8.5,
                    MaxFees = 100.0,
                    FeeType = Enums.FeeType.CancellationFee,
                    IsSpecial = true,
                    ServiceId = 15,
                    CreatedAt = DateTime.Now,
                },
            };

            modelBuilder.Entity<Fee>().HasData(fees);
        }
    }
}
