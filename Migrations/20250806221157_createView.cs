using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class createView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE VIEW WorkerDetailsView AS
        SELECT 
            u.ImageURL,
            u.Email,
            u.PhoneNumber,
			u.FullName,
            cl.LivingCityId,
            cl.Address,
            cl.UserId,
            cl.JobId,
            cl.IsCompany,
			Bio,
			IdentityImageURL,
			CompletedOrders,
			RateRito
        FROM AspNetUsers u
        JOIN WorkerSpecifications cl ON u.Id = cl.UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW WorkerDetailsView");
        }
    }
}
