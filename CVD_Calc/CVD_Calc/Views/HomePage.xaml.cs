using CVD_Calc.Models;
using CVD_Calc.Resx;
using CVD_Calc.ViewModels;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CVD_Calc.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
            Title = AppResources.HomePageTitle;
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //configurestate();
        }
        public void configurestate()
        {
            HT.IsVisible = HomeViewModel.uptempalert;
            LT.IsVisible = HomeViewModel.downtempalert;
            NAQ.IsVisible = HomeViewModel.normalaq;
            SAQ.IsVisible = HomeViewModel.notsuitaq;
            BAQ.IsVisible = HomeViewModel.badaq;
            LowerCVD.IsVisible = HomeViewModel.showlowcvd;
            BetterCVD.IsVisible = HomeViewModel.showbettercvd;
            MediumCVD.IsVisible = HomeViewModel.showmedcvd;
            HighCVD.IsVisible = HomeViewModel.showhighcvd;
            LowerCVD1.Text = HomeViewModel.seevd.ToString();
            BetterCVD1.Text= HomeViewModel.seevd.ToString();
            MediumCVD1.Text= HomeViewModel.seevd.ToString();
            HighCVD1.Text= HomeViewModel.seevd.ToString();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            await Task.Run(() => App.calcvdAsync(false));            
            System.Threading.Thread.Sleep(3000);
            configurestate();
        }
        
    }
}