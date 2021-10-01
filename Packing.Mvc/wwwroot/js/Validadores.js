$('.validarRut').blur(function () {
    var id = $(this).attr('id');
    if ($(this).val().length > 0) {
        if (!revisarDigito2($(this).val())) {
            Swal.fire({
                icon: 'info',
                title: 'Información',
                text: 'Ingresa un R.U.T válido'
            })
            $("#" + id).val("");
        }
        else {
            var elRut = $(this).val().split("-");
            if (elRut.length == 1) {
                var largo = $(this).val().length;
                var rut = $(this).val().substring(0, largo - 1);
                var dv = $(this).val().charAt(largo - 1);
                $("#" + id).val(rut + "-" + dv);
            }
        }
    }
});

function revisarDigito(dvr) {
    dv = dvr + "";
    if (dv != '0' && dv != '1' && dv != '2' && dv != '3' && dv != '4' && dv != '5' && dv != '6' && dv != '7' && dv != '8' && dv != '9' && dv != 'k' && dv != 'K') {
        return false;
    }
    return true;
}

function revisarDigito2(crut) {
    largo = crut.length;
    var elRut = crut.split("-");
    if (elRut.length > 1) {
        rut = elRut[0];
        dv = elRut[1];
    }
    else {
        if (largo > 2)
            rut = crut.substring(0, largo - 1);
        else
            rut = crut.charAt(0);
        dv = crut.charAt(largo - 1);
    }
    if (!revisarDigito(dv))
        return false;
    if (rut == null || dv == null)
        return 0;
    var dvr = '0';
    suma = 0;
    mul = 2;

    for (i = rut.length - 1; i >= 0; i--) {
        suma = suma + rut.charAt(i) * mul;
        if (mul == 7)
            mul = 2;
        else
            mul++;
    }
    res = suma % 11;
    if (res == 1)
        dvr = 'k';
    else if (res == 0)
        dvr = '0';
    else {
        dvi = 11 - res;
        dvr = dvi + "";
    }
    if (dvr != dv.toLowerCase()) {
        return false;
    }
    return true;
}

$(".email").blur(function () {
    if ($(this).val().length > 0) {
        var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
        if (!testEmail.test($(this).val())) {
            var id = $(this).attr('id');
            Swal.fire({
                icon: 'info',
                title: 'Información',
                text: 'Ingresa un correo con formato válido'
            });
            $("#" + id).val("");
            $("#" + id).focus();
        }
    }
});



$("#rutEmpresa").on("keyup", function () {
    document.getElementById('rutEmpresa').addEventListener('input', function (event) {
        let value = this.value.replace(/\./g, '').replace('-', '');

        if (value.match(/^(\d{2})(\d{3}){2}(\w{1})$/)) {
            value = value.replace(/^(\d{2})(\d{3})(\d{3})(\w{1})$/, '$1$2$3-$4');
            this.value = value;
        }
        else if (value.match(/^(\d)(\d{3}){2}(\w{0,1})$/)) {
            value = value.replace(/^(\d)(\d{3})(\d{3})(\w{0,1})$/, '$1$2$3-$4');
            this.value = value;
        }
    });
});