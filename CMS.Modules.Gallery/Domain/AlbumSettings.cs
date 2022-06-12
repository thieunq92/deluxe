namespace CMS.Modules.Gallery.Domain
{
    public class AlbumSettings
    {
        private bool _showNumberOfViews;
        private int _numberOfColumns;
        private int _numberOfItemsOnPage;
        private int _maxImageDimension;
        private int _maxThumbDimension;
        private bool _showGraphicalButtonsInViewer;


        public bool ShowNumberOfViews
        {
            get { return _showNumberOfViews; }
            set { _showNumberOfViews = value; }
        }

        public int NumberOfColumns
        {
            get { return _numberOfColumns; }
            set { _numberOfColumns = value; }
        }

        public int NumberOfItemsOnPage
        {
            get { return _numberOfItemsOnPage; }
            set { _numberOfItemsOnPage = value; }
        }

        public int MaxImageDimension
        {
            get { return _maxImageDimension; }
            set { _maxImageDimension = value; }
        }

        public int MaxThumbDimension
        {
            get { return _maxThumbDimension; }
            set { _maxThumbDimension = value; }
        }

        public bool ShowGraphicalButtonsInViewer
        {
            get { return _showGraphicalButtonsInViewer; }
            set { _showGraphicalButtonsInViewer = value; }
        }
    }
}
