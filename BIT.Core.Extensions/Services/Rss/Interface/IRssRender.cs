using System;
using System.IO;
using BIT.Core.Extensions.Util;

namespace BIT.Core.Extensions.Services.Rss.Interface
{
    public interface IRssRender
    {
        /* TODO: TYPE OF SYNDICATION */

        /* Get rssChannel element from url */
        AdvancedRssChannel GetChannel(Uri url);

        /* Set a Xml from RssChannel Object */
        void SetChannelRss(AdvancedRssChannel rssChannel, Stream OutputStream);
    }
}