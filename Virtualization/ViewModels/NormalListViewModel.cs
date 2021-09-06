using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Virtualization.Controller;
using Virtualization.VO;

namespace Virtualization.ViewModels
{
    [Export(typeof(NormalListViewModel))]
    public class NormalListViewModel : Conductor<ThumbnailPageViewModel>.Collection.AllActive
    {
        private int currentDocumentPage = 1;
        private int thumbnailSelectedIndex;

        private double thumbnailWidth = 200;

        private double thumbnailHeight = 251;

        private DataController controller;

        public override string DisplayName
        {
            get
            {
                return "普通列表";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        public int ThumbnailSelectedIndex
        {
            get
            {
                return this.thumbnailSelectedIndex;
            }
            set
            {
                if (this.thumbnailSelectedIndex != value)
                {
                    this.thumbnailSelectedIndex = value;
                    this.NotifyOfPropertyChange(() => this.ThumbnailSelectedIndex);
                }
            }
        }

        public int CurrentDocumentPage
        {
            set
            {
                currentDocumentPage = value;
                this.ThumbnailSelectedIndex = value - 1;
            }
        }

        private string message;

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
                this.NotifyOfPropertyChange(() => this.Message);
            }
        }

        public void OnSelectionChanged(ListBox source)
        {
            //source.ScrollIntoView
        }

        /// <summary>
        /// 二级初始化，需要耗时操作
        /// </summary>
        /// <param name="thumbnailList"></param>
        /// <param name="currentPageIndex"></param>
        /// <returns></returns>
        public void InitailizeStageTwo(IEnumerable<ThumbnailVO> thumbnailList, int currentPageIndex)
        {
            //await Task.Delay(50);

            //int i = 0;

            foreach (var thumbnailVO in thumbnailList)
            {
                var thumbnailPageVM = IoC.Get<ThumbnailPageViewModel>();
                thumbnailPageVM.Initailize(thumbnailVO, thumbnailWidth, thumbnailHeight);
                this.ActivateItem(thumbnailPageVM);
                /*
                i++;
                if (i == 20)
                {
                    //await Task.Delay(50);
                    i = 0;
                }

                if (thumbnailVO.PageNumber == currentPageIndex)
                {
                    //await Task.Delay(50);
                    this.CurrentDocumentPage = currentPageIndex;
                    //await Task.Delay(50);
                    
                }
                 */
               // Console.WriteLine(thumbnailVO.PageNumber.ToString() + "-----------");
            }

            //ScrollSelectedItemToCenter();

        }

        public static DateTime startTime;

        protected override void OnInitialize()
        {
            startTime = DateTime.Now;
            
            base.OnInitialize();
            this.controller = IoC.Get<DataController>();
            var thumbnailList = this.controller.GetThumbnailList();
            this.InitailizeStageTwo(thumbnailList, 1);
           
        }

       
    }
}
