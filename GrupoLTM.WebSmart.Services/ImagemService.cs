using AutoMapper;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Infrastructure.Configuration;
using GrupoLTM.WebSmart.Infrastructure.Data;
using GrupoLTM.WebSmart.Infrastructure.TXT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GrupoLTM.WebSmart.Services
{
    public class ImagemService
    {

        public static SMSAgendamentoImagem CadastrarImagem(string fileName, string filePath, Guid SmsAgendamentoId)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                     //Grava o arquivo na tabela de Arquivos
                    IRepository repImagem = context.CreateRepository<SMSAgendamentoImagem>();
                    SMSAgendamentoImagem imagem = new SMSAgendamentoImagem();
                    imagem.NomeImagem = fileName;
                    imagem.CaminhoArquivo = filePath;
                    imagem.DataAlteracao = DateTime.Now;
                    imagem.DataInclusao = DateTime.Now;
                    imagem.SMSAgendamentoId = SmsAgendamentoId;

                  //  using (TransactionScope scope = new TransactionScope())
                 //   {
                        repImagem.Create<SMSAgendamentoImagem>(imagem);
                        repImagem.SaveChanges();
                  //    scope.Complete();
                 //   }
                    return imagem;
                }
            }
            catch (Exception ex)
            {
            //    gravaLogErro("Erro no Cadastro de Imagem", ex.Message, "GrupoLTM.WebSmart.Services", string.Format("CadastrarImagem({0})", filePath), "jobCatalog");
                throw ex;
            }

        }
        public static bool AtualizaImagem(int idImagem, EnumDomain.StatusArquivo eStatusArquivo)
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repImagem = context.CreateRepository<SMSAgendamentoImagem>();
                    var imagem = repImagem.Find<SMSAgendamentoImagem>(idImagem);
                    if (imagem != null)
                    {
                       imagem.DataAlteracao = DateTime.Now;
                    }

                    repImagem.Update<SMSAgendamentoImagem>(imagem);
                    repImagem.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                throw;
            }

        }        
   
        public List<SMSAgendamentoImagem> GetAll()
        {
            try
            {
                using (IUnitOfWork context = UnitOfWorkFactory.Create())
                {
                    IRepository repImagem = context.CreateRepository<SMSAgendamentoImagem>();


                    var lstImagems = repImagem.All<SMSAgendamentoImagem>().ToList();

                    return lstImagems;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

    }
}
