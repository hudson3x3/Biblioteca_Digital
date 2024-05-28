function validaEmail(email) {
    if (email.includes('@ltm.digital')) {
        return true;
    }

    var reg1 = /(@.*@)|(\.\.)|(@\.)|(\.@)|(^\.)/; // not valid
    var reg2 = /^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$/; // valid

    if (!reg1.test(email) && reg2.test(email)) // if syntax is valid
        return true;
    else
        return false;
}

function validaData(objName) {
    var strDate;
    var strDateArray;
    var strDay;
    var strMonth;
    var strYear;
    var booFound = false;
    var intday;
    var intMonth;
    var intYear;
    var datefield = objName;

    var strSeparatorArray = new Array("/");

    var intElementNr;
    var strMonthArray = new Array(12);
    strMonthArray[0] = "Jan";
    strMonthArray[1] = "Feb";
    strMonthArray[2] = "Mar";
    strMonthArray[3] = "Apr";
    strMonthArray[4] = "May";
    strMonthArray[5] = "Jun";
    strMonthArray[6] = "Jul";
    strMonthArray[7] = "Aug";
    strMonthArray[8] = "Sep";
    strMonthArray[9] = "Oct";
    strMonthArray[10] = "Nov";
    strMonthArray[11] = "Dec";
    strDate = objName;

    if (strDate.length < 5) {
        if (strDate.length < 1) {
            return true;
        } else {
            return false;
        }
    }

    //Separator - Day - Month - Year
    for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
        if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) {
            strDateArray = strDate.split(strSeparatorArray[intElementNr]);
            if (strDateArray.length != 3) {
                err = 1;
                return false;
            } else {
                strDay = strDateArray[0];
                strMonth = strDateArray[1];
                strYear = strDateArray[2];
            }
            booFound = true;
        }

    }

    if (!booFound) { return false; }

    if (strYear.length == 2) {
        strYear = '20' + strYear;
    }

    intday = parseInt(strDay, 10);
    if (isNaN(intday)) {
        err = 2;
        return false;
    }

    intMonth = parseInt(strMonth, 10);
    if (isNaN(intMonth)) {
        for (i = 0; i < 12; i++) {
            if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) {
                intMonth = i + 1;
                strMonth = strMonthArray[i];
                i = 12;
            }
        }
        if (isNaN(intMonth)) {
            err = 3;
            return false;
        }
    }
    intYear = parseInt(strYear, 10);
    if (isNaN(intYear)) {
        err = 4;
        return false;
    }
    if (intMonth > 12 || intMonth < 1) {
        err = 5;
        return false;
    }
    if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
        err = 6;
        return false;
    }
    if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
        err = 7;
        return false;
    }
    if (intMonth == 2) {
        if (intday < 1) {
            err = 8;
            return false;
        }
        if (LeapYear(intYear) == true) {
            if (intday > 29) {
                err = 9;
                return false;
            }
        } else {
            if (intday > 28) {
                err = 10;
                return false;
            }
        }
    }
    return true;
}

function LeapYear(intYear) {
    if (intYear % 100 == 0) {
        if (intYear % 400 == 0) { return true; }
    } else {
        if ((intYear % 4) == 0) { return true; }
    }
    return false;
}

function validaCNPJ(cnpj) {
    var numeros, digitos, soma, i, resultado, pos, tamanho, digitos_iguais;
    digitos_iguais = 1;
    if (cnpj.length < 14 && cnpj.length < 15)
        return false;
    for (i = 0; i < cnpj.length - 1; i++)
        if (cnpj.charAt(i) != cnpj.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        tamanho = cnpj.length - 2
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            return false;
        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1))
            return false;
        return true;
    }
    else
        return false;
}

function fctVerificaNumero() {
    var varCaractere = String.fromCharCode(event.keyCode)

    if (!fctEhDigitoNumerico(varCaractere)) {
        event.returnValue = false;
    }
}

