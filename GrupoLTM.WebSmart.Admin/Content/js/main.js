var pleaseWaitDiv = $('<div class="modal hide" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"><div class="modal-header"><center><h3>Processando...</h3></center></div><div class="modal-body"><div class="progress progress-striped active"><div class="bar" style="width: 100%;"></div></div></div></div>');

var Site = {
    Loading: function () {
        pleaseWaitDiv.modal();
    },
    LoadingOff: function () {
        pleaseWaitDiv.modal('hide');
    },
    Alerta: function (msg, css) {
        $(".alerta").empty();
        if (msg != "") {
            var html = "<div class='alert alert-" + css + " alert-dismissable'>";
            html += "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>";
            html += msg;
            html += "</div>";
            $(".alerta").append(html);
        }
    },
    SendData: function (url, data, fSuccess, fnErro, fBeforeSend, fComplete) {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            contentType: "application/json; charset=utf8",
            beforeSend: function () {
                if (typeof (fBeforeSend) == "function")
                    fBeforeSend.call();
                else if (fBeforeSend == null)
                    Site.StartLoad();
            },
            success: function (result) {
                if (typeof (fSuccess) == "function")
                    fSuccess.call(this, result);
            },
            error: function (error) {
                if (typeof (fnErro) == "function")
                    fnErro(error);
            },
            complete: function () {
                if (typeof (fComplete) == "function")
                    fComplete.call();
                else if (fComplete == null)
                    Site.StopLoad();
            }
        });
    },
    StartLoad: function () {
        $.unblockUI();
        $.blockUI({ message: "<img src='" + $("#urlLoader").val() + "' /><h1 style='font-family: verdana,arial; color:#FFF5F5; margin: 0; font-size:13px; padding: 2px;'>Aguarde...</h1>" });
    },
    StopLoad: function () {
        $.unblockUI();
    },
    GetData: function (url, fSuccess, fError) {
        $.ajax({
            type: "GET",
            url: url,
            cache: false,
            contentType: "aplication/json; charset=utf8",
            success: function (result) {
                if (typeof (fSuccess) == "function")
                    fSuccess.call(this, result);
            },
            error: function () {
                if (typeof (fError) == "function")
                    fError.call();
            },
        });
    },
    ShowModal: function (title, msg) {
        var modal = $("#modalMensagens");

        $(modal).find(".modal-title").text(title);
        $(modal).find(".modal-body").html("<p>" + msg + "</p>");
        $(modal).removeClass("hidden");
        $(modal).modal();
    },
    ModalConfirmar: function (title, msg, object) {
        var modal = $("#modalConfirmar");

        $(modal).find(".modal-title").text(title);
        $(modal).find(".modal-body").html("<p>" + msg + "</p>");

        if (!$(modal).find(".btn").hasClass('ativo')) {
            if (object) {
                $(modal).find(".btn").addClass('ativo');
                $(modal).find(".btn").click(function () {
                    object();
                });
            };
        }
        $(modal).removeClass("hidden");
        $(modal).modal();
    },
    ModalConfirmarVisualizar: function (title, msg, object) {
        var modal = $("#modalConfirmarVisualizar");

        $(modal).find(".modal-title").text(title);
        $(modal).find(".modal-body").html("<p>" + msg + "</p>");

        if (!$(modal).find(".btn-visualizar").hasClass('ativo')) {
            if (object) {
                $(modal).find(".btn-visualizar").addClass('ativo');
                $(modal).find(".btn-visualizar").click(function () {
                    object();
                });
            };
        }
        $(modal).removeClass("hidden");
        $(modal).modal();
    },
    ConfirmaTrueFalse: function (title, msg, object) {
        var modal = $("#modalConfirmarTrueFalse");

        $(modal).find(".modal-title").text(title);
        $(modal).find(".modal-body").html("<p>" + msg + "</p>");

        if (!$(modal).find(".btnConfirmar").hasClass('ativo')) {
            if (object) {
                $(modal).find(".btnConfirmar").addClass('ativo');
                $(modal).find(".btnConfirmar").click(function () {
                    object();
                });
            };
        }
        $(modal).removeClass("hidden");
        //$(modal).find(".btnConfirmar").click(function () { object; });
        $(modal).modal();
    },
    ConfirmaSimNao: function (title, msg, object) {
        var modal = $("#modalSimNao");

        $(modal).find(".modal-title").text(title);
        $(modal).find(".modal-body").html("<p>" + msg + "</p>");

        if (!$(modal).find(".btnConfirmar").hasClass('ativo')) {
            if (object) {
                $(modal).find(".btnConfirmar").addClass('ativo');
                $(modal).find(".btnConfirmar").click(function () {
                    object();
                });
            };
        }

        $(modal).removeClass("hidden");
        $(modal).modal();
    },
    AlertaID: function (msg, css, formID) {
        $("#" + formID + " .alerta").empty();
        if (msg != "") {
            var html = "<div class='alert alert-" + css + " alert-dismissable'>";
            html += "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>";
            html += msg;
            html += "</div>";
            $("#" + formID + " .alerta").append(html);
        }
    },
    AlertaObj: function (msg, css, objMensagem) {
        $(objMensagem).empty();
        if (msg != "") {
            var html = "<div class='alert alert-" + css + " alert-dismissable'>";
            html += "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>";
            html += msg;
            html += "</div>";
            $(objMensagem).append(html);
        }
    },
    AlertaMessagemModel: function (mensagemModel) {
        if (mensagemModel != null) {

            $(".alerta").empty();

            var css = "";

            if (mensagemModel.ok) {
                css = "danger";
            }
            else {
                css = "success";
            }

            Site.Alerta(mensagemModel.mensagem, css);
        }
    },
    AlertaModal: function (titulo, msg) {
        $(".alerta").empty();
        if (msg != "") {
            var html = "<div class='modal fade' id='myModal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>";
            html += "<div class='modal-dialog'>";
            html += "<div class='modal-content'>";
            html += "<div class='modal-header'>";
            html += "<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>";
            html += "<h4 class='modal-title' id='myModalLabel'>" + titulo + "</h4>";
            html += "</div>";
            html += "<div class='modal-body'>";
            html += msg;
            html += "</div>";
            html += "<div class='modal-footer'>";
            html += "<button type='button' class='btn btn-default' data-dismiss='modal'>Fechar</button>";
            html += "</div>";
            html += "</div>";
            html += "  </div>";
            html += "</div>";
            $(".alerta").append(html);
            $('#myModal').modal('show');
        }
    },
    AlertaErroGenerico: function () {
        //Requisitos
        //Layout BASE_URL
        //View DIV class alerta
        $('.alerta').on('hidden', function () {
            window.location.href = BASE_URL;
        })
        Site.AlertaModal("Atenção", "Erro ao Adicionar, Sessão expirada!");
    },
    ValidarSenha: function (senha) {
        return validarSenha(senha);
    },
    ValidarFormAlerta: function () {

        //Limpando a div da mensagem
        $(".alerta").empty();

        var exitSubmit = false;

        //filtrando os inputs
        $("form > radio,input,textarea,select,file").each(function () {

            //Selecionando o tipo do campo
            var type = $(this).prop('type');

            //Checando a propriedade "validar"
            if ($(this).attr('validar') !== undefined) {

                //Tipo radio
                if (type == "radio" || type == "checkbox") {
                    var name = $(this).attr('name');
                    if (!$("input[name='" + name + "']:checked").val()) {
                        Site.Alerta("Por favor, selecione o campo " + $(this).attr('validar-msg') + ".", "danger");
                        $(this).focus();
                        exitSubmit = true;
                        return false;
                    }
                }

                //Tipos input, textare, select
                if (type == "text" || type == "select-one" || type == "select-multiple" || type == "password" || type == "textarea" || type == "file" || type == "number") {

                    var labelInfo = "";

                    if (type == "select-one" || type == "select-multiple" || type == "file") {
                        labelInfo = "selecione";
                    }
                    else {
                        labelInfo = "preencha";
                    }

                    if (!$(this).val()) {
                        Site.Alerta("Por favor, " + labelInfo + " o campo " + $(this).attr('validar-msg') + ".", "danger");
                        $(this).focus();
                        exitSubmit = true;
                        return false;
                    }

                    //Validando datas
                    if ($(this).attr('data') !== undefined) {
                        if (!validaData($(this).val())) {
                            Site.Alerta($(this).attr('validar-msg') + " é inválida.", "danger");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando e-mails
                    if ($(this).attr('email') !== undefined) {
                        if (!validaEmail($(this).val())) {
                            Site.Alerta($(this).attr('validar-msg') + " é inválido.", "danger");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CPF
                    if ($(this).attr('cpf') !== undefined) {
                        if (!validaCPF($(this).val())) {
                            Site.Alerta($(this).attr('validar-msg') + " é inválido.", "danger");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CNPJ
                    if ($(this).attr('cnpj') !== undefined) {
                        if (!validaCNPJ($(this).val())) {
                            Site.Alerta($(this).attr('validar-msg') + " é inválido.", "danger");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                }
            }
        });
        if (exitSubmit) {
            return false;
        }
        return true;
    },
    ValidarFormAlertaModal: function () {

        //Limpando a div da mensagem
        $(".alerta").empty();

        var exitSubmit = false;

        //filtrando os inputs
        $("form > radio,input,textarea,select").each(function () {

            //Selecionando o tipo do campo
            var type = $(this).prop('type');

            //Checando a propriedade "validar"
            if ($(this).attr('validar') !== undefined) {

                //Tipo radio
                if (type == "radio" || type == "checkbox") {
                    var name = $(this).attr('name');
                    if (!$("input[name='" + name + "']:checked").val()) {
                        Site.AlertaModal("Atenção", "Por favor, selecione o campo " + $(this).attr('validar-msg') + ".");
                        $(this).focus();
                        exitSubmit = true;
                        return false;
                    }
                }

                //Tipos input, textare, select
                if (type == "text" || type == "select-one" || type == "password" || type == "textarea") {

                    var labelInfo = "";

                    if (type == "select-one") {
                        labelInfo = "selecione"
                    }
                    else {
                        labelInfo = "preencha"
                    }

                    if (!$(this).val()) {
                        Site.AlertaModal("Atenção", "Por favor, " + labelInfo + " o campo " + $(this).attr('validar-msg') + ".");
                        exitSubmit = true;
                        return false;
                    }

                    //Validando datas
                    if ($(this).attr('data') !== undefined) {
                        if (!validaData($(this).val())) {
                            Site.Alerta($(this).attr('validar-msg') + " é inválida.", "danger");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando e-mails
                    if ($(this).attr('email') !== undefined) {
                        if (!validaEmail($(this).val())) {
                            Site.AlertaModal("Atenção", $(this).attr('validar-msg') + " é inválido.");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CPF
                    if ($(this).attr('cpf') !== undefined) {
                        if (!validaCPF($(this).val())) {
                            Site.AlertaModal("Atenção", $(this).attr('validar-msg') + " é inválido."); $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CNPJ
                    if ($(this).attr('cnpj') !== undefined) {
                        if (!validaCNPJ($(this).val())) {
                            Site.AlertaModal("Atenção", $(this).attr('validar-msg') + " é inválido.");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }
                }
            }
        });

        if (exitSubmit) {
            return false;
        }
        return true;
    },
    ValidarFormIDAlerta: function (formID) {

        //Limpando a div da mensagem
        $(".alerta").empty();

        var exitSubmit = false;

        //filtrando os inputs do form especifico
        var frm = "#" + formID;
        $(frm + " radio, " + frm + " input, " + frm + " textarea, " + frm + " select, " + frm + " file").each(function () {

            //Selecionando o tipo do campo
            var type = $(this).prop('type');

            //Checando a propriedade "validar"
            if ($(this).attr('validar') !== undefined) {

                //Tipo radio
                if (type == "radio" || type == "checkbox") {
                    var name = $(this).attr('name');
                    if (!$("input[name='" + name + "']:checked").val()) {
                        Site.AlertaID("Por favor, selecione o campo " + $(this).attr('validar-msg') + ".", "danger", formID);
                        $(this).focus();
                        exitSubmit = true;
                        return false;
                    }
                }

                //Tipos input, textare, select
                if (type == "text" || type == "select-one" || type == "password" || type == "textarea" || type == "file") {

                    var labelInfo = "";

                    if (type == "select-one" || type == "file") {
                        labelInfo = "selecione"
                    }
                    else {
                        labelInfo = "preencha"
                    }

                    if (!$(this).val()) {
                        Site.AlertaID("Por favor, " + labelInfo + " o campo " + $(this).attr('validar-msg') + ".", "danger", formID);
                        $(this).focus();
                        exitSubmit = true;
                        return false;
                    }

                    //Validando datas
                    if ($(this).attr('data') !== undefined) {
                        if (!validaData($(this).val())) {
                            Site.AlertaID($(this).attr('validar-msg') + " é inválida.", "danger", formID);
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando e-mails
                    if ($(this).attr('email') !== undefined) {
                        if (!validaEmail($(this).val())) {
                            Site.AlertaID($(this).attr('validar-msg') + " é inválido.", "danger", formID);
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CPF
                    if ($(this).attr('cpf') !== undefined) {
                        if (!validaCPF($(this).val())) {
                            Site.AlertaID($(this).attr('validar-msg') + " é inválido.", "danger", formID);
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CNPJ
                    if ($(this).attr('cnpj') !== undefined) {
                        if (!validaCNPJ($(this).val())) {
                            Site.AlertaID($(this).attr('validar-msg') + " é inválido.", "danger", formID);
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                }
            }
        });
        if (exitSubmit) {
            return false;
        }
        return true;
    },
    ValidarFormIDAlertaModal: function (formID) {

        //Limpando a div da mensagem
        $(".alerta").empty();

        var exitSubmit = false;

        //filtrando os inputs
        $(formID + " > radio,input,textarea,select").each(function () {

            //Selecionando o tipo do campo
            var type = $(this).prop('type');

            //Checando a propriedade "validar"
            if ($(this).attr('validar') !== undefined) {

                //Tipo radio
                if (type == "radio" || type == "checkbox") {
                    var name = $(this).attr('name');
                    if (!$("input[name='" + name + "']:checked").val()) {
                        Site.AlertaModal("Atenção", "Por favor, selecione o campo " + $(this).attr('validar-msg') + ".");
                        $(this).focus();
                        exitSubmit = true;
                        return false;
                    }
                }

                //Tipos input, textare, select
                if (type == "text" || type == "select-one" || type == "password" || type == "textarea") {

                    var labelInfo = "";

                    if (type == "select-one") {
                        labelInfo = "selecione"
                    }
                    else {
                        labelInfo = "preencha"
                    }

                    if (!$(this).val()) {
                        Site.AlertaModal("Atenção", "Por favor, " + labelInfo + " o campo " + $(this).attr('validar-msg') + ".");
                        exitSubmit = true;
                        return false;
                    }

                    //Validando datas
                    if ($(this).attr('data') !== undefined) {
                        if (!validaData($(this).val())) {
                            Site.Alerta($(this).attr('validar-msg') + " é inválida.", "danger");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando e-mails
                    if ($(this).attr('email') !== undefined) {
                        if (!validaEmail($(this).val())) {
                            Site.AlertaModal("Atenção", $(this).attr('validar-msg') + " é inválido.");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CPF
                    if ($(this).attr('cpf') !== undefined) {
                        if (!validaCPF($(this).val())) {
                            Site.AlertaModal("Atenção", $(this).attr('validar-msg') + " é inválido."); $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }

                    //Validando CNPJ
                    if ($(this).attr('cnpj') !== undefined) {
                        if (!validaCNPJ($(this).val())) {
                            Site.AlertaModal("Atenção", $(this).attr('validar-msg') + " é inválido.");
                            $(this).focus();
                            exitSubmit = true;
                            return false;
                        }
                    }
                }
            }
        });

        if (exitSubmit) {
            return false;
        }
        return true;
    },
    SetCookie: function (key, value, expireDays, expireHours, expireMinutes, expireSeconds) {
        var expireDate = new Date();
        if (expireDays) {
            expireDate.setDate(expireDate.getDate() + expireDays);
        }
        if (expireHours) {
            expireDate.setHours(expireDate.getHours() + expireHours);
        }
        if (expireMinutes) {
            expireDate.setMinutes(expireDate.getMinutes() + expireMinutes);
        }
        if (expireSeconds) {
            expireDate.setSeconds(expireDate.getSeconds() + expireSeconds);
        }
        document.cookie = key + "=" + escape(value) +
            ";domain=" + window.location.hostname +
            ";path=/" +
            ";expires=" + expireDate.toUTCString();
    }
}
