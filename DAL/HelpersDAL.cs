using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace DAL
{
    public class HelpersDAL
    {
        public void ResizeAndSaveImage(int newWidth, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                var scaleFactor = (image.Height / image.Width);
                var newHeight = (int)(newWidth * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image,imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
                
            }
        }

        public void ResizeAndSaveAzure(int newWidth, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath,false, false))
            {
                
                var scaleFactor = image.Height / image.Width;
                var newHeight = (int)(newWidth * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);


                string connStr = ConfigurationManager.AppSettings["AzureStorage"].ToString();

                CloudStorageAccount account = CloudStorageAccount.Parse(connStr);

                CloudBlobClient client = account.CreateCloudBlobClient();

                CloudBlobContainer container = client.GetContainerReference("images");

                container.CreateIfNotExists();

                String FileName = Path.GetFileName(targetPath);
                String FileExt = Path.GetExtension(targetPath).ToUpper();

                CloudBlockBlob blob = container.GetBlockBlobReference(FileName);
                if (FileExt == ".PNG")
                {
                    blob.Properties.ContentType = "image/png";
                }
                else
                {
                    blob.Properties.ContentType = "image/jpeg";
                }
                blob.UploadFromFile(targetPath);

                File.Delete(targetPath);
            }
        }

        public void SaveAzure(string FileType, HttpPostedFileBase File, string FileName)
        {
            string connStr = ConfigurationManager.AppSettings["AzureStorage"].ToString();

            CloudStorageAccount account = CloudStorageAccount.Parse(connStr);

            CloudBlobClient client = account.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(FileType);

            container.CreateIfNotExists();

            CloudBlockBlob blob = container.GetBlockBlobReference(FileName);

            blob.UploadFromStream(File.InputStream);                
            
        }
    }
}