function validateNumber(event) {
    var key = window.event ? event.keyCode : event.which;

    if (//event.keyCode == 46 || // Ponto
        event.keyCode == 44 || // Virgula   
        event.keyCode == 8 || // Back-space
        event.keyCode == 9 || // Tab
        event.keyCode == 27 || // Esc
        event.keyCode == 13 || // Enter
        (event.keyCode == 65 && event.ctrlKey === true))
    {
        return true;
    }
    else if (key < 48 || key > 57) {
        return false;
    }
    else return true;
};

function verificaNumero() {
    var varCaractere = String.fromCharCode(event.keyCode)
    if (!fctEhDigitoNumerico(varCaractere)) {
        event.returnValue = false;
    }
}

function fctEhDigitoNumerico(istrDig) {
    if ((istrDig == '0') || (istrDig == '1') || (istrDig == '2') || (istrDig == '3') || (istrDig == '4') || (istrDig == '5') || (istrDig == '6') || (istrDig == '7') || (istrDig == '8') || (istrDig == '9')) {
        return true;
    }
    else {
        return false;
    }
}

function validarSenha(senha) {
    //min 6, max 50, pelo menos um símbolo, letras maiúsculas e minúsculas e um número
    var regex = /^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{6,50}$/;
    return regex.test(senha);
}

function validaDataCompara(dtInicial, dtFinal) {

    var dtini = dtInicial;
    var dtfim = dtFinal;

    if ((dtini == '') && (dtfim == '')) {
        return false;
    }

    datInicio = new Date(dtini.substring(6, 10),
                         dtini.substring(3, 5),
                         dtini.substring(0, 2));
    datInicio.setMonth(datInicio.getMonth() - 1);


    datFim = new Date(dtfim.substring(6, 10),
                      dtfim.substring(3, 5),
                      dtfim.substring(0, 2));

    datFim.setMonth(datFim.getMonth() - 1);

    if (datInicio <= datFim) {
        return true;
    } else {
        return false;
    }
}

function formatarValores(obj) {
    //$(obj).unmaskMoney();
    $(obj).maskMoney({ thousands: '.', decimal: ',', allowZero: true });
}


function validaCPF(s) {

    if ((s == "00000000000") || (s == "11111111111") || (s == "22222222222") || (s == "33333333333") || (s == "44444444444") || (s == "55555555555") || (s == "6666666666") || (s == "77777777777") || (s == "88888888888") || (s == "9999999999")) {
        return false;
    }

    if (s.length > 11) {
        return false;
    }
    
    var i;
    var c = s.substr(0, 9);
    var dv = s.substr(9, 2);
    var d1 = 0;

    for (i = 0; i < 9; i++) {
        d1 += c.charAt(i) * (10 - i);
    }

    if (d1 == 0) {
        return false;
    }

    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(0) != d1) {
        //alert("CPF Invalido")   
        return false;
    }
    d1 *= 2;
    for (i = 0; i < 9; i++) {
        d1 += c.charAt(i) * (11 - i);
    }
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(1) != d1) {
        return false;
    }
    return true;
}

function formatDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/') + " " + inputFormat.toLocaleTimeString();
}

function parseJsonDate(jsonDate) {
    var date = new Date(parseInt(jsonDate.substr(6)));
    var dd = date.getDate();
    var MM = date.getMonth()+1;
    var yyyy = date.getFullYear();
    var hh = date.getHours();
    var mm = date.getMinutes();
    dd = (dd.toString().length > 1) ? dd : "0" + dd;
    MM = (MM.toString().length > 1) ? MM : "0" + MM;
    yyyy = (yyyy.toString().length > 1) ? yyyy : "0" + yyyy;
    hh = (hh.toString().length > 1) ? hh : "0" + hh;
    mm = (mm.toString().length > 1) ? mm : "0" + mm;
    return dd + "/" + MM + "/" + yyyy + " " + hh + ":" + mm;
}

function RadionButtonSelectedValueSet(name, SelectdValue) {
    $('input[name="' + name + '"][value="' + SelectdValue + '"]').prop('checked', true);
}




