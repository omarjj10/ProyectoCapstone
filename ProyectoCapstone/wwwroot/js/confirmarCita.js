function confirmar(e) {
    e.preventDefault();
    Swal.fire({
        title: 'Deseas Confirmar la cita?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Si'
    }).then((resultado) => {
        if (resultado.isConfirmed) {
            const formulario = document.getElementById('formulario');
            formulario.submit();
        }
    })
}
function cancelar(e) {
    e.preventDefault();
    Swal.fire({
        title: 'Deseas Cancelar la cita?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Si'
    }).then((resultado) => {
        if (resultado.isConfirmed) {
            const formulario = document.getElementById('formulario2');
            formulario.submit();
        }
    })
}