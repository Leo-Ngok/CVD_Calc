using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace CVD_Calc.ViewModels
{
    public class AboutViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }
        public string Title { get; set; }
        public ICommand OpenWebCommand { get; }
    }
}