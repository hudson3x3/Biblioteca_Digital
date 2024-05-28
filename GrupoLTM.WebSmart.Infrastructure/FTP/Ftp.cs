

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;

namespace GrupoLTM.WebSmart.Infrastructure.FTP
{
    public class Ftp
    {
        private readonly string _host;
        private readonly string _user;
        private readonly string _pass;
        private FtpWebRequest _ftpRequest;
        private FtpWebResponse _ftpResponse;
        private Stream _ftpStream;
        private const int BufferSize = 2048;

        /* Construct Object */
        public Ftp(string host, string userName, string password)
        {
            _host = host; _user = userName; _pass = password;
        }

        /* Download FileService */
        public void Download(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", remoteFile));
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                //_ftpRequest.UsePassive = true;
                _ftpRequest.UsePassive = false;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Open a FileService Stream to Write the Downloaded FileService */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[BufferSize];
                if (_ftpStream != null)
                {
                    int bytesRead = _ftpStream.Read(byteBuffer, 0, BufferSize);
                    /* Download the FileService by Writing the Buffered Data Until the Transfer is Complete */
                    try
                    {
                        while (bytesRead > 0)
                        {
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = _ftpStream.Read(byteBuffer, 0, BufferSize);
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }

                /* Resource Cleanup */
                localFileStream.Close();

                if (_ftpStream != null)
                    _ftpStream.Close();

                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Upload FileService */
        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                int contentLength = 0;

                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", remoteFile));
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = false;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpRequest.GetRequestStream();
                /* Open a FileService Stream to Read the FileService for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[localFileStream.Length];
                int bytesToRead = (int)localFileStream.Length;
                int bytesRead = 0;

                while (bytesToRead > 0)
                {
                    int bytesSent = localFileStream.Read(byteBuffer, 0, bytesToRead);
                    if (bytesSent == 0)
                        break;

                    bytesRead += bytesSent;
                    bytesToRead -= bytesSent;
                }
                /* Upload the FileService by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    using (var dataStream = new MemoryStream(byteBuffer))
                    {
                        contentLength = dataStream.Read(byteBuffer, 0, (int)byteBuffer.Length);

                        while (contentLength != 0)
                        {
                            _ftpStream.Write(byteBuffer, 0, byteBuffer.Length);
                            contentLength = dataStream.Read(byteBuffer, 0, byteBuffer.Length);
                        }
                    }
                    _ftpRequest = null;
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                _ftpStream.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        public void Upload(string remoteFile, Stream localFile)
        {
            try
            {
                int contentLength = 0;

                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", remoteFile));
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                //_ftpRequest.UsePassive = false;
                _ftpRequest.UsePassive = false;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 600000;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpRequest.GetRequestStream();
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[localFile.Length];
                int bytesToRead = (int)localFile.Length;
                int bytesRead = 0;

                while (bytesToRead > 0)
                {
                    int bytesSent = localFile.Read(byteBuffer, 0, bytesToRead);
                    if (bytesSent == 0)
                        break;

                    bytesRead += bytesSent;
                    bytesToRead -= bytesSent;
                }
                /* Upload the FileService by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    using (var dataStream = new MemoryStream(byteBuffer))
                    {
                        contentLength = dataStream.Read(byteBuffer, 0, (int)byteBuffer.Length);

                        while (contentLength != 0)
                        {
                            _ftpStream.Write(byteBuffer, 0, byteBuffer.Length);
                            contentLength = dataStream.Read(byteBuffer, 0, byteBuffer.Length);
                        }
                    }
                    _ftpRequest = null;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                    //throw ex;
                }
                /* Resource Cleanup */
                localFile.Close();
                _ftpStream.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
            return;
        }

        public void UploadToFile(string diretorio, byte[] texto, string nomeArquivo)
        {
            using (var dataStream = new MemoryStream(texto))
            {
                FileStream file = new FileStream(Path.Combine(diretorio, nomeArquivo), FileMode.Create, FileAccess.Write);
                dataStream.WriteTo(file);
                file.Close();
                dataStream.Close();
            }
        }

        ///* Create File FileService */
        public void CreateFile(string remoteFile, byte[] texto)
        {
            try
            {
                int contentLength = 0;

                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", remoteFile));
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = false;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                using (var dataStream = new MemoryStream(texto))
                using (var requestStream = _ftpRequest.GetRequestStream())
                {
                    contentLength = dataStream.Read(texto, 0, (int)texto.Length);

                    while (contentLength != 0)
                    {
                        requestStream.Write(texto, 0, (int)texto.Length);
                        contentLength = dataStream.Read(texto, 0, (int)texto.Length);
                    }
                }

                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Delete FileService */
        public void Delete(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Resource Cleanup */
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Rename FileService */
        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + currentFileNameAndPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the FileService */
                _ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Resource Cleanup */
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Create a New Directory on the FTP Server */
        public void CreateDirectory(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + newDirectory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Resource Cleanup */
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Get the Date/Time a FileService was Created */
        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { fileInfo = ftpReader.ReadToEnd(); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
                /* Return FileService Created Date Time */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* Get the Size of a FileService */
        public string GetFileSize(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
                /* Return FileService Size */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* List Directory Contents FileService/Folder Name Only */
        public string[] DirectoryListSimple(string directory)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", directory));
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.Proxy = null;
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = false;
                _ftpRequest.KeepAlive = false;
                //_ftpRequest.EnableSsl = true;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                if (_ftpStream != null)
                {
                    StreamReader ftpReader = new StreamReader(_ftpStream);
                    /* Store the Raw Response */
                    string directoryRaw = null;
                    /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                    try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    /* Resource Cleanup */
                    ftpReader.Close();
                    _ftpStream.Close();
                    _ftpResponse.Close();
                    _ftpRequest = null;
                    /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                    try
                    {
                        if (directoryRaw != null)
                        {
                            string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                /* Create an FTP Request */
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                /* When in doubt, use these options */
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                /* Specify the Type of FTP Request */
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                _ftpStream = _ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(_ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        public string[] ReadFile(string filePath)
        {
            /* Create an FTP Request */
            _ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", filePath));
            /* Log in to the FTP Server with the User Name and Password Provided */
            _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
            /* When in doubt, use these options */
            _ftpRequest.UseBinary = true;
            //_ftpRequest.UsePassive = true;
            _ftpRequest.UsePassive = false;
            _ftpRequest.KeepAlive = false;

            FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();
            //use the response like below
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            var retorno = reader.ReadToEnd()
                .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            reader.Close();

            return retorno;
        }

        public byte[] ReadFileToBytes(string filePath)
        {
            /* Create an FTP Request */
            _ftpRequest = (FtpWebRequest)WebRequest.Create(string.Concat(ConfigurationManager.AppSettings["ftp"], "/", filePath));
            /* Log in to the FTP Server with the User Name and Password Provided */
            _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
            /* When in doubt, use these options */
            _ftpRequest.UseBinary = true;
            //_ftpRequest.UsePassive = true;
            _ftpRequest.UsePassive = false;
            _ftpRequest.KeepAlive = false;

            FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();
            //use the response like below
            Stream responseStream = response.GetResponseStream();

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
            responseStream.Close();
            response.Close();
        }
    }
}
