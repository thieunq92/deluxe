using CMS.Core.Util;

namespace CMS.Core
{
    /// <summary>
    /// The ISyndicatable interface provides functionality to retrieve syndicatable content.
    /// </summary>
    public interface ISyndicatable
    {
        /// <summary>
        /// Get the rss feed. Let the implementor decide how to handle this.
        /// </summary>
        /// <returns></returns>
        RssChannel GetRssFeed();
    }
}