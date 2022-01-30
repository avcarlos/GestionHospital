function mostrarVentanaCargando() {
    if ($('#windowLoading').data('kendoWindow') == null) {
        $('#windowLoading').kendoWindow({
            title: '',
            model: true,
            resizable: false,
            draggable: false
        });

        $('#.k-window-titlebar', $('#windowLoading').closest('.k_window')).hide();
    }

    $('#windowLoading').data('kendoWindow').center().open();
}

function cerrarVentanaCargando() {
    if ($('#windowLoading').data('kendoWindow') != null) {
        $('#windowLoading').data('kendoWindow').close();
    }
}