using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using GrupoLTM.WebSmart.Infrastructure.Storage;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public class Imagem
    {
        public IStorage Storage = new Storage.Azure.Blob.Storage();

        public bool ConverteImagem(string nomeArquivo, string imagemCaminho, string imagemCaminhoSalvar, int maxWidth, int maxHeight)
        {
            try
            {
                var image = Image.FromFile(imagemCaminho);
                //var img = Image.FromStream(stream);
                var newImage = FixedSize(image, maxWidth, maxHeight);
                newImage.Save(imagemCaminhoSalvar + nomeArquivo, ImageFormat.Png);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConverteImagem(string nomeArquivo, string imagemCaminho, float DPI, string imagemCaminhoSalvar)
        {
            try
            {
                Bitmap bitmap = new Bitmap(imagemCaminho);
                Bitmap newBitmap = new Bitmap(bitmap);
                newBitmap.SetResolution(DPI, DPI);
                imagemCaminhoSalvar = imagemCaminhoSalvar + nomeArquivo;
                newBitmap.Save(imagemCaminhoSalvar, ImageFormat.Jpeg);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        static Image FixedSize(Image image, int Width, int Height)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format32bppRgb);

            bmPhoto.MakeTransparent(Color.White);

            bmPhoto.SetResolution(image.HorizontalResolution,
                             image.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Transparent);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public bool ConverteImagem(Stream arquivo, string name, string diretorio, int w, int h)
        {
            try
            {
                var image = Image.FromStream(arquivo);
                var newImage = FixedSize(image, w, h);
                var format = GetFormat(name);

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    newImage.Save(ms, format);
                    Storage.UploadBlobAsync(ms, diretorio + "/" + name);
                }

                return true;
            }
            catch (Exception re)
            {
                return false;
            }

        }

        private static ImageFormat GetFormat(string format)
        {
            format = format.ToLower();

            if (format.Contains("jpg"))
                return ImageFormat.Jpeg;
            if (format.Contains("png"))
                return ImageFormat.Png;
            if (format.Contains("ico"))
                return ImageFormat.Icon;
            if (format.Contains("jpeg"))
                return ImageFormat.Jpeg;
            if (format.Contains("bmp"))
                return ImageFormat.Bmp;
            
            return ImageFormat.Jpeg;
        }
    }
}
