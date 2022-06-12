using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using BIT.Core.Extensions.Services.Rss.Interface;
using BIT.Core.Extensions.Util;
using CMS.Core.Domain;

namespace BIT.Core.Extensions.Services.Rss
{
    public class utilrssargotic : IRssRender
    {
        #region IRssRender Members

        public AdvancedRssChannel GetChannel(Uri url)
        {
            AdvancedRssChannel rssChannel = new AdvancedRssChannel();

            rssChannel = GetRssChannelFromRss(url);

            return rssChannel;

            //Select the syndication type
            //switch (Argotic.Common.SyndicationDiscovery.DetermineSyndicationFormat(url))
            //{
            //    case Argotic.Common..Rss: rssChannel = GetRssChannelFromRss(url); break;
            //    case Argotic.Common.SyndicationFormat.Atom: rssChannel = GetRssChannelFromAtom(url); break;
            //}

            //TODO
            //feed.SortAndFilter();
        }

        public void SetChannelRss(AdvancedRssChannel rssChannel, Stream OutputStream)
        {
            RssFeed ArgoticRssFeed = new RssFeed();

            SetBasicChannelData(rssChannel, ArgoticRssFeed.Channel);

            if (rssChannel.RssItems != null)
            {
                foreach (AdvancedRssItem RssItem in rssChannel.RssItems)
                {
                    //New item
                    RssItem ArgoRssItem = new RssItem();

                    //Basic item of the item
                    SetBasicItemData(RssItem, ArgoRssItem);

                    //Extention for Geocoding
                    SetBasicGeocodingExtension(RssItem, ArgoRssItem);

                    //Extention Slash for nomber of comment
                    SetSlashExtension(RssItem, ArgoRssItem);

                    //Yahoo extention for media content
                    if (RssItem.RssfeedItemsMedia != null)
                    {
                        YahooMediaSyndicationExtension yme = new YahooMediaSyndicationExtension();
                        SetYahooExtension(RssItem.RssfeedItemsMedia, yme);
                        ArgoRssItem.AddExtension(yme);
                    }

                    ArgoticRssFeed.Channel.AddItem(ArgoRssItem);
                }
            }

            ArgoticRssFeed.Save(OutputStream);
        }

        #endregion

        private AdvancedRssChannel GetRssChannelFromRss(Uri url)
        {
            AdvancedRssChannel Channel = new AdvancedRssChannel();

            //Argotic object declaration
            RssFeed ArgoticRssFeed = new RssFeed();

            //Load from uri
            try
            {
                ArgoticRssFeed.Load(url, null, null);
            }
            catch
            {
            }

            //Init Channel basic data
            GetBasicChannelData(ArgoticRssFeed.Channel, Channel);

            //Init data for each item
            foreach (RssItem item in ArgoticRssFeed.Channel.Items)
            {
                //Init basic data
                AdvancedRssItem RssItem = new AdvancedRssItem();
                GetBasicItemData(item, RssItem);

                //Extensions type content
                if (item.HasExtensions)
                {
                    //Dublin Core Extension
                    DublinCoreMetadataTermsSyndicationExtension BublinExtension =
                        (DublinCoreMetadataTermsSyndicationExtension)
                        item.FindExtension(DublinCoreMetadataTermsSyndicationExtension.MatchByType);
                    if (BublinExtension != null)
                    {
                        GetDublinCoreExtension(BublinExtension.Context, RssItem);
                    }

                    //Dublin Extension
                    DublinCoreMetadataTermsSyndicationExtension DublinExtension =
                        (DublinCoreMetadataTermsSyndicationExtension)
                        item.FindExtension(DublinCoreMetadataTermsSyndicationExtension.MatchByType);
                    if (DublinExtension != null)
                    {
                        GetDublinCoreExtension(DublinExtension.Context, RssItem);
                    }

                    //Slash Extension
                    SiteSummarySlashSyndicationExtension SlashExtension =
                        (SiteSummarySlashSyndicationExtension)
                        item.FindExtension(SiteSummarySlashSyndicationExtension.MatchByType);
                    if (SlashExtension != null)
                    {
                        GetSlashCoreExtension(SlashExtension.Context, RssItem);

                        /*-----------------------------------------------------------------------------------*/
                        /* TODO: Comment problem                                                             */
                        /* Arrangment for a stupid argotic buc, they wrote "coment" and no "comment" !!!!!!  */
                        /* The news module comment URL, it's the URL + #comments                             */
                        /*-----------------------------------------------------------------------------------*/
                        if (SlashExtension.Context.Comments >= 0 && RssItem.Commenturl == null)
                        {
                            RssItem.Commenturl = item.Link.AbsoluteUri + "#comments";
                        }
                        /*-----------------------------------------------------------------------------------*/
                    }

                    //Geo localization
                    BasicGeocodingSyndicationExtension GMLExtension =
                        (BasicGeocodingSyndicationExtension)
                        item.FindExtension(BasicGeocodingSyndicationExtension.MatchByType);
                    if (GMLExtension != null)
                    {
                        GetBasicGeocodingExtension(GMLExtension.Context, RssItem);
                    }

                    ////Yahoo media tag
                    YahooMediaSyndicationExtension YahooExtension =
                        (YahooMediaSyndicationExtension) item.FindExtension(YahooMediaSyndicationExtension.MatchByType);
                    if (YahooExtension != null)
                    {
                        RssItem.RssfeedItemsMedia = new List<AdvancedRssItemMedia>();
                        GetYahooExtension(YahooExtension.Context, RssItem.RssfeedItemsMedia);
                    }
                }

                Channel.RssItems.Add(RssItem);
            }

            return Channel;
        }

