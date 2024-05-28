using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Cripto;
using System;
using System.Collections.Generic;
using System.Linq;
using GrupoLTM.WebSmart.Infrastructure.ExtensionMethods;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GrupoLTM.WebSmart.Domain.DTO;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.Excel;
using GrupoLTM.WebSmart.Domain.Repositories;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System.Net;
using GrupoLTM.WebSmart.Domain.Models.MktPlace;
using RestSharp;
using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Services
{
    public class ParticipanteService : BaseService<Participante>
    {
        #region "Services"

        private readonly EstruturaService _estruturaService;
        private readonly CatalogoService _catalogoService;

        private readonly CatalogRepository _CatalogRepository;
        private readonly ParticipanteCatalogoRepository _ParticipanteCatalogoRepository;


        #endregion

        #region "Construtor"

        public ParticipanteService()
        {
            _CatalogRepository = new CatalogRepository();
            _estruturaService = new EstruturaService();
            _ParticipanteCatalogoRepository = new ParticipanteCatalogoRepository();
            _catalogoService = new CatalogoService();
        }

        #endregion

        public LoginModel ChecarParticipante(string login, string senha)
        {
            LoginModel _loginModel = new LoginModel();
            LoginModel model = new LoginModel();

            string _senha = GrupoLTM.WebSmart.Infrastructure.Cripto.HexEncoding.Encriptar(senha);
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            Participante _participante = _ParticipantRepository.GetByLogin(login, senha);
            _ParticipantRepository.Dispose();
            if (_participante != null)
            {
                _loginModel.Id = _participante.Id;
                _loginModel.Nome = _participante.Nome;
                _loginModel.PrimeiroNome = _participante.Nome;
                _loginModel.participanteTeste = _participante.ParticipanteTeste;
                _loginModel.Login = _participante.Login;
                _loginModel.PerfilId = _participante.ParticipantePerfil.Where(x => x.Ativo).ToList()[0].PerfilId;
                _loginModel.EstruturaId = _participante.ParticipanteEstrutura.Where(x => x.Ativo).ToList()[0].EstruturaId;

                model = PreencheLoginModel(_loginModel);

            }
            return model;
        }

        public LoginModel PreencheLoginModel(LoginModel loginModel)
        {
            var model = new LoginModel();

            model.Id = loginModel.Id;
            model.Nome = loginModel.Nome;
            model.PrimeiroNome = loginModel.PrimeiroNome;
            model.participanteTeste = loginModel.participanteTeste.HasValue ? loginModel.participanteTeste.Value : false;
            model.Login = loginModel.Login;
            model.PerfilId = loginModel.PerfilId;
            model.EstruturaId = loginModel.EstruturaId;

            ConfiguracaoCampanhaService _configuracaoCampanhaService = new ConfiguracaoCampanhaService();
            var campanhaConfiguracaoModel = _configuracaoCampanhaService.ListarCampanhaConfiguracao();
            if (campanhaConfiguracaoModel != null)
            {
                model.LIVE_PROJECTCONFIGURATIONID = campanhaConfiguracaoModel.LIVE_PROJECTCONFIGURATIONID;
                model.LIVEAPI_CLIENTEID = campanhaConfiguracaoModel.LIVEAPI_CLIENTID;
                model.LIVEAPI_PROJECTID = campanhaConfiguracaoModel.LIVEAPI_PROJECTID;
                model.LIVEAPI_COOKIENAME = campanhaConfiguracaoModel.LIVEAPI_COOKIENAME;
                model.LIVEAPI_ENDPOINT = campanhaConfiguracaoModel.LIVEAPI_ENDPOINT;
                model.LIVEAPI_PASSWORD = campanhaConfiguracaoModel.LIVEAPI_PASSWORD;
                model.LIVEAPI_URL = campanhaConfiguracaoModel.LIVEAPI_URL;
                model.LIVEAPI_USERNAME = campanhaConfiguracaoModel.LIVEAPI_USERNAME;
            }

            return model;
        }

        [CacheAttribute]
        public ParticipanteModel ObterParticipante(int id)
        {
            //Insntancia Variáveis
            ParticipanteModel _participanteModel = new ParticipanteModel();

            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            var _participante = _ParticipantRepository.GetById(id);
            _ParticipantRepository.Dispose();
            _participanteModel = BuildParticipante(_participante);

            return _participanteModel;
        }

        [CacheAttribute]
        public CatalogoModel ObterCatalogoByParticipanteMktPlaceId(int id)
        {
            //Insntancia Variáveis
            CatalogoModel _catalogoModel = new CatalogoModel();
            var _catalogo = _ParticipanteCatalogoRepository.GetByCodigoMktPlace(id);
            _ParticipanteCatalogoRepository.Dispose();
            _catalogoModel = _catalogoService.BuildCatalogo(_catalogo);

            return _catalogoModel;
        }

        private ParticipanteModel BuildParticipante(Participante participante)
        {
            if (participante == null)
                return new ParticipanteModel();

            ConfiguracaoCampanhaService _campanha = new ConfiguracaoCampanhaService();

            //var dadosCampanha = _campanha.ListarCampanhaConfiguracao();
            var participanteModel = new ParticipanteModel();
            if (participante != null)
            {
                //De Para Banco Model
                participanteModel.Id = participante.Id;
                participanteModel.Login = participante.Login;
                participanteModel.StatusId = participante.StatusId;
                participanteModel.Status = participante.StatusParticipante?.Nome;
                participanteModel.Nome = participante.Nome;
                participanteModel.RazaoSocial = participante.RazaoSocial;
                participanteModel.NomeFantasia = participante.NomeFantasia;
                participanteModel.CNPJ = participante.CNPJ;
                participanteModel.CPF = participante.CPF;
                participanteModel.Senha = participante.Senha;
                participanteModel.RG = participante.RG;
                participanteModel.Sexo = participante.Sexo;
                participanteModel.DataNascimento = participante.DataNascimento;
                participanteModel.Bairro = participante.Bairro;
                participanteModel.Endereco = participante.Endereco;
                participanteModel.Numero = participante.Numero;
                participanteModel.Complemento = participante.Complemento;
                participanteModel.CEP = participante.CEP;
                participanteModel.Cidade = participante.Cidade;
                participanteModel.EstadoId = participante.EstadoId;
                participanteModel.Estado = participante.Estado == null ? string.Empty : participante.Estado.Nome;
                var participantePerfil = participante.ParticipantePerfil.FirstOrDefault();
                if (participantePerfil != null)
                {
                    participanteModel.Perfil = participantePerfil.Perfil.Nome;
                    participanteModel.PerfilId = participantePerfil.Perfil.Id;
                }

                var participanteEstrutura = participante.ParticipanteEstrutura.FirstOrDefault();
                if (participanteEstrutura != null)
                {
                    participanteModel.Estrutura = participanteEstrutura.Estrutura.Nome;
                    participanteModel.EstruturaId = participanteEstrutura.Estrutura.Id;
                }
                participanteModel.DDDCel = participante.DDDCel;
                participanteModel.Celular = participante.Celular;
                participanteModel.DDDTel = participante.DDDTel;
                participanteModel.TipoAcessoId = 1;
                participanteModel.Telefone = participante.Telefone;
                participanteModel.DDDTelComercial = participante.DDDTelComercial;
                participanteModel.TelefoneComercial = participante.TelefoneComercial;
                participanteModel.Email = participante.Email;
                participanteModel.Ativo = participante.Ativo;
                participanteModel.DataCadastro = participante.DataCadastro;
                participanteModel.DataDesligamento = participante.DataDesligamento;
                participanteModel.DataInclusao = participante.DataInclusao;
                participanteModel.DataAlteracao = participante.DataAlteracao;
                participanteModel.ParticipanteTeste = participante.ParticipanteTeste;
                participanteModel.PontoReferencia = participante.PontoReferencia;
                if (participante.OptInSms != null)
                    participanteModel.OptInSms = (bool)participante.OptInSms;
                if (participante.OptInComunicacaoFisica != null)
                    participanteModel.OptInComunicacaoFisica = (bool)participante.OptInComunicacaoFisica;
                if (participante.OptInEmail != null)
                    participanteModel.OptInEmail = (bool)participante.OptInEmail;
                if (participante.OptinAceite != null)
                    participanteModel.OptinAceite = (bool)participante.OptinAceite;

                foreach (var participanteCatalogo in participante.ParticipanteCatalogos)
                {
                    participanteModel.Catalogos.Add(
                        new ParticipanteCatalogoModel()
                        {
                            CatalogoId = participanteCatalogo.Catalogo.Id,
                            CodigoMktPlace = participanteCatalogo.CodigoMktPlace,
                            Ativo = participanteCatalogo.Ativo,
                            Catalogo = new CatalogoModel()
                            {
                                Id = participanteCatalogo.Catalogo.Id,
                                //IdCampanha = participanteCatalogo.Catalogo.IdCampanha,
                                Codigo = participanteCatalogo.Catalogo.Codigo,
                                Ativo = participanteCatalogo.Catalogo.Ativo
                            }
                        }
                    );
                }

                return participanteModel;
            }
            else
            {
                return null;
            }
        }

        private ParticipanteModel BuildParticipanteDTO(GrupoLTM.WebSmart.Domain.DTO.ParticipanteDTO participante)
        {
            if (participante == null)
                return new ParticipanteModel();

            var participanteModel = new ParticipanteModel();
            if (participante != null)
            {
                //De Para Banco Model
                participanteModel.Id = participante.Id;
                participanteModel.Login = participante.Login;
                participanteModel.StatusId = participante.StatusId;
                participanteModel.Status = participante.Status;
                participanteModel.Nome = participante.Nome;
                participanteModel.RazaoSocial = participante.RazaoSocial;
                participanteModel.NomeFantasia = participante.NomeFantasia;
                participanteModel.CNPJ = participante.CNPJ;
                participanteModel.CPF = participante.CPF;
                participanteModel.Senha = participante.Senha;
                participanteModel.RG = participante.RG;
                participanteModel.Sexo = participante.Sexo;
                participanteModel.DataNascimento = participante.DataNascimento;
                participanteModel.Bairro = participante.Bairro;
                participanteModel.Endereco = participante.Endereco;
                participanteModel.Numero = participante.Numero;
                participanteModel.Complemento = participante.Complemento;
                participanteModel.CEP = participante.CEP;
                participanteModel.Cidade = participante.Cidade;
                participanteModel.EstadoId = participante.EstadoId;
                participanteModel.Estado = participante.Estado;
                participanteModel.Perfil = participante.Perfil;
                if (participante.PerfilId != null)
                    participanteModel.PerfilId = (int)participante.PerfilId;
                participanteModel.Estrutura = participante.Estrutura;
                if (participante.EstruturaId != null)
                    participanteModel.EstruturaId = (int)participante.EstruturaId;
                participanteModel.DDDCel = participante.DDDCel;
                participanteModel.Celular = participante.Celular;
                participanteModel.DDDTel = participante.DDDTel;
                participanteModel.TipoAcessoId = 1;
                participanteModel.Telefone = participante.Telefone;
                participanteModel.DDDTelComercial = participante.DDDTelComercial;
                participanteModel.TelefoneComercial = participante.TelefoneComercial;
                participanteModel.Email = participante.Email;
                participanteModel.Ativo = participante.Ativo;
                participanteModel.DataCadastro = participante.DataCadastro;
                participanteModel.DataDesligamento = participante.DataDesligamento;
                participanteModel.DataInclusao = participante.DataInclusao;
                participanteModel.DataAlteracao = participante.DataAlteracao;
                participanteModel.ParticipanteTeste = participante.ParticipanteTeste;
                participanteModel.PontoReferencia = participante.PontoReferencia;
                participanteModel.OptInSms = (bool)participante.OptInSms;
                participanteModel.OptInComunicacaoFisica = (bool)participante.OptInComunicacaoFisica;
                participanteModel.OptInEmail = (bool)participante.OptInEmail;
                participanteModel.OptinAceite = (bool)participante.OptinAceite;
                participanteModel.SimuladorVisualizado = participante.SimuladorVisualizado;

                if (participante.CodigoMktPlace != null)
                {
                    var participanteCatalogoModel = new ParticipanteCatalogoModel();
                    participanteCatalogoModel.CodigoMktPlace = (long)participante.CodigoMktPlace;
                    participanteCatalogoModel.Ativo = true;
                    if (participante.CatalogoId != null)
                        participanteCatalogoModel.CatalogoId = (int)participante.CatalogoId;

                    var catalogoModel = new CatalogoModel();
                    catalogoModel.Ativo = true;
                    if (participante.CatalogoId != null)
                        catalogoModel.Id = (int)participante.CatalogoId;
                    //if (participante.CatalogoIdCampanha != null)
                    //    catalogoModel.IdCampanha = (int)participante.CatalogoIdCampanha;
                    if (participante.MktPlaceCatalogoId != null)
                        catalogoModel.Codigo = (long)participante.MktPlaceCatalogoId;

                    participanteCatalogoModel.Catalogo = catalogoModel;
                    participanteModel.Catalogos.Add(participanteCatalogoModel);
                }
                return participanteModel;
            }
            else
            {
                return null;
            }
        }

        public HomeMMAExtratoExterno BuscarPontuacaoParticipante(string accountNumber)
        {
            var urlApi = ConfigurationManager.AppSettings["apiAvon_url"];
            var apiSubscriptionKey = ConfigurationManager.AppSettings["apiMan_subscriptionKey"];

            var client = new RestClient(urlApi);

            var request = new RestRequest("/extrato/GetExtratoHomeMMA", Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-key", apiSubscriptionKey);

            var body = new
            {
                accountNumber = accountNumber,
                catalogId = 1,
                grant_type = "password",
                pageName = "extrato",
                tokenGetInfo = "InternoLTM"
            };

            request.AddParameter("application/json", body.ToJson(), ParameterType.RequestBody);

            var env = ConfigurationManager.AppSettings["env"];

            if (env == "local")
            {
                var avonAuthentication = new AvonAuthentication
                {
                    AccountNumber = accountNumber,
                    TokenGetInfo = "InternoLTM",
                    PageName = "extrato",
                    CatalogId = 1,
                    i = 0,
                };

                var token = OAuthService.GetToken(avonAuthentication);

                request.AddHeader("Authorization", "Bearer " + token);
            }

            var response = client.Execute(request);

            if (!response.IsSuccessStatusCode())
                throw new Exception("Houve um erro ao consultar a api GetExtratoHomeMMA, status: " + response.StatusDescription);

            var result = JsonConvert.DeserializeObject<List<HomeMMAExtratoExterno>>(response.Content);

            return result.FirstOrDefault();
        }

        private int BuscaEstado(string siglaEstado)
        {
            Dictionary<string, int> estados = new Dictionary<string, int>();
            estados.Add("AC", 1);
            estados.Add("AL", 2);
            estados.Add("AP", 3);
            estados.Add("AM", 4);
            estados.Add("BA", 5);
            estados.Add("CE", 6);
            estados.Add("DF", 7);
            estados.Add("ES", 8);
            estados.Add("GO", 9);
            estados.Add("MA", 10);
            estados.Add("MT", 11);
            estados.Add("MS", 12);
            estados.Add("MG", 13);
            estados.Add("PR", 14);
            estados.Add("PB", 15);
            estados.Add("PA", 16);
            estados.Add("PE", 17);
            estados.Add("PI", 18);
            estados.Add("RJ", 19);
            estados.Add("RN", 20);
            estados.Add("RS", 21);
            estados.Add("RO", 22);
            estados.Add("RR", 23);
            estados.Add("SC", 24);
            estados.Add("SE", 25);
            estados.Add("SP", 26);
            estados.Add("TO", 27);

            return estados[siglaEstado];
        }

        public bool AtualizarParticipante(ParticipanteModel participanteModel, string cpAvon, long mktPlaceCatalogoId)
        {
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            var _participante = _ParticipantRepository.GetAllByLogin(participanteModel.Login);

            //verifica a existencia do _participante
            if (_participante != null)
            {
                //verifica se o _participante já possui um login
                if (_participante.Login != null)
                {
                    //inicia atualização do _participante
                    _participante.Nome = participanteModel.Nome;
                    _participante.CPF = participanteModel.CPF;
                    _participante.RazaoSocial = participanteModel.RazaoSocial;
                    _participante.NomeFantasia = participanteModel.NomeFantasia;
                    _participante.CNPJ = participanteModel.CNPJ;
                    _participante.RG = participanteModel.RG;
                    _participante.Sexo = participanteModel.Sexo;
                    _participante.DataNascimento = participanteModel.DataNascimento;
                    _participante.CEP = participanteModel.CEP.Replace("-", "");
                    _participante.Endereco = participanteModel.Endereco;
                    _participante.Numero = participanteModel.Numero;
                    _participante.Complemento = participanteModel.Complemento;
                    _participante.Bairro = participanteModel.Bairro;
                    _participante.Cidade = participanteModel.Cidade;
                    _participante.EstadoId = this.BuscaEstado(participanteModel.Estado);
                    _participante.PontoReferencia = participanteModel.PontoReferencia;
                    _participante.DDDCel = participanteModel.DDDCel;
                    //_participante.SimuladorVisualizado = participanteModel.SimuladorVisualizado;

                    if (participanteModel.Celular != null)
                        _participante.Celular = participanteModel.Celular.Replace("-", "");

                    //_participante.DDDTel = participanteModel.DDDTel;
                    //if (participanteModel.Telefone != null)
                    //    _participante.Telefone = participanteModel.Telefone.Replace("-", "");

                    _participante.Email = participanteModel.Email;
                    _participante.DDDTelComercial = participanteModel.DDDTelComercial;
                    _participante.OptInComunicacaoFisica = participanteModel.OptInComunicacaoFisica;
                    _participante.OptInSms = participanteModel.OptInSms;
                    _participante.OptInEmail = participanteModel.OptInEmail;
                    _participante.OptinAceite = participanteModel.OptinAceite;

                    if (participanteModel.TelefoneComercial != null)
                        _participante.TelefoneComercial = participanteModel.TelefoneComercial.Replace("-", "");
                    _participante.DataAlteracao = DateTime.Now;

                    if (participanteModel.Senha != null)
                        _participante.Senha = HexEncoding.Encriptar(participanteModel.Senha);

                    _participante.StatusId = _participante.StatusId != (int)EnumDomain.StatusParticipante.Ativo ? (int)EnumDomain.StatusParticipante.Ativo : _participante.StatusId;
                    _participante.Ativo = _participante.StatusId == (int)EnumDomain.StatusParticipante.Ativo ? true : false;

                    if (_participante.DataCadastro == null)
                        _participante.DataCadastro = DateTime.Now;

                    //Inclusão de Catalogos
                    if (_participante.ParticipanteCatalogos == null)
                        _participante.ParticipanteCatalogos = new List<ParticipanteCatalogo>();


                    //Adiciona os catálogos caso não tenha
                    foreach (var catalogoModel in participanteModel.Catalogos)
                    {
                        var participanteCatalogo = _participante.ParticipanteCatalogos.FirstOrDefault(x => x.CatalogoId == catalogoModel.CatalogoId && x.Ativo == true);

                        //Caso não localize o catálogo
                        if (participanteCatalogo == null)
                        {
                            //Inativa todos catálogos dos participantes
                            foreach (var participanteCatalogoInativacao in _participante.ParticipanteCatalogos)
                            {
                                participanteCatalogoInativacao.Ativo = false;
                                participanteCatalogoInativacao.DataAlteracao = DateTime.Now;
                            }

                            //Adiciona catálogo
                            _participante.ParticipanteCatalogos.Add(new ParticipanteCatalogo()
                            {
                                ParticipanteId = _participante.Id,
                                DataInclusao = DateTime.Now,
                                CodigoMktPlace = catalogoModel.ParticipanteId,
                                CatalogoId = catalogoModel.CatalogoId,
                                Ativo = true
                            });
                        }
                        else if (participanteCatalogo != null && participanteCatalogo.CodigoMktPlace == 0)
                        {
                            //Participante não era cadastrado na base do mktplace, gerando id para o mesmo
                            participanteCatalogo.CodigoMktPlace = catalogoModel.ParticipanteId;
                            participanteCatalogo.DataAlteracao = DateTime.Now;
                        }
                    }

                    //Adiciona os CPs Avon caso não o tenha
                    var catalogoCP = new CatalogoCPService().ObterCatalogoCP(cpAvon, mktPlaceCatalogoId);
                    if (_participante.ParticipanteCPs.Count(x => x.CatalogoCPId == catalogoCP.Id) == 0)
                    {
                        if (_participante.ParticipanteCPs == null)
                            _participante.ParticipanteCPs = new List<ParticipanteCP>();

                        _participante.ParticipanteCPs.Add(new ParticipanteCP()
                        {
                            Ativo = true,
                            DataInclusao = DateTime.Now,
                            CatalogoCPId = new CatalogoCPService().ObterCatalogoCP(cpAvon, mktPlaceCatalogoId).Id
                        });
                    }

                    _ParticipantRepository.Update(_participante);
                    _ParticipantRepository.Dispose();
                    return true;
                }
                //se o login do _participante for nulo
                else
                {
                    throw new Exception("Login não definido para este participante, contate o administrador do sistema.");
                }
                // Se o _participante não foi encontrado:
            }
            else
            {
                //Cadastra a estrutura, caso não exista
                var estrutura = _ParticipantRepository.GetStructure(participanteModel.Estrutura);

                if (estrutura == null)
                {
                    EstruturaModel estruturaModel = new EstruturaModel()
                    {
                        Nome = participanteModel.Estrutura,
                        Ativo = true,
                        DataInclusao = DateTime.Now
                    };
                    var estruturaCadastrada = _estruturaService.CadastrarEstrutura(estruturaModel);

                    participanteModel.EstruturaId = estruturaCadastrada.Id;
                }
                else
                {
                    participanteModel.EstruturaId = estrutura.Id;
                }

                var participante = new Participante();
                participante.StatusId = Convert.ToInt32(EnumDomain.StatusParticipante.Ativo);
                participante.Login = participanteModel.Login;
                //participante.Senha = HexEncoding.Encriptar(participanteModel.Senha);
                participante.Nome = participanteModel.Nome;
                participante.CPF = participanteModel.CPF.RemoveFormatacaoCPF();
                participante.RazaoSocial = participanteModel.RazaoSocial;
                participante.CNPJ = participanteModel.CNPJ;
                participante.NomeFantasia = participanteModel.NomeFantasia;
                participante.RG = participanteModel.RG;
                participante.Sexo = participanteModel.Sexo;
                participante.DataNascimento = participanteModel.DataNascimento;
                participante.Endereco = participanteModel.Endereco;
                participante.Numero = participanteModel.Numero;
                participante.Complemento = participanteModel.Complemento;
                participante.Bairro = participanteModel.Bairro;
                participante.CEP = participanteModel.CEP.RemoverMascara();
                participante.Cidade = participanteModel.Cidade;
                participante.EstadoId = this.BuscaEstado(participanteModel.Estado);
                participante.PontoReferencia = participanteModel.PontoReferencia;
                participante.DDDCel = participanteModel.DDDCel;
                participante.Celular = participanteModel.Celular.RemoverMascara();
                //participante.DDDTel = participanteModel.DDDTel;
                //participante.Telefone = participanteModel.Telefone.RemoverMascara();
                participante.DDDTelComercial = participanteModel.DDDTelComercial;
                participante.TelefoneComercial = participanteModel.TelefoneComercial.RemoverMascara();
                participante.Ativo = true;
                participante.DataInclusao = DateTime.Now;
                participante.DataAlteracao = DateTime.Now;
                participante.OptInComunicacaoFisica = participanteModel.OptInComunicacaoFisica;
                participante.OptInEmail = participanteModel.OptInEmail;
                participante.OptInSms = participanteModel.OptInSms;
                participante.ParticipanteTeste = participanteModel.ParticipanteTeste;
                participante.Email = participanteModel.Email;
                participante.DataCadastro = DateTime.Now;

                if (participanteModel.OptinAceite == true)
                    participante.OptinAceite = true;
                else
                    participante.OptinAceite = false;

                List<ParticipantePerfil> listParticipantePerfil = new List<ParticipantePerfil>();
                listParticipantePerfil.Add(new ParticipantePerfil
                {
                    ParticipanteId = participante.Id,
                    PerfilId = Convert.ToInt32(participanteModel.PerfilId),
                    DataAlteracao = DateTime.Now,
                    DataInclusao = DateTime.Now,
                    Ativo = true
                });
                participante.ParticipantePerfil = listParticipantePerfil;

                List<ParticipanteEstrutura> listParticipanteEstrutura = new List<ParticipanteEstrutura>();
                listParticipanteEstrutura.Add(new ParticipanteEstrutura
                {
                    ParticipanteId = participante.Id,
                    EstruturaId = participanteModel.EstruturaId,
                    DataInclusao = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Ativo = true
                });
                participante.ParticipanteEstrutura = listParticipanteEstrutura;

                //Inclusão de Catalogos
                if (participante.ParticipanteCatalogos == null)
                    participante.ParticipanteCatalogos = new List<ParticipanteCatalogo>();

                var codigoMktPlace = participanteModel.Catalogos[0].CodigoMktPlace;
                var participantId = participanteModel.Catalogos[0].ParticipanteId;

                var listCatalogo = _CatalogRepository.GetCatalog(codigoMktPlace);
                _CatalogRepository.Dispose();
                var participanteCatalogo = new ParticipanteCatalogo();

                if (listCatalogo != null)
                {
                    participanteCatalogo.CodigoMktPlace = participantId;
                    participanteCatalogo.CatalogoId = listCatalogo.Id;
                    participanteCatalogo.Ativo = true;
                    participanteCatalogo.DataInclusao = DateTime.Now;
                }

                participante.ParticipanteCatalogos.Add(participanteCatalogo);

                //Inclusão de CatálogoCP Avon
                if (participante.ParticipanteCPs == null)
                    participante.ParticipanteCPs = new List<ParticipanteCP>();

                participante.ParticipanteCPs.Add(new ParticipanteCP()
                {
                    Ativo = true,
                    DataInclusao = DateTime.Now,
                    CatalogoCPId = new CatalogoCPService().ObterCatalogoCP(cpAvon, mktPlaceCatalogoId).Id
                });

                _ParticipantRepository.Create(participante);
                //Enviar e-mail de boas vindas
                //var participanteService = new ParticipanteService();
                //participanteService.EnviarEmailDeBoasVindas(participante.Login);

                _ParticipantRepository.Dispose();
                return true;
            }

        }

        public void AlterarSenha(ParticipanteModel participanteModel)
        {
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            var _participante = _ParticipantRepository.GetById(participanteModel.Id);

            //verifica a existencia do _participante
            if (_participante != null)
            {
                //verifica se o _participante já possui um login
                if (_participante.Login != null)
                {

                    if (_participante.Senha != null && _participante.Senha != HexEncoding.Encriptar(participanteModel.SenhaAtual))
                        throw new Exception("Senha Atual não confere.");

                    //inicia atualização da senha do Participante
                    if (participanteModel.Senha != null)
                        _participante.Senha = HexEncoding.Encriptar(participanteModel.Senha);
                    else
                        throw new Exception("Senha não pode ser vazia.");

                    //Atualiza Senha do participante
                    _ParticipantRepository.Update(_participante);
                    _ParticipantRepository.Dispose();
                }
                //se o login do _participante for nulo
                else
                {
                    throw new Exception("Login não definido para este participante, contate o administrador do sistema.");
                }
                // Se o _participante não foi encontrado:
            }
            else
            {
                throw new Exception("Participante não encontrado no sistema");
            }

        }

        public void AtualizarSimuladorVisualizado(ParticipanteModel participanteModel)
        {
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            var _participante = _ParticipantRepository.GetAllByLogin(participanteModel.Login);

            //verifica a existencia do _participante
            if (_participante != null)
            {
                //simulador couch marke visualizado
                _participante.SimuladorVisualizado = true;

                //Atualiza a validação de visualização do couchmarke do participante
                _ParticipantRepository.Update(_participante);
                _ParticipantRepository.Dispose();
            }
            else
            {
                throw new Exception("Participante não encontrado no sistema");
            }

        }

        public string ObterEmailParticipante(string login)
        {
            var _participantRepository = new ParticipantRepository();

            var email = _participantRepository.GetByLogin(login)?.Email;

            _participantRepository.Dispose();

            return email;
        }

        public ResgatesCampanha ObterCatalogoCPParticipante(string login)
        {
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();

            var _participante = _ParticipantRepository.GetAllByLogin(login);

            var CatalogoCPId = _participante.ParticipanteCPs.Max(x => x.CatalogoCPId);

            var catalogoCP = new CatalogoCPService().ObterCPsPorCatalogoCPId(CatalogoCPId);

            var resgatesCampanha = new ResgatesCampanha();
            {
                resgatesCampanha.Campanha = catalogoCP.CP;
                resgatesCampanha.TotalResgates = catalogoCP.Resgates;
            };

            _ParticipantRepository.Dispose();

            return resgatesCampanha;
        }

        public ParticipanteModel ObterParticipantePorLogin(string login)
        {
            var _participantRepository = new ParticipantRepository();

            var _participante = _participantRepository.GetByLogin(login);

            var _participanteModel = BuildParticipante(_participante);

            _participantRepository.Dispose();

            return _participanteModel;
        }

        // ValidaTokenAvon
        //"mrktCd": "BR",
        //"userId": "57669043",
        //"token": "xTo5txGmql2petfVPO7BTh/xn764vDLsBrh7S64iTSoxkiG8eX9bqJux1Cxf+58GGgxPHkpc2WY="

        //"errCd": "302",
        //"errMsg": "A validação token de segurança falhou",
        //"logId": "77424190-d539-4b13-a0b9-91219cc4278e"

        // Response Messages
        //302 A validação token de segurança falhou
        //400 Bad Request. The URI requested is invalid or malformed.
        //401 Unauthorized user.
        //404 Not Found. The URI requested is invalid or the resource requested was not found.
        //500 Internal Error. An application Error was encountered while executing the request
        public bool ValidaTokenAvon(string ra, string token)
        {
            try
            {
                if (token == "InternoLTM") //requisições internas (admin, hotsite)
                    return true;

                if (Convert.ToBoolean(ConfiguracaoService.UrlGetValidarTokenAvon()))
                {
                    var jsonRequest = new JsonRequest(ConfiguracaoService.UrlGetTokenAvon(),
                    new KeyValuePair<string, string>("devKey", ConfiguracaoService.DevkeyAvon()),
                    new KeyValuePair<string, string>("acctNr", ra),
                    new KeyValuePair<string, string>("X-Sec-Token", token));

                    jsonRequest.Post<dynamic>("", "", out HttpStatusCode responseStatusCode);

                    return responseStatusCode == HttpStatusCode.OK;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ParticipanteModel ObterParticipantePorLogin(string login, long catalogId)
        {
            //Insntancia Variáveis
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            var _participante = _ParticipantRepository.GetByLogin(login, catalogId);
            var participanteModel = BuildParticipanteDTO(_participante);
            _ParticipantRepository.Dispose();
            return participanteModel;
        }

        public ParticipanteModel ObterParticipanteCatalogoPorLogin(string login, long catalogId)
        {
            string chave = "participanteDto_";
            var _participante = ObterHeaderParticipanteDtoCache(login, chave);
            if (_participante != null)
            {
                if (_participante.MktPlaceCatalogoId != catalogId)
                    _participante = null;

            }
            if (_participante == null)
            {
                //Busca Participante na base de dados
                ParticipantRepository _ParticipantRepository = new ParticipantRepository();
                _participante = _ParticipantRepository.GetParticipanteCatalogoByLogin(login, catalogId);
                _ParticipantRepository.Dispose();
                if (_participante != null)
                {
                    LimparParticipanteDtoCache(_participante.Login, chave);
                    GravarParticipanteDtoCache(_participante.Login, chave, _participante);
                }
            }

            var participanteModel = BuildParticipanteDTO(_participante);
            return participanteModel;
        }

        public bool IncluirCatalogo(int participanteId, long mktPlaceCatalogoId)
        {
            ParticipantRepository _ParticipantRepository = new ParticipantRepository();
            var _participante = _ParticipantRepository.GetById(participanteId);

            foreach (var participanteCatalogo in _participante.ParticipanteCatalogos)
            {
                participanteCatalogo.Ativo = false;
                participanteCatalogo.DataAlteracao = DateTime.Now;
            }

            var catalogo = _CatalogRepository.GetCatalog(mktPlaceCatalogoId);
            _CatalogRepository.Dispose();

            if (catalogo == null)
                throw new ApplicationException("Catálogo Inexistente");

            _participante.ParticipanteCatalogos.Add(new ParticipanteCatalogo()
            {
                Ativo = true,
                CatalogoId = catalogo.Id,
                ParticipanteId = participanteId,
                DataInclusao = DateTime.Now
            });

            //Inserir dados do participanteCatalogo
            _ParticipantRepository.Update(_participante);
            _ParticipantRepository.Dispose();
            return true;
        }

        public static DataTable ListaParticipante(int? Id, int? EstruturaId, int? PerfilId, int? StatusId, int Inicio, int Quantidade, int sortColumn, string sortDirection, string CPF, string Login, string CNPJ, string nome, out int total)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;

            string proc = "JP_Sel_ParticipanteResumo";
            if (Id > 0)
            {
                proc = "JP_Sel_Participante";
            }

            List<SqlParameter> listParam = new List<SqlParameter>();

            listParam.Add(new SqlParameter { ParameterName = "@Id", Value = Id, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@EstruturaId", Value = EstruturaId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PerfilId", Value = PerfilId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@StatusId", Value = StatusId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@StartNumber", Value = Inicio, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            //listParam.Add(new SqlParameter { ParameterName = "@Quantidade", Value = Quantidade, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CPF", Value = CPF, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@RowsPerPage", Value = Quantidade, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@CNPJ", Value = CNPJ, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@Nome", Value = nome, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@SortColumn", Value = sortColumn, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@SortDirection", Value = sortDirection, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
            SqlParameter totalCount = new SqlParameter { ParameterName = "@Total", Value = 0, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            DataTable table = DataProvider.SelectStoreProcedureWithOutPut(proc, listParam, totalCount);

            total = Convert.ToInt32(totalCount.Value);

            return table;

        }

        public static DataTable ListaParticipanteAtivoDmenos1(int MktPlaceCatalogoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ParticipanteAtivoTop10";
            List<SqlParameter> listParam = new List<SqlParameter>();
            DateTime DataInclusao = DateTime.Now.AddDays(-10);
            listParam.Add(new SqlParameter { ParameterName = "@MktPlaceCatalogoId", Value = MktPlaceCatalogoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@createdate", Value = DataInclusao, SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ListaImportacaoParticipanteErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpParticipanteErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static DataTable ListaImportacaoParticipanteHierarquiaErro(int ArquivoId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_ImpParticipanteHierarquiaErro";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);
            return table;
        }

        public static bool ProcessaParticipanteArquivo(int ArquivoId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_Participante";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[1].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[2].Value;

            return blnSucesso;
        }

        public static bool ProcessaParticipanteHierarquiaArquivo(int ArquivoId, int PeriodoId, out int countErro)
        {
            countErro = 0;
            bool blnSucesso = false;

            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Imp_ParticipanteHierarquia";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@ArquivoId", Value = ArquivoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@PeriodoId", Value = PeriodoId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@ImportadoSucesso", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
            listParam.Add(new SqlParameter { ParameterName = "@CountErro", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });

            DataProvider.NonqueryProc(proc, listParam);

            if ((Boolean)listParam[2].Value)
            {
                blnSucesso = true;
            }
            else
            {
                blnSucesso = false;
            }
            countErro = (int)listParam[3].Value;

            return blnSucesso;
        }

        public static bool ImportarArquivoParticipante(DataTable dtParticipante, int ArquivoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.ParticipanteImportacao";
                try
                {
                    dtParticipante.Columns.Add("ArquivoId");
                    foreach (DataRow dr in dtParticipante.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                    }
                    bulkCopy.WriteToServer(dtParticipante);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool ImportarArquivoParticipanteHierarquia(DataTable dtParticipanteHierarquia, int ArquivoId, int PeriodoId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString))
            {
                bulkCopy.DestinationTableName =
                    "dbo.ParticipanteHierarquiaImportacao";
                try
                {
                    dtParticipanteHierarquia.Columns.Add("ArquivoId");
                    dtParticipanteHierarquia.Columns.Add("PeriodoId");
                    foreach (DataRow dr in dtParticipanteHierarquia.Rows)
                    {
                        dr["ArquivoId"] = ArquivoId;
                        dr["PeriodoId"] = PeriodoId;
                    }
                    bulkCopy.WriteToServer(dtParticipanteHierarquia);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static string ExportaArquivoParticipanteErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoParticipanteErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "participante/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "participante/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        public static string ExportaArquivoParticipanteHierarquiaErro(int ArquivoId)
        {
            string strDownloadArquivoErro = "";

            DataSet ds = new DataSet();
            ds.Tables.Add(ListaImportacaoParticipanteHierarquiaErro(ArquivoId));

            string strArquivoGerado = "";
            ExcelExport.WriteXLSFile(
                ds,
                "hierarquia/erro/",
                out strArquivoGerado);

            strDownloadArquivoErro = "hierarquia/erro/" + strArquivoGerado;

            return strDownloadArquivoErro;
        }

        #region Redis
        private ParticipanteDTO ObterHeaderParticipanteDtoCache(string accountNumber, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            return cacheAttribute.GetHashObjectExtrato<ParticipanteDTO>(chave + "_" + accountNumber);
        }
        private void GravarParticipanteDtoCache(string accountNumber, string chave, object participanteModel)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.StoreObjectExtrato(chave + "_" + accountNumber, participanteModel, "CacheParticipanteDto");
        }
        private void LimparParticipanteDtoCache(string accountNumber, string chave)
        {
            CacheAttribute cacheAttribute = new CacheAttribute();
            cacheAttribute.KeyDeleteAsync(chave + "_" + accountNumber);

        }

        #endregion
    }
}
