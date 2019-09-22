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
	public partial class ConfigsPage : ContentPage
	{
		public ConfigsPage ()
		{
			InitializeComponent ();
		}
        private async void TextCell_Tapped(object sender, EventArgs e)
        {
            string unit;
            unit = await DisplayActionSheet( "Unit of weight", "Cancel", null,  "Kilogram",  "Pounds");
            if (unit != null) Weightset.Detail = unit;
            //savedata();
        }
        private async void Heightset_Tapped(object sender, EventArgs e)
        {
            string unit;
            unit = await DisplayActionSheet("Unit of height",  "Cancel", null, "Meter", "Feet" );
            if (unit != null) Heightset.Detail = unit;
            //savedata();
        }
        
        private async void Tpset_Tapped(object sender, EventArgs e)
        {
            string unit;
            unit = await DisplayActionSheet("Unit of Temperature","Cancel",null,"Celsius", "Fahrenheit");
            if (unit != null)
                Tpset.Detail = unit;
            //savedata();
        }
        private void Lang_Tapped(object sender, EventArgs e)
        {
            
        }
    }
}