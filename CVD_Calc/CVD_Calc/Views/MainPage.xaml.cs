using CVD_Calc.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CVD_Calc.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            //homepg.
            MasterBehavior = MasterBehavior.Popover;
            Detail = new NavigationPage(new HomePage());
            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            bool havedata = false;
            using (var dbp=new SQLite.SQLiteConnection(App.DB_persondata))
            {
                dbp.CreateTable<DB_pdata>();
                if (dbp.Table<DB_pdata>().ToList().Count != 0) havedata = true;
            }
            if(!havedata) await NavigateFromMenu((int)MenuItemType.Profile);
            
        }
        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.Profile:
                        MenuPages.Add(id, new NavigationPage(new ProfilePage()));
                        break;
                    case (int)MenuItemType.Configs:
                        MenuPages.Add(id, new NavigationPage(new ConfigsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.Interval_Timer:
                        MenuPages.Add(id, new NavigationPage(new IntervalTimerPage()));
                        break;
                    default:
#if DEBUG
                        throw new NotImplementedException();
#else
                        break;
#endif

                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}