using System.Collections.Generic;
using CMS.Core.Util;

namespace BIT.Core.Extensions.Util
{
    public class AdvancedRssItem : RssItem
    {
        //Cuyahoga.Core.Util.RssItem field

        //public string Title { get; set; }
        //public string Author { get; set; }
        //public string Category { get; set; }
        //public string Description { get; set; }
        //public int ItemId { get; set; }
        //public string Link { get; set; }
        //public DateTime PubDate { get; set; }

        private string _authoremail;
        private IList<string> _categories;
        private int _commentnumber;
        private string _commenturl;
        private long _enclosurelength;
        private string _enclosuretype;
        private string _enclosureurl;
        private float _geolat;
        private float _geolon;
        private string _imageUrl;
        private IList<AdvancedRssItemMedia> _rssfeedItemsMedia;


        public string Enclosureurl
        {
            get { return _enclosureurl; }
            set { _enclosureurl = value; }
        }

        public string Enclosuretype
        {
            get { return _enclosuretype; }
            set { _enclosuretype = value; }
        }

        public string Commenturl
        {
            get { return _commenturl; }
            set { _commenturl = value; }
        }

        public int Commentnumber
        {
            get { return _commentnumber; }
            set { _commentnumber = value; }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public float Geolat
        {
            get { return _geolat; }
            set { _geolat = value; }
        }

        public float Geolon
        {
            get { return _geolon; }
            set { _geolon = value; }
        }

        public IList<AdvancedRssItemMedia> RssfeedItemsMedia
        {
            get { return _rssfeedItemsMedia; }
            set { _rssfeedItemsMedia = value; }
        }

        public new IList<string> Category
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public string AuthorEmail
        {
            get { return _authoremail; }
            set { _authoremail = value; }
        }

        public long Enclosurelength
        {
            get { return _enclosurelength; }
            set { _enclosurelength = value; }
        }
    }
}