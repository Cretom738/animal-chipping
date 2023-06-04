using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gender = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalLifeStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LifeStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalLifeStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AnimalRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AnimalRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaPoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    AreaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaPoints_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    ChippingDateTime = table.Column<long>(type: "bigint", nullable: false),
                    DeathDateTime = table.Column<long>(type: "bigint", nullable: true),
                    GenderId = table.Column<int>(type: "integer", nullable: false),
                    LifeStatusId = table.Column<int>(type: "integer", nullable: false),
                    ChipperId = table.Column<int>(type: "integer", nullable: false),
                    ChippingLocationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Accounts_ChipperId",
                        column: x => x.ChipperId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animals_AnimalGenders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "AnimalGenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animals_AnimalLifeStatuses_LifeStatusId",
                        column: x => x.LifeStatusId,
                        principalTable: "AnimalLifeStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animals_Locations_ChippingLocationId",
                        column: x => x.ChippingLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimalAnimalType",
                columns: table => new
                {
                    AnimalsId = table.Column<long>(type: "bigint", nullable: false),
                    TypesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAnimalType", x => new { x.AnimalsId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_AnimalTypes_TypesId",
                        column: x => x.TypesId,
                        principalTable: "AnimalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_Animals_AnimalsId",
                        column: x => x.AnimalsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimalVisitedLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VisitDateTime = table.Column<long>(type: "bigint", nullable: false),
                    LocationId = table.Column<long>(type: "bigint", nullable: false),
                    AnimalId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalVisitedLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalVisitedLocations_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimalVisitedLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AnimalGenders",
                columns: new[] { "Id", "Gender" },
                values: new object[,]
                {
                    { 1, "MALE" },
                    { 2, "FEMALE" },
                    { 3, "OTHER" }
                });

            migrationBuilder.InsertData(
                table: "AnimalLifeStatuses",
                columns: new[] { "Id", "LifeStatus" },
                values: new object[,]
                {
                    { 1, "ALIVE" },
                    { 2, "DEAD" }
                });

            migrationBuilder.InsertData(
                table: "AnimalRoles",
                columns: new[] { "Id", "Role" },
                values: new object[,]
                {
                    { 1, "ADMIN" },
                    { 2, "CHIPPER" },
                    { 3, "USER" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, "admin@simbirsoft.com", "adminFirstName", "adminLastName", "qwerty123", 1 },
                    { 2, "chipper@simbirsoft.com", "chipperFirstName", "chipperLastName", "qwerty123", 2 },
                    { 3, "user@simbirsoft.com", "userFirstName", "userLastName", "qwerty123", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAnimalType_TypesId",
                table: "AnimalAnimalType",
                column: "TypesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalGenders_Gender",
                table: "AnimalGenders",
                column: "Gender",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLifeStatuses_LifeStatus",
                table: "AnimalLifeStatuses",
                column: "LifeStatus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalRoles_Role",
                table: "AnimalRoles",
                column: "Role",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChipperId",
                table: "Animals",
                column: "ChipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ChippingLocationId",
                table: "Animals",
                column: "ChippingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_GenderId",
                table: "Animals",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_LifeStatusId",
                table: "Animals",
                column: "LifeStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTypes_Type",
                table: "AnimalTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalVisitedLocations_AnimalId",
                table: "AnimalVisitedLocations",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalVisitedLocations_LocationId",
                table: "AnimalVisitedLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaPoints_AreaId",
                table: "AreaPoints",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_Name",
                table: "Areas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Latitude_Longitude",
                table: "Locations",
                columns: new[] { "Latitude", "Longitude" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAnimalType");

            migrationBuilder.DropTable(
                name: "AnimalVisitedLocations");

            migrationBuilder.DropTable(
                name: "AreaPoints");

            migrationBuilder.DropTable(
                name: "AnimalTypes");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AnimalGenders");

            migrationBuilder.DropTable(
                name: "AnimalLifeStatuses");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "AnimalRoles");
        }
    }
}
