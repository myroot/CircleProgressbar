using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                if (Progress.Progress == 1.0)
                    Progress.Progress = 0;
                Progress.Progress += 0.001;
                return true;
            });
        }
    }
}