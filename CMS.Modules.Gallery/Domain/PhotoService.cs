using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using CMS.Modules.Gallery.Domain;
using ImageHelper;
using NHibernate;
using NHibernate.Criterion;

namespace CMS.Modules.Gallery
{
    public class PhotoService
    {
        private ISessionManager _sessionManager { get { return _galleryModule.SessionManager; } }
        private GalleryModule _galleryModule;
        private GalleryPathBuilder _galleryPathBuilder;

        public PhotoService(GalleryModule galleryModule)
        {  
            _galleryModule = galleryModule;
            _galleryPathBuilder = _galleryModule.PathBuilder;
        }

        /// <summary>
        /// Retrieve the meta-information of all photos that belong to an Album.
        /// </summary>
        /// <param name="albumId">Id of Album.</param>
        public IList GetPhotoList(int albumId)
        {
            ISession session = this._sessionManager.OpenSession();
            string hql = "from Photo p where p.Album = :albumId order by p.Order";

            IQuery q = session.CreateQuery(hql);
            q.SetInt32("albumId", albumId);

            try
            {
                return q.List();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Photos for album: " + albumId, ex);
            }
        }

        /// <summary>
        /// Retrieve the meta-information of a Photo.
        /// </summary>
        /// <param name="photoId">Id of Photo.</param>
        public Photo GetPhotoById(int photoId)
        {
            ISession session = this._sessionManager.OpenSession();
            try
            {
                return (Photo)session.Load(typeof(Photo), photoId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Photo with identifier: " + photoId, ex);
            }
        }

        /// <summary>
        /// Retrieve the meta-information of the first photo in an Album.
        /// </summary>
        /// <param name="albumId">Id of Album.</param>
        public Photo GetFirstPhotoOfAlbum(int albumId)
        {
            ISession session = this._sessionManager.OpenSession();

            string hql = "select p from CMS.Modules.Gallery.Domain.Photo p where p.Album.Id = :albumId";
            IQuery q = session.CreateQuery(hql);
            q.SetInt32("albumId", albumId);
            Random random = new Random();            
            if (q.List().Count > 0)
            {
                return (Photo)q.List()[random.Next(q.List().Count-1)];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SavePhoto(Photo photo, Stream fileStream)
        {
            Image imageFromStream = Image.FromStream(fileStream);
            imageFromStream.Save(_galleryPathBuilder.GetPhotoOriginPath(photo), ImageFormat.Jpeg);

            ImageResizeUtil imageResizeUtilThumb = new ImageResizeUtil(imageFromStream, _galleryModule.AlbumSettings.MaxThumbDimension);
            //ImageResizeUtil imageResizeUtil = new ImageResizeUtil(imageFromStream, _galleryModule.AlbumSettings.MaxImageDimension);

            ResizeThumbnail(photo, imageResizeUtilThumb);
            //ResizeImage(photo, imageResizeUtil);

            SavePhotoInfo(photo);
        }


        public void ResizeThumbnail(Photo photo, ImageResizeUtil imageResizeUtil)
        {
            Image thumbnail = imageResizeUtil.GetThumbnail();

            photo.ThumbHeight = thumbnail.Height;
            photo.ThumbWidth = thumbnail.Width;

            string thumbPath = _galleryPathBuilder.GetThumbDirectory(photo.Album);
            DirectoryInfo directory = new DirectoryInfo(thumbPath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            thumbnail.Save(_galleryPathBuilder.GetThumbPath(photo), ImageFormat.Jpeg);
        }

        public void ResizeImage(Photo photo, ImageResizeUtil imageResizeUtil)
        {
            Image image = imageResizeUtil.GetThumbnail();

            photo.ImageHeight = image.Height;
            photo.ImageWidth = image.Width;

            image.Save(_galleryPathBuilder.GetPath(photo), ImageFormat.Jpeg);
        }


        [Transaction(TransactionMode.Requires)]
        public virtual void SavePhotoInfo(Photo photo)
        {
            // Obtain current NHibernate session.
            ISession session = this._sessionManager.OpenSession();

            // can't seem to get this to work automagically
            if (photo.Id == -1)
            {
                int order;
                if (photo.Album.PhotoCount > 0)
                    order = Convert.ToInt32(session.CreateCriteria(typeof(Photo))
                                                    .SetProjection(Projections.Max("Id")).List()[0]) + 1;
                else
                    order = 1;
                photo.Order = order;
                session.Save(photo);
                UpdatePhotoCount(photo.Album.Id);
            }
            else
            {
                session.SaveOrUpdate(photo);
                UpdatePhotoCount(photo.Album.Id);
            }
        }

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void DeletePhoto(Photo photo)
        {
            try
            {
                _galleryModule.FileService.DeleteFile(_galleryPathBuilder.GetPhotoOriginPath(photo));
                _galleryModule.FileService.DeleteFile(_galleryPathBuilder.GetThumbPath(photo));
                _galleryModule.FileService.DeleteFile(_galleryPathBuilder.GetPath(photo));
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to remove files", ex);
            }

            // Delete meta information.
            ISession session = this._sessionManager.OpenSession();
            session.Delete(photo);
            UpdatePhotoCount(photo.Album.Id);
        }

        public virtual void IncreaseNrOfViews(Photo photo)
        {
            ISession session = this._sessionManager.OpenSession();
            // First refresh the file to prevent stale updates because downloads can take a little while.
            session.Refresh(photo);
            photo.NrOfViews++;
            SavePhotoInfo(photo);
        }

        /// <summary>
        /// Update the photo count for an album.
        /// There no doubt is a much better method of doing this, but I don't have one right now.
        /// </summary>
        /// <param name="albumId">Album to update</param>
        public virtual void UpdatePhotoCount(int albumId)
        {
            ISession session = this._sessionManager.OpenSession();
            string hql = "from Photo p where p.Album = :albumId";

            IQuery q = session.CreateQuery(hql);
            q.SetInt32("albumId", albumId);

            try
            {
                Album tempAlbum = _galleryModule.GetAlbumService().GetAlbumById(albumId);

                tempAlbum.PhotoCount = q.List().Count;

                session.SaveOrUpdate(tempAlbum);
                session.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update photo count for album: " + albumId, ex);
            }
        }

        static public string CreateServerFilename(string clientFilename)
        {
            if (clientFilename.LastIndexOf(Path.DirectorySeparatorChar) > -1)
            {
                return clientFilename.Substring(clientFilename.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            }
            else
            {
                return clientFilename;
            }
        }

    }
}