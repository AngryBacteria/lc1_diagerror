using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RenamePizzasToQuestionnaires : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pizzas",
                table: "Pizzas");

            migrationBuilder.RenameTable(
                name: "Pizzas",
                newName: "Questionnaires");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questionnaires",
                table: "Questionnaires",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Questionnaires",
                table: "Questionnaires");

            migrationBuilder.RenameTable(
                name: "Questionnaires",
                newName: "Pizzas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pizzas",
                table: "Pizzas",
                column: "Id");
        }
    }
}
