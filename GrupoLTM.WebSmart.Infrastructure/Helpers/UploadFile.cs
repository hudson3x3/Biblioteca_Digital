using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using GrupoLTM.WebSmart.Infrastructure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Drawing;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public static class UploadFile
    {
        static UploadFile()
        {
            Storage = new Storage.Azure.Blob.Storage();
        }

        private static readonly IStorage Storage;


        public static void Upload(Stream stream, string filename)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "Argument is null");

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename", "Argument is null or empty");

            stream.Position = 0;

            Storage.UploadBlobAsync(stream, filename);
        }


        public static UploadFileResult Upload(HttpPostedFileBase file, string[] extensoes, int tamanhoMaximoMB, string caminhoSalvar)
        {
            UploadFileResult uploadFileResult = new UploadFileResult();

            try
            {
                //Verifica arquivo
                if (file.ContentLength == 0)
                {
                    uploadFileResult.arquivoSalvo = false;
                    uploadFileResult.nomeArquivoGerado = "";
                    uploadFileResult.mensagem = "Arquivo vazio.";
                    return uploadFileResult;
                }
                //Verifica extensao
                if (!extensoes.Contains(file.FileName.Substring(file.FileName.LastIndexOf("."))))
                {
                    uploadFileResult.arquivoSalvo = false;
                    uploadFileResult.nomeArquivoGerado = "";
                    uploadFileResult.mensagem = "Extensão do arquivo não permitida.";
                    return uploadFileResult;
                }
                //Verifica tamanho máximo (Converter para megabyte)
                double tamanho = file.ContentLength / 1024;
                if (tamanho > tamanhoMaximoMB)
                {
                    uploadFileResult.arquivoSalvo = false;
                    uploadFileResult.nomeArquivoGerado = "";
                    uploadFileResult.mensagem = "Tamanho do arquivo não permitido.";
                    return uploadFileResult;
                }

                var fileName = DateTime.Now.ToString("ddMMyyyy_HHmmss")+ file.FileName.Substring(file.FileName.LastIndexOf("."));

                var destinationBlob = caminhoSalvar + fileName;

                Storage.UploadBlobAsync(file.InputStream, destinationBlob);

                uploadFileResult.arquivoSalvo = true;
                uploadFileResult.nomeArquivoGerado = fileName;
                uploadFileResult.mensagem = "Arquivo salvo com sucesso";

                return uploadFileResult;
            }
            catch (Exception exc)
            {
                uploadFileResult.arquivoSalvo = false;
                uploadFileResult.nomeArquivoGerado = "";
                uploadFileResult.mensagem = exc.Message;

                return uploadFileResult;
            }
        }
        public static UploadFileResult UploadWhatsApp(HttpPostedFileBase file, string fileName, string[] extensoes, int tamanhoMaximoMB, string caminhoSalvar)
        {
            UploadFileResult uploadFileResult = new UploadFileResult();

            try
            {
                //Verifica arquivo
                if (file.ContentLength == 0)
                {
                    uploadFileResult.arquivoSalvo = false;
                    uploadFileResult.nomeArquivoGerado = "";
                    uploadFileResult.mensagem = "Arquivo vazio.";
                    return uploadFileResult;
                }
                //Verifica extensao
                if (!extensoes.Contains(file.FileName.Substring(file.FileName.LastIndexOf("."))))
                {
                    uploadFileResult.arquivoSalvo = false;
                    uploadFileResult.nomeArquivoGerado = "";
                    uploadFileResult.mensagem = "Extensão do arquivo não permitida.";
                    return uploadFileResult;
                }
                //Verifica tamanho máximo (Converter para megabyte)
                double tamanho = file.ContentLength / 1024;
                if (tamanho > tamanhoMaximoMB)
                {
                    uploadFileResult.arquivoSalvo = false;
                    uploadFileResult.nomeArquivoGerado = "";
                    uploadFileResult.mensagem = "Tamanho do arquivo não permitido.";
                    return uploadFileResult;
                }

                //var fileName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + file.FileName.Substring(file.FileName.LastIndexOf("."));

                var destinationBlob = fileName;

                Storage.UploadBlobAsyncWhatsApp(file.InputStream, destinationBlob);

                uploadFileResult.arquivoSalvo = true;
                uploadFileResult.nomeArquivoGerado = fileName;
                uploadFileResult.mensagem = "Arquivo salvo com sucesso";

                return uploadFileResult;
            }
            catch (Exception exc)
            {
                uploadFileResult.arquivoSalvo = false;
                uploadFileResult.nomeArquivoGerado = "";
                uploadFileResult.mensagem = exc.Message;

                return uploadFileResult;
            }
        }

    }

    public static class DownloadFile
    {
        static DownloadFile()
        {
            Storage = new Storage.Azure.Blob.Storage();
        }

        private static readonly IStorage Storage;

        public static Stream Download(string fileName, string filePath)
        {
            return Storage.GetFile(Path.Combine(filePath, fileName));
        }

    }
}