        #region advanced to argotic

        // Get basic data for channel
        private void SetBasicChannelData(AdvancedRssChannel ChannelSource, RssChannel ChannelDest)
        {
            //Description
            if (ChannelSource.Description != null)
            {
                ChannelDest.Description = ChannelSource.Description;
            }

            //Title
            if (ChannelSource.Title != null)
            {
                ChannelDest.Title = ChannelSource.Title;
            }

            //Generator
            if (ChannelSource.Generator != null)
            {
                ChannelDest.Generator = ChannelSource.Generator;
            }

            //Language
            if (ChannelSource.Language != null && ChannelSource.Language != "")
            {
                //TODO: Cultura del feed
                //ChannelDest.Language =   .ChannelSource.Language;
            }

            //LastBuildDate
            if (ChannelSource.LastBuildDate != null)
            {
                ChannelDest.LastBuildDate = ChannelSource.LastBuildDate;
            }

            //AbsoluteUri
            if (ChannelSource.Link != null && ChannelSource.Link != "")
            {
                try
                {
                    ChannelDest.Link = new Uri(ChannelSource.Link);
                }
                catch
                {
                }
            }

            //PublicationDate
            if (ChannelSource.PubDate != null)
            {
                ChannelDest.PublicationDate = ChannelSource.PubDate;
            }
        }

        // Get basic data for Item
        private void SetBasicItemData(AdvancedRssItem RssItemSource, RssItem RssItemDest)
        {
            //Title
            if (RssItemSource.Title != null)
            {
                RssItemDest.Title = RssItemSource.Title;
            }

            //Author
            if (RssItemSource.Author != "" && RssItemSource.Author != "")
            {
                RssItemDest.Author = RssItemSource.Author;
            }

            //Category
            if (RssItemSource.Category != null)
            {
                foreach (String item in RssItemSource.Category)
                {
                    RssItemDest.Categories.Add(new RssCategory(item));
                }
            }

            //Content of the RSS
            if (RssItemSource.Description != null)
            {
                RssItemDest.Description = RssItemSource.Description;
            }

            //Link of info
            if (RssItemSource.Link != null && RssItemSource.Link != "")
            {
                try
                {
                    RssItemDest.Link = new Uri(RssItemSource.Link);
                }
                catch
                {
                }
            }

            //Publiched date
            if (RssItemSource.PubDate != null)
            {
                RssItemDest.PublicationDate = RssItemSource.PubDate;
            }

            //Enclosure (attachment)
            if (RssItemSource.Enclosureurl != null && RssItemSource.Enclosureurl != "")
            {
                try
                {
                    string enclosMimeType = "";

                    if (RssItemSource.Enclosuretype != null)
                    {
                        enclosMimeType = RssItemSource.Enclosuretype;
                    }

                    RssEnclosure newEnclosure = new RssEnclosure(RssItemSource.Enclosurelength, enclosMimeType,
                                                                 new Uri(RssItemSource.Enclosureurl));

                    RssItemDest.Enclosures.Add(newEnclosure);
                }
                catch
                {
                }
            }

            //Comment info (number isn't a correct rss tag but maybe util)
            if (RssItemSource.Commenturl != null && RssItemSource.Commenturl != "")
            {
                try
                {
                    RssItemDest.Comments = new Uri(RssItemSource.Commenturl);
                }
                catch
                {
                }
            }
        }

        // Get slash extension data
        private void SetSlashExtension(AdvancedRssItem RssItemSource, RssItem RssItemDest)
        {
            //TODO:we just use it for comment number

            //Number of comment
            if (RssItemSource.Commentnumber >= 0)
            {
                SiteSummarySlashSyndicationExtension SlashExtension = new SiteSummarySlashSyndicationExtension();
                SlashExtension.Context.Comments = RssItemSource.Commentnumber;
                RssItemDest.AddExtension(SlashExtension);
            }
        }

