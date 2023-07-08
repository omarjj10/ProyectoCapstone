var datatable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Cita/obtenerTodos"
        },
        "columns": [
            { "data": "nombreVoluntario", "width": "15%" },
            { "data": "nombreDiscapacitado", "width": "15%" },
            { "data": "fecha", "width": "15%" },
            { "data": "lugarEncuentro", "width": "10%" },
            { "data": "horaInicio", "width": "10%" },
            { "data": "tiempoCita", "width": "10%" },
            {
                "data": "citaID",
                "render": function (data) {
                    return `
                                <div>
                                    <a onclick=Eliminar("/Cita/Eliminar/${data}") class="btn btn-danger text-white">
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
        title: "Estas seguro de eliminar esta Cita?",
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