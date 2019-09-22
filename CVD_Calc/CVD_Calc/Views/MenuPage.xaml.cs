using CVD_Calc.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CVD_Calc.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem{Id=MenuItemType.Home,Title="Home" },
                new HomeMenuItem{Id=MenuItemType.Interval_Timer,Title="Interval Timer"},
                new HomeMenuItem{Id=MenuItemType.Profile,Title="Profile" },
                new HomeMenuItem{Id=MenuItemType.Configs,Title="Configurations" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" }
            };

            ListViewMenu.ItemsSource = menuItems;

            //ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;
                using(var db=new SQLite.SQLiteConnection(App.DB_persondata))
                {
                    db.CreateTable<DB_pdata>();
                    if (db.Table<DB_pdata>().ToList().Count == 0)
                    {
                        await DisplayAlert("Warning", "Please fill in all your personal data before leaving", "dismiss");
                        ((ListView)sender).SelectedItem = null;
                        return;
                    }
                }
                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
                ((ListView)sender).SelectedItem = null;
            };
        }
    }
}