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
using System.Windows.Threading;
using Virtualization.ViewModels;

namespace Virtualization.Views
{
    /// <summary>
    /// NormalListView.xaml 的交互逻辑
    /// </summary>
    public partial class NormalListView : UserControl
    {
        public NormalListView()
        {
            InitializeComponent();
        }
        public static void DoEvents()
        {
            DispatcherPriority priority = DispatcherPriority.ApplicationIdle;
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                priority,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            Console.WriteLine("Time Sec:" + (DateTime.Now - NormalListViewModel.startTime).TotalSeconds);
            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("Time Sec:" + (DateTime.Now - NormalListViewModel.startTime).TotalSeconds);
            //DoEvents();
        }
    }
}
