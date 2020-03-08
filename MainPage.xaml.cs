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
            Progress.TextFormat = "{0:P0} Complete";

            /*
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (Progress.Progress == 1.0)
                    Progress.Progress = 0;
                
                Progress.Progress += 0.008;
                return true;
            });
            */
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var ani = new Animation((d) =>
            {
                Progress.Progress = d * 0.65;
                Progress2.Progress = d * 0.70;
                Progress3.Progress = d * 0.50;
            });
            ani.Commit(this, "Up", length:1000, easing:Easing.SinIn);
        }
    }
}