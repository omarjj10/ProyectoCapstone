document.addEventListener("keyup", e => {

    if (e.target.matches("#buscador")) {

        if (e.key === "Escape") e.target.value = ""

        document.querySelectorAll(".voluntario").forEach(voluntario => {

            voluntario.textContent.toLowerCase().includes(e.target.value.toLowerCase())
                ? voluntario.classList.remove("filtro")
                : voluntario.classList.add("filtro")
        })
    }
})