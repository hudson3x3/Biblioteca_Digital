using System;
using System.Linq;
using System.Collections.Generic;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Infrastructure.Mail;
using GrupoLTM.WebSmart.Infrastructure.Cripto;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Infrastructure.Configuration;

namespace GrupoLTM.WebSmart.Services
{
    public class UsuarioAdminService : BaseService<UsuarioAdm>
    {
        public UsuarioAdm Login(string login, string senha)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();

                var usuario = repUsuario.Filter<UsuarioAdm>(x => x.Login == login && x.Ativo).FirstOrDefault();

                if (usuario == null)
                    throw new SistemaException("Usuario não cadastrado ou inativo: " + login);

                //if (usuario.Senha != HexEncoding.Encriptar(senha))
                if (usuario.Senha != senha)
                {
                    usuario.TentativasLogin += 1;

                    if (usuario.TentativasLogin >= 3)
                    {

                        usuario.Ativo = false;
                    }

                    usuario.DataAlteracao = DateTime.Now;
                    repUsuario.Update(usuario);

                    throw new SistemaException($"Senha inválida, id: {usuario.Id}, login: {usuario.Login}, tentativas: {usuario.TentativasLogin}");
                }

                usuario.TentativasLogin = 0;
                usuario.DataAlteracao = DateTime.Now;
                repUsuario.Update(usuario);

                return usuario;
            }
        }

        public bool EnviarSenhaEmail(string login, string novaSenha = null)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();

                var usuario = repUsuario.Filter<UsuarioAdm>(x => x.Login == login && x.Ativo).FirstOrDefault();

                if (usuario != null)
                {
                    var senha = novaSenha ?? HexEncoding.Descriptar(usuario.Senha);

                    var loginModel = new LoginModel
                    {
                        Login = usuario.Login,
                        Nome = usuario.Nome,
                        Email = usuario.Email
                    };

                    var _sendGrid = new SendGridModel
                    {
                        template_id = Settings.EmailConfiguracao.SendGrid.TemplateId,
                        from = new From()
                    };

                    _sendGrid.from.name = "Grupo LTM";
                    _sendGrid.from.email = "comunicacao@grupoltm.com.br";

                    var personalizations = new Personalizations();

                    var destinatario = new Destinatario
                    {
                        email = usuario.Email,
                        name = usuario.Nome
                    };

                    personalizations.To = new List<Destinatario> { destinatario };

                    personalizations.dynamic_template_data = new ParamDinamico
                    {
                        NOME = usuario.Nome,
                        LOGIN = usuario.Login,
                        SENHA = senha
                    };

                    _sendGrid.personalizations = new List<Personalizations> { personalizations };

                    var obj = _sendGrid.ToJson();

                    var emailService = new EmailSendGridService();
                    var envio = emailService.EnviarEmail(_sendGrid);

                    return envio;
                }
                else
                {
                    throw new SistemaException("Usuario não encontrado");
                }
            }
        }

        public bool EnviarEmailDeCadastroDeUsuario(string login)
        {
            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();

                var usuario = repUsuario.Filter<UsuarioAdm>(
                    x => x.Login == login
                    ).FirstOrDefault();

                if (usuario != null)
                {
                    var senha = HexEncoding.Descriptar(usuario.Senha);

                    var loginModel = new LoginModel
                    {
                        Login = usuario.Login,
                        Nome = usuario.Nome,
                        Email = usuario.Email
                    };

                    var _sendGrid = new SendGridModel
                    {
                        template_id = Settings.EmailConfiguracao.SendGrid.TemplateId,
                        from = new From()
                    };

                    _sendGrid.from.name = "Grupo LTM";
                    _sendGrid.from.email = "comunicacao@grupoltm.com.br";

                    var personalizations = new Personalizations();

                    var destinatario = new Destinatario
                    {
                        email = usuario.Email,
                        name = usuario.Nome
                    };

                    personalizations.To = new List<Destinatario> { destinatario };

                    personalizations.dynamic_template_data = new ParamDinamico
                    {
                        NOME = usuario.Nome,
                        LOGIN = usuario.Login,
                        SENHA = senha
                    };

                    _sendGrid.personalizations = new List<Personalizations> { personalizations };

                    var emailSendGrid = new EmailSendGrid();
                    var envio = emailSendGrid.EnviarEmail(_sendGrid);

                    if (envio)
                    {
                        var data = new { ok = true, msg = "Mensagem enviada com sucesso." };
                        return true;
                    }
                    else
                    {
                        throw new Exception("Houve um erro ao enviar sua senha.");
                    }
                }
                else
                {
                    var data = new { ok = false, msg = "Não foi possível enviar a senha" };
                    return false;
                }
            }
        }
    }
}
