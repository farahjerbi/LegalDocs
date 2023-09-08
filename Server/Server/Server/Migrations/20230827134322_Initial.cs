using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id_Language = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameOfLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id_Language);
                });

            migrationBuilder.CreateTable(
                name: "Log_Docs",
                columns: table => new
                {
                    Id_Doc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log_Docs", x => x.Id_Doc);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Log_Doc_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Languages_Language_FK",
                        column: x => x.Language_FK,
                        principalTable: "Languages",
                        principalColumn: "Id_Language",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Templates_Log_Docs_Log_Doc_FK",
                        column: x => x.Log_Doc_FK,
                        principalTable: "Log_Docs",
                        principalColumn: "Id_Doc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRepeatCard = table.Column<bool>(type: "bit", nullable: false),
                    GridDisplay = table.Column<int>(type: "int", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    Template_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Templates_Template_FK",
                        column: x => x.Template_FK,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aliases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Display = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aliases_Groups_Group_FK",
                        column: x => x.Group_FK,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeSettings",
                columns: table => new
                {
                    IdSettings = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    AliasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSettings", x => x.IdSettings);
                    table.ForeignKey(
                        name: "FK_TypeSettings_Aliases_AliasId",
                        column: x => x.AliasId,
                        principalTable: "Aliases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersAliases",
                columns: table => new
                {
                    AliasesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAliases", x => new { x.AliasesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UsersAliases_Aliases_AliasesId",
                        column: x => x.AliasesId,
                        principalTable: "Aliases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersAliases_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aliases_Group_FK",
                table: "Aliases",
                column: "Group_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Template_FK",
                table: "Groups",
                column: "Template_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Language_FK",
                table: "Templates",
                column: "Language_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Log_Doc_FK",
                table: "Templates",
                column: "Log_Doc_FK");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSettings_AliasId",
                table: "TypeSettings",
                column: "AliasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAliases_UsersId",
                table: "UsersAliases",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeSettings");

            migrationBuilder.DropTable(
                name: "UsersAliases");

            migrationBuilder.DropTable(
                name: "Aliases");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Log_Docs");
        }
    }
}
