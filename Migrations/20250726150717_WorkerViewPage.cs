using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class WorkerViewPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW WorkerPageView AS
                                    SELECT 
                                        ISNULL((SELECT COUNT(*) 
                                         FROM AspNetUserRoles ur 
                                         JOIN AspNetRoles r ON ur.RoleId = r.Id
                                         WHERE r.Name = 'Worker'), 0) AS totalClients,

                                        ISNULL((SELECT COUNT(*) 
                                         FROM AspNetUsers u 
                                         JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                         JOIN AspNetRoles r ON ur.RoleId = r.Id
                                         WHERE r.Name = 'Worker' 
                                         AND u.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE())), 0) AS totalNewClients,

                                        ISNULL((SELECT COUNT(*) 
                                         FROM AspNetUsers u 
                                         JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                         JOIN AspNetRoles r ON ur.RoleId = r.Id
                                         WHERE r.Name = 'Worker' AND u.IsDeleted = 0), 0) AS totalActiveClients");
            migrationBuilder.Sql(@"CREATE VIEW NewWorkerView AS
                                    SELECT 
                                            u.Id,
                                            u.UserName,
                                            u.Email,
                                            u.CreatedAt
                                        FROM AspNetUsers u
                                        JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                        JOIN AspNetRoles r ON ur.RoleId = r.Id
                                        WHERE r.Name = 'Worker'
                                          AND u.CreatedAt >= DATEADD(DAY, -7, GETUTCDATE());");
            migrationBuilder.Sql(@"CREATE VIEW AllWorkertView AS
                                    SELECT 
                                        u.Id,
                                        u.UserName,
                                        u.Email,
                                        u.CreatedAt
                                    FROM AspNetUsers u
                                    JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                                    JOIN AspNetRoles r ON ur.RoleId = r.Id
                                    WHERE r.Name = 'Worker';");
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
                                    WHERE r.Name = 'Worker';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS WorkerPageView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS NewWorkerView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS AllWorkertView;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS SuspendedWorker;");
        }
    }
}
