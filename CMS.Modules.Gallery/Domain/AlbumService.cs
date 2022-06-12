using System;
using System.IO;
using System.Collections;
using Castle.Services.Transaction;
using Castle.Facilities.NHibernateIntegration;
using CMS.Modules.Gallery.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace CMS.Modules.Gallery
{
    public class AlbumService
    {
        private ISessionManager _sessionManager{ get { return _galleryModule.SessionManager; }}
        private readonly GalleryModule _galleryModule;
        private readonly GalleryPathBuilder _galleryPathBuilder;

        public AlbumService(GalleryModule galleryModule)
        {  
            _galleryModule = galleryModule;
            _galleryPathBuilder = _galleryModule.PathBuilder;
        }

        /// <summary>
        /// Retrieve the meta-information of all albums that belong to this module.
        /// </summary>
        /// <returns></returns>
        public IList GetAlbumList(bool OnlyActive)
        {
            ISession session = this._sessionManager.OpenSession();

            string hql = "from Album a where a.Section.Id = :sectionId";
            if (OnlyActive)
            {
                hql += " and a.Active = 1";
            }

            IQuery q = session.CreateQuery(hql);
            q.SetInt32("sectionId", _galleryModule.Section.Id);

            try
            {
                IList result = q.List();
                foreach (object item in result)
                    ((Album) item).SetGalleryModule(_galleryModule);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Albums for section: " + _galleryModule.Section.Title, ex);
            }
        }

        /// <summary>
        /// Retrieve the meta-information of all albums that belong to this module.
        /// </summary>
        /// <returns></returns>
        public IList GetAlbumList(bool OnlyActive,string title)
        {
            ISession session = _sessionManager.OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof (Album));
            criteria.Add(Expression.Eq("Section", _galleryModule.Section));
            if (OnlyActive)
            {
                criteria.Add(Expression.Eq("Active", OnlyActive));
            }
            if (!string.IsNullOrEmpty(title))
            {
                criteria.Add(Expression.Like("Title", title, MatchMode.Anywhere));
            }
            try
            {
                IList result = criteria.List();
                foreach (object item in result)
                    ((Album)item).SetGalleryModule(_galleryModule);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Albums for section: " + _galleryModule.Section.Title, ex);
            }
        }

        /// <summary>
        /// Retrieve the meta-information of an Album.
        /// </summary>
        /// <param name="albumId">Id of Album.</param>
        public Album GetAlbumById(int albumId)
        {
            ISession session = this._sessionManager.OpenSession();
            try
            {
                Album result = (Album)session.Load(typeof(Album), albumId);
                result.SetGalleryModule(_galleryModule);

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Album with identifier: " + albumId, ex);
            }
        }

        /// <summary>
        /// Retrieve the meta-information of the Album a Photo belongs to 
        /// </summary>
        /// <param name="photoId">Id of Photo.</param>
        public Album GetAlbumByPhotoId(int photoId)
        {
            ISession session = this._sessionManager.OpenSession();
            try
            {
                Photo _tempPhoto = _galleryModule.GetPhotoService().GetPhotoById(photoId);
    
                Album result = (Album)session.Load(typeof(Album), _tempPhoto.Album.Id);
                result.SetGalleryModule(_galleryModule);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Album for Photo with identifier: " + photoId, ex);
            }
        }

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SaveAlbum(Album album)
        {
            SaveAlbumInfo(album);

            Directory.CreateDirectory(_galleryPathBuilder.GetAlbumDirectory(album.Id));
        }

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SaveAlbumInfo(Album album)
        {
            ISession session = this._sessionManager.OpenSession();

            // can't seem to get this to work automagically
            if (album.Id == -1)
            {
                session.Save(album);
            }
            else
            {
                session.Update(album);
            }
            session.Flush();
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteAlbum(Album album)
        {
            // try to remove album directory with contents

            string physicalAlbumPath = _galleryPathBuilder.GetAlbumDirectory(album.Id);

            if (Directory.Exists(physicalAlbumPath))
            {
                try
                {
                    Directory.Delete(physicalAlbumPath, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to remove directory", ex);
                }
            }

            // Delete meta information.
            ISession session = this._sessionManager.OpenSession();
            session.Delete(album);
            session.Flush();
        }

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SaveOrUpdate(AlbumComment comment)
        {
            ISession session = _sessionManager.OpenSession();

            // can't seem to get this to work automagically
            if (comment.Id == -1)
            {
                session.Save(comment);
            }
            else
            {
                session.Update(comment);
            }
            session.Flush();            
        }
    }
}
