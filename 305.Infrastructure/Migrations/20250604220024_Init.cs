using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _305.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistedToken",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    black_listed_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedToken", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_mobile_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    failed_login_count = table.Column<int>(type: "int", nullable: false),
                    lock_out_end_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_login_date_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    security_stamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_locked_out = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_delete_able = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image_alt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blog_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    show_blog = table.Column<bool>(type: "bit", nullable: false),
                    keywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    meta_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estimated_read_time = table.Column<int>(type: "int", nullable: false),
                    blog_category_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.id);
                    table.ForeignKey(
                        name: "FK_Blog_BlogCategory_blog_category_id",
                        column: x => x.blog_category_id,
                        principalTable: "BlogCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    permission_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_permission_id",
                        column: x => x.permission_id,
                        principalTable: "Permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    roleid = table.Column<long>(type: "bigint", nullable: false),
                    userid = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.userid, x.roleid, x.id });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_roleid",
                        column: x => x.roleid,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id", "created_at", "name", "slug", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Admin_Role", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2L, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "Customer_Role", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3L, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "MainAdmin", "Main_Admin_Role", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "concurrency_stamp", "created_at", "email", "failed_login_count", "is_active", "is_delete_able", "is_locked_out", "is_mobile_confirmed", "last_login_date_time", "lock_out_end_time", "mobile", "name", "password_hash", "refresh_token", "refresh_token_expiry_time", "security_stamp", "slug", "updated_at" },
                values: new object[] { 1L, "X3JO2EOCURAEBU6HHY6OBYEDD2877FXU", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "info@305.com", 0, true, false, false, true, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "09309309393", "admin-user", "omTtMfA5EEJCzjH5t/Q67cRXK5TRwerSqN7sJSm41No=.FRLmTm9jwMcEFnjpjgivJw==", "refeshToken", new DateTime(2026, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "098NTB7E5LFFXREHBSEHDKLI0DOBIKST", "Admin-User", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "id", "roleid", "userid", "created_at", "name", "slug", "updated_at" },
                values: new object[] { 1L, 3L, 1L, new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Main Admin User", "Main-Admin-User", new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedToken_slug",
                table: "BlacklistedToken",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blog_blog_category_id",
                table: "Blog",
                column: "blog_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_slug",
                table: "Blog",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategory_slug",
                table: "BlogCategory",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_slug",
                table: "Permission",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_slug",
                table: "Role",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_permission_id",
                table: "RolePermission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_role_id",
                table: "RolePermission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_slug",
                table: "RolePermission",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_name",
                table: "User",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_slug",
                table: "User",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_roleid",
                table: "UserRole",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_slug",
                table: "UserRole",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedToken");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "BlogCategory");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
