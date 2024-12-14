using System.IO;
using System.Web;

namespace CloudERP.HelperClass
{
    public class FileHelper
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string name)
        {
            if (file == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(folder))
            {
                System.Diagnostics.Debug.WriteLine("Problem with the parameter");
                return false;
            }
            try
            {
                string path = string.Empty;

                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                    file.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                return true;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("File Helper Class Error");
                return false;
            }
        }
    }
}