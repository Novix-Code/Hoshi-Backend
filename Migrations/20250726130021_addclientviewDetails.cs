using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class addclientviewDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE VIEW ClientDetailsView AS
        SELECT 
            cl.ImageURL,
            u.Email,
            u.PhoneNumber,
            cl.LivingCityId,
            cl.Address,
            cl.UserId
        FROM AspNetUsers u
        JOIN ClientSpecifications cl ON u.Id = cl.UserId;
    ");
            migrationBuilder.Sql(@" Create View CitiesgetView As
                Select * from Cities");
            migrationBuilder.Sql(@" Create View OrdersGetView As
                Select * from Orders");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS CitiesgetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS OrdersGetView;");
        }
    }
}
