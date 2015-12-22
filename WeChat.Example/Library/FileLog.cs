using System.IO;
using System.Web.Hosting;

namespace WeChat.Example.Library
{
    public class FileLog
    {
        public static void Append(string text)
        {
            var fileFullPath = HostingEnvironment.MapPath("~/App_Data/log.txt");
            if (!File.Exists(fileFullPath))
            {
                using (StreamWriter sw = new StreamWriter(File.Create(fileFullPath)))
                {
                    sw.WriteLine(text);
                    return;
                }
            }
            using (StreamWriter sw = File.AppendText(fileFullPath))
            {
                sw.WriteLine(text);
            }
        }
        public static void Append(string filePath, string text)
        {
            var fileFullPath = HostingEnvironment.MapPath(filePath);
            if (!File.Exists(fileFullPath))
            {
                using (StreamWriter sw = new StreamWriter(File.Create(fileFullPath)))
                {
                    sw.WriteLine(text);
                    return;
                }
            }
            using (StreamWriter sw = File.AppendText(fileFullPath))
            {
                sw.WriteLine(text);
            }
        }
    }
}