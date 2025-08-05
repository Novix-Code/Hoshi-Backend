using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class WorkerSpecificationView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE VIEW WorkerDetailsView AS
        SELECT 
            cl.ImageURL,
            u.Email,
            u.PhoneNumber,
            cl.LivingCityId,
            cl.Address,
            cl.UserId,
            cl.JobId,
            cl.IsCompany
        FROM AspNetUsers u
        JOIN WorkerSpecifications cl ON u.Id = cl.UserId");
            migrationBuilder.Sql(@"CREATE VIEW JobView AS
                                    SELECT JobTitle, IsDeleted,Id From Jobs ");
            migrationBuilder.Sql(@"CREATE VIEW PortfolioView AS
                                    SELECT FileURL,WorkerId from WorkerPortfolios ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS WorkerDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS JobView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS PortfolioView;");
        }
    }
}
