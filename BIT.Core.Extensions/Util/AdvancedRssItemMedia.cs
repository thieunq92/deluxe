namespace BIT.Core.Extensions.Util
{
    public class AdvancedRssItemMedia
    {
        private string _category;
        private string _content;
        private string _contentType;
        private string _credit;
        private string _description;
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public string Credit
        {
            get { return _credit; }
            set { _credit = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
    }
}