using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace Virtualization.VO
{
    public class ThumbnailPageVO:PropertyChangedBase
    {
        private ImageSource image;

        private int pageNumber;

        public ImageSource Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.NotifyOfPropertyChange(() => this.Image);
            }
        }

        public int PageNumber
        {
            get
            {
                return this.pageNumber;
            }
            set
            {
                this.pageNumber = value;
                this.NotifyOfPropertyChange(() => this.PageNumber);
            }
        }

        public async void Initailize(ThumbnailVO item)
        {
            this.PageNumber = item.PageNumber;

            this.Image = await Task.Run( ()=> this.CreateImage(item.Path ) );
        }

        private BitmapImage CreateImage(string path)
        {
            var image = new BitmapImage();
           
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                image.BeginInit();
                image.StreamSource = fs;
                //image.DecodePixelHeight = Convert.ToInt32(size.Height);
                //image.DecodePixelWidth = Convert.ToInt32(size.Width);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }

            return image;
        }
    }
}
