using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class SeedNeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityCode", "CityName", "CreatedAt", "Latitude", "Longitude", "ModifiedAt" },
                values: new object[,]
                {
                    { 1, "TRI", "طرابلس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(191), 32.8872, 13.1913, null },
                    { 2, "BEN", "بنغازي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(351), 32.116500000000002, 20.0686, null },
                    { 3, "MIS", "مصراتة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(356), 32.374200000000002, 15.0876, null },
                    { 4, "BAY", "البيضاء", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(360), 32.756900000000002, 21.755600000000001, null },
                    { 5, "ZAW", "الزاوية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(363), 32.757300000000001, 12.7278, null },
                    { 6, "SRT", "سرت", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(383), 31.2089, 16.588699999999999, null },
                    { 7, "SAB", "سبها", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(398), 27.037700000000001, 14.4283, null },
                    { 8, "DER", "درنة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(402), 32.756900000000002, 22.636700000000001, null },
                    { 9, "TOB", "طبرق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(406), 32.084000000000003, 23.957899999999999, null },
                    { 10, "AJD", "أجدابيا", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(411), 30.755400000000002, 20.226299999999998, null },
                    { 11, "GHR", "غريان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(419), 32.171900000000001, 13.0219, null },
                    { 12, "ZUW", "زوارة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(423), 32.930799999999998, 12.0831, null },
                    { 13, "KHO", "الخمس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(426), 32.648600000000002, 14.2607, null },
                    { 14, "SAH", "صبراتة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(430), 32.793199999999999, 12.488799999999999, null },
                    { 15, "ZLI", "زليتن", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(434), 32.467399999999998, 14.5687, null },
                    { 16, "MAR", "المرج", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(461), 32.492800000000003, 20.831299999999999, null },
                    { 17, "RAS", "رأس لانوف", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(464), 30.499300000000002, 18.566700000000001, null },
                    { 18, "BRE", "البريقة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(469), 30.409199999999998, 19.5731, null },
                    { 19, "BGW", "بن جواد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(473), 31.034700000000001, 16.127800000000001, null },
                    { 20, "NOF", "النوفلية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(477), 31.0608, 16.909700000000001, null },
                    { 21, "GHA", "غات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(480), 24.964700000000001, 10.180300000000001, null },
                    { 22, "MUR", "مرزق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(483), 25.915400000000002, 13.917999999999999, null },
                    { 23, "HON", "هون", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(487), 29.125800000000002, 15.9474, null },
                    { 24, "WAD", "ودان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(491), 29.1614, 16.138999999999999, null },
                    { 25, "BRA", "براك", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(494), 27.548300000000001, 14.2706, null },
                    { 26, "UBA", "أوباري", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(497), 26.590699999999998, 12.7719, null },
                    { 27, "TKR", "تكركيبة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(500), 24.84, 10.69, null },
                    { 28, "SHW", "الشويرف", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(505), 27.966699999999999, 12.783300000000001, null },
                    { 29, "TMN", "تمنهنت", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(508), 26.2333, 13.783300000000001, null },
                    { 30, "QTR", "القطرون", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(521), 24.416699999999999, 15.8833, null },
                    { 31, "SHA", "شحات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(563), 32.823599999999999, 21.8581, null },
                    { 32, "SUS", "سوسة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(566), 32.866700000000002, 21.966699999999999, null },
                    { 33, "MKH", "المخيلي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(569), 32.533299999999997, 22.7667, null },
                    { 34, "BAT", "بطة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(574), 32.700000000000003, 22.366700000000002, null },
                    { 35, "TAZ", "تازربو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(577), 25.449999999999999, 23.183299999999999, null },
                    { 36, "AUG", "أوجلة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(581), 29.100000000000001, 21.116700000000002, null },
                    { 37, "JAL", "جالو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(585), 29.033000000000001, 21.550000000000001, null },
                    { 38, "KUF", "الكفرة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(588), 24.178699999999999, 23.3109, null },
                    { 39, "TAS", "تاسيلي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(592), 24.5, 23.5, null },
                    { 40, "RBY", "ربيانة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(595), 24.199999999999999, 23.616700000000002, null },
                    { 41, "NAL", "نالوت", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(722), 31.8733, 10.984999999999999, null },
                    { 42, "JAD", "جادو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(727), 31.949999999999999, 9.9666999999999994, null },
                    { 43, "YFR", "يفرن", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(732), 32.063299999999998, 12.5283, null },
                    { 44, "ZIN", "الزنتان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(735), 31.931100000000001, 12.2531, null },
                    { 45, "RJB", "الرجبان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(739), 32.083300000000001, 12.783300000000001, null },
                    { 46, "MZD", "مزدة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(742), 31.433299999999999, 12.9833, null },
                    { 47, "ASB", "الأصابعة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(746), 31.616700000000002, 12.7333, null },
                    { 48, "KBA", "كباو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(749), 31.916699999999999, 10.1167, null },
                    { 49, "TAR", "ترهونة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(753), 32.433300000000003, 13.6333, null },
                    { 50, "BWL", "بني وليد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(757), 31.7547, 13.9869, null },
                    { 51, "MSL", "مسلاتة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(760), 32.616700000000002, 14.0, null },
                    { 52, "QRB", "القره بوللي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(764), 32.75, 13.033300000000001, null },
                    { 53, "AJL", "العجيلات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(767), 32.7667, 12.3667, null },
                    { 54, "RGD", "رقدالين", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(770), 32.816699999999997, 12.1, null },
                    { 55, "SRM", "صرمان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(774), 32.75, 12.566700000000001, null },
                    { 56, "MAY", "الماية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(777), 32.7333, 12.916700000000001, null },
                    { 57, "ASP", "الآ سبيعة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(780), 32.700000000000003, 13.15, null },
                    { 58, "GAN", "جنزور", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(784), 32.866700000000002, 13.033300000000001, null },
                    { 59, "SID", "السيدرة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(787), 30.649999999999999, 18.433299999999999, null },
                    { 60, "AGH", "أغدامس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(790), 30.116700000000002, 9.4832999999999998, null },
                    { 61, "RQB", "الرقيبة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(794), 31.2667, 11.1, null },
                    { 62, "GHD", "غدامس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(797), 30.133299999999998, 9.5, null },
                    { 63, "BOZ", "بوزريق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(801), 32.366700000000002, 13.949999999999999, null },
                    { 64, "QWL", "القواليش", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(804), 32.4833, 14.300000000000001, null },
                    { 65, "MRA", "مرادة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(807), 31.083300000000001, 13.933299999999999, null },
                    { 66, "HRB", "الحرابة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(814), 31.366700000000002, 14.433299999999999, null },
                    { 67, "TMS", "تمساح", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(818), 30.966699999999999, 15.533300000000001, null },
                    { 68, "JUF", "الجفرة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(821), 29.199999999999999, 16.100000000000001, null },
                    { 69, "SOK", "سوكنة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(825), 29.116700000000002, 15.916700000000001, null },
                    { 70, "ZLA", "زلة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(828), 29.183299999999999, 16.050000000000001, null },
                    { 71, "QRY", "القريات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(831), 31.666699999999999, 22.333300000000001, null },
                    { 72, "MSD", "مساعد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(835), 32.616700000000002, 22.083300000000001, null },
                    { 73, "ABR", "الأبرق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(838), 32.083300000000001, 20.2333, null },
                    { 74, "QMN", "قمينس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(842), 32.7333, 22.0, null },
                    { 75, "UMR", "أم الرزم", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(845), 31.416699999999999, 21.633299999999998, null },
                    { 76, "SLM", "السلوم", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(848), 31.533300000000001, 25.116700000000002, null },
                    { 77, "BRD", "البردي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(852), 31.7667, 25.083300000000001, null },
                    { 78, "IMS", "إمساعد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(855), 31.7333, 25.0167, null }
                });

            migrationBuilder.InsertData(
                table: "Fees",
                columns: new[] { "Id", "CreatedAt", "FeeType", "IsDeleted", "IsSpecial", "MainFees", "MaxFees", "MinFees", "ModifiedAt", "ServiceId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2057), 0, false, false, 25.0, 200.0, 25.0, null, null },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2063), 1, false, false, 15.0, 180.0, 15.0, null, null },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2067), 2, false, false, 18.0, 300.0, 20.0, null, null },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2070), 3, false, false, 0.0, 250.0, 0.0, null, null },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2074), 4, false, false, 10.0, 0.0, 0.0, null, null }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "JobTitle", "ModifiedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1641), false, "عامل تنظيف", null },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1647), false, "فني صيانة عامة", null },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1650), false, "كهربائي", null },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1653), false, "سباك", null },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1656), false, "فني تكييف وتبريد", null },
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1660), false, "دهان", null },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1663), false, "نجار", null },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1667), false, "مهندس ديكور", null },
                    { 9, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1670), false, "بستاني", null },
                    { 10, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1674), false, "ميكانيكي سيارات", null },
                    { 11, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1677), false, "سائق توصيل", null },
                    { 12, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1680), false, "فني كمبيوتر", null },
                    { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1683), false, "مبرمج", null },
                    { 14, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1686), false, "حارس أمن", null },
                    { 15, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1689), false, "فني أنظمة أمان", null },
                    { 16, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1692), false, "عامل نقل", null },
                    { 17, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1695), false, "مصمم حدائق", null },
                    { 18, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1699), false, "فني أجهزة منزلية", null }
                });

            migrationBuilder.InsertData(
                table: "ServiceCategories",
                columns: new[] { "Id", "CategoryName", "CreatedAt", "IsDeleted", "ModifiedAt" },
                values: new object[,]
                {
                    { 1, "خدمات التنظيف", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1213), false, null },
                    { 2, "الصيانة والإصلاح", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1224), false, null },
                    { 3, "الخدمات الكهربائية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1228), false, null },
                    { 4, "السباكة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1231), false, null },
                    { 5, "التكييف والتبريد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1234), false, null },
                    { 6, "الدهان والديكور", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1239), false, null },
                    { 7, "النجارة والأثاث", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1242), false, null },
                    { 8, "البستنة والحدائق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1245), false, null },
                    { 9, "خدمات السيارات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1248), false, null },
                    { 10, "التوصيل والنقل", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1253), false, null },
                    { 11, "الخدمات التقنية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1256), false, null },
                    { 12, "الأمن والحراسة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1259), false, null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedAt", "ImageURL", "IsDeleted", "JobId", "ModifiedAt", "ServiceCategoryId", "ServiveName", "WorkerSpecificationId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1398), "/images/services/house-cleaning.jpg", false, null, null, 1, "تنظيف المنازل الشامل", null },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1404), "/images/services/window-cleaning.jpg", false, null, null, 1, "تنظيف النوافذ والزجاج", null },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1408), "/images/services/carpet-cleaning.jpg", false, null, null, 1, "تنظيف السجاد والموكيت", null },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1411), "/images/services/kitchen-bathroom-cleaning.jpg", false, null, null, 1, "تنظيف المطابخ والحمامات", null },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1415), "/images/services/furniture-cleaning.jpg", false, null, null, 1, "تنظيف الأثاث والمفروشات", null },
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1419), "/images/services/appliance-repair.jpg", false, null, null, 2, "صيانة الأجهزة المنزلية", null },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1422), "/images/services/door-window-repair.jpg", false, null, null, 2, "إصلاح الأبواب والنوافذ", null },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1426), "/images/services/generator-maintenance.jpg", false, null, null, 2, "صيانة المولدات الكهربائية", null },
                    { 9, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1429), "/images/services/tile-repair.jpg", false, null, null, 2, "إصلاح البلاط والأرضيات", null },
                    { 10, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1434), "/images/services/electrical-installation.jpg", false, null, null, 3, "تركيب الكهرباء المنزلية", null },
                    { 11, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1437), "/images/services/electrical-repair.jpg", false, null, null, 3, "إصلاح الأعطال الكهربائية", null },
                    { 12, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1441), "/images/services/lighting-installation.jpg", false, null, null, 3, "تركيب الإنارة والثريات", null },
                    { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1444), "/images/services/security-systems.jpg", false, null, null, 3, "تركيب أنظمة الأمان", null },
                    { 14, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1447), "/images/services/plumbing-leak-repair.jpg", false, null, null, 4, "إصلاح تسريبات المياه", null },
                    { 15, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1450), "/images/services/faucet-installation.jpg", false, null, null, 4, "تركيب وصيانة الحنفيات", null },
                    { 16, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1454), "/images/services/drain-cleaning.jpg", false, null, null, 4, "تسليك المجاري والأنابيب", null },
                    { 17, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1458), "/images/services/water-tank-installation.jpg", false, null, null, 4, "تركيب خزانات المياه", null },
                    { 18, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1462), "/images/services/ac-installation.jpg", false, null, null, 5, "تركيب أجهزة التكييف", null },
                    { 19, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1465), "/images/services/ac-maintenance.jpg", false, null, null, 5, "صيانة وتنظيف المكيفات", null },
                    { 20, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1469), "/images/services/refrigerator-repair.jpg", false, null, null, 5, "إصلاح الثلاجات والمجمدات", null },
                    { 21, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1473), "/images/services/interior-painting.jpg", false, null, null, 6, "دهان الجدران الداخلية", null },
                    { 22, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1476), "/images/services/exterior-painting.jpg", false, null, null, 6, "دهان الواجهات الخارجية", null },
                    { 23, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1481), "/images/services/wallpaper-installation.jpg", false, null, null, 6, "تركيب ورق الجدران", null },
                    { 24, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1484), "/images/services/interior-design.jpg", false, null, null, 6, "الديكور والتصميم الداخلي", null },
                    { 25, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1487), "/images/services/furniture-making.jpg", false, null, null, 7, "تفصيل وتركيب الأثاث", null },
                    { 26, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1491), "/images/services/furniture-repair.jpg", false, null, null, 7, "إصلاح الأثاث المكسور", null },
                    { 27, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1494), "/images/services/shelving-installation.jpg", false, null, null, 7, "تركيب الأرفف والخزائن", null },
                    { 28, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1497), "/images/services/garden-design.jpg", false, null, null, 8, "تنسيق وتصميم الحدائق", null },
                    { 29, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1500), "/images/services/tree-trimming.jpg", false, null, null, 8, "قص وتهذيب الأشجار", null },
                    { 30, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1504), "/images/services/planting.jpg", false, null, null, 8, "زراعة النباتات والورود", null },
                    { 31, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1507), "/images/services/car-washing.jpg", false, null, null, 9, "غسيل وتنظيف السيارات", null },
                    { 32, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1511), "/images/services/car-maintenance.jpg", false, null, null, 9, "صيانة السيارات المنزلية", null },
                    { 33, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1514), "/images/services/tire-change.jpg", false, null, null, 9, "تغيير إطارات السيارات", null },
                    { 34, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1518), "/images/services/delivery.jpg", false, null, null, 10, "توصيل الطلبات والمشتريات", null },
                    { 35, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1522), "/images/services/furniture-moving.jpg", false, null, null, 10, "نقل الأثاث والعفش", null },
                    { 36, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1525), "/images/services/personal-transport.jpg", false, null, null, 10, "خدمات النقل الشخصي", null },
                    { 37, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1528), "/images/services/computer-repair.jpg", false, null, null, 11, "تركيب وصيانة الكمبيوتر", null },
                    { 38, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1532), "/images/services/internet-installation.jpg", false, null, null, 11, "تركيب شبكات الإنترنت", null },
                    { 39, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1535), "/images/services/web-development.jpg", false, null, null, 11, "برمجة وتطوير المواقع", null },
                    { 40, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1538), "/images/services/home-security.jpg", false, null, null, 12, "خدمات الحراسة المنزلية", null },
                    { 41, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1542), "/images/services/security-cameras.jpg", false, null, null, 12, "تركيب كاميرات المراقبة", null }
                });

            migrationBuilder.InsertData(
                table: "Fees",
                columns: new[] { "Id", "CreatedAt", "FeeType", "IsDeleted", "IsSpecial", "MainFees", "MaxFees", "MinFees", "ModifiedAt", "ServiceId" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2080), 0, false, true, 18.0, 210.0, 19.0, null, 1 },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2084), 1, false, true, 12.0, 175.0, 15.0, null, 8 },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(2088), 2, false, true, 9.0, 100.0, 8.5, null, 15 }
                });

            migrationBuilder.InsertData(
                table: "JobServices",
                columns: new[] { "Id", "CreatedAt", "JobId", "ModifiedAt", "ServiceId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1808), 1, null, 1 },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1815), 1, null, 2 },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1818), 1, null, 3 },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1821), 1, null, 4 },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1824), 1, null, 5 },
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1828), 2, null, 6 },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1832), 2, null, 7 },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1835), 2, null, 8 },
                    { 9, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1838), 2, null, 9 },
                    { 10, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1842), 3, null, 10 },
                    { 11, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1845), 3, null, 11 },
                    { 12, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1848), 3, null, 12 },
                    { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1851), 3, null, 13 },
                    { 14, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1854), 4, null, 14 },
                    { 15, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1857), 4, null, 15 },
                    { 16, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1861), 4, null, 16 },
                    { 17, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1864), 4, null, 17 },
                    { 18, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1868), 5, null, 18 },
                    { 19, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1871), 5, null, 19 },
                    { 20, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1874), 5, null, 20 },
                    { 21, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1877), 6, null, 21 },
                    { 22, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1880), 6, null, 22 },
                    { 23, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1883), 6, null, 23 },
                    { 24, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1886), 7, null, 25 },
                    { 25, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1889), 7, null, 26 },
                    { 26, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1892), 7, null, 27 },
                    { 27, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1895), 8, null, 24 },
                    { 28, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1898), 8, null, 23 },
                    { 29, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1901), 9, null, 28 },
                    { 30, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1904), 9, null, 29 },
                    { 31, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1907), 9, null, 30 },
                    { 32, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1910), 10, null, 31 },
                    { 33, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1913), 10, null, 32 },
                    { 34, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1917), 10, null, 33 },
                    { 35, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1920), 11, null, 34 },
                    { 36, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1923), 11, null, 36 },
                    { 37, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1926), 12, null, 37 },
                    { 38, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1929), 12, null, 38 },
                    { 39, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1932), 13, null, 39 },
                    { 40, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1935), 14, null, 40 },
                    { 41, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1938), 15, null, 13 },
                    { 42, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1941), 15, null, 41 },
                    { 43, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1945), 16, null, 35 },
                    { 44, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1948), 17, null, 28 },
                    { 45, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1950), 18, null, 6 },
                    { 46, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Local).AddTicks(1953), 18, null, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "JobServices",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