        // Get BasicGeo extension data
        private void SetBasicGeocodingExtension(AdvancedRssItem RssItemSource, RssItem RssItemDest)
        {
            BasicGeocodingSyndicationExtension GeoCoding = new BasicGeocodingSyndicationExtension();

            GeoCoding.Context.Longitude = (decimal) RssItemSource.Geolon;
            GeoCoding.Context.Latitude = (decimal) RssItemSource.Geolon;

            RssItemDest.AddExtension(GeoCoding);
        }

        // Get Yahoo extension data
        private void SetYahooExtension(IList<AdvancedRssItemMedia> ilarim, YahooMediaSyndicationExtension yme)
        {
            //TODO, case of group!!! for type ??
            foreach (AdvancedRssItemMedia item in ilarim)
            {
                YahooMediaContent ymc = new YahooMediaContent();

                ymc.Url = new Uri(item.Content);

                //Title
                if (item.Title != null && item.Title != "")
                {
                    ymc.Title = new YahooMediaTextConstruct(item.Title);
                }

                //Content
                if (item.Content != null && item.Content != "")
                {
                    try
                    {
                        ymc.Url = new Uri(item.Content);
                    }
                    catch
                    {
                    }
                }

                //ContentType
                if (item.ContentType != null)
                {
                    ymc.ContentType = item.ContentType;
                }

                //Credit
                if (item.Credit != null && item.Credit != "")
                {
                    ymc.Credits.Add(new YahooMediaCredit(item.Credit));
                }

                //Descripcion 
                if (item.Description != null && item.Description != "")
                {
                    ymc.Description = new YahooMediaTextConstruct(item.Description);
                }

                yme.Context.AddContent(ymc);
            }
        }

        #endregion

        #region argotic to advanced

        // Get basic data for channel
        private void GetBasicChannelData(RssChannel ChannelSource, AdvancedRssChannel ChannelDest)
        {
            //Description
            if (ChannelSource.Description != null)
            {
                ChannelDest.Description = ChannelSource.Description;
            }
            if (ChannelSource.Image != null)
            {
                ChannelDest.ImageUrl = ChannelSource.Image.Url.AbsoluteUri;
            }

            //Title
            if (ChannelSource.Title != null)
            {
                ChannelDest.Title = ChannelSource.Title;
            }

            //Generator
            if (ChannelSource.Generator != null)
            {
                ChannelDest.Generator = ChannelSource.Generator;
            }

            //Language
            if (ChannelSource.Language != null)
            {
                ChannelDest.Language = ChannelSource.Language.ToString();
            }

            //LastBuildDate
            if (ChannelSource.LastBuildDate != null)
            {
                ChannelDest.LastBuildDate = ChannelSource.LastBuildDate;
            }

            //AbsoluteUri
            if (ChannelSource.Link != null)
            {
                ChannelDest.Link = ChannelSource.Link.AbsoluteUri;
            }

            //PublicationDate
            if (ChannelSource.PublicationDate != null)
            {
                ChannelDest.PubDate = ChannelSource.PublicationDate;
            }
        }

        // Get basic data for Item
        private void GetBasicItemData(RssItem RssItemSource, AdvancedRssItem RssItemDest)
        {
            //Title
            if (RssItemSource.Title != null)
            {
                RssItemDest.Title = RssItemSource.Title;
            }

            //Author
            if (RssItemSource.Author != null)
            {
                //TODO: can be extend width more data
                if (RssItemSource.Author != null)
                {
                    RssItemDest.Author = RssItemSource.Author;
                }
            }

            //Publiched date
            if (RssItemSource.PublicationDate != null)
            {
                RssItemDest.PubDate = RssItemSource.PublicationDate;
            }

            //Comment info (number isn't a correct rss tag but maybe util)
            if (RssItemSource.Comments != null)
            {
                RssItemDest.Commenturl = RssItemSource.Comments.ToString();
            }

            //Content of the RSS
            if (RssItemSource.Description != null)
            {
                RssItemDest.Description = RssItemSource.Description;
            }

            //TODO: Collection of enclosure
            //Enclosure (attachment)
            if (RssItemSource.Enclosures != null && RssItemSource.Enclosures.Count > 0)
            {
                RssItemDest.Enclosureurl = RssItemSource.Enclosures[0].Url.AbsoluteUri;
                RssItemDest.Enclosuretype = RssItemSource.Enclosures[0].ContentType;
            }

            //Link of info
            if (RssItemSource.Link != null)
            {
                RssItemDest.Link = RssItemSource.Link.ToString();
            }
        }

