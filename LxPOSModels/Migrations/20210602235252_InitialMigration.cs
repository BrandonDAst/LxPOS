using Microsoft.EntityFrameworkCore.Migrations;

namespace LxPOSModels.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    subcat = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    value = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    value = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
                });

      #region Populate Tables
      //0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00
      migrationBuilder.Sql(@"INSERT INTO [dbo].[Catalog] ([category], [subcat], [value]) VALUES 
        (N'Currency', N'USD', N'0.01'),
        (N'Currency', N'USD', N'0.05'),
        (N'Currency', N'USD', N'0.10'),
        (N'Currency', N'USD', N'0.25'),
        (N'Currency', N'USD', N'0.50'),
        (N'Currency', N'USD', N'1.00'),
        (N'Currency', N'USD', N'2.00'),
        (N'Currency', N'USD', N'5.00'),
        (N'Currency', N'USD', N'10.00'),
        (N'Currency', N'USD', N'20.00'),
        (N'Currency', N'USD', N'50.00'),
        (N'Currency', N'USD', N'100.00')");
      //0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00
      migrationBuilder.Sql(@"INSERT INTO [dbo].[Catalog] ([category], [subcat], [value]) VALUES 
        (N'Currency', N'MXN', N'0.05'),
        (N'Currency', N'MXN', N'0.10'),
        (N'Currency', N'MXN', N'0.25'),
        (N'Currency', N'MXN', N'0.50'),
        (N'Currency', N'MXN', N'1.00'),
        (N'Currency', N'MXN', N'2.00'),
        (N'Currency', N'MXN', N'5.00'),
        (N'Currency', N'MXN', N'10.00'),
        (N'Currency', N'MXN', N'20.00'),
        (N'Currency', N'MXN', N'50.00'),
        (N'Currency', N'MXN', N'100.00')");

      migrationBuilder.Sql(@"INSERT INTO [dbo].[Products] ([name], [price]) VALUES
        (N'Botella de Agua', 10.50),
        (N'Taza de cafe', 30.99),
        (N'Pila portatil', 348.25),
        (N'Cable USB', 150.00),
        (N'Juego de Lapiceros', 50.72),
        (N'Bloc de notas', 46.78),
        (N'Libreta', 30.50),
        (N'Mouse', 999.99),
        (N'Galletas', 8.90),
        (N'Cacahuates', 10.20)");
			#endregion
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catalog");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
