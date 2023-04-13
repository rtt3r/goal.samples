using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goal.Samples.Identity.Data.Migrations.Application
{
    /// <inheritdoc />
    public partial class Application01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                schema: "Identity",
                table: "Roles",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "Roles",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applications",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleRoles",
                schema: "Identity",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRoles", x => new { x.RoleId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_RoleRoles_Roles_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                schema: "Identity",
                columns: table => new
                {
                    MemberOfId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.MemberOfId, x.UserMembersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_MemberOfId",
                        column: x => x.MemberOfId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UserMembersId",
                        column: x => x.UserMembersId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ApplicationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "Identity",
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ApplicationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "Identity",
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ResourceId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OperationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "Identity",
                        principalTable: "Operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permissions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "Identity",
                        principalTable: "Resources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PermissionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Allowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Identity",
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authorizations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authorizations_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "NameIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ApplicationId_NormalizedName",
                schema: "Identity",
                table: "Roles",
                columns: new[] { "ApplicationId", "NormalizedName" },
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_NormalizedName",
                schema: "Identity",
                table: "Applications",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_PermissionId_RoleId",
                schema: "Identity",
                table: "Authorizations",
                columns: new[] { "PermissionId", "RoleId" },
                unique: true,
                filter: "[RoleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_PermissionId_UserId",
                schema: "Identity",
                table: "Authorizations",
                columns: new[] { "PermissionId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_RoleId",
                schema: "Identity",
                table: "Authorizations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                schema: "Identity",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ApplicationId_NormalizedName",
                schema: "Identity",
                table: "Operations",
                columns: new[] { "ApplicationId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_OperationId",
                schema: "Identity",
                table: "Permissions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ResourceId_OperationId",
                schema: "Identity",
                table: "Permissions",
                columns: new[] { "ResourceId", "OperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ApplicationId_NormalizedName",
                schema: "Identity",
                table: "Resources",
                columns: new[] { "ApplicationId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleRoles_MemberId",
                schema: "Identity",
                table: "RoleRoles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserMembersId",
                schema: "Identity",
                table: "RoleUser",
                column: "UserMembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Applications_ApplicationId",
                schema: "Identity",
                table: "Roles",
                column: "ApplicationId",
                principalSchema: "Identity",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Applications_ApplicationId",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "Authorizations",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RoleRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RoleUser",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Operations",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Applications",
                schema: "Identity");

            migrationBuilder.DropIndex(
                name: "NameIndex",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ApplicationId_NormalizedName",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                schema: "Identity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "Roles");
        }
    }
}
