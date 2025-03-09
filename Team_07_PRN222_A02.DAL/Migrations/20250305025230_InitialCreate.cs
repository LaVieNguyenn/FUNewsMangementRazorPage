using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team_07_PRN222_A02.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentCategoryID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__19093A2B8697420A", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Category_Parent",
                        column: x => x.ParentCategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "SystemAccount",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountRole = table.Column<byte>(type: "tinyint", nullable: false),
                    AccountPassword = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SystemAc__349DA5864A0E01F9", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tag__657CFA4C41037181", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "NewsArticle",
                columns: table => new
                {
                    NewsArticleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    NewsContent = table.Column<string>(type: "text", nullable: false),
                    NewsSource = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    NewsStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedByID = table.Column<int>(type: "int", nullable: false),
                    UpdatedByID = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NewsArti__4CD0926C904E5A39", x => x.NewsArticleID);
                    table.ForeignKey(
                        name: "FK_NewsArticle_Category",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_NewsArticle_CreatedBy",
                        column: x => x.CreatedByID,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_NewsArticle_UpdatedBy",
                        column: x => x.UpdatedByID,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "NewsTag",
                columns: table => new
                {
                    NewsArticleID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NewsTag__9A875DC807326F71", x => new { x.NewsArticleID, x.TagID });
                    table.ForeignKey(
                        name: "FK_NewsTag_Article",
                        column: x => x.NewsArticleID,
                        principalTable: "NewsArticle",
                        principalColumn: "NewsArticleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsTag_Tag",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryID",
                table: "Category",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_CategoryID",
                table: "NewsArticle",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_CreatedByID",
                table: "NewsArticle",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_UpdatedByID",
                table: "NewsArticle",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_TagID",
                table: "NewsTag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "UQ__Tag__BDE0FD1D43DA3399",
                table: "Tag",
                column: "TagName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsTag");

            migrationBuilder.DropTable(
                name: "NewsArticle");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "SystemAccount");
        }
    }
}
