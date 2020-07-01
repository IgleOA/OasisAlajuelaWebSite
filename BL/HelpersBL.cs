using System.IO;
using System.Web;
using DAL;
using ET;

namespace BL
{
    public class HelpersBL
    {
        private HelpersDAL HDAL = new HelpersDAL();

        //public void ResizeAndSaveImage(int newWidth, Stream sourcePath, string targetPath)
        //{
        //    HDAL.ResizeAndSaveImage(newWidth, sourcePath, targetPath);
        //}

        public void ResizeAndSaveAzure(int newWidth, HttpPostedFileBase File, string targetPath)
        {
            HDAL.ResizeAndSaveAzure(newWidth, File, targetPath);
        }

        public void URLResizeAndSaveAzure(int newWidth, string File, string targetPath)
        {
            HDAL.URLResizeAndSaveAzure(newWidth, File, targetPath);
        }

        public void SaveAzure(AzureStorage File)
        {
            HDAL.SaveAzure(File);
        }
    }
}
