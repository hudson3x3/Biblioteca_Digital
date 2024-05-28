var Global = {

    ListaPerfilAdm: function (input, selectedItem) {
        obj = "#" + input;
        $(obj).append("<option value=''>--Selecione--</option>");

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaPerfilAdm",
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaPerfil: function (input, PaiId, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaPerfil",
            data: { PaiId: PaiId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaGrupoItem: function (input, PaiId, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaGrupoItem",
            data: { PaiId: PaiId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoEstrutura: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoEstrutura",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaMenu: function (input, PaiId, UsuarioAdmId) {
        obj = "#" + input;
        $(obj).empty();
        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaMenu",
            data: { PaiId: PaiId, UsuarioAdmId: UsuarioAdmId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
            }
        })
    },
    ListaMenuSite: function (input) {
        obj = "#" + input;
        $(obj).empty();
        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaMenuSite",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
            }
        })
    },

    ListaEstrutura: function (input, PaiId, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaEstrutura",
            data: { PaiId: PaiId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },
    ListaCampanhaSimulador: function (input) {
        obj = "#" + input;
        $(obj).empty();
        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaCampanhaSimulador",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
            }
        })
    },
    ListaMecanicaSimulador: function (input) {
        obj = "#" + input;
        $(obj).empty();
        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaMecanicaSimulador",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
            }
        })
    },
    ListaSubMecanicaSimulador: function (input) {
        obj = "#" + input;
        $(obj).empty();
        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaSubMecanicaSimulador",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    $(obj).append("<option value='" + this.Id + "'>" + this.Descricao + "</option>");
                });
            }
        })
    },
    ListaEstruturaPorTipo: function (input, tipoEstruturaId, selectedItem, blnInSelecione, callback) {

        obj = "#" + input;
        $(obj).empty();


        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaEstruturaPorTipo",
            data: { TipoEstruturaId: tipoEstruturaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
                callback();
            }
        })
    },

    ListaEstruturaPorTipoSemCall: function (input, tipoEstruturaId, selectedItem, blnInSelecione) {

        obj = "#" + input;
        $(obj).empty();


        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaEstruturaPorTipo",
            data: { TipoEstruturaId: tipoEstruturaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoQuestionario: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        console.log(obj);

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoQuestionario",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoResposta: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        console.log(obj);

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoResposta",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaArquivos: function (eTipoArquivo, obj) {
        Site.Loading();
        $.ajax({
            url: BASE_URL + "Home/PartialArquivo",
            data: { eTipoArquivo: eTipoArquivo },
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data, textStatus, XMLHttpRequest) {
                $(obj).html(data);
                Site.LoadingOff();
            },
            error: function () {
                Site.LoadingOff();
            }
        });
    },

    ListaArquivosCampanha: function (eTipoArquivo, obj, campanhaId) {
        Site.Loading();
        $.ajax({
            url: BASE_URL + "Home/PartialArquivo",
            data: { eTipoArquivo: eTipoArquivo, campanhaId: campanhaId },
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data, textStatus, XMLHttpRequest) {
                $(obj).html(data);
                Site.LoadingOff();
            },
            error: function () {
                Site.LoadingOff();
            }
        });
    },

    ListaParticipantesConsulta: function (obj, EstruturaId, PerfilId, StatusId) {
        Site.Loading();
        $.ajax({
            url: BASE_URL + "Participante/PartialParticipanteConsulta",
            data: { EstruturaId: EstruturaId, PerfilId: PerfilId, StatusId: StatusId },
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data, textStatus, XMLHttpRequest) {
                $(obj).html(data);
                Site.LoadingOff();
            },
            error: function () {
                Site.LoadingOff();
            }
        });
    },

    ListaMenuAdmin: function (obj) {
        $.ajax({
            url: BASE_URL + "Home/PartialMenuAdmin",
            data: {},
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data, textStatus, XMLHttpRequest) {
                $(obj).html(data);
            }
        });
    },

    ListaStatusParticipante: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaStatusParticipante",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaEstado: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaEstado",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.EstadoId) {
                        $(obj).append("<option value='" + this.EstadoId + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.EstadoId + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoAcesso: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoAcesso",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoCadastro: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoCadastro",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoValidacaoPositiva: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoValidacaoPositiva",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTema: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTema",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' style='background-color:" + this.Cor + ";' selected></option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "' style='background-color:" + this.Cor + ";'></option>");
                    }
                });
            }
        })
    },

    ListaPeriodo: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaPeriodo",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaPeriodoCampanha: function (input, selectedItem, blnInSelecione, IdCampanha) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaPeriodoCampanha",
            data: { IdCampanha: IdCampanha },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoPontuacao: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoPontuacao",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaNivelHierarquia: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaNivelHierarquia",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Value) {
                        $(obj).append("<option value='" + this.Key + "' selected>" + this.Value + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Key + "'>" + this.Value + "</option>");
                    }
                });
            }
        })
    },

    ListaTipoCampanha: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaTipoCampanha",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaPeriodo: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListarCampanhaPeriodo",
            data: { CampanhaId: CampanhaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaPeriodoNaoApurado: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListarCampanhaPeriodoNaoApurado",
            data: { CampanhaId: CampanhaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });

                if (data.length == 0) {
                    $(obj).empty();
                    $(obj).append("<option value=''>--Sem Períodos Pendentes de Aprovação--</option>");
                }
            }
        })
    },

    ListaCampanhaPerfil: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaCampanhaPerfil",
            data: { CampanhaId: CampanhaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaPerfilParticipante: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaCampanhaPerfilParticipante",
            data: { CampanhaId: CampanhaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanha: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListarCampanhas",
            async: false,
            success: function (data) {
                $.each(data.campanhas, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaEstrutura: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaCampanhaEstrutura",
            data: { CampanhaId: CampanhaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaEstruturaParticipante: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaCampanhaEstruturaParticipante",
            data: { CampanhaId: CampanhaId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaGrupoItemAssociacao: function (input, selectedItem, blnInSelecione, PerfilId, EstruturaId) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        var CampanhaId = $("#hfCampanhaId").val();

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaGrupoItemAssociacao",
            data: {
                CampanhaId: CampanhaId,
                PerfilId: PerfilId,
                EstruturaId: EstruturaId
            },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaStatusCampanha: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaStatusCampanha",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaMeses: function (input, selectedItem, blnInSelecione) {
        obj = "#" + input;
        $(obj).empty();

        if (blnInSelecione) {
            $(obj).append("<option value=''>--Selecione--</option>");
        }

        $.ajax({
            type: "GET",
            url: BASE_URL + "Home/ListaMeses",
            data: {},
            async: false,
            success: function (data) {
                $.each(data, function () {
                    if (selectedItem == this.Id) {
                        $(obj).append("<option value='" + this.Id + "' selected>" + this.Nome + "</option>");
                    }
                    else {
                        $(obj).append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                    }
                });
            }
        })
    },

    ListaCampanhaLogArquivo: function (input) {

        var CampanhaId = $("#hfCampanhaId").val();
        var TipoArquivoId = $("#hfEtipoArquivo").val();

        $(input).empty();

        var html = "";

        html += "<div class='span12' style='margin:0px;'>";
        html += "<h6>Informações carregadas por período.</h6>"
        html += "<table cellpadding='0' cellspacing='0' border='0' class='table tables-striped table-bordered span10' style='margin-left:0px;' id='tblCampanhaLogArquivo'>";
        html += "<thead>";
        html += "<tr>";
        html += "<th class='span2'>Período</th>";
        html += "<th class='span2'>Período De</th>";
        html += "<th class='span2'>Período Até</th>";
        html += "<th class='span2'>Data Fechamento</th>";
        html += "<th class='span2'>Data Inclusão</th>";
        html += "<th class='span1'>Download</th>";
        html += "</tr>";

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaCampanhaLogArquivo",
            data: { CampanhaId: CampanhaId, TipoArquivoId: TipoArquivoId },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    html += "<tr>";
                    html += " <td class='span4'>" + this.CampanhaPeriodo + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.PeriodoDe) + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.PeriodoAte) + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.DataFechamento) + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.DataInclusao) + "</td>";
                    html += " <td class='span4'><a href='" + this.UrlAccess + "'>" + this.Nome + "</a></td>";
                    html += " </tr>";
                });
            }
        });

        html += "</thead>";
        html += "<tbody></tbody>";
        html += "</table>";
        html += "</div>";

        $(input).append(html);
    },

    ListaCampanhaLogArquivoTipo: function (input, TipoArquivoId) {

        var CampanhaId = $("#hfCampanhaId").val();

        $(input).empty();

        var html = "";

        html += "<div class='span12' style='margin:0px;'>";
        html += "<h6>Informações carregadas por período.</h6>"
        html += "<table cellpadding='0' cellspacing='0' border='0' class='table tables-striped table-bordered span10' style='margin-left:0px;' id='tblCampanhaLogArquivo'>";
        html += "<thead>";
        html += "<tr>";
        html += "<th class='span2'>Período</th>";
        html += "<th class='span2'>Período De</th>";
        html += "<th class='span2'>Período Até</th>";
        html += "<th class='span2'>Data Fechamento</th>";
        html += "<th class='span2'>Data Inclusão</th>";
        html += "<th class='span1'>Download</th>";
        html += "</tr>";

        $.ajax({
            type: "GET",
            url: BASE_URL + "Campanha/ListaCampanhaLogArquivo",
            data: {
                CampanhaId: CampanhaId,
                TipoArquivoId: TipoArquivoId
            },
            async: false,
            success: function (data) {
                $.each(data, function () {
                    html += "<tr>";
                    html += " <td class='span4'>" + this.CampanhaPeriodo + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.PeriodoDe) + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.PeriodoAte) + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.DataFechamento) + "</td>";
                    html += " <td class='span2'>" + parseJsonDate(this.DataInclusao) + "</td>";
                    html += " <td class='span4'><a href='" + this.UrlAccess + "'>" + this.Nome + "</a></td>";
                    html += " </tr>";
                });
            }
        });

        html += "</thead>";
        html += "<tbody></tbody>";
        html += "</table>";
        html += "</div>";

        $(input).append(html);
    }
}