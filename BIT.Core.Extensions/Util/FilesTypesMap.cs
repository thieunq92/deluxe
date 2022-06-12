using System.Collections;

namespace BIT.Core.Extensions.Util
{
    public class FileTypesMap
    {
        private static Hashtable fileTypesMap;

        private FileTypesMap()
        {
        }

        /// <summary>
        /// Try to find a corresponding icon image for the give extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetIconFilename(string extension)
        {
            InitMap();

            string iconFilename = "file.gif"; // default
            if (fileTypesMap[extension] != null)
            {
                iconFilename = fileTypesMap[extension].ToString();
            }
            return iconFilename;
        }

        /// <summary>
        /// Try to find a corresponding type for the give extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetClass(string extension)
        {
            InitMapType();

            string iconFilename = "type_default";
            if (fileTypesMap[extension] != null)
            {
                iconFilename = fileTypesMap[extension].ToString();
            }
            return iconFilename;
        }

        public static string GetNameoffile(string uri)
        {
            return uri.Substring(uri.LastIndexOf("/") + 1, uri.Length - (uri.LastIndexOf("/") + 1));
        }

        public static string GetExtension(string uri)
        {
            return uri.Substring(uri.LastIndexOf("."), uri.Length - uri.LastIndexOf("."));
        }

        private static void InitMap()
        {
            fileTypesMap = new Hashtable();
            fileTypesMap.Add(".asf", "mpg.gif");
            fileTypesMap.Add(".avi", "mpg.gif");
            fileTypesMap.Add(".bmp", "bmp.gif");
            fileTypesMap.Add(".chm", "chm.gif");
            fileTypesMap.Add(".cs", "cs.gif");
            fileTypesMap.Add(".doc", "doc.gif");
            fileTypesMap.Add(".dot", "doc.gif");
            fileTypesMap.Add(".exe", "exe.gif");
            fileTypesMap.Add(".gif", "gif.gif");
            fileTypesMap.Add(".gz", "zip.gif");
            fileTypesMap.Add(".gzip", "zip.gif");
            fileTypesMap.Add(".htm", "htm.gif");
            fileTypesMap.Add(".html", "html.gif");
            fileTypesMap.Add(".jpg", "jpg.gif");
            fileTypesMap.Add(".jpeg", "jpg.gif");
            fileTypesMap.Add(".mdb", "mdb.gif");
            fileTypesMap.Add(".mov", "mpg.gif");
            fileTypesMap.Add(".mp3", "wav.gif");
            fileTypesMap.Add(".mpg", "mpg.gif");
            fileTypesMap.Add(".mpeg", "mpg.gif");
            fileTypesMap.Add(".pdf", "pdf.gif");
            fileTypesMap.Add(".ppt", "ppt.gif");
            fileTypesMap.Add(".rar", "zip.gif");
            fileTypesMap.Add(".rtf", "doc.gif");
            fileTypesMap.Add(".tgz", "zip.gif");
            fileTypesMap.Add(".txt", "txt.gif");
            fileTypesMap.Add(".wav", "wav.gif");
            fileTypesMap.Add(".wma", "wav.gif");
            fileTypesMap.Add(".xls", "xls.gif");
            fileTypesMap.Add(".xml", "xml.gif");
            fileTypesMap.Add(".zip", "zip.gif");
        }

        private static void InitMapType()
        {
            fileTypesMap = new Hashtable();
            fileTypesMap.Add(".asf", "type_mpg");
            fileTypesMap.Add(".avi", "type_mpg");
            fileTypesMap.Add(".bmp", "type_bmp");
            fileTypesMap.Add(".chm", "type_chm");
            fileTypesMap.Add(".cs", "type_cs");
            fileTypesMap.Add(".doc", "type_doc");
            fileTypesMap.Add(".dot", "type_doc");
            fileTypesMap.Add(".exe", "type_exe");
            fileTypesMap.Add(".gif", "type_gif");
            fileTypesMap.Add(".gz", "type_zip");
            fileTypesMap.Add(".gzip", "type_zip");
            fileTypesMap.Add(".htm", "type_htm");
            fileTypesMap.Add(".html", "type_html");
            fileTypesMap.Add(".jpg", "type_jpg");
            fileTypesMap.Add(".jpeg", "type_jpg");
            fileTypesMap.Add(".mdb", "type_mdb");
            fileTypesMap.Add(".mov", "type_mpg");
            fileTypesMap.Add(".mp3", "type_wav");
            fileTypesMap.Add(".mpg", "type_mpg");
            fileTypesMap.Add(".mpeg", "type_mpg");
            fileTypesMap.Add(".pdf", "type_pdf");
            fileTypesMap.Add(".ppt", "type_ppt");
            fileTypesMap.Add(".rar", "type_zip");
            fileTypesMap.Add(".rtf", "type_doc");
            fileTypesMap.Add(".tgz", "type_zip");
            fileTypesMap.Add(".txt", "type_txt");
            fileTypesMap.Add(".wav", "type_wav");
            fileTypesMap.Add(".wma", "type_wav");
            fileTypesMap.Add(".xls", "type_xls");
            fileTypesMap.Add(".xml", "type_xml");
            fileTypesMap.Add(".zip", "type_zip");
        }
    }
}