using System;
using System.Collections;
using System.Web;
using CMS.Core.Communication;
using CMS.Core.Domain;
using CMS.Core.Search;
using CMS.Core.Service;

namespace CMS.Modules.Search
{
    /// <summary>
    /// The searchmodule provides search capabilities on the DotLucene search index.
    /// <seealso cref="CMS.Core.Search"/>
    /// </summary>
    public class SearchModule : ModuleBase, IActionConsumer
    {
        private readonly ActionCollection _inboundActions;
        private CMS.Core.Communication.Action _currentAction;
        private int _resultsPerPage = 10;
        private string _searchQuery;
        private bool _showInputPanel = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SearchModule()
        {
            // Init inbound actions
            _inboundActions = new ActionCollection();
            _inboundActions.Add(new CMS.Core.Communication.Action("Search", new string[0]));
            _currentAction = _inboundActions[0];
        }

        /// <summary>
        /// The number of results to show at one page. Default is 10.
        /// </summary>
        public int ResultsPerPage
        {
            get { return _resultsPerPage; }
        }

        /// <summary>
        /// Show the search input panel?
        /// </summary>
        public bool ShowInputPanel
        {
            get { return _showInputPanel; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CMS.Core.Communication.Action CurrentAction
        {
            get { return _currentAction; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SearchQuery
        {
            get { return _searchQuery; }
            set { _searchQuery = value; }
        }

        #region IActionConsumer Members

        public ActionCollection GetInboundActions()
        {
            return _inboundActions;
        }

        #endregion

        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            if (Section.Settings["RESULTS_PER_PAGE"] != null)
            {
                _resultsPerPage = Convert.ToInt32(Section.Settings["RESULTS_PER_PAGE"]);
            }
            if (Section.Settings["SHOW_INPUT_PANEL"] != null)
            {
                _showInputPanel = Convert.ToBoolean(Section.Settings["SHOW_INPUT_PANEL"]);
            }
        }


        /// <summary>
        /// Get paged search results.
        /// </summary>
        /// <param name="queryText">The query to search for.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="indexDir"></param>
        /// <returns></returns>
        public SearchResultCollection GetSearchResults(string queryText, int pageIndex, int pageSize, string indexDir)
        {
            IndexQuery query = new IndexQuery(indexDir);
            Hashtable keywordFilter = new Hashtable();
            keywordFilter.Add("site", Section.Node.Site.Name);
            SearchResultCollection nonFilteredResults = query.Find(queryText, keywordFilter, pageIndex, pageSize);
            // Filter results where the current user doesn't have access to.
            return FilterResults(nonFilteredResults);
        }

        /// <summary>
        /// Get all search results with a maximum of 200.
        /// </summary>
        /// <param name="queryText"></param>
        /// <param name="indexDir"></param>
        /// <returns></returns>
        public SearchResultCollection GetSearchResults(string queryText, string indexDir)
        {
            return GetSearchResults(queryText, 0, 200, indexDir);
        }

        protected override void ParsePathInfo()
        {
            base.ParsePathInfo();
            if (ModuleParams != null)
            {
                if (ModuleParams.Length == 1)
                {
                    // First argument is the module action
                    _currentAction = _inboundActions.FindByName(ModuleParams[0]);
                    if (_currentAction != null)
                    {
                        if (_currentAction.Name == "Search")
                        {
                            _searchQuery =
                                HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["q"]);
                        }
                    }
                    else
                    {
                        throw new Exception("Error when parsing module action: " + ModuleParams[0]);
                    }
                }
            }
        }

        /// <summary>
        /// A searchresult contains a SectionId propery that indicates to which section the 
        /// result belongs. We need to get a real Section to determine if the current user 
        /// has view access to that Section.
        /// </summary>
        /// <param name="nonFilteredResults"></param>
        /// <returns></returns>
        private static SearchResultCollection FilterResults(SearchResultCollection nonFilteredResults)
        {
            SearchResultCollection filteredResults = new SearchResultCollection();
            CoreRepository cr = HttpContext.Current.Items["CoreRepository"] as CoreRepository;
            if (cr != null)
            {
                foreach (SearchResult result in nonFilteredResults)
                {
                    Section section = (Section) cr.GetObjectById(typeof (Section), result.SectionId);
                    if (section.ViewAllowed(HttpContext.Current.User.Identity))
                    {
                        filteredResults.Add(result);
                    }
                }
            }

            return filteredResults;
        }
    }
}