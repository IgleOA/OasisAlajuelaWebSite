using System.IO;
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
    }
}
