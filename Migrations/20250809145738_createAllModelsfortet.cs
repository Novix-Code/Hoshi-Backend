using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class createAllModelsfortet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminNotificationFlags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FalgOn = table.Column<bool>(type: "bit", nullable: false),
                    LastActivation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotificationFlags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Archives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForClient = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerGrowthRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientsValue = table.Column<int>(type: "int", nullable: false),
                    WorkersValue = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGrowthRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeGrowthRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrowthValue = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeGrowthRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForClient = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumericalStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumericalStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumericalStatisticsValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentValue = table.Column<double>(type: "float", nullable: false),
                    PercentageValue = table.Column<double>(type: "float", nullable: true),
                    IsIncreased = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastValueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumericalStatisticsValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumericalStatisticsValues_NumericalStatisticsValues_LastValueId",
                        column: x => x.LastValueId,
                        principalTable: "NumericalStatisticsValues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ParentPageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Pages_ParentPageId",
                        column: x => x.ParentPageId,
                        principalTable: "Pages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(type: "float", nullable: false),
                    TitleFirstPart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleSecondPart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    UntilBeUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PromotionFor = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuspendReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspendReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermsAndCondetions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndCondetions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdminNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminNotifications_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletedOrders = table.Column<int>(type: "int", nullable: false),
                    RateRito = table.Column<double>(type: "float", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    Indebtedness = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSpecifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RateValue = table.Column<double>(type: "float", nullable: false),
                    FromClient = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rates_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserCollectionAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertDays = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemovingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollectionAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCollectionAlerts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserOTPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecreteKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOTPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOTPs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerPaymentHistroys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPaymentHistroys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerPaymentHistroys_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerPortfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPortfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerPortfolios_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerRejections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerRejections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerRejections_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    HitLimit = table.Column<bool>(type: "bit", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerWallets_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComplaintSolvingRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalComplaints = table.Column<int>(type: "int", nullable: false),
                    UnderSolvingNumber = table.Column<int>(type: "int", nullable: false),
                    SolvedNumber = table.Column<int>(type: "int", nullable: false),
                    NotSolvedNumber = table.Column<int>(type: "int", nullable: false),
                    ForClient = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComplaintTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintSolvingRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintSolvingRates_ComplaintTypes_ComplaintTypeId",
                        column: x => x.ComplaintTypeId,
                        principalTable: "ComplaintTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CompletedOrders = table.Column<int>(type: "int", nullable: false),
                    RateRito = table.Column<double>(type: "float", nullable: false),
                    IsCompany = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LivingCityId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerSpecifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerSpecifications_Cities_LivingCityId",
                        column: x => x.LivingCityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerSpecifications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserNotifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdminPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminPages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdminPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissionPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissionPages_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryRequestRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryPercentage = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRequestRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryRequestRates_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderComplaetionRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancelledValue = table.Column<int>(type: "int", nullable: false),
                    AssignedValue = table.Column<int>(type: "int", nullable: false),
                    CompletedValue = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderComplaetionRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderComplaetionRates_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiveName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SuspendedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SuspendReasonId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspendedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspendedUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuspendedUsers_SuspendReasons_SuspendReasonId",
                        column: x => x.SuspendReasonId,
                        principalTable: "SuspendReasons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerWalletHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    IsIncome = table.Column<bool>(type: "bit", nullable: false),
                    WorkerWalletId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerWalletHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerWalletHistories_WorkerWallets_WorkerWalletId",
                        column: x => x.WorkerWalletId,
                        principalTable: "WorkerWallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainFees = table.Column<double>(type: "float", nullable: false),
                    MaxFees = table.Column<double>(type: "float", nullable: false),
                    MinFees = table.Column<double>(type: "float", nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: true),
                    IsSpecial = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fees_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobServices_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProposalPrice = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ServicingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalClientCost = table.Column<double>(type: "float", nullable: true),
                    TotalWorkerCost = table.Column<double>(type: "float", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    AppliedPromotionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Promotions_AppliedPromotionId",
                        column: x => x.AppliedPromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PromotionServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionServices_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromotionServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicePercentage = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequestRates_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkerServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerServices_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRevenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRevenues_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ComplaintStatus = table.Column<int>(type: "int", nullable: false),
                    ComplaintTypeId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Complaints_ComplaintTypes_ComplaintTypeId",
                        column: x => x.ComplaintTypeId,
                        principalTable: "ComplaintTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Complaints_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferedPrice = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OfferStatus = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AppliedPromotionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_AspNetUsers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_Promotions_AppliedPromotionId",
                        column: x => x.AppliedPromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderImages_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistory_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderVisits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitPrice = table.Column<double>(type: "float", nullable: false),
                    VisitingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    VisitStatus = table.Column<int>(type: "int", nullable: false),
                    VisitNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderVisits_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PromotionsTaken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionsTaken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionsTaken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromotionsTaken_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromotionsTaken_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromotionsTaken_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPrice = table.Column<double>(type: "float", nullable: false),
                    CommissionFee = table.Column<double>(type: "float", nullable: false),
                    VisitingFee = table.Column<double>(type: "float", nullable: false),
                    CancellationFee = table.Column<double>(type: "float", nullable: false),
                    WorkerPromotionFee = table.Column<double>(type: "float", nullable: false),
                    ClientPromotionFee = table.Column<double>(type: "float", nullable: false),
                    ClientIndebtednessFee = table.Column<double>(type: "float", nullable: false),
                    ClientTotalPrice = table.Column<double>(type: "float", nullable: false),
                    WorkerTotalPrice = table.Column<double>(type: "float", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderVisitId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_OrderVisits_OrderVisitId",
                        column: x => x.OrderVisitId,
                        principalTable: "OrderVisits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityCode", "CityName", "CreatedAt", "Latitude", "Longitude", "ModifiedAt" },
                values: new object[,]
                {
                    { 1, "TRI", "طرابلس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.8872, 13.1913, null },
                    { 2, "BEN", "بنغازي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.116500000000002, 20.0686, null },
                    { 3, "MIS", "مصراتة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.374200000000002, 15.0876, null },
                    { 4, "BAY", "البيضاء", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.756900000000002, 21.755600000000001, null },
                    { 5, "ZAW", "الزاوية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.757300000000001, 12.7278, null },
                    { 6, "SRT", "سرت", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.2089, 16.588699999999999, null },
                    { 7, "SAB", "سبها", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 27.037700000000001, 14.4283, null },
                    { 8, "DER", "درنة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.756900000000002, 22.636700000000001, null },
                    { 9, "TOB", "طبرق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.084000000000003, 23.957899999999999, null },
                    { 10, "AJD", "أجدابيا", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.755400000000002, 20.226299999999998, null },
                    { 11, "GHR", "غريان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.171900000000001, 13.0219, null },
                    { 12, "ZUW", "زوارة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.930799999999998, 12.0831, null },
                    { 13, "KHO", "الخمس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.648600000000002, 14.2607, null },
                    { 14, "SAH", "صبراتة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.793199999999999, 12.488799999999999, null },
                    { 15, "ZLI", "زليتن", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.467399999999998, 14.5687, null },
                    { 16, "MAR", "المرج", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.492800000000003, 20.831299999999999, null },
                    { 17, "RAS", "رأس لانوف", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.499300000000002, 18.566700000000001, null },
                    { 18, "BRE", "البريقة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.409199999999998, 19.5731, null },
                    { 19, "BGW", "بن جواد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.034700000000001, 16.127800000000001, null },
                    { 20, "NOF", "النوفلية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.0608, 16.909700000000001, null },
                    { 21, "GHA", "غات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 24.964700000000001, 10.180300000000001, null },
                    { 22, "MUR", "مرزق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 25.915400000000002, 13.917999999999999, null },
                    { 23, "HON", "هون", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.125800000000002, 15.9474, null },
                    { 24, "WAD", "ودان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.1614, 16.138999999999999, null },
                    { 25, "BRA", "براك", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 27.548300000000001, 14.2706, null },
                    { 26, "UBA", "أوباري", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 26.590699999999998, 12.7719, null },
                    { 27, "TKR", "تكركيبة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 24.84, 10.69, null },
                    { 28, "SHW", "الشويرف", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 27.966699999999999, 12.783300000000001, null },
                    { 29, "TMN", "تمنهنت", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 26.2333, 13.783300000000001, null },
                    { 30, "QTR", "القطرون", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 24.416699999999999, 15.8833, null },
                    { 31, "SHA", "شحات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.823599999999999, 21.8581, null },
                    { 32, "SUS", "سوسة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.866700000000002, 21.966699999999999, null },
                    { 33, "MKH", "المخيلي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.533299999999997, 22.7667, null },
                    { 34, "BAT", "بطة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.700000000000003, 22.366700000000002, null },
                    { 35, "TAZ", "تازربو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 25.449999999999999, 23.183299999999999, null },
                    { 36, "AUG", "أوجلة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.100000000000001, 21.116700000000002, null },
                    { 37, "JAL", "جالو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.033000000000001, 21.550000000000001, null },
                    { 38, "KUF", "الكفرة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 24.178699999999999, 23.3109, null },
                    { 39, "TAS", "تاسيلي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 24.5, 23.5, null },
                    { 40, "RBY", "ربيانة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 24.199999999999999, 23.616700000000002, null },
                    { 41, "NAL", "نالوت", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.8733, 10.984999999999999, null },
                    { 42, "JAD", "جادو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.949999999999999, 9.9666999999999994, null },
                    { 43, "YFR", "يفرن", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.063299999999998, 12.5283, null },
                    { 44, "ZIN", "الزنتان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.931100000000001, 12.2531, null },
                    { 45, "RJB", "الرجبان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.083300000000001, 12.783300000000001, null },
                    { 46, "MZD", "مزدة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.433299999999999, 12.9833, null },
                    { 47, "ASB", "الأصابعة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.616700000000002, 12.7333, null },
                    { 48, "KBA", "كباو", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.916699999999999, 10.1167, null },
                    { 49, "TAR", "ترهونة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.433300000000003, 13.6333, null },
                    { 50, "BWL", "بني وليد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.7547, 13.9869, null },
                    { 51, "MSL", "مسلاتة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.616700000000002, 14.0, null },
                    { 52, "QRB", "القره بوللي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.75, 13.033300000000001, null },
                    { 53, "AJL", "العجيلات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.7667, 12.3667, null },
                    { 54, "RGD", "رقدالين", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.816699999999997, 12.1, null },
                    { 55, "SRM", "صرمان", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.75, 12.566700000000001, null },
                    { 56, "MAY", "الماية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.7333, 12.916700000000001, null },
                    { 57, "ASP", "الآ سبيعة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.700000000000003, 13.15, null },
                    { 58, "GAN", "جنزور", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.866700000000002, 13.033300000000001, null },
                    { 59, "SID", "السيدرة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.649999999999999, 18.433299999999999, null },
                    { 60, "AGH", "أغدامس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.116700000000002, 9.4832999999999998, null },
                    { 61, "RQB", "الرقيبة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.2667, 11.1, null },
                    { 62, "GHD", "غدامس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.133299999999998, 9.5, null },
                    { 63, "BOZ", "بوزريق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.366700000000002, 13.949999999999999, null },
                    { 64, "QWL", "القواليش", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.4833, 14.300000000000001, null },
                    { 65, "MRA", "مرادة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.083300000000001, 13.933299999999999, null },
                    { 66, "HRB", "الحرابة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.366700000000002, 14.433299999999999, null },
                    { 67, "TMS", "تمساح", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 30.966699999999999, 15.533300000000001, null },
                    { 68, "JUF", "الجفرة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.199999999999999, 16.100000000000001, null },
                    { 69, "SOK", "سوكنة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.116700000000002, 15.916700000000001, null },
                    { 70, "ZLA", "زلة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 29.183299999999999, 16.050000000000001, null },
                    { 71, "QRY", "القريات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.666699999999999, 22.333300000000001, null },
                    { 72, "MSD", "مساعد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.616700000000002, 22.083300000000001, null },
                    { 73, "ABR", "الأبرق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.083300000000001, 20.2333, null },
                    { 74, "QMN", "قمينس", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 32.7333, 22.0, null },
                    { 75, "UMR", "أم الرزم", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.416699999999999, 21.633299999999998, null },
                    { 76, "SLM", "السلوم", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.533300000000001, 25.116700000000002, null },
                    { 77, "BRD", "البردي", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.7667, 25.083300000000001, null },
                    { 78, "IMS", "إمساعد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 31.7333, 25.0167, null }
                });

            migrationBuilder.InsertData(
                table: "Fees",
                columns: new[] { "Id", "CreatedAt", "FeeType", "IsDeleted", "IsSpecial", "MainFees", "MaxFees", "MinFees", "ModifiedAt", "ServiceId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 0, false, false, 25.0, 200.0, 25.0, null, null },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, false, false, 15.0, 180.0, 15.0, null, null },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 2, false, false, 18.0, 300.0, 20.0, null, null },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 3, false, false, 0.0, 250.0, 0.0, null, null },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 4, false, false, 10.0, 0.0, 0.0, null, null }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "JobTitle", "ModifiedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "عامل تنظيف", null },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "فني صيانة عامة", null },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "كهربائي", null },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "سباك", null },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "فني تكييف وتبريد", null },
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "دهان", null },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "نجار", null },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "مهندس ديكور", null },
                    { 9, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "بستاني", null },
                    { 10, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "ميكانيكي سيارات", null },
                    { 11, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "سائق توصيل", null },
                    { 12, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "فني كمبيوتر", null },
                    { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "مبرمج", null },
                    { 14, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "حارس أمن", null },
                    { 15, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "فني أنظمة أمان", null },
                    { 16, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "عامل نقل", null },
                    { 17, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "مصمم حدائق", null },
                    { 18, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, "فني أجهزة منزلية", null }
                });

            migrationBuilder.InsertData(
                table: "ServiceCategories",
                columns: new[] { "Id", "CategoryName", "CreatedAt", "IsDeleted", "ModifiedAt" },
                values: new object[,]
                {
                    { 1, "خدمات التنظيف", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 2, "الصيانة والإصلاح", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 3, "الخدمات الكهربائية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 4, "السباكة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 5, "التكييف والتبريد", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 6, "الدهان والديكور", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 7, "النجارة والأثاث", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 8, "البستنة والحدائق", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 9, "خدمات السيارات", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 10, "التوصيل والنقل", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 11, "الخدمات التقنية", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null },
                    { 12, "الأمن والحراسة", new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), false, null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedAt", "ImageURL", "IsDeleted", "JobId", "ModifiedAt", "ServiceCategoryId", "ServiveName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/house-cleaning.jpg", false, null, null, 1, "تنظيف المنازل الشامل" },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/window-cleaning.jpg", false, null, null, 1, "تنظيف النوافذ والزجاج" },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/carpet-cleaning.jpg", false, null, null, 1, "تنظيف السجاد والموكيت" },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/kitchen-bathroom-cleaning.jpg", false, null, null, 1, "تنظيف المطابخ والحمامات" },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/furniture-cleaning.jpg", false, null, null, 1, "تنظيف الأثاث والمفروشات" },
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/appliance-repair.jpg", false, null, null, 2, "صيانة الأجهزة المنزلية" },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/door-window-repair.jpg", false, null, null, 2, "إصلاح الأبواب والنوافذ" },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/generator-maintenance.jpg", false, null, null, 2, "صيانة المولدات الكهربائية" },
                    { 9, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/tile-repair.jpg", false, null, null, 2, "إصلاح البلاط والأرضيات" },
                    { 10, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/electrical-installation.jpg", false, null, null, 3, "تركيب الكهرباء المنزلية" },
                    { 11, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/electrical-repair.jpg", false, null, null, 3, "إصلاح الأعطال الكهربائية" },
                    { 12, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/lighting-installation.jpg", false, null, null, 3, "تركيب الإنارة والثريات" },
                    { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/security-systems.jpg", false, null, null, 3, "تركيب أنظمة الأمان" },
                    { 14, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/plumbing-leak-repair.jpg", false, null, null, 4, "إصلاح تسريبات المياه" },
                    { 15, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/faucet-installation.jpg", false, null, null, 4, "تركيب وصيانة الحنفيات" },
                    { 16, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/drain-cleaning.jpg", false, null, null, 4, "تسليك المجاري والأنابيب" },
                    { 17, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/water-tank-installation.jpg", false, null, null, 4, "تركيب خزانات المياه" },
                    { 18, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/ac-installation.jpg", false, null, null, 5, "تركيب أجهزة التكييف" },
                    { 19, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/ac-maintenance.jpg", false, null, null, 5, "صيانة وتنظيف المكيفات" },
                    { 20, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/refrigerator-repair.jpg", false, null, null, 5, "إصلاح الثلاجات والمجمدات" },
                    { 21, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/interior-painting.jpg", false, null, null, 6, "دهان الجدران الداخلية" },
                    { 22, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/exterior-painting.jpg", false, null, null, 6, "دهان الواجهات الخارجية" },
                    { 23, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/wallpaper-installation.jpg", false, null, null, 6, "تركيب ورق الجدران" },
                    { 24, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/interior-design.jpg", false, null, null, 6, "الديكور والتصميم الداخلي" },
                    { 25, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/furniture-making.jpg", false, null, null, 7, "تفصيل وتركيب الأثاث" },
                    { 26, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/furniture-repair.jpg", false, null, null, 7, "إصلاح الأثاث المكسور" },
                    { 27, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/shelving-installation.jpg", false, null, null, 7, "تركيب الأرفف والخزائن" },
                    { 28, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/garden-design.jpg", false, null, null, 8, "تنسيق وتصميم الحدائق" },
                    { 29, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/tree-trimming.jpg", false, null, null, 8, "قص وتهذيب الأشجار" },
                    { 30, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/planting.jpg", false, null, null, 8, "زراعة النباتات والورود" },
                    { 31, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/car-washing.jpg", false, null, null, 9, "غسيل وتنظيف السيارات" },
                    { 32, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/car-maintenance.jpg", false, null, null, 9, "صيانة السيارات المنزلية" },
                    { 33, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/tire-change.jpg", false, null, null, 9, "تغيير إطارات السيارات" },
                    { 34, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/delivery.jpg", false, null, null, 10, "توصيل الطلبات والمشتريات" },
                    { 35, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/furniture-moving.jpg", false, null, null, 10, "نقل الأثاث والعفش" },
                    { 36, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/personal-transport.jpg", false, null, null, 10, "خدمات النقل الشخصي" },
                    { 37, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/computer-repair.jpg", false, null, null, 11, "تركيب وصيانة الكمبيوتر" },
                    { 38, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/internet-installation.jpg", false, null, null, 11, "تركيب شبكات الإنترنت" },
                    { 39, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/web-development.jpg", false, null, null, 11, "برمجة وتطوير المواقع" },
                    { 40, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/home-security.jpg", false, null, null, 12, "خدمات الحراسة المنزلية" },
                    { 41, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), "/images/services/security-cameras.jpg", false, null, null, 12, "تركيب كاميرات المراقبة" }
                });

            migrationBuilder.InsertData(
                table: "Fees",
                columns: new[] { "Id", "CreatedAt", "FeeType", "IsDeleted", "IsSpecial", "MainFees", "MaxFees", "MinFees", "ModifiedAt", "ServiceId" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 0, false, true, 18.0, 210.0, 19.0, null, 1 },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, false, true, 12.0, 175.0, 15.0, null, 8 },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 2, false, true, 9.0, 100.0, 8.5, null, 15 }
                });

            migrationBuilder.InsertData(
                table: "JobServices",
                columns: new[] { "Id", "CreatedAt", "JobId", "ModifiedAt", "ServiceId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, null, 1 },
                    { 2, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, null, 2 },
                    { 3, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, null, 3 },
                    { 4, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, null, 4 },
                    { 5, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 1, null, 5 },
                    { 6, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 2, null, 6 },
                    { 7, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 2, null, 7 },
                    { 8, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 2, null, 8 },
                    { 9, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 2, null, 9 },
                    { 10, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 3, null, 10 },
                    { 11, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 3, null, 11 },
                    { 12, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 3, null, 12 },
                    { 13, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 3, null, 13 },
                    { 14, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 4, null, 14 },
                    { 15, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 4, null, 15 },
                    { 16, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 4, null, 16 },
                    { 17, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 4, null, 17 },
                    { 18, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 5, null, 18 },
                    { 19, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 5, null, 19 },
                    { 20, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 5, null, 20 },
                    { 21, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 6, null, 21 },
                    { 22, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 6, null, 22 },
                    { 23, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 6, null, 23 },
                    { 24, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 7, null, 25 },
                    { 25, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 7, null, 26 },
                    { 26, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 7, null, 27 },
                    { 27, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 8, null, 24 },
                    { 28, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 8, null, 23 },
                    { 29, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 9, null, 28 },
                    { 30, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 9, null, 29 },
                    { 31, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 9, null, 30 },
                    { 32, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 10, null, 31 },
                    { 33, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 10, null, 32 },
                    { 34, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 10, null, 33 },
                    { 35, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 11, null, 34 },
                    { 36, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 11, null, 36 },
                    { 37, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 12, null, 37 },
                    { 38, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 12, null, 38 },
                    { 39, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 13, null, 39 },
                    { 40, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 14, null, 40 },
                    { 41, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 15, null, 13 },
                    { 42, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 15, null, 41 },
                    { 43, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 16, null, 35 },
                    { 44, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 17, null, 28 },
                    { 45, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 18, null, 6 },
                    { 46, new DateTime(2025, 7, 20, 23, 16, 34, 134, DateTimeKind.Utc), 18, null, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_AdminId",
                table: "AdminNotifications",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminPages_PageId",
                table: "AdminPages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminPages_UserId",
                table: "AdminPages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRequestRates_ServiceCategoryId",
                table: "CategoryRequestRates",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecifications_UserId",
                table: "ClientSpecifications",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRevenues_OrderId",
                table: "CompanyRevenues",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplaintTypeId",
                table: "Complaints",
                column: "ComplaintTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_OrderId",
                table: "Complaints",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_UserId",
                table: "Complaints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintSolvingRates_ComplaintTypeId",
                table: "ComplaintSolvingRates",
                column: "ComplaintTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_ServiceId",
                table: "Fees",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderVisitId",
                table: "Invoices",
                column: "OrderVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_JobServices_JobId",
                table: "JobServices",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobServices_ServiceId_JobId",
                table: "JobServices",
                columns: new[] { "ServiceId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NumericalStatisticsValues_LastValueId",
                table: "NumericalStatisticsValues",
                column: "LastValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_AppliedPromotionId",
                table: "Offers",
                column: "AppliedPromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OrderId",
                table: "Offers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_WorkerId",
                table: "Offers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComplaetionRates_ServiceCategoryId",
                table: "OrderComplaetionRates",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderImages_OrderId",
                table: "OrderImages",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppliedPromotionId",
                table: "Orders",
                column: "AppliedPromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CityId",
                table: "Orders",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ServiceId",
                table: "Orders",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistory_OrderId",
                table: "OrderStatusHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderVisits_OrderId",
                table: "OrderVisits",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ParentPageId",
                table: "Pages",
                column: "ParentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_UserId",
                table: "PasswordResetRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPages_PageId_PermissionId",
                table: "PermissionPages",
                columns: new[] { "PageId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPages_PermissionId",
                table: "PermissionPages",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServices_PromotionId",
                table: "PromotionServices",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServices_ServiceId_PromotionId",
                table: "PromotionServices",
                columns: new[] { "ServiceId", "PromotionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionsTaken_OfferId",
                table: "PromotionsTaken",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionsTaken_OrderId",
                table: "PromotionsTaken",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionsTaken_PromotionId",
                table: "PromotionsTaken",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionsTaken_UserId",
                table: "PromotionsTaken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ClientId",
                table: "Rates",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_WorkerId",
                table: "Rates",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestRates_ServiceId",
                table: "ServiceRequestRates",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_JobId",
                table: "Services",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceCategoryId",
                table: "Services",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedUsers_SuspendReasonId",
                table: "SuspendedUsers",
                column: "SuspendReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedUsers_UserId",
                table: "SuspendedUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectionAlerts_UserId",
                table: "UserCollectionAlerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationTypeId",
                table: "UserNotifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOTPs_UserId",
                table: "UserOTPs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId_PermissionId",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPaymentHistroys_WorkerId",
                table: "WorkerPaymentHistroys",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPortfolios_WorkerId",
                table: "WorkerPortfolios",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRejections_WorkerId",
                table: "WorkerRejections",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServices_ServiceId",
                table: "WorkerServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServices_WorkerId_ServiceId",
                table: "WorkerServices",
                columns: new[] { "WorkerId", "ServiceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSpecifications_JobId",
                table: "WorkerSpecifications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSpecifications_LivingCityId",
                table: "WorkerSpecifications",
                column: "LivingCityId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSpecifications_UserId",
                table: "WorkerSpecifications",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWalletHistories_WorkerWalletId",
                table: "WorkerWalletHistories",
                column: "WorkerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWallets_WorkerId",
                table: "WorkerWallets",
                column: "WorkerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminNotificationFlags");

            migrationBuilder.DropTable(
                name: "AdminNotifications");

            migrationBuilder.DropTable(
                name: "AdminPages");

            migrationBuilder.DropTable(
                name: "Archives");

            migrationBuilder.DropTable(
                name: "ArchiveSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CategoryRequestRates");

            migrationBuilder.DropTable(
                name: "ClientSpecifications");

            migrationBuilder.DropTable(
                name: "CompanyRevenues");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "ComplaintSolvingRates");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "CustomerGrowthRates");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "IncomeGrowthRates");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "JobServices");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NumericalStatistics");

            migrationBuilder.DropTable(
                name: "NumericalStatisticsValues");

            migrationBuilder.DropTable(
                name: "OrderComplaetionRates");

            migrationBuilder.DropTable(
                name: "OrderImages");

            migrationBuilder.DropTable(
                name: "OrderStatusHistory");

            migrationBuilder.DropTable(
                name: "PasswordResetRequests");

            migrationBuilder.DropTable(
                name: "PermissionPages");

            migrationBuilder.DropTable(
                name: "PromotionServices");

            migrationBuilder.DropTable(
                name: "PromotionsTaken");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "ServiceRequestRates");

            migrationBuilder.DropTable(
                name: "SuspendedUsers");

            migrationBuilder.DropTable(
                name: "TermsAndCondetions");

            migrationBuilder.DropTable(
                name: "UserCollectionAlerts");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserOTPs");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "WorkerPaymentHistroys");

            migrationBuilder.DropTable(
                name: "WorkerPortfolios");

            migrationBuilder.DropTable(
                name: "WorkerRejections");

            migrationBuilder.DropTable(
                name: "WorkerServices");

            migrationBuilder.DropTable(
                name: "WorkerSpecifications");

            migrationBuilder.DropTable(
                name: "WorkerWalletHistories");

            migrationBuilder.DropTable(
                name: "ComplaintTypes");

            migrationBuilder.DropTable(
                name: "OrderVisits");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "SuspendReasons");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "WorkerWallets");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "ServiceCategories");
        }
    }
}
