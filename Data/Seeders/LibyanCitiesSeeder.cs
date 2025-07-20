using Hoshi.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.Seeders
{
    public static class LibyanCitiesSeeder
    {
        public static void SeedLibyanCities(ModelBuilder modelBuilder)
        {
            var cities = new List<City>
            {
                // Major Cities
                new City
                {
                    Id = 1,
                    CityName = "طرابلس",
                    CityCode = "TRI",
                    Latitude = 32.8872,
                    Longitude = 13.1913,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 2,
                    CityName = "بنغازي",
                    CityCode = "BEN",
                    Latitude = 32.1165,
                    Longitude = 20.0686,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 3,
                    CityName = "مصراتة",
                    CityCode = "MIS",
                    Latitude = 32.3742,
                    Longitude = 15.0876,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 4,
                    CityName = "البيضاء",
                    CityCode = "BAY",
                    Latitude = 32.7569,
                    Longitude = 21.7556,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 5,
                    CityName = "الزاوية",
                    CityCode = "ZAW",
                    Latitude = 32.7573,
                    Longitude = 12.7278,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 6,
                    CityName = "سرت",
                    CityCode = "SRT",
                    Latitude = 31.2089,
                    Longitude = 16.5887,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 7,
                    CityName = "سبها",
                    CityCode = "SAB",
                    Latitude = 27.0377,
                    Longitude = 14.4283,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 8,
                    CityName = "درنة",
                    CityCode = "DER",
                    Latitude = 32.7569,
                    Longitude = 22.6367,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 9,
                    CityName = "طبرق",
                    CityCode = "TOB",
                    Latitude = 32.0840,
                    Longitude = 23.9579,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 10,
                    CityName = "أجدابيا",
                    CityCode = "AJD",
                    Latitude = 30.7554,
                    Longitude = 20.2263,
                    CreatedAt = DateTime.Now
                },

                // Coastal Cities
                new City
                {
                    Id = 11,
                    CityName = "غريان",
                    CityCode = "GHR",
                    Latitude = 32.1719,
                    Longitude = 13.0219,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 12,
                    CityName = "زوارة",
                    CityCode = "ZUW",
                    Latitude = 32.9308,
                    Longitude = 12.0831,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 13,
                    CityName = "الخمس",
                    CityCode = "KHO",
                    Latitude = 32.6486,
                    Longitude = 14.2607,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 14,
                    CityName = "صبراتة",
                    CityCode = "SAH",
                    Latitude = 32.7932,
                    Longitude = 12.4888,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 15,
                    CityName = "زليتن",
                    CityCode = "ZLI",
                    Latitude = 32.4674,
                    Longitude = 14.5687,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 16,
                    CityName = "المرج",
                    CityCode = "MAR",
                    Latitude = 32.4928,
                    Longitude = 20.8313,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 17,
                    CityName = "رأس لانوف",
                    CityCode = "RAS",
                    Latitude = 30.4993,
                    Longitude = 18.5667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 18,
                    CityName = "البريقة",
                    CityCode = "BRE",
                    Latitude = 30.4092,
                    Longitude = 19.5731,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 19,
                    CityName = "بن جواد",
                    CityCode = "BGW",
                    Latitude = 31.0347,
                    Longitude = 16.1278,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 20,
                    CityName = "النوفلية",
                    CityCode = "NOF",
                    Latitude = 31.0608,
                    Longitude = 16.9097,
                    CreatedAt = DateTime.Now
                },
                // Interior Cities - Fezzan Region
                new City
                {
                    Id = 21,
                    CityName = "غات",
                    CityCode = "GHA",
                    Latitude = 24.9647,
                    Longitude = 10.1803,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 22,
                    CityName = "مرزق",
                    CityCode = "MUR",
                    Latitude = 25.9154,
                    Longitude = 13.9180,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 23,
                    CityName = "هون",
                    CityCode = "HON",
                    Latitude = 29.1258,
                    Longitude = 15.9474,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 24,
                    CityName = "ودان",
                    CityCode = "WAD",
                    Latitude = 29.1614,
                    Longitude = 16.1390,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 25,
                    CityName = "براك",
                    CityCode = "BRA",
                    Latitude = 27.5483,
                    Longitude = 14.2706,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 26,
                    CityName = "أوباري",
                    CityCode = "UBA",
                    Latitude = 26.5907,
                    Longitude = 12.7719,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 27,
                    CityName = "تكركيبة",
                    CityCode = "TKR",
                    Latitude = 24.8400,
                    Longitude = 10.6900,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 28,
                    CityName = "الشويرف",
                    CityCode = "SHW",
                    Latitude = 27.9667,
                    Longitude = 12.7833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 29,
                    CityName = "تمنهنت",
                    CityCode = "TMN",
                    Latitude = 26.2333,
                    Longitude = 13.7833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 30,
                    CityName = "القطرون",
                    CityCode = "QTR",
                    Latitude = 24.4167,
                    Longitude = 15.8833,
                    CreatedAt = DateTime.Now
                },
                // Cyrenaica Region
                new City
                {
                    Id = 31,
                    CityName = "شحات",
                    CityCode = "SHA",
                    Latitude = 32.8236,
                    Longitude = 21.8581,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 32,
                    CityName = "سوسة",
                    CityCode = "SUS",
                    Latitude = 32.8667,
                    Longitude = 21.9667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 33,
                    CityName = "المخيلي",
                    CityCode = "MKH",
                    Latitude = 32.5333,
                    Longitude = 22.7667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 34,
                    CityName = "بطة",
                    CityCode = "BAT",
                    Latitude = 32.7000,
                    Longitude = 22.3667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 35,
                    CityName = "تازربو",
                    CityCode = "TAZ",
                    Latitude = 25.4500,
                    Longitude = 23.1833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 36,
                    CityName = "أوجلة",
                    CityCode = "AUG",
                    Latitude = 29.1000,
                    Longitude = 21.1167,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 37,
                    CityName = "جالو",
                    CityCode = "JAL",
                    Latitude = 29.0330,
                    Longitude = 21.5500,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 38,
                    CityName = "الكفرة",
                    CityCode = "KUF",
                    Latitude = 24.1787,
                    Longitude = 23.3109,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 39,
                    CityName = "تاسيلي",
                    CityCode = "TAS",
                    Latitude = 24.5000,
                    Longitude = 23.5000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 40,
                    CityName = "ربيانة",
                    CityCode = "RBY",
                    Latitude = 24.2000,
                    Longitude = 23.6167,
                    CreatedAt = DateTime.Now
                },
                // Western Mountains Region
                new City
                {
                    Id = 41,
                    CityName = "نالوت",
                    CityCode = "NAL",
                    Latitude = 31.8733,
                    Longitude = 10.9850,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 42,
                    CityName = "جادو",
                    CityCode = "JAD",
                    Latitude = 31.9500,
                    Longitude = 9.9667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 43,
                    CityName = "يفرن",
                    CityCode = "YFR",
                    Latitude = 32.0633,
                    Longitude = 12.5283,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 44,
                    CityName = "الزنتان",
                    CityCode = "ZIN",
                    Latitude = 31.9311,
                    Longitude = 12.2531,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 45,
                    CityName = "الرجبان",
                    CityCode = "RJB",
                    Latitude = 32.0833,
                    Longitude = 12.7833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 46,
                    CityName = "مزدة",
                    CityCode = "MZD",
                    Latitude = 31.4333,
                    Longitude = 12.9833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 47,
                    CityName = "الأصابعة",
                    CityCode = "ASB",
                    Latitude = 31.6167,
                    Longitude = 12.7333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 48,
                    CityName = "كباو",
                    CityCode = "KBA",
                    Latitude = 31.9167,
                    Longitude = 10.1167,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 49,
                    CityName = "ترهونة",
                    CityCode = "TAR",
                    Latitude = 32.4333,
                    Longitude = 13.6333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 50,
                    CityName = "بني وليد",
                    CityCode = "BWL",
                    Latitude = 31.7547,
                    Longitude = 13.9869,
                    CreatedAt = DateTime.Now
                },
                // Additional Cities and Towns
                new City
                {
                    Id = 51,
                    CityName = "مسلاتة",
                    CityCode = "MSL",
                    Latitude = 32.6167,
                    Longitude = 14.0000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 52,
                    CityName = "القره بوللي",
                    CityCode = "QRB",
                    Latitude = 32.7500,
                    Longitude = 13.0333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 53,
                    CityName = "العجيلات",
                    CityCode = "AJL",
                    Latitude = 32.7667,
                    Longitude = 12.3667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 54,
                    CityName = "رقدالين",
                    CityCode = "RGD",
                    Latitude = 32.8167,
                    Longitude = 12.1000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 55,
                    CityName = "صرمان",
                    CityCode = "SRM",
                    Latitude = 32.7500,
                    Longitude = 12.5667,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 56,
                    CityName = "الماية",
                    CityCode = "MAY",
                    Latitude = 32.7333,
                    Longitude = 12.9167,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 57,
                    CityName = "الآ سبيعة",
                    CityCode = "ASP",
                    Latitude = 32.7000,
                    Longitude = 13.1500,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 58,
                    CityName = "جنزور",
                    CityCode = "GAN",
                    Latitude = 32.8667,
                    Longitude = 13.0333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 59,
                    CityName = "السيدرة",
                    CityCode = "SID",
                    Latitude = 30.6500,
                    Longitude = 18.4333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 60,
                    CityName = "أغدامس",
                    CityCode = "AGH",
                    Latitude = 30.1167,
                    Longitude = 9.4833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 61,
                    CityName = "الرقيبة",
                    CityCode = "RQB",
                    Latitude = 31.2667,
                    Longitude = 11.1000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 62,
                    CityName = "غدامس",
                    CityCode = "GHD",
                    Latitude = 30.1333,
                    Longitude = 9.5000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 63,
                    CityName = "بوزريق",
                    CityCode = "BOZ",
                    Latitude = 32.3667,
                    Longitude = 13.9500,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 64,
                    CityName = "القواليش",
                    CityCode = "QWL",
                    Latitude = 32.4833,
                    Longitude = 14.3000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 65,
                    CityName = "مرادة",
                    CityCode = "MRA",
                    Latitude = 31.0833,
                    Longitude = 13.9333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 66,
                    CityName = "الحرابة",
                    CityCode = "HRB",
                    Latitude = 31.3667,
                    Longitude = 14.4333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 67,
                    CityName = "تمساح",
                    CityCode = "TMS",
                    Latitude = 30.9667,
                    Longitude = 15.5333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 68,
                    CityName = "الجفرة",
                    CityCode = "JUF",
                    Latitude = 29.2000,
                    Longitude = 16.1000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 69,
                    CityName = "سوكنة",
                    CityCode = "SOK",
                    Latitude = 29.1167,
                    Longitude = 15.9167,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 70,
                    CityName = "زلة",
                    CityCode = "ZLA",
                    Latitude = 29.1833,
                    Longitude = 16.0500,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 71,
                    CityName = "القريات",
                    CityCode = "QRY",
                    Latitude = 31.6667,
                    Longitude = 22.3333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 72,
                    CityName = "مساعد",
                    CityCode = "MSD",
                    Latitude = 32.6167,
                    Longitude = 22.0833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 73,
                    CityName = "الأبرق",
                    CityCode = "ABR",
                    Latitude = 32.0833,
                    Longitude = 20.2333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 74,
                    CityName = "قمينس",
                    CityCode = "QMN",
                    Latitude = 32.7333,
                    Longitude = 22.0000,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 75,
                    CityName = "أم الرزم",
                    CityCode = "UMR",
                    Latitude = 31.4167,
                    Longitude = 21.6333,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 76,
                    CityName = "السلوم",
                    CityCode = "SLM",
                    Latitude = 31.5333,
                    Longitude = 25.1167,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 77,
                    CityName = "البردي",
                    CityCode = "BRD",
                    Latitude = 31.7667,
                    Longitude = 25.0833,
                    CreatedAt = DateTime.Now
                },
                new City
                {
                    Id = 78,
                    CityName = "إمساعد",
                    CityCode = "IMS",
                    Latitude = 31.7333,
                    Longitude = 25.0167,
                    CreatedAt = DateTime.Now
                }
            };

            modelBuilder.Entity<City>().HasData(cities);
        }
    }
}
