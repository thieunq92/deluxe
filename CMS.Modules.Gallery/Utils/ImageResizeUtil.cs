/*

	code taken from http://www.devx.com/dotnet/Article/22079
	
	original author Alex Hildyard (alexhildyard@hotmail.com)
	
	butchered by me

*/

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ImageHelper
{
    public class ImageResizeUtil
    {
        private ImageResizeUtil _cache;
        private Image _dstImage;
        private Graphics _graphics;
        private double _height;

        private string _imagePath;
        private Image _srcImage;
        private bool _useAspect = true;
        private bool _usepercentage;
        private double _width;

        public ImageResizeUtil(Image image, int maxImageWidth)
        {
            _width = maxImageWidth;
            _srcImage = image;
        }

        public string File
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        public Image Image
        {
            get { return _srcImage; }
            set { _srcImage = value; }
        }

        public bool PreserveAspectRatio
        {
            get { return _useAspect; }
            set { _useAspect = value; }
        }

        public bool UsePercentages
        {
            get { return _usepercentage; }
            set { _usepercentage = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        protected virtual bool IsSameSrcImage(ImageResizeUtil other)
        {
            if (other != null)
            {
                return (File == other.File
                        && Image == other.Image);
            }

            return false;
        }

        /// <summary>
        ///  Check whether the required destination image properties have
        ///  changed
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool IsSameDstImage(ImageResizeUtil other)
        {
            if (other != null)
            {
                return (Width == other.Width
                        && Height == other.Height
                        && UsePercentages == other.UsePercentages
                        && PreserveAspectRatio == other.PreserveAspectRatio);
            }

            return false;
        }

        public virtual Image GetThumbnail()
        {
            // Flag whether a new image is required
            bool recalculate = false;
            recalculate = IsInCache(recalculate);

            double new_width = Width;
            double new_height = Height;


            if (!IsSameDstImage(_cache))
            {
                CalucateWidthAndHeight(ref new_height, ref new_width);
                recalculate = true;
            }

            if (recalculate)
            {
                // Calculate the new image
                if (_dstImage != null)
                {
                    _dstImage.Dispose();
                    _graphics.Dispose();
                }

                Bitmap bitmap = new Bitmap((int) new_width, (int) new_height, _srcImage.PixelFormat);
                _graphics = Graphics.FromImage(bitmap);
                _graphics.SmoothingMode = SmoothingMode.HighQuality;
                _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                _graphics.DrawImage(_srcImage, 0, 0, bitmap.Width, bitmap.Height);
                _dstImage = bitmap;

                // Cache the image and its associated settings
                _cache = MemberwiseClone() as ImageResizeUtil;
            }

            return _dstImage;
        }

        private void CalucateWidthAndHeight(ref double new_height, ref double new_width)
        {
            // Yes, so we need to recalculate.
            // If you opted to specify width and height as percentages of the original
            // image's width and height, compute these now
            if (UsePercentages)
            {
                if (Width != 0)
                {
                    new_width = _srcImage.Width*Width/100;

                    if (PreserveAspectRatio)
                    {
                        new_height = new_width*_srcImage.Height/_srcImage.Width;
                    }
                }

                if (Height != 0)
                {
                    new_height = _srcImage.Height*Height/100;

                    if (PreserveAspectRatio)
                    {
                        new_width = new_height*_srcImage.Width/_srcImage.Height;
                    }
                }
            }
            else
            {
                // If you specified an aspect ratio and absolute width or height, then calculate this 
                // now; if you accidentally specified both a width and height, ignore the 
                // PreserveAspectRatio flag
                if (PreserveAspectRatio)
                {
                    // alternative test resizing
                    if (Width != 0 && Height == 0)
                    {
                        if (_srcImage.Height > _srcImage.Width)
                        {
                            new_height = Width;
                            new_width = _srcImage.Width*(Width/_srcImage.Height);
                        }
                        else
                        {
                            new_width = Width;
                            new_height = _srcImage.Height*(Width/_srcImage.Width);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Is there a cached source image available? If not,
        /// load the image if a filename was specified; otherwise
        /// use the image in the Image property
        /// </summary>
        /// <param name="recalculate"></param>
        /// <returns></returns>
        private bool IsInCache(bool recalculate)
        {
            if (!IsSameSrcImage(_cache))
            {
                if (_imagePath != null)
                {
                    if (_imagePath.Length > 0)
                    {
                        // Load via stream rather than Image.FromFile to release the file
                        // handle immediately
                        if (_srcImage != null)
                            _srcImage.Dispose();

                        // Wrap the FileStream in a "using" directive, to ensure the handle
                        // gets closed when the object goes out of scope
                        using (Stream stream = new FileStream(_imagePath, FileMode.Open))
                            _srcImage = Image.FromStream(stream);

                        return true;
                    }
                }
            }
            return false;
        }

        ~ImageResizeUtil()
        {
            // Free resources
            if (_dstImage != null)
            {
                _dstImage.Dispose();
                _graphics.Dispose();
            }

            if (_srcImage != null)
                _srcImage.Dispose();
        }
    }
}