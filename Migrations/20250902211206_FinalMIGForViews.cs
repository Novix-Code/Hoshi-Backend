using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class FinalMIGForViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP VIEW IF EXISTS WorkerPageView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS WorkerDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS SuspendedWorker;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS SuspendedUser;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS PortfolioView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS OverviewView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS OrdersGetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewWorkerView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewClientView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS JobView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientPageView4;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS CitiesgetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllWorkertView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllClientView;");
            //////////////////////////
            /////////////////////////
            migrationBuilder.Sql(@"CREATE VIEW AllClientView AS
                            SELECT 
                                u.Id,
                                u.UserName,
                                u.Email,
                                u.CreatedAt
                            FROM AspNetUsers u
                            JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                            JOIN AspNetRoles r ON ur.RoleId = r.Id
                            WHERE ur.RoleId=1;");
            migrationBuilder.Sql(@"CREATE VIEW AllWorkertView AS
                            SELECT 
                                u.Id,
                                u.UserName,
                                u.Email,
                                u.CreatedAt
                            FROM AspNetUsers u
                            JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                            JOIN AspNetRoles r ON ur.RoleId = r.Id
                            WHERE ur.RoleId = 3;");
            migrationBuilder.Sql(@" Create View CitiesgetView As
                             Select * from Cities");
            migrationBuilder.Sql(@"
                            CREATE VIEW ClientDetailsView AS
                            SELECT 
                                u.ImageURL,
                                u.Email,
                                u.PhoneNumber,
                                u.FullName,
                                cl.Address,
                                cl.UserId
                            FROM AspNetUsers u
                            JOIN ClientSpecifications cl ON u.Id = cl.UserId;");
            migrationBuilder.Sql(@" CREATE VIEW ClientPageView4 AS
                            SELECT 
                                ISNULL((SELECT COUNT(*) 
                                 FROM AspNetUserRoles ur 
                                 JOIN AspNetRoles r ON ur.RoleId = r.Id
                                 WHERE ur.RoleId = 1), 0) AS totalClients,

                                ISNULL((SELECT COUNT(*) 
                                 FROM AspNetUsers u 
                                 JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                 JOIN AspNetRoles r ON ur.RoleId = r.Id
                                 WHERE ur.RoleId = 1
                                 AND u.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE())), 0) AS totalNewClients,

                                ISNULL((
										SELECT COUNT(*) 
										FROM AspNetUsers u
										JOIN AspNetUserRoles ur ON u.Id = ur.UserId
										JOIN AspNetRoles r ON ur.RoleId = r.Id
										WHERE  ur.RoleId = 1
										  AND u.IsDeleted = 0
										  AND NOT EXISTS (
											  SELECT 1 
											  FROM SuspendedUsers s
											  WHERE s.UserId = u.Id
										  )
									), 0) AS totalActiveClients,


                                ISNULL((SELECT AVG(CAST(ProposalPrice AS FLOAT)) 
                                 FROM Orders 
                                 WHERE ProposalPrice IS NOT NULL), 0) AS AverageOrdering
                                FROM (SELECT 1 AS Dummy) AS Base;");
            migrationBuilder.Sql(@"CREATE VIEW JobView AS
                            SELECT JobTitle, IsDeleted,Id From Jobs");
            migrationBuilder.Sql(@"CREATE VIEW NewClientView AS
                            SELECT 
                                    u.Id,
                                    u.UserName,
                                    u.Email,
                                    u.CreatedAt
                                FROM AspNetUsers u
                                JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                JOIN AspNetRoles r ON ur.RoleId = r.Id
                                WHERE ur.RoleId = 1
                                  AND u.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE());");
            migrationBuilder.Sql(@"CREATE VIEW NewWorkerView AS
                            SELECT 
                                    u.Id,
                                    u.UserName,
                                    u.Email,
                                    u.CreatedAt
                                FROM AspNetUsers u
                                JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                JOIN AspNetRoles r ON ur.RoleId = r.Id
                                WHERE ur.RoleId = 3
                                  AND u.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE());");
            migrationBuilder.Sql(@" CREATE View OrdersGetView As
                            Select Id,
                            CityId, 
                            Description,
                            ProposalPrice,
                            Location,
                            Latitude,
                            Longitude,
                            ServicingDateTime,
                            TotalClientCost,
                            TotalWorkerCost,
                            ClientId,
                            WorkerId,
                            OrderStatus from Orders");
            migrationBuilder.Sql(@"
                            CREATE VIEW OverviewView AS
                            SELECT 
                                (SELECT COUNT(*) FROM AspNetUsers) AS TotalUsers,
                                (SELECT COUNT(*) FROM AspNetUserRoles WHERE RoleId = 1) AS TotalClients,
                                (SELECT COUNT(*) FROM AspNetUserRoles WHERE RoleId = 3) AS TotalWorkers,
                                (SELECT COUNT(*) FROM Orders) AS TotalOrders,
                                (SELECT COUNT(*) FROM Orders WHERE OrderStatus = 2) AS TotalCompletedOrders,
                                (SELECT SUM(TotalClientCost- TotalWorkerCost - ProposalPrice) FROM Orders) AS TotalOrderIncome,
                                (SELECT SUM(ProposalPrice) FROM Orders) AS TotalOrderPrice;");
            migrationBuilder.Sql(@"CREATE VIEW PortfolioView AS
                            SELECT FileURL,WorkerId from WorkerPortfolios");
            migrationBuilder.Sql(@"CREATE VIEW SuspendedUser AS
                            SELECT 
                                u.Id,
                                u.UserName,
                                u.Email,
                                u.CreatedAt
                            FROM AspNetUsers u
                            JOIN SuspendedUsers sus On u.Id = sus.UserId
                            JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                            JOIN AspNetRoles r ON ur.RoleId = r.Id
                            WHERE ur.RoleId=1;");
            migrationBuilder.Sql(@"CREATE VIEW SuspendedWorker AS
                            SELECT 
                                u.Id,
                                u.UserName,
                                u.Email,
                                u.CreatedAt
                            FROM AspNetUsers u
                            JOIN SuspendedUsers sus On u.Id = sus.UserId
                            JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                            JOIN AspNetRoles r ON ur.RoleId = r.Id
                            WHERE ur.RoleId = 3;");
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
                        JOIN WorkerSpecifications cl ON u.Id = cl.UserId;");
            migrationBuilder.Sql(@"CREATE VIEW WorkerPageView AS
                            SELECT 
                                ISNULL((SELECT COUNT(*) 
                                 FROM AspNetUserRoles ur 
                                 JOIN AspNetRoles r ON ur.RoleId = r.Id
                                 WHERE ur.RoleId = 3), 0) AS totalClients,

                                ISNULL((SELECT COUNT(*) 
                                 FROM AspNetUsers u 
                                 JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                 JOIN AspNetRoles r ON ur.RoleId = r.Id
                                 WHERE ur.RoleId = 3 
                                 AND u.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE())), 0) AS totalNewClients,

                                ISNULL((
									SELECT COUNT(*) 
									FROM AspNetUsers u
									JOIN AspNetUserRoles ur ON u.Id = ur.UserId
									JOIN AspNetRoles r ON ur.RoleId = r.Id
									WHERE ur.RoleId =3
									  AND u.IsDeleted = 0
									  AND NOT EXISTS (
										  SELECT 1 
										  FROM SuspendedUsers s
										  WHERE s.UserId = u.Id
									  )
								), 0) AS totalActiveClients,


								ISNULL(
									CAST((
										SELECT AVG(CAST(OrderCount AS FLOAT)) 
										FROM (
											SELECT COUNT(*) AS OrderCount 
											FROM Orders o 
											JOIN AspNetUsers u ON o.WorkerId = u.Id
											JOIN AspNetUserRoles ur ON u.Id = ur.UserId
											JOIN AspNetRoles r ON ur.RoleId = r.Id
											where ur.RoleId = 3
											GROUP BY o.WorkerId
										) AS WorkerOrders
									) AS FLOAT),
									0.0
								) AS AverageOrdering;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS WorkerPageView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS WorkerDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS SuspendedWorker;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS SuspendedUser;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS PortfolioView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS OverviewView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS OrdersGetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewWorkerView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewClientView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS JobView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientPageView4;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS CitiesgetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllWorkertView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllClientView;");
        }
    }
}
