using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.DataGrid;

namespace Virtualization.Views
{
    /// <summary>
    /// VirtualizationListView.xaml 的交互逻辑
    /// </summary>
    public partial class VirtualizationListView : UserControl
    {
        public VirtualizationListView()
        {
            InitializeComponent();
            //this.xcdg.ItemsSource = this.CreateVirtualCollection();//new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //this.xcdg.BringIntoView(  )
        }

        protected DataGridVirtualizingCollectionView CreateVirtualCollection()
        {
            var c = new DataGridVirtualizingCollectionView(typeof(object), false, 10, 100);
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

        protected virtual void OnQueryItemCount(object sender, QueryItemCountEventArgs e)
        {
            e.Count = 800;
        }

        protected virtual async void OnQueryItems(object sender, QueryItemsEventArgs e)
        {
            Console.WriteLine("Request item from{0} count {1}", e.AsyncQueryInfo.StartIndex, e.AsyncQueryInfo.RequestedItemCount);
            var data = new List<object>();
            for (int i = e.AsyncQueryInfo.StartIndex; i < e.AsyncQueryInfo.StartIndex + e.AsyncQueryInfo.RequestedItemCount; i++)
            {
                data.Add(i + 1);
            }
            await Task.Delay(10000);
            e.AsyncQueryInfo.EndQuery(data.ToArray());
        }

        protected virtual void OnAbortQueryItems(object sender, QueryItemsEventArgs e)
        {
        }
    }
}
