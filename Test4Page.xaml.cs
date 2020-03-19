using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CircleProgressBar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Test4Page : ContentPage
    {
        public Test4Page()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var ani = new Animation((d) =>
            {
                Heart.LeftProgress = d * 0.45;
                Heart.RightProgress = d * 0.70;
                Heart.BottomProgress = d * 0.90;
            });
            ani.Commit(this, "Up", length: 1000, easing: Easing.SinIn);
        }
    }
}