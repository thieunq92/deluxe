using System;
using System.Collections;
using CMS.Core.Communication;
using NHibernate;
using NHibernate.Criterion;

using CMS.Core;
using CMS.Core.Service;
using CMS.Core.Domain;
using CMS.Core.Search;
using CMS.Core.Util;

namespace CMS.Modules.StaticHtml
{
    /// <summary>
    /// The StaticHtmlModule provides the content of simple static page. It needs at least its 
    /// Section to be set to do something with the content (load, update, delete).
    /// </summary>
    public class StaticHtmlModule : ModuleBase, ISearchable, INHibernateModule, IActionConsumer
    {
        public StaticHtmlModule()
        {
        }

        /// <summary>
        /// Get the content that belongs to the Section where the module is attached to.
        /// </summary>
        /// <returns></returns>
        public StaticHtmlContent GetContent()
        {
            if (base.Section != null)
            {
                ISession session = this.NHSession;
                string hql = "from StaticHtmlContent s where s.Section.Id = :sectionId ";
                var query = session.CreateQuery(hql);
                query.SetParameter("sectionId", this.Section.Id);
                IList results = query.List();
                if (results.Count == 1)
                {
                    return (StaticHtmlContent)results[0];
                }
                else if (results.Count > 1)
                {
                    throw new Exception("A request for StaticHtmlContent should only return one item");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Unable to get the StaticHtmlContent when no Section is available");
            }
        }

        /// <summary>
        /// Save the content.
        /// </summary>
        /// <param name="content"></param>
        public void SaveContent(StaticHtmlContent content)
        {
            ITransaction tx = base.NHSession.BeginTransaction();
            try
            {
                if (content.Id == -1)
                {
                    content.UpdateTimestamp = DateTime.Now;
                    base.NHSession.Save(content);
                    OnContentCreated(new IndexEventArgs(StaticHtmlContentToSearchContent(content)));
                }
                else
                {
                    base.NHSession.Update(content);
                    OnContentUpdated(new IndexEventArgs(StaticHtmlContentToSearchContent(content)));
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new Exception("Unable to save content: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete the content.
        /// </summary>
        /// <param name="content"></param>
        public void DeleteContent(StaticHtmlContent content)
        {
            ITransaction tx = base.NHSession.BeginTransaction();
            try
            {
                base.NHSession.Delete(content);
                tx.Commit();
                OnContentDeleted(new IndexEventArgs(StaticHtmlContentToSearchContent(content)));
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new Exception("Unable to delete content: " + ex.Message, ex);
            }
        }

        public override void DeleteModuleContent()
        {
            // Delete the associated StaticHtmlContent
            StaticHtmlContent content = this.GetContent();
            if (content != null)
            {
                DeleteContent(content);
            }
        }


        private SearchContent StaticHtmlContentToSearchContent(StaticHtmlContent shc)
        {
            SearchContent sc = new SearchContent();
            sc.Title = shc.Section.Title;
            sc.Summary = Text.TruncateText(shc.Content, 200); // trunctate the summary to 200 chars
            sc.Contents = shc.Content;
            sc.Author = shc.ModifiedBy.FullName;
            sc.ModuleType = shc.Section.ModuleType.Name;
            sc.Path = this.SectionUrl;
            sc.Category = String.Empty;
            sc.Site = (shc.Section.Node != null ? shc.Section.Node.Site.Name : String.Empty);
            sc.DateCreated = shc.UpdateTimestamp;
            sc.DateModified = shc.UpdateTimestamp;
            sc.SectionId = shc.Section.Id;

            return sc;
        }

        public ActionCollection GetInboundActions()
        {
            ActionCollection _inboundActions = new ActionCollection();
            _inboundActions.Add(new CMS.Core.Communication.Action("StaticContent", new string[0]));
            return _inboundActions;
        }

        #region ISearchable Members

        public SearchContent[] GetAllSearchableContent()
        {
            StaticHtmlContent shc = GetContent();
            if (shc != null)
            {
                SearchContent[] searchContents = new SearchContent[1];
                searchContents[0] = StaticHtmlContentToSearchContent(shc);
                return searchContents;
            }
            else
            {
                return new SearchContent[0];
            }
        }

        public event CMS.Core.Search.IndexEventHandler ContentCreated;

        protected void OnContentCreated(IndexEventArgs e)
        {
            if (ContentCreated != null)
            {
                ContentCreated(this, e);
            }
        }

        public event CMS.Core.Search.IndexEventHandler ContentDeleted;

        protected void OnContentDeleted(IndexEventArgs e)
        {
            if (ContentDeleted != null)
            {
                ContentDeleted(this, e);
            }
        }

        public event CMS.Core.Search.IndexEventHandler ContentUpdated;

        protected void OnContentUpdated(IndexEventArgs e)
        {
            if (ContentUpdated != null)
            {
                ContentUpdated(this, e);
            }
        }

        #endregion
    }
}
