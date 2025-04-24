using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRangeConstrintToWeekAndDayInScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Schedule_Day",
                table: "Schedule",
                sql: "[Day] >= 0 AND [Day] <= 6");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Schedule_Week",
                table: "Schedule",
                sql: "[Week] >= 1 AND [Week] <= 52");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Schedule_Day",
                table: "Schedule");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Schedule_Week",
                table: "Schedule");
        }
    }
}
