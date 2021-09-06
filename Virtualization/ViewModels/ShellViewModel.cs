using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

namespace Virtualization.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel: Conductor<Screen>.Collection.OneActive,IShell
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            var normalVM = IoC.Get<NormalListViewModel>();
            var virtualizeVM = IoC.Get<VirtualizationListViewModel>();
            //this.ActivateItem(normalVM);
            this.ActivateItem(virtualizeVM);
            //this.ActivateItem(normalVM);

            //this.ActiveItem
        }
    }
}
