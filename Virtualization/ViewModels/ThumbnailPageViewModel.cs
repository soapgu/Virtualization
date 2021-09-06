using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Virtualization.VO;

namespace Virtualization.ViewModels
{
    [Export(typeof(ThumbnailPageViewModel)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class ThumbnailPageViewModel:Screen
    {
        private ImageSource image;

        private int pageNumber;

        private double width = 178;

        private double height = 251;

        private static int count;

        public void Initailize(ThumbnailVO thumbnailVO, double width, double height)
        {
            this.PageNumber = thumbnailVO.PageNumber;
            this.BindingImage(thumbnailVO.Path);

            this.width = width;
            this.height = height;
        }

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

        public double Width
        {
            get
            {
                return this.width;
            }
        }

        public double Height
        {
            get
            {
                return this.height;
            }
        }

        private void BindingImage(string path)
        {
            try
            {
                //var cache = Page.Path;
                Size size = new Size(this.width, this.height); // ImageHelper.GetSize(path);

                ThreadPool.QueueUserWorkItem(obj =>
                {
                    if (File.Exists(path))
                    {
                        CreateImage(path, size, null);
                    }
                }, path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateImage(string path, Size size, Uri uri)
        {
            var image = new BitmapImage();
            if (path != null)
            {
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
            }
            else
            {
                image.BeginInit();
                image.UriSource = uri;
                //image.DecodePixelHeight = Convert.ToInt32(size.Height);
                //image.DecodePixelWidth = Convert.ToInt32(size.Width);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }

            Execute.BeginOnUIThread((System.Action)(() =>
            {
                this.width = image.Width;
                this.height = image.Height;
                this.NotifyOfPropertyChange(() => this.Width);
                this.NotifyOfPropertyChange(() => this.Height);
                Image = image;
                //Console.WriteLine("render image----");
            }));
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                this.Image = null;
            }
            base.OnDeactivate(close);
        }

        public void PageLoaded()
        {
            count++;
            var sec = (DateTime.Now - NormalListViewModel.startTime).TotalSeconds;
            Console.WriteLine("Page Time Sec:" + (DateTime.Now - NormalListViewModel.startTime).TotalSeconds);
            ((NormalListViewModel)this.Parent).Message = string.Format("加载数量{0} 已使用时间{1}秒", count ,sec);
        }

        protected override void OnViewAttached(object view, object context)
        {
            Console.WriteLine("OnViewAttached, Page:" + this.PageNumber + "View Hash:" + view.GetHashCode());
            base.OnViewAttached(view, context);
        }
    }
}
