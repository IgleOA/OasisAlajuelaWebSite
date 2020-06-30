using System.IO;
using System.Web;
using DAL;

namespace BL
{
    public class HelpersBL
    {
        private HelpersDAL HDAL = new HelpersDAL();

        public void ResizeAndSaveImage(int newWidth, Stream sourcePath, string targetPath)
        {
            HDAL.ResizeAndSaveImage(newWidth, sourcePath, targetPath);
        }

        public void ResizeAndSaveAzure(int newWidth, Stream sourcePath, string targetPath)
        {
            HDAL.ResizeAndSaveAzure(newWidth, sourcePath, targetPath);
        }

        public void SaveAzure(string FileType, HttpPostedFileBase File, string FileName)
        {
            HDAL.SaveAzure(FileType, File, FileName);
        }
    }
}
