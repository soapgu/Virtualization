using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Virtualization.Controller;
using Virtualization.VO;
using Xceed.Wpf.DataGrid;

namespace Virtualization.ViewModels
{
    [Export(typeof(VirtualizationListViewModel))]
    public class VirtualizationListViewModel:Screen
    {
        private DataController controller;
        List<ThumbnailVO> thumbnailList;

        private int currentDocumentPage = 1;
        private int thumbnailSelectedIndex;

        private string message;

        public override string DisplayName
        {
            get
            {
                return "虚拟化列表";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        private DataGridCollectionViewBase items;

        public DataGridCollectionViewBase Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
                this.NotifyOfPropertyChange(() => this.Items);
            }
        }

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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.controller = IoC.Get<DataController>();
            this.thumbnailList = this.controller.GetThumbnailList();
            this.Items = this.CreateDataSource();
        }

        protected DataGridVirtualizingCollectionView CreateDataSource()
        {
            var c = new DataGridVirtualizingCollectionView(typeof(ThumbnailPageVO), false, 10, 30);
            c.QueryItemCount += OnQueryItemCount;
            c.QueryItems += OnQueryItems;
            c.AbortQueryItems += OnAbortQueryItems;

            //event handler must be attached before setting filtering,guessing filtering setting triggered them.

            c.DistinctValuesConstraint = DistinctValuesConstraint.Filtered;
            c.FilterCriteriaMode = FilterCriteriaMode.None; //because we use autofilter, not manually entered
            //GenerateItemProperties(c, GetDisplayFields());
            //c.AutoFilterValuesChanged += AutoFilterValuesChanged;
            //c.QueryGroups += new EventHandler<QueryGroupsEventArgs>(QueryGroups);
            return c;
        }

        private void OnQueryItemCount(object sender, QueryItemCountEventArgs e)
        {
            var count = this.thumbnailList.Count;
            e.Count = count;
        }

        private async void OnQueryItems(object sender, QueryItemsEventArgs e)
        {
            Console.WriteLine("Request item from{0} count {1}", e.AsyncQueryInfo.StartIndex, e.AsyncQueryInfo.RequestedItemCount);
            var data = new List<object>();
            for (int i = e.AsyncQueryInfo.StartIndex; i < e.AsyncQueryInfo.StartIndex + e.AsyncQueryInfo.RequestedItemCount; i++)
            {
                
                var thumbnail = this.thumbnailList[i];
                var page = new ThumbnailPageVO();
                page.Initailize(thumbnail);
                if (i == 19)//select second item for reference test,becauce selectitem will not release reference!
                    this.reference = new WeakReference(page);
                data.Add(page);
            }
            await Task.Delay(3000);
            e.AsyncQueryInfo.EndQuery(data.ToArray());
        }

        private void OnAbortQueryItems(object sender, QueryItemsEventArgs e)
        {
        }

        private WeakReference reference,uiReference;

        public void TestRefence()
        {
            if (this.reference != null)
            {
                GC.Collect();
                MessageBox.Show("第20条数据引用还在吗？" + this.reference.IsAlive);
            }
            if (this.uiReference != null)
            {
                MessageBox.Show("UI控件引用还在吗？" + this.uiReference.IsAlive);
            }
        }

        public void AddUIReference(object uiElement)
        {
            this.uiReference = new WeakReference(uiElement);
            MessageBox.Show("控件已经加入引用测试中...");
        }

        public void OnSelectionChanged( DataGridControl source)
        {
            //source.ScrollIntoView
            this.Message = string.Format("跳转到第{0}页", this.ThumbnailSelectedIndex + 1);
            source.BringItemIntoView(source.SelectedItem);
        }

        public void JumpTo(string pageNumber)
        {
            if (string.IsNullOrWhiteSpace(pageNumber))
            {
                return;
            }
            int result;
            if (int.TryParse(pageNumber, out result))
            {
                this.ThumbnailSelectedIndex = result - 1;
            }
            
        }

       
        
    }
}
