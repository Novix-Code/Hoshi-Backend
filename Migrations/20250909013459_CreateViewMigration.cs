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

            // View: OverviewView — Shows global statistics (users, orders, revenue)
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


            // View: ClientPageView — Shows general client statistics
            migrationBuilder.Sql(
                @"CREATE VIEW ClientPageView AS
                    SELECT 
                        (
                            SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Client'
                        ) AS TotalClients,

                        (
                            SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Client'
                            AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        ) AS TotalNewClientsThisMonth,

                       (
                            SELECT COUNT(*) 
                            FROM AspNetUsers u
                            WHERE u.UserType = 'Client'
                                AND u.IsDeleted = 0
                                AND NOT EXISTS (
                                    SELECT 1 
                                    FROM SuspendedUsers s
                                    WHERE s.UserId = u.Id 
                                )
                        ) AS TotalActiveClients,

                        ISNULL((SELECT AVG(OrderCount) AS AvgOrdersPerClient
                            FROM (
                                SELECT COUNT(*) AS OrderCount
                                FROM Orders o
                                GROUP BY o.ClientId
                            ) AS ClientOrders
                        ), 0) AS AverageOrdering

                    FROM (SELECT 1 AS Dummy) AS Base;"
            );

            // View: NewClientView — Shows clients registered this month
            migrationBuilder.Sql(
                @"CREATE VIEW NewClientView AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        cs.Address,
                        u.CreatedAt
                    FROM AspNetUsers u
                    JOIN ClientSpecifications cs ON u.Id = cs.UserId
                    WHERE u.UserType = 'Client'
                        AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        AND u.IsDeleted = 0;"
            );

            // View: AllClientView — Shows all users of type 'Client'
            migrationBuilder.Sql(
                @"CREATE VIEW AllClientView AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        cs.Address,
                        cs.RateRito,
                        cs.CompletedOrders,
                        (SELECT COUNT(*) FROM Orders o WHERE o.ClientId = u.Id and o.OrderStatus = 'Cancelled') AS CancellationNumber,
                        cs.Balance,
                        u.CreatedAt
                    FROM AspNetUsers u
                    JOIN ClientSpecifications cs ON u.Id = cs.UserId
                    WHERE u.UserType = 'Client';"
            );

            // View: SuspendedUser — Shows suspended clients
            migrationBuilder.Sql(
                @"CREATE VIEW SuspendedUser AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        cs.Address,
                        cs.RateRito,
                        cs.CompletedOrders,
                        (SELECT COUNT(*) FROM Orders o WHERE o.ClientId = u.Id and o.OrderStatus = 'Cancelled') AS CancellationNumber,
                        cs.Balance,
                        sr.Reason,
                        su.CreatedAt
                    FROM AspNetUsers u
                    JOIN ClientSpecifications cs ON u.Id = cs.UserId
                    JOIN SuspendedUsers su ON u.Id = su.UserId
                    JOIN SuspendReasons sr ON su.SuspendReasonId = sr.Id
                    WHERE u.UserType = 'Client';"
            );

            // View: ClientDetailsView — Shows detailed info about clients and their specifications
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


            // View: WorkerPageView — Shows general worker statistics
            migrationBuilder.Sql(
                @"CREATE VIEW WorkerPageView AS
                    SELECT 
                        (SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Worker'
                        ) AS TotalWorkers,

                        (
                            SELECT COUNT(*) 
                            FROM AspNetUsers u 
                            WHERE u.UserType = 'Worker'
                                AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        ) AS TotalNewWorkers,

                        (
                            SELECT COUNT(*) 
                            FROM AspNetUsers u
                            WHERE u.UserType = 'Worker'
                                AND u.IsDeleted = 0
                                AND NOT EXISTS (
                                    SELECT 1 
                                    FROM SuspendedUsers s
                                    WHERE s.UserId = u.Id 
                                )
                        ) AS TotalActiveWorkers,

                        ISNULL((
                            SELECT CAST(ROUND(AVG(WorkersServices * 1.0), 0) AS INT) 
                            FROM (
                                SELECT COUNT(ws.WorkerId) AS WorkersServices 
                                FROM WorkerServices ws GROUP BY ws.ServiceId
                            ) AS Base
                        ), 0) AS AverageWorkersPerService;"
            );

            // View: NewWorkerView — Shows workers registered this month
            migrationBuilder.Sql(
                @"CREATE VIEW NewWorkerView AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        c.CityName,
                        ws.Address,
                        j.JobTitle,
                        ws.IsCompany,
                        u.CreatedAt
                    FROM AspNetUsers u
                    JOIN WorkerSpecifications ws ON u.Id = ws.UserId AND ws.IsApproved IS NULL
                    JOIN Cities c ON ws.LivingCityId = c.Id
                    JOIN Jobs j ON ws.JobId = j.Id
                    WHERE u.UserType = 'Worker'
                        AND u.CreatedAt >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
                        AND u.IsDeleted = 0;"
            );

            // View: AllWorkersView — Shows all users of type 'Worker'
            migrationBuilder.Sql(
                @"CREATE VIEW AllWorkersView AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        j.JobTitle,
                        c.CityName,
                        ws.RateRito,
                        ws.CompletedOrders,
                        (SELECT COUNT(*) FROM Orders o WHERE o.WorkerId = u.Id and o.OrderStatus = 'Cancelled') AS CancellationNumber,
                        ws.IsCompany,
                        ww.Balance,
                        u.CreatedAt
                    FROM AspNetUsers u
                    JOIN WorkerSpecifications ws ON u.Id = ws.UserId AND ws.IsApproved = 1
                    JOIN Cities c ON ws.LivingCityId = c.Id
                    JOIN Jobs j ON ws.JobId = j.Id
                    JOIN WorkerWallets ww ON u.Id = ww.WorkerId
                    WHERE u.UserType = 'Worker';"
            );

            // View: SuspendedWorker — Shows suspended workers
            migrationBuilder.Sql(
                @"CREATE VIEW SuspendedWorker AS
                    SELECT 
                        u.Id,
                        u.FullName,
                        u.ImageURL,
                        u.Email,
                        u.PhoneNumber,
                        ws.Address,
                        j.JobTitle,
                        ws.RateRito,
                        (SELECT COUNT(*) FROM Orders o WHERE o.WorkerId = u.Id and o.OrderStatus = 'Cancelled') AS CancellationNumber,
                        ws.IsCompany,
                        ww.Balance,
                        sr.Reason,
                        su.CreatedAt
                    FROM AspNetUsers u
                    JOIN WorkerSpecifications ws ON u.Id = ws.UserId
                    JOIN Cities c ON ws.LivingCityId = c.Id
                    JOIN Jobs j ON ws.JobId = j.Id
                    JOIN WorkerWallets ww ON u.Id = ww.WorkerId
                    JOIN SuspendedUsers su ON u.Id = su.UserId
                    JOIN SuspendReasons sr ON su.SuspendReasonId = sr.Id
                    WHERE u.UserType = 'Worker';"
            );

            // View: WorkerDetailsView — Shows detailed info about workers and their specifications
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


            // View: OrderDetailsView — Shows full order details
            migrationBuilder.Sql(
                @"CREATE VIEW OrderDetailsView AS
                    SELECT 
                        o.Id,
                        o.Description,
                        o.ProposalPrice,
                        o.ServicingDateTime,
                        o.OrderStatus,
                        o.ClientId,
                        u.FullName,
                        u.Email,
                        u.ImageURL, 
                        c.CityName,
                        s.ServiveName
                    FROM Orders o
                    JOIN Cities c ON o.CityId = c.Id
                    JOIN Services s ON o.ServiceId = s.Id
                    JOIN AspNetUsers u ON o.ClientId = u.Id;"
            );

            // View: ActiveOrdersView - Show details for Published, InProgress and Assigned orders
            migrationBuilder.Sql(
                @"CREATE VIEW ActiveOrdersView AS
                    SELECT *
                    FROM OrderDetailsView o
                    WHERE o.OrderStatus = 'Published' OR o.OrderStatus = 'InProgress' OR o.OrderStatus = 'Assigned';"
            );

            // View: FinishedOrdersView - Show details for Completed and Cancelled orders
            migrationBuilder.Sql(
                @"CREATE VIEW FinishedOrdersView AS
                    SELECT *
                    FROM OrderDetailsView o
                    WHERE o.OrderStatus = 'Completed' OR o.OrderStatus = 'Cancelled';"
            );

            // View: OrdersPageView - Shows orders page statistics
            migrationBuilder.Sql(
                @"CREATE VIEW OrdersPageView AS
                    SELECT 
                        (
                            SELECT COUNT(*) 
                            FROM OrderDetailsView
                        ) AS TotalOrders,

                        (
                            SELECT COUNT(*) 
                            FROM ActiveOrdersView
                        ) AS TotalActiveOrders,

                        (
                            SELECT COUNT(*) 
                            FROM FinishedOrdersView o 
                            WHERE o.OrderStatus = 'Completed'
                        ) AS TotalCompletedOrders,

                        (
                            SELECT COUNT(*) 
                            FROM FinishedOrdersView o 
                            WHERE o.OrderStatus = 'Cancelled'
                        ) AS TotalCancelledOrders;"
            );

            // View: JobsTableView - Shows all Jobs data
            migrationBuilder.Sql(
                @"CREATE VIEW JobsTableView AS
                    SELECT 
                        j.JobTitle,
                        COUNT(DISTINCT s.ServiceCategoryId) AS TotalRelatedCategories,
                        COALESCE(ws.TotalRelatedWorkers, 0) AS TotalRelatedWorkers,
                        COALESCE(o.IncomeAvg, 0) AS IncomeAvg
                    FROM 
                        JobServices js
                        INNER JOIN Jobs j ON js.JobId = j.Id
                        INNER JOIN Services s ON js.ServiceId = s.Id
                        INNER JOIN ServiceCategories sc ON s.ServiceCategoryId = sc.Id
                        LEFT JOIN (
                            -- Subquery to calculate average income per job
                            SELECT 
                                js2.JobId,
                                AVG(i.OrderPrice) AS IncomeAvg
                            FROM 
                                JobServices js2
                                INNER JOIN Orders ord ON js2.ServiceId = ord.ServiceId
                                INNER JOIN Invoices i ON i.OrderId = ord.Id
                            GROUP BY 
                                js2.JobId
                        ) o ON j.Id = o.JobId
                        LEFT JOIN (
                            -- Subquery to count workers per job
                            SELECT 
                                JobId,
                                COUNT(*) AS TotalRelatedWorkers
                            FROM 
                                WorkerSpecifications
                            GROUP BY 
                                JobId
                        ) ws ON j.Id = ws.JobId
                    GROUP BY 
                        j.Id, 
                        j.JobTitle, 
                        ws.TotalRelatedWorkers, 
                        o.IncomeAvg;
                "
            );

            // View: CategoriesTableView - Shows all Categories data
            migrationBuilder.Sql(
                @"CREATE VIEW CategoriesTableView AS
                    SELECT 
                        sc.CategoryName, 
                        COUNT(s.Id) AS ServicesNum, 
                        COALESCE(w.WorkersNum, 0) AS WorkersNum,
                        COALESCE(o.IncomeAvg, 0) AS IncomeAvg
                    FROM ServiceCategories sc 
                        JOIN Services s ON s.ServiceCategoryId = sc.Id
                        LEFT JOIN (
                            SELECT 
                                s.ServiceCategoryId, 
                                COUNT(ws.Id) AS WorkersNum
                            FROM WorkerServices ws 
                            RIGHT JOIN Services s ON ws.ServiceId = s.Id 
                            GROUP BY s.ServiceCategoryId
                        ) w ON w.ServiceCategoryId = sc.Id
                        LEFT JOIN (
                            SELECT
                                s.ServiceCategoryId,
                                AVG(i.OrderPrice) AS IncomeAvg
                            FROM Services s 
                            JOIN Orders o ON o.ServiceId = s.Id
                            JOIN Invoices i ON i.OrderId = o.Id
                            GROUP BY s.ServiceCategoryId
                        ) o ON o.ServiceCategoryId = sc.Id
                    GROUP BY 
                        sc.Id, 
                        sc.CategoryName, 
                        w.WorkersNum,
                        o.IncomeAvg;
                "
            );

            // View: ServicesTableView - Shows all Services data
            migrationBuilder.Sql(
                @"CREATE VIEW ServicesTableView AS
                    SELECT 
                        s.ServiveName,
                        c.CategoryName,
                        s.IsDeleted,
                        Count(o.Id) as OrdersNum,
                        COALESCE(w.WorkersNum, 0) AS WorkersNum,
                        COALESCE(i.IncomeAvg, 0) AS IncomeAvg
                    FROM Services s
                        LEFT JOIN Orders o ON o.ServiceId = s.Id
                        LEFT JOIN (
                            SELECT 
                                ws.ServiceId, 
                                COUNT(ws.Id) AS WorkersNum
                            FROM WorkerServices ws 
                            GROUP BY ws.ServiceId
                        ) w ON w.ServiceId = s.Id
                        LEFT JOIN (
                            SELECT
                                o.ServiceId,
                                AVG(inv.OrderPrice) AS IncomeAvg
                            FROM Orders o 
                            JOIN Invoices inv ON inv.OrderId = o.Id
                            GROUP BY o.ServiceId
                        ) i ON i.ServiceId = s.Id
                        LEFT JOIN (
                            SELECT 
                                s.Id AS ServiceId,
                                c.CategoryName
                            FROM Services s
                            JOIN ServiceCategories c ON c.Id = s.ServiceCategoryId
                        ) c ON c.ServiceId = s.Id
                    GROUP BY 
                        s.Id, 
                        s.ServiveName,
                        c.CategoryName,
                        s.IsDeleted,
                        w.WorkersNum,
                        i.IncomeAvg;
                "
            );

            // View: PortfolioView — Shows workers’ portfolio files
            migrationBuilder.Sql(
                @"CREATE VIEW PortfolioView AS
                    SELECT FileURL, WorkerId FROM WorkerPortfolios;"
            );

            // View: CitiesgetView — Shows all cities from the Cities table
            migrationBuilder.Sql(
                @"CREATE VIEW CitiesgetView AS
                    SELECT * FROM Cities;"
            );

            // View: JobView — Shows basic job information
            migrationBuilder.Sql(
                @"CREATE VIEW JobView AS
                    SELECT JobTitle, IsDeleted, Id FROM Jobs;"
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
