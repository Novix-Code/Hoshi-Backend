using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class CreateViewMigration : Migration
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
            migrationBuilder.Sql("DROP VIEW IF EXISTS OrderDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewWorkerView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewClientView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS JobView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientPageView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS CitiesgetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllWorkersView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllClientView;");

            migrationBuilder.Sql(
                @"CREATE VIEW AllClientView AS
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.CreatedAt
                    FROM AspNetUsers u
                    WHERE u.UserType = 'Client';"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW AllWorkersView AS
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.CreatedAt
                    FROM AspNetUsers u
                    WHERE u.UserType = 'Worker';"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW CitiesgetView AS
                    SELECT * FROM Cities;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW ClientDetailsView AS
                    SELECT 
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        u.FullName,
                        cl.Address,
                        cl.UserId
                    FROM AspNetUsers u
                    JOIN ClientSpecifications cl ON u.Id = cl.UserId;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW ClientPageView AS
                    SELECT 
                        ISNULL((SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Client'
                        ), 0) AS TotalClients,

                        ISNULL((
                            SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Client'
                            AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        ), 0) AS TotalNewClientsThisMonth,

                        ISNULL((
							SELECT COUNT(*) 
							FROM AspNetUsers u
                            WHERE u.UserType = 'Client'
								AND u.IsDeleted = 0
								AND NOT EXISTS (
									SELECT 1 
									FROM SuspendedUsers s
									WHERE s.UserId = u.Id 
                                )
						), 0) AS TotalActiveClients,

                        ISNULL((SELECT AVG(OrderCount) AS AvgOrdersPerClient
                            FROM (
                                SELECT COUNT(*) AS OrderCount
                                FROM Orders o
                                GROUP BY o.ClientId
                            ) AS ClientOrders
                        ), 0) AS AverageOrdering

                    FROM (SELECT 1 AS Dummy) AS Base;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW JobView AS
                    SELECT JobTitle, IsDeleted,Id FROM Jobs;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW NewClientView AS
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.ImageURL,
                        u.CreatedAt
                    FROM AspNetUsers u
                    WHERE u.UserType = 'Client'
                        AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        AND u.IsDeleted = 0;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW NewWorkerView AS
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.CreatedAt
                    FROM AspNetUsers u
                    WHERE u.UserType = 'Worker'
                        AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        AND u.IsDeleted = 0;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW OrderDetailsView AS
                    SELECT Id,
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
                        OrderStatus 
                    FROM Orders;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW OverviewView AS
                    SELECT 
	                    (SELECT COUNT(*) FROM AspNetUsers u WHERE u.UserType != 'Admin') AS TotalUsers,
	                    (SELECT COUNT(*) FROM AspNetUsers u WHERE u.UserType = 'Client') AS TotalClients,
	                    (SELECT COUNT(*) FROM AspNetUsers u WHERE u.UserType = 'Worker') AS TotalWorkers,
	                    (SELECT COUNT(*) FROM Orders) AS TotalOrders,
	                    (SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Completed') AS TotalCompletedOrders,
	                    (SELECT SUM(i.OrderPrice) 
                            FROM Orders o 
                            JOIN Invoices i on i.OrderId = o.Id AND o.OrderStatus = 'Completed') AS TotalOrderPrice,
	                    (SELECT SUM(i.CommissionFee) 
                            FROM Orders o 
                            JOIN Invoices i on i.OrderId = o.Id AND o.OrderStatus = 'Completed') AS TotalOrderIncome;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW PortfolioView AS
                    SELECT FileURL, WorkerId FROM WorkerPortfolios;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW SuspendedUser AS
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.ImageURL,
                        u.CreatedAt
                    FROM AspNetUsers u
                    JOIN SuspendedUsers sus On u.Id = sus.UserId AND u.UserType = 'Client';"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW SuspendedWorker AS
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.ImageURL,
                        u.CreatedAt
                    FROM AspNetUsers u
                    JOIN SuspendedUsers sus On u.Id = sus.UserId AND u.UserType = 'Worker';"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW WorkerDetailsView AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.Email,
                        u.PhoneNumber,
                        u.ImageURL,
                        ws.Bio,
                        ws.Address,
                        ws.IdentityImageURL,
                        ws.IsCompany,
                        ws.CompletedOrders,
                        ws.RateRito,
                        ws.JobId,
                        ws.LivingCityId
                    FROM AspNetUsers u
                    JOIN WorkerSpecifications ws ON ws.UserId = u.Id;"
            );

            migrationBuilder.Sql(
                @"CREATE VIEW WorkerPageView AS
                    SELECT 
                        ISNULL((SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Worker'
                        ), 0) AS TotalWorkers,

                        ISNULL((
                            SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Worker'
                                AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        ), 0) AS TotalNewWorkers,

                        ISNULL((
							SELECT COUNT(*) 
							FROM AspNetUsers u
                            WHERE u.UserType = 'Worker'
								AND u.IsDeleted = 0
								AND NOT EXISTS (
									SELECT 1 
									FROM SuspendedUsers s
									WHERE s.UserId = u.Id 
                                )
						), 0) AS TotalActiveWorkers,

						ISNULL((
	                        SELECT CAST(ROUND(AVG(WorkersServices * 1.0), 0) AS INT) 
	                        FROM (
		                        SELECT COUNT(ws.WorkerId) AS WorkersServices 
		                        FROM WorkerServices ws GROUP BY ws.ServiceId
	                        ) AS Base
                        ), 0) AS AverageWorkersPerService;"
            );
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
            migrationBuilder.Sql("DROP VIEW IF EXISTS OrderDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewWorkerView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewClientView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS JobView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientPageView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ClientDetailsView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS CitiesgetView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllWorkersView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllClientView;");
        }
    }
}
