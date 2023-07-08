var datatable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Usuario/obtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "apellido", "width": "15%" },
            { "data": "correo", "width": "15%" },
            { "data": "numeroDni", "width": "10%" },
            { "data": "telefono", "width": "10%" },
            {
                "data": "rolID",
                "render": function (data) {
                    if (data == 2) {
                        return "Voluntario";
                    }
                    if (data == 3) {
                        return "Discapacitado";
                    }
                }, "width": "10%",
            },
            {
                "data": "usuarioID",
                "render": function (data) {
                    return `
                                <div>
                                    <a onclick=Eliminar("/Usuario/Eliminar/${data}") class="btn btn-danger text-white">
                                        <i class="fa-solid fa-trash-can"></i>
                                    </a>
                                </div>
                            `
                }, "width": "10%"
            }
        ]
    });
}
function Eliminar(url) {
    swal({
        title: "Estas seguro de eliminar este Usuario?",
        text: "Este registro no se puede recuperar",
        icon: "warning",
        buttons: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        alert(data.message);
                        datatable.ajax.reload();
                    } else {
                        alert(data.message);
                    }
                }
            });
        }
    });
}