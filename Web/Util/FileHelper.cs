using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.ServerControls.FileUpload;
using log4net;
using CMS.Core.Util;

namespace CMS.Web.Util
{
    public class FileHelper
    {
        public static ILog logger = LogManager.GetLogger(typeof(FileHelper));

        public const int KILOBYTE = 1024;

        #region -- Upload --
        /// <summary>
        /// Upload file from FileUpload Control and return virtual path
        /// </summary>
        /// <param name="fileUploadControl">FileUpload Control to be upload</param>
        /// <param name="path">Path from User File folder</param>
        /// <param name="timeseed"></param>
        /// <returns>Virtual path to newly uploaded file</returns>
        public static string Upload(FileUpload fileUploadControl, string path, bool timeseed = true)
        {
            // Image\\News\\
            try
            {
                string savePath = AppDomain.CurrentDomain.BaseDirectory;
                string virtualPath = "UserFiles\\" + path;

                if (!Directory.Exists(savePath + virtualPath))
                {
                    Directory.CreateDirectory(savePath + virtualPath);
                }

                if (timeseed)
                {
                    virtualPath += string.Format("{2}_{1}_{0}\\", DateTime.Now.Day, DateTime.Now.Month,
                                                 DateTime.Now.Year);
                }

                if (!Directory.Exists(savePath + virtualPath))
                {
                    Directory.CreateDirectory(savePath + virtualPath);
                }

                if (fileUploadControl.HasFile)
                {
                    string fileName = fileUploadControl.FileName;
                    //    savePath += string.Format("{0}{1}\\", DateTime.Now.Month, DateTime.Now.Year);
                    if (timeseed)
                        virtualPath += DateTime.Now.Hour + DateTime.Now.Minute + fileName;
                    else
                    {
                        virtualPath += fileName;
                    }
                    fileUploadControl.SaveAs(savePath + virtualPath);
                    virtualPath = virtualPath.Replace("\\", "/");
                    return "/" + virtualPath;
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                logger.Error("Can not upload files", ex);
                return String.Empty;
            }
        }
        /// <summary>
        /// Upload file from FileUpload Control and return virtual path
        /// </summary>
        /// <param name="fileUploadControl">FileUpload Control to be upload</param>
        /// <param name="path">Path from User File folder</param>
        /// <returns>Virtual path to newly uploaded file</returns>
        public static string Upload(FileUpload fileUploadControl, string path)
        {
            // Image\\News\\
            try
            {
                string savePath = AppDomain.CurrentDomain.BaseDirectory;
                string virtualPath = "UserFiles\\" + path;

                if (!Directory.Exists(savePath + virtualPath))
                {
                    Directory.CreateDirectory(savePath + virtualPath);
                }

                if (!Directory.Exists(savePath + virtualPath))
                {
                    Directory.CreateDirectory(savePath + virtualPath);
                }

                if (fileUploadControl.HasFile)
                {
                    string fileName = fileUploadControl.FileName;
                    //    savePath += string.Format("{0}{1}\\", DateTime.Now.Month, DateTime.Now.Year);
                    virtualPath += fileName;
                    fileUploadControl.SaveAs(savePath + virtualPath);
                    virtualPath = virtualPath.Replace("\\", "/");
                    return "/" + virtualPath;
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                logger.Error("Can not upload files", ex);
                return String.Empty;
            }
        }

        /// <summary>
        /// Upload file from FileUpload Control to default upload folder and return virtual path
        /// </summary>
        /// <param name="fileUploadControl">FileUpload Control to be upload</param>
        /// <returns>Virtual path to newly uploaded file</returns>
        public static string Upload(FileUpload fileUploadControl)
        {
            return Upload(fileUploadControl, "Uploaded\\");
        }

        public static string Upload(FileUploaderAJAX fileUploadControl, string path)
        {
            return Upload(fileUploadControl, path, false);
        }

        /// <summary>
        /// Upload file from FileUpload Control and return virtual path
        /// </summary>
        /// <param name="fileUploadControl">FileUpload Control to be upload</param>
        /// <param name="path">Path from User File folder</param>
        /// <param name="random">Generate Random file name</param>
        /// <param name="createByDate"></param>
        /// <returns>Virtual path to newly uploaded file</returns>
        public static string Upload(FileUploaderAJAX fileUploadControl, string path, bool random, bool createByDate)
        {
            // Image\\News\\
            try
            {
                string savePath = AppDomain.CurrentDomain.BaseDirectory;
                string virtualPath;
                if (path[0] != '~')
                {
                    virtualPath = "UserFiles\\" + path;
                }
                else
                {
                    path = path.Remove(0, 2);
                    virtualPath = path;
                }

                virtualPath = Text.EnsureSlash(virtualPath);

                if (!Directory.Exists(savePath + virtualPath))
                {
                    Directory.CreateDirectory(savePath + virtualPath);
                }

                if (createByDate)
                {
                    virtualPath += string.Format("{2}_{1}_{0}\\", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                }
                if (!Directory.Exists(savePath + virtualPath))
                {
                    Directory.CreateDirectory(savePath + virtualPath);
                }

                if (fileUploadControl.IsPosting)
                {
                    string fileName;
                    HttpPostedFileAJAX pf = fileUploadControl.PostedFile;
                    if (random)
                    {
                        fileName = Text.RandomString(24, true) + pf.FileName.Substring(pf.FileName.IndexOf('.'));
                    }
                    else
                    {
                        fileName = pf.FileName;
                    }
                    //    savePath += string.Format("{0}{1}\\", DateTime.Now.Month, DateTime.Now.Year);
                    virtualPath += fileName;
                    fileUploadControl.SaveAs(savePath + virtualPath);
                    virtualPath = virtualPath.Replace("\\", "/");
                    return "/" + virtualPath;
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                logger.Error("Can not upload files", ex);
                return String.Empty;
            }
        }

        public static string Upload(FileUploaderAJAX fileUploadControl, string path, bool random)
        {
            return Upload(fileUploadControl, path, random, true);
        }

        /// <summary>
        /// Upload file from FileUpload Control to default upload folder and return virtual path
        /// </summary>
        /// <param name="fileUploadControl">FileUpload Control to be upload</param>
        /// <returns>Virtual path to newly uploaded file</returns>
        public static string Upload(FileUploaderAJAX fileUploadControl)
        {
            return Upload(fileUploadControl, "Uploaded\\");
        }
        #endregion

        #region -- Ultilities --
        /// <summary>
        /// Lấy đường dẫn đến thư mục upload của ngày hôm nay nếu sử dụng hàm upload
        /// </summary>
        /// <param name="path">Đường dẫn cơ sở</param>
        /// <returns></returns>
        public static string GetCurrentUploadFolder(string path)
        {
            string savePath = AppDomain.CurrentDomain.BaseDirectory;
            string virtualPath = "UserFiles\\" + path;

            if (!Directory.Exists(savePath + virtualPath))
            {
                Directory.CreateDirectory(savePath + virtualPath);
            }

            virtualPath += string.Format("{2}_{1}_{0}\\", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            if (!Directory.Exists(savePath + virtualPath))
            {
                Directory.CreateDirectory(savePath + virtualPath);
            }

            virtualPath = virtualPath.Replace("\\", "/");
            return "/" + virtualPath;
        }

        /// <summary>
        /// Lấy đường dẫn tương đối từ đường dẫn vật lý
        /// </summary>
        /// <param name="path">đường dẫn vật lý</param>
        /// <returns></returns>
        public static string GetVirtualPathFromPath(string path)
        {
            return path.Substring(path.IndexOf("/UserFiles"));
        }
        #endregion

        #region -- Ajax upload helper --

        /// <summary>
        /// Hỗ trợ việc insert ảnh vào thẻ div có id cho trước sau khi upload xong
        /// </summary>
        /// <param name="divId">Id của thẻ div cần chèn ảnh</param>
        /// <param name="textBox">TextBox chứa đường dẫn sau khi upload xong</param>
        /// <returns></returns>
        public static string InsertImagePostUploadJS(string divId, TextBox textBox)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", divId);
            sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", textBox.ClientID);
            sb.Append(
                @"       
        var message = '';
        var str= FileName_Path.substring(FileName_Path.indexOf('/UserFiles'));
       if (Type == 'image')
       {
          message += '<br/>';                           
          message += '<img src=""'+str+'"" style=""max-width:200px""/>';
       }
        
    ");
            sb.AppendFormat("{0}.value = str;", textBox.ClientID);
            sb.AppendFormat("{0}.innerHTML = message;", divId);

            return sb.ToString();
        }
        /// <summary>
        /// Hàm tạo mã JavaScript xử lý hiển thị đường dẫn tại sự kiện sau khi upload ảnh
        /// </summary>
        /// <param name="labelPath"></param>
        /// <returns></returns>
        public static string InsertFileNameUploadJS(Control labelPath)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", labelName.ClientID);
            sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", labelPath.ClientID);
            //sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", labelType.ClientID);
            sb.Append(
                @"       
        var str= FileName_Path.substring(FileName_Path.indexOf('/UserFiles'));
    ");
            sb.AppendFormat("{0}.value = str;", labelPath.ClientID);
            //sb.AppendFormat("{0}.value = FileName_Path;", labelName.ClientID);
            //sb.AppendFormat("{0}.value = Type;", labelType.ClientID);
            return sb.ToString();
        }

        public static string InsertImagePostloadJS(string divId, TextBox textBox, string imageUrl)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("var {0} = document.getElementById('{0}');", divId);
            sb.AppendFormat("var {0} = document.getElementById('{0}');", textBox.ClientID);
            sb.AppendFormat(@"       
        var message = '';
        var str= '{0}';
        message += '<br/>';                           
        message += '<img src=""'+str+'"" style=""max-width:200px""/>';
        
    ", imageUrl);
            sb.AppendFormat("{0}.value = str;", textBox.ClientID);
            sb.AppendFormat("{0}.innerHTML = message;", divId);

            return sb.ToString();
        }

        public static string ClearData(string divId, TextBox textBox)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", divId);
            sb.AppendFormat("{0}.innerHTML = '';", divId);
            sb.AppendFormat("var {0} = parent.document.getElementById('{0}');", textBox.ClientID);
            sb.AppendFormat("{0}.value = '';", textBox.ClientID);

            return sb.ToString();
        }

        public static void ManageAjaxPost(FileUploaderAJAX fileupload, int maxKB, string path)
        {
            HttpPostedFileAJAX pf = fileupload.PostedFile;

            if (maxKB == 0 || pf.ContentLength <= maxKB * KILOBYTE)
            {
                Upload(fileupload, path, true);
            }
        }

        public static void ManageAjaxPost(FileUploaderAJAX fileupload, int maxKB, string path, HttpPostedFileAJAX.fileType fileType)
        {
            ManageAjaxPost(fileupload, maxKB, path, fileType, true);
        }

        public static void ManageAjaxPost(FileUploaderAJAX fileupload, int maxKB, string path, HttpPostedFileAJAX.fileType fileType, bool isCreateByDate)
        {
            ManageAjaxPost(fileupload, maxKB, path, fileType, isCreateByDate, false);
        }

        public static void ManageAjaxPost(FileUploaderAJAX fileupload, int maxKB, string path, HttpPostedFileAJAX.fileType fileType, bool isCreateByDate, bool isEncrypted)
        {
            HttpPostedFileAJAX pf = fileupload.PostedFile;

            if ((maxKB == 0 || pf.ContentLength <= maxKB * KILOBYTE) && pf.Type == fileType && pf.Type != HttpPostedFileAJAX.fileType.application)
            {
                Upload(fileupload, path, isEncrypted, isCreateByDate);
            }
        }

        #endregion

        public static string GetFileName(string url)
        {
            int index = url.LastIndexOf("/", StringComparison.Ordinal);
            return url.Substring(index + 1);
        }
    }
}