        // Get dublin extension data
        private void GetDublinCoreExtension(DublinCoreMetadataTermsSyndicationExtensionContext DublinSource,
                                            AdvancedRssItem RssItemDest)
        {
            //TODO:we just use it for title, descripcion, creator and date if they don't exist 

            //Title
            if (DublinSource.Title != null && RssItemDest.Title == null)
            {
                RssItemDest.Title = DublinSource.Title;
            }

            //Description
            if (DublinSource.Description != null && RssItemDest.Description == null)
            {
                RssItemDest.Description = DublinSource.Description;
            }

            //Creator
            if (DublinSource.Creator != null && RssItemDest.Author == null)
            {
                RssItemDest.Author = DublinSource.Creator;
            }

            //Date
            if (DublinSource.Date != null && RssItemDest.PubDate == null)
            {
                RssItemDest.PubDate = DublinSource.Date;
            }
        }

        // Get slash extension data
        private void GetSlashCoreExtension(SiteSummarySlashSyndicationExtensionContext SlashSource,
                                           AdvancedRssItem RssItemDest)
        {
            //TODO:we just use it for comment number

            //Number of comment
            if (SlashSource.Comments >= 0)
            {
                RssItemDest.Commentnumber = SlashSource.Comments;
            }
        }

        // Get BasicGeo extension data
        private void GetBasicGeocodingExtension(BasicGeocodingSyndicationExtensionContext GMLSource,
                                                AdvancedRssItem RssItemDest)
        {
            RssItemDest.Geolat = (float) GMLSource.Latitude;
            RssItemDest.Geolon = (float) GMLSource.Longitude;
        }

        // Get Yahoo extension data
        private void GetYahooExtension(YahooMediaSyndicationExtensionContext yme, IList<AdvancedRssItemMedia> ilarim)
        {
            //Simple content yahoo rss item
            foreach (YahooMediaContent ymc in yme.Contents)
            {
                AdvancedRssItemMedia RssItemMedia = new AdvancedRssItemMedia();

                RssItemMedia.Content = ymc.Url.AbsoluteUri;

                //Credit 
                if (ymc.Credits != null && ymc.Credits.Count>0)
                {
                    RssItemMedia.Credit = ymc.Credits[0].Role;
                }

                //Title
                if (ymc.Title != null)
                {
                    RssItemMedia.Title = ymc.Title.ToString();
                }

                //Descripcion 
                if (ymc.Description != null)
                {
                    RssItemMedia.Description = ymc.Description.ToString();
                }

                ilarim.Add(RssItemMedia);
            }

            //Group of content yahoo rss item
            if (yme.Groups != null)
            {
                foreach (YahooMediaGroup ymg in yme.Groups)
                {
                    foreach (YahooMediaContent ymc in ymg.Contents)
                    {
                        AdvancedRssItemMedia RssItemMedia = new AdvancedRssItemMedia();

                        RssItemMedia.Content = ymc.Url.AbsoluteUri;

                        //Credit 
                        if (ymc.Credits != null)
                        {
                            RssItemMedia.Credit = ymc.Credits[0].Role;
                        }

                        //Title
                        if (ymc.Title != null)
                        {
                            RssItemMedia.Title = ymc.Title.ToString();
                        }

                        //Descripcion 
                        if (ymc.Description != null)
                        {
                            RssItemMedia.Description = ymc.Description.ToString();
                        }

                        ilarim.Add(RssItemMedia);
                    }
                }
            }
        }

        #endregion

        //private AdvancedRssChannel GetRssChannelFromAtom(Uri url)
        //{

        //    AdvancedRssChannel Channel = new AdvancedRssChannel();

        //    Argotic.Core.Atom.AtomFeed ArgoticAtomFeed = new Argotic.Core.Atom.AtomFeed();
        //    ArgoticAtomFeed.Load(url.AbsoluteUri);

        //    foreach (Argotic.Core.Atom.AtomEntry entry in ArgoticAtomFeed.Entries)
        //    {

        //        AdvancedRssItem RssItem = new AdvancedRssItem();

        //        RssItem.Title = entry.Title.Text;
        //        RssItem.Author = entry.Authors[0].Name;
        //        RssItem.PubDate = entry.UpdatedOn.Date;
        //        RssItem.Commenturl = "";
        //        RssItem.Commentnumber = 0;
        //        RssItem.Description = entry.Content.Text;
        //        RssItem.Enclosureurl = null;
        //        RssItem.Geolat = 0;
        //        RssItem.Geolon = 0;
        //        RssItem.Link = entry.Links[0].ResourceLocation.AbsoluteUri;

        //        Channel.RssItems.Add(RssItem);

        //    }

        //    return Channel;

        //}
    }
}