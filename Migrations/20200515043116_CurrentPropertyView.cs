using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class CurrentPropertyView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[CurrentProperties]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[CurrentProperties]
AS
	select r.*
	from Properties r
	join (
		select 
			Id,
			row_number() over (partition by TaxKey order by SourceDate desc) as RowNumber
		from Properties
		where Source = ''CurrentMprop''
	) q on q.Id = r.Id
	where q.RowNumber = 1
' ");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "c0dab7cc-c744-47cc-963a-54f168477053");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9dbda4f4-6546-4a77-a571-c4ae3f232a12", "AQAAAAEAACcQAAAAEMS1YdMlRmuXPJpgDdq52FDFKkEsIH1Eo6jhu2qpzwaRlluwgk5BHJ3d8GBVl6O/pw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "ce2d53c7-d50c-4c0d-9744-ed0003adbffe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b7f778c4-caa2-42b3-aecf-b5c5f28823ca", "AQAAAAEAACcQAAAAEMbcVvowOqyaLesPgLClrVf/Aw3rt3rukkESD802mCmp/zcrbKsnNMMmqqw7Vcq7lA==" });
        }
    }
}
