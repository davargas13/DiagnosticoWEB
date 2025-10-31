using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddIntentosDomicilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("NumIntentosCoordenadas", "Domicilio", defaultValue:0);
            migrationBuilder.AddColumn<string>("LatitudCorregida", "Domicilio", nullable:true);
            migrationBuilder.AddColumn<string>("LongitudCorregida", "Domicilio", nullable:true);
            migrationBuilder.AddColumn<string>("EstatusDireccion", "Domicilio", nullable:true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("LongitudCorregida", "Domicilio");
            migrationBuilder.DropColumn("LatitudCorregida", "Domicilio");
            migrationBuilder.DropColumn("NumIntentosCoordenadas", "Domicilio");
            migrationBuilder.DropColumn("EstatusDireccion", "Domicilio");
        }
    }
}
