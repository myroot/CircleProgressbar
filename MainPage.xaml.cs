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
            
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (Progress.Progress == 1.0)
                    Progress.Progress = 0;
                /*
                if (Progress2.Progress == 1.0)
                    Progress2.Progress = 0;
                if (Progress3.Progress == 1.0)
                    Progress3.Progress = 0;
                if (Progress4.Progress == 1.0)
                    Progress4.Progress = 0;
                    */

                Progress.Progress += 0.008;
                /*Progress2.Progress += 0.008;
                Progress3.Progress += 0.008;
                Progress4.Progress += 0.008;*/

                return true;
            });
            
        }
    }
}