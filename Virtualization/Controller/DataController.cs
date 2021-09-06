using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtualization.VO;



namespace Virtualization.Controller
{
    [Export(typeof(DataController))]
    public class DataController
    {
        private static readonly string folderConfig = ConfigurationManager.AppSettings["Path"];
        private static readonly string thumbnailPrefix = "_";
        private static readonly string[] allowsExtensions = new string[] { ".jpg", ".png" };

        private List<ThumbnailVO> cache;

        public void Initailize()
        {
            cache = new List<ThumbnailVO>();
            var d = new DirectoryInfo(folderConfig);
            var p = d.GetFiles().Where(q => !q.Attributes.HasFlag(FileAttributes.Hidden)
                         && !q.Name.StartsWith(thumbnailPrefix)
                         && allowsExtensions.Contains(Path.GetExtension(q.FullName).ToLower()));

            foreach (var each in p)
            {
                var index = Convert.ToInt32(System.IO.Path.GetFileNameWithoutExtension(each.Name));
                var thumbnailPath = Path.Combine(folderConfig, string.Format("_{0}.jpg", index.ToString().PadLeft(8, '0')));
               
                ThumbnailVO thumbnail = new ThumbnailVO();
                thumbnail.Path = thumbnailPath;
                thumbnail.PageNumber = index;

                cache.Add(thumbnail);
            }            
        }

        public List<ThumbnailVO> GetThumbnailList()
        {
            return cache;
        }
    }
}
