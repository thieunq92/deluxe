using System;
using System.IO;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Modules.Gallery.Domain;
using CMS.Web.UI;

namespace CMS.Modules.Gallery.Web
{
    public partial class SimpleViewer : BaseModuleControl
    {
        private GalleryModule _galleryModule;
        private Album _album;

        protected void Page_Load(object sender, EventArgs e)
        {
            _galleryModule = Module as GalleryModule;
            _album = _galleryModule.GetAlbumService().GetAlbumById(_galleryModule.CurrentAlbumId);
            if (_album.PhotoCount == 0)
            {
                Visible = false;
                return;
            }
            rptReviews.DataSource = _album.Comments;
            rptReviews.DataBind();
            //GetXmlPath
        }

        public string GetXmlPath()
        {            
            return Page.ResolveUrl(_galleryModule.VirtualPath(GetXmlPath(_album)));
        }

        public string GetXmlPath(Album album)
        {
            string xmlPath = _galleryModule.PathBuilder.GetAlbumDirectory(album.Id) + Path.DirectorySeparatorChar + "album.xml";
            FileInfo xmlFile = new FileInfo(xmlPath);
            if (xmlFile.Exists && xmlFile.LastWriteTime.AddMinutes(5) > DateTime.Now)
            {
                return xmlPath;
            }

            string imgPath =
                Page.ResolveUrl(_galleryModule.VirtualPath(_galleryModule.PathBuilder.GetAlbumDirectory(album.Id)));

            string thumbPath = imgPath + "/thumbnails";

            StreamWriter writer = new StreamWriter(xmlPath);
            string xml =
                string.Format(
                    @"<SIMPLEVIEWER_DATA maxImageDimension=""{0}"" textColor=""0xFFFFFF"" frameColor=""0xA0A0A0"" frameWidth=""{1}"" stagePadding=""{2}"" thumbnailColumns=""{3}"" thumbnailRows=""{4}"" navPosition=""top"" navDirection=""LTR"" title="""" imagePath=""{5}/"" thumbPath=""{6}/"" vAlign=""top"">",
                    _galleryModule.AlbumSettings.MaxImageDimension,0,0,5,1,imgPath, thumbPath);

            foreach (Photo photo in album.Photos)
            {
                xml = xml + "<IMAGE>";
                xml = xml + "<NAME>" + photo.FileName + "</NAME>";
                xml = xml + "<CAPTION>" + photo.Title + "</CAPTION>";
                xml = xml + "</IMAGE>";
            }

            xml = xml + "</SIMPLEVIEWER_DATA>";

            writer.Write(xml);
            writer.Close();
            return xmlPath;
            //FileStream xmlFile.OpenWrite();
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            // Nếu đã đánh dấu I agree, có nội dung comment, có tên author, email hoặc đã đăng nhập
            if (chkAgree.Checked && !string.IsNullOrEmpty(textBoxComment.Text) && ((!string.IsNullOrEmpty(textBoxAuthor.Text) && !string.IsNullOrEmpty(textBoxEmail.Text)) || PageEngine.User.Identity is User))
            {
                if (textBoxVerifierCode.Text == imageVerifier.Text)
                {
                    AlbumComment comment = new AlbumComment();
                    comment.Deleted = false;
                    if (Page.User.Identity is User)
                    {
                        comment.Author = ((User)Page.User.Identity).FullName;
                        comment.AuthorId = ((User)Page.User.Identity);
                        comment.Email = ((User)Page.User.Identity).Email;
                    }
                    else
                    {
                        comment.Author = textBoxAuthor.Text;
                        comment.AuthorId = null;
                        comment.Email = textBoxEmail.Text;
                    }

                    comment.DateCreated = DateTime.Now;
                    comment.DateModified = DateTime.Now;
                    comment.Comment = textBoxComment.Text;
                    comment.Status = 0;
                    comment.Album = _album;
                    comment.IP = Request.UserHostAddress;
                    chkAgree.Checked = false;
                    textBoxComment.Text = string.Empty;

                    _galleryModule.GetAlbumService().SaveOrUpdate(comment);

                    rptReviews.DataSource = _album.Comments;
                    rptReviews.DataBind();

                    imageVerifier.Refresh();
                }
                else
                {
                    labelFailedVerified.Visible = true;
                }
            }
        }

        protected void rptReviews_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AlbumComment)
            {
                AlbumComment comment = (AlbumComment)e.Item.DataItem;
                Label labelAuthor = e.Item.FindControl("labelAuthor") as Label;
                if (labelAuthor != null)
                {
                    if (comment.AuthorId != null)
                    {
                        comment.Author = comment.AuthorId.FullName;
                    }
                    labelAuthor.Text = string.Format("{0:F} by {1}", comment.DateCreated, comment.Author);
                }

                Label labelContent = e.Item.FindControl("labelContent") as Label;
                if (labelContent != null)
                {
                    labelContent.Text = string.Format("{0}", comment.Comment);
                }
            }
        }
    }
}