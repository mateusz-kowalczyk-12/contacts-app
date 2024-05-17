using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsApp_API.Migrations
{
    /// <inheritdoc />
    public partial class RenamedPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Contacts",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Contacts",
                newName: "Password");
        }
    }
}
