using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class testfortest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkerWallets_WorkerId",
                table: "WorkerWallets");

            migrationBuilder.DropIndex(
                name: "IX_WorkerServices_WorkerId",
                table: "WorkerServices");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PromotionServices_ServiceId",
                table: "PromotionServices");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPages_PageId",
                table: "PermissionPages");

            migrationBuilder.DropIndex(
                name: "IX_JobServices_ServiceId",
                table: "JobServices");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWallets_WorkerId",
                table: "WorkerWallets",
                column: "WorkerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServices_WorkerId_ServiceId",
                table: "WorkerServices",
                columns: new[] { "WorkerId", "ServiceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId_PermissionId",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServices_ServiceId_PromotionId",
                table: "PromotionServices",
                columns: new[] { "ServiceId", "PromotionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPages_PageId_PermissionId",
                table: "PermissionPages",
                columns: new[] { "PageId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobServices_ServiceId_JobId",
                table: "JobServices",
                columns: new[] { "ServiceId", "JobId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkerWallets_WorkerId",
                table: "WorkerWallets");

            migrationBuilder.DropIndex(
                name: "IX_WorkerServices_WorkerId_ServiceId",
                table: "WorkerServices");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_UserId_PermissionId",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PromotionServices_ServiceId_PromotionId",
                table: "PromotionServices");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPages_PageId_PermissionId",
                table: "PermissionPages");

            migrationBuilder.DropIndex(
                name: "IX_JobServices_ServiceId_JobId",
                table: "JobServices");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerWallets_WorkerId",
                table: "WorkerWallets",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServices_WorkerId",
                table: "WorkerServices",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionServices_ServiceId",
                table: "PromotionServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPages_PageId",
                table: "PermissionPages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_JobServices_ServiceId",
                table: "JobServices",
                column: "ServiceId");
        }
    }
}
