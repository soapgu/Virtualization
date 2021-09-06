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
using Virtualization.ViewModels;

namespace Virtualization.Views
{
    /// <summary>
    /// ThumbnailPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ThumbnailPageView : UserControl
    {
        public ThumbnailPageView()
        {
            InitializeComponent();
            Console.WriteLine("Create ThumbnailPageView--------" + this.GetHashCode() );
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("Page Time Sec:" + (DateTime.Now - NormalListViewModel.startTime).TotalSeconds);
        }
    }
}
