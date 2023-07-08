using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoCapstone.Migrations
{
    public partial class AddDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blocUsuarios",
                columns: table => new
                {
                    BlocUsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigoVerificarCorreo = table.Column<int>(type: "int", nullable: true),
                    clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    discapacidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diaDisponible = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recomendacion = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numeroDni = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefono = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocUsuarios", x => x.BlocUsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "contactanos",
                columns: table => new
                {
                    ContactanosID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactanos", x => x.ContactanosID);
                });

            migrationBuilder.CreateTable(
                name: "rols",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rols", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolID = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    codigoVerificarCorreo = table.Column<int>(type: "int", nullable: true),
                    clave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discapacidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diaDisponible = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recomendacion = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numeroDni = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefono = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.UsuarioID);
                    table.ForeignKey(
                        name: "FK_usuarios_rols_RolID",
                        column: x => x.RolID,
                        principalTable: "rols",
                        principalColumn: "RolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocCitas",
                columns: table => new
                {
                    BlocCitaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lugarEncuentro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    horario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tiempoCita = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreDiscapacitado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreVoluntario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocCitas", x => x.BlocCitaID);
                    table.ForeignKey(
                        name: "FK_blocCitas_usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "cita",
                columns: table => new
                {
                    CitaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lugarEncuentro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    horaInicio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tiempoCita = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreDiscapacitado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreVoluntario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cita", x => x.CitaID);
                    table.ForeignKey(
                        name: "FK_cita_usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "recomendaciones",
                columns: table => new
                {
                    RecomendacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreVoluntario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombreDiscapacitado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    calificacion = table.Column<int>(type: "int", nullable: true),
                    comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recomendaciones", x => x.RecomendacionID);
                    table.ForeignKey(
                        name: "FK_recomendaciones_usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_blocCitas_UsuarioID",
                table: "blocCitas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_cita_UsuarioID",
                table: "cita",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_recomendaciones_UsuarioID",
                table: "recomendaciones",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_RolID",
                table: "usuarios",
                column: "RolID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blocCitas");

            migrationBuilder.DropTable(
                name: "blocUsuarios");

            migrationBuilder.DropTable(
                name: "cita");

            migrationBuilder.DropTable(
                name: "contactanos");

            migrationBuilder.DropTable(
                name: "recomendaciones");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "rols");
        }
    }
}
