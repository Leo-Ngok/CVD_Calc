using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CVD_Calc.Views;
using SQLite;
using CVD_Calc.Models;
using Plugin.LocalNotifications;
using System.Net.Http;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using CVD_Calc.Services;
using Plugin.Connectivity;
using Newtonsoft.Json.Linq;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CVD_Calc
{

    public partial class App : Application
    {
        public static string DB_persondata = string.Empty;
        public static string everydayinfo = string.Empty;
        public static int[][] holiday =
            { new int[] { 1 },
            new int[]{5,6,7},
            new int[]{},
            new int[]{5,19,20},
            new int[]{1,12},
            new int[]{7},
            new int[]{},
            new int[]{},
            new int[]{14},
            new int[]{1,2,7},
            new int[]{2},
            new int[]{8,20,22,24,25}
            };
        public App() { InitializeComponent(); }
        public App(string persondatapath, string arg2)
        {
           // CrossLocalNotifications.Current.Show("Note", "App is running normally", 7000);
            InitializeComponent();
            DB_persondata = persondatapath;
            everydayinfo = arg2;
            MainPage = new MainPage();
            var assembly = typeof(App).GetTypeInfo().Assembly;
            /*if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                //var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                Resx.AppResources.Culture = new System.Globalization.CultureInfo("zh-TW");
                    //ci;
                DependencyService.Get<ILocalize>().SetLocale(new System.Globalization.CultureInfo("zh-TW"));//ci
            }
        */
            AppCenter.Start("android=8acd780f-d450-439f-92d8-d9eabcec6a85;",
                      typeof(Analytics), typeof(Crashes));
        }

        protected override async void OnStart()
        {
            await Crashes.SetEnabledAsync(true);
            bool has_crashed = await Crashes.HasCrashedInLastSessionAsync();
            if (has_crashed)
            {
                ErrorReport errorReport = await Crashes.GetLastSessionCrashReportAsync();
                Crashes.ShouldProcessErrorReport = (ErrorReport report) => true;
            }
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        static double getwwmean(
           string arg1, string arg2,
           string arg3, string arg4,
           string arg5, string arg6)
        {
            double x = getwmean(arg1, arg2);
            double y = getwmean(arg3, arg4);
            double z = getwmean(arg5, arg6);
            return (11.4 * x + 13.9 * y + 8.6 * z) / (11.4 * Math.Sign(x) + 13.9 * Math.Sign(y) + 8.6 * Math.Sign(z));
        }
        static double getwwmean(
          double arg1, double arg2,
           double arg3, double arg4,
           double arg5, double arg6)
        {
            double x = getwmean(arg1, arg2);
            double y = getwmean(arg3, arg4);
            double z = getwmean(arg5, arg6);
            return (11.4 * x + 13.9 * y + 8.6 * z) / (11.4 * Math.Sign(x) + 13.9 * Math.Sign(y) + 8.6 * Math.Sign(z));
        }
        static double getwmean(string arg1, string arg2)
        {
            var a = (std(arg1) + std(arg2)) / (Math.Sign(std(arg1)) + Math.Sign(std(arg2)));
            if (!double.IsNaN(a))
                return a;
            else
                return 0.0;
        }
        static double getwmean(double arg1, double arg2)
        {
            return (arg1 + arg2) / 2;
        }
        static double std(string x)
        {
            if (x == "null") return 0;
            if (x == null) return 0;
            return double.Parse(x);
        }
        public static async void calcvdAsync(bool showaqi)
        {
            //https://icon-library.net/icon/air-quality-icon-22.html (the air quality icon are from this free website)
            //以下獲取個人記錄
            if (!CrossConnectivity.Current.IsConnected)
            {
                while (true)
                {
                    bool v = await new ContentPage().DisplayAlert("Warning", "You are not connected to the Internet, records cannot be updated", "retry", "cancel");
                    if (!v) return;
                    if (CrossConnectivity.Current.IsConnected) break;
                }                
            }
            try
            {      
                double cvd_idx = double.NaN;
                DB_pdata c;

                using (SQLiteConnection dbc = new SQLiteConnection(DB_persondata))
                {
                    dbc.CreateTable<DB_pdata>();
                    if (dbc.Table<DB_pdata>().ToList().Count == 0) return;
                    c = dbc.Table<DB_pdata>().ToList().First();
                }
                
                //以下假期因素
                bool isholiday = holiday[DateTime.Now.Month - 1].Contains(DateTime.Now.Day);
                //以下空氣污染物
                string res = await new HttpClient().GetStringAsync("http://www.smg.gov.mo/smg/airQuality/latestAirConcentration.json");
                var stuff = JsonConvert.DeserializeObject<AirQualityResponse>(res);
                var aq = new weatherkey()
                {
                    date = DateTime.Parse(stuff.datetime),
                    CO_level = getwwmean(stuff.pohopolu.HE_CO, stuff.enhopolu.HE_CO, stuff.tchopolu.HE_CO, stuff.tghopolu.HE_CO, stuff.cdhopolu.HE_CO, stuff.khhopolu.HE_CO),
                    NO2_level = getwwmean(stuff.pohopolu.HE_NO2, stuff.enhopolu.HE_NO2, stuff.tchopolu.HE_NO2, stuff.tghopolu.HE_NO2, stuff.cdhopolu.HE_NO2, stuff.khhopolu.HE_NO2),
                    PM_10_level = getwwmean(stuff.pohopolu.HE_PM10, stuff.enhopolu.HE_PM10, stuff.tchopolu.HE_PM10, stuff.tghopolu.HE_PM10, stuff.cdhopolu.HE_PM10, stuff.khhopolu.HE_PM10),
                    PM2_5_level = getwwmean(stuff.pohopolu.HE_PM2_5, stuff.enhopolu.HE_PM2_5, stuff.tchopolu.HE_PM2_5, stuff.tghopolu.HE_PM2_5, stuff.cdhopolu.HE_PM2_5, stuff.khhopolu.HE_PM2_5),
                    O3_level = getwwmean(stuff.pohopolu.HE_O3, stuff.enhopolu.HE_O3, stuff.tchopolu.HE_O3, stuff.tghopolu.HE_O3, stuff.cdhopolu.HE_O3, stuff.khhopolu.HE_O3),
                    SO2_level = getwwmean(stuff.pohopolu.HE_SO2, stuff.enhopolu.HE_SO2, stuff.tchopolu.HE_SO2, stuff.tghopolu.HE_SO2, stuff.cdhopolu.HE_SO2, stuff.khhopolu.HE_SO2)
                };
                //以下溫度
                var tmp = await new HttpClient().GetStreamAsync("http://www.smg.gov.mo/smg/latestWeather/e_7daysforecast.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(SevenDaysForecast));
                SevenDaysForecast items = (SevenDaysForecast)serializer.Deserialize(tmp);
                var temps = items.WeatherItem.WeatherForecast[0].Temperature;
                var futuretemps= items.WeatherItem.WeatherForecast[1].Temperature;
                double todaytemp = (double.Parse(temps.First(x => x.Type == "1").Value) + double.Parse(temps.First(x => x.Type == "2").Value)) / 2;
                double futuretemp= (double.Parse(futuretemps.First(x => x.Type == "1").Value) + double.Parse(futuretemps.First(x => x.Type == "2").Value)) / 2;
                //以下計算cvd
                cvd_idx = calcvd(c, todaytemp, aq, isholiday);
                CrossLocalNotifications.Current.Show("CVD Calculated", "Your CVD Index for today is " + (Math.Round(cvd_idx * 10000) / 100).ToString() + "%", 100000);
                //以下求空氣質量指數
                var web = new HttpClient();
                string aqilink = await web.GetStringAsync("http://www.smg.gov.mo/smg/airQuality/ho_api_history.json");
                JObject aqi = JObject.Parse(aqilink);
                string[] places = new string[] { "en", "tc", "tg", "kh", "po", "cd" };
                var aqi0 = aqi["sysdate"].ToObject<DateTime>();
                //澳門高密度
                var aqi1 = aqi["en"].Children().ToList().Last().ToObject<AQIVal>().value;
                //澳門路邊
                var aqi2 = aqi["po"].Children().ToList().Last().ToObject<AQIVal>().value;
                //氹仔高密度
                var aqi3 = aqi["tc"].Children().ToList().Last().ToObject<AQIVal>().value;
                //氹仔一般性
                var aqi4 = aqi["tg"].Children().ToList().Last().ToObject<AQIVal>().value;
                //九澳路邊
                var aqi5 = aqi["kh"].Children().ToList().Last().ToObject<AQIVal>().value;
                //路環一般性
                var aqi6 = aqi["cd"].Children().ToList().Last().ToObject<AQIVal>().value;
                double aqia = getwwmean(aqi1, aqi2, aqi3, aqi4, aqi5, aqi6);
                // if(showaqi) 
                CrossLocalNotifications.Current.Show("Air Quality Notice", "The AQI for today is " +aqia.ToString(), 100001);
                using (SQLiteConnection savedt = new SQLiteConnection(everydayinfo))
                {
                    savedt.CreateTable<weatherkey>();
                    savedt.Insert(new weatherkey() { date = DateTime.Now, CVD_idx = cvd_idx ,AQI_idx=aqia,todaytemp=todaytemp,futuretemp=futuretemp});
                }
            }catch(Exception EX) {
#if DEBUG
                CrossLocalNotifications.Current.Show("error", EX.Message + " " + EX.Source);
#endif
            }
        }

        private static double calcvd(DB_pdata c, double tem, weatherkey aq, bool ishol)
        {
            DateTime td = DateTime.Today;
            double age = (DateTime.Today.Year - c.dob.Year);
            if (td.Month < c.dob.Month || td.Day < c.dob.Day) age--;

            double height = c.heig;
            double weight = c.weig;
            bool gender = c.genD;
            double SP = c.Sbp;
            double DP = c.Dbp;
            double CL = c.Cho;
            double BH = c.hdll;
            bool is_smoker = c.smoK;
            bool is_diabetic = c.diaB;

            double temp = tem;

            double frs = 0;

            double pm_2_5 = aq.PM2_5_level;
            double pm_10 = aq.PM_10_level;
            double so2 = aq.SO2_level;
            double no2 = aq.NO2_level;
            //age factor=====================================
            if (age >= 30 && age < 35) frs--;
            else if (age >= 40 && age < 45) frs++;
            else if (age >= 45 && age < 50) frs += 2;
            else if (age >= 50 && age < 55) frs += 3;
            else if (age >= 55 && age < 60) frs += 4;
            else if (age >= 60 && age < 65) frs += 5;
            else if (age >= 65 && age < 70) frs += 6;
            else if (age >= 70 && age < 75) frs += 7;
            //cholesterol level===============================
            if (CL < 160) frs -= 3;
            else if (CL >= 160 && CL < 200) frs += 0;
            else if (CL >= 200 && CL < 240) frs++;
            else if (CL >= 240 && CL < 280) frs += 2;
            else frs += 3;
            //blood HDL=======================================
            if (BH < 35) frs += 2;
            else if (BH >= 35 && BH < 45) frs++;
            else if (BH >= 45 && BH < 60) frs += 0;
            else frs -= 2;
            //blood pressure==================================
            if (SP >= 160 || DP >= 100) frs += 3;
            else if (SP >= 140 || DP >= 90) frs += 2;
            else if (SP >= 130 || DP >= 85) frs++;
            //else frs += 0;
            //is_diabetic=====================================
            if (is_diabetic) frs += 2;
            //is_smoker=======================================
            if (is_smoker) frs += 2;
            //convert to probability
            if (frs <= -3) frs = 0.01;
            else if (frs == -2) frs = 0.02;
            else if (frs == -1) frs = 0.02;
            else if (frs == 0) frs = 0.03;
            else if (frs == 1) frs = 0.04;
            else if (frs == 2) frs = 0.04;
            else if (frs == 3) frs = 0.06;
            else if (frs == 4) frs = 0.07;
            else if (frs == 5) frs = 0.09;
            else if (frs == 6) frs = 0.11;
            else if (frs == 7) frs = 0.14;
            else if (frs == 8) frs = 0.18;
            else if (frs == 9) frs = 0.22;
            else if (frs == 10) frs = 0.27;
            else if (frs == 11) frs = 0.33;
            else if (frs == 12) frs = 0.4;
            else if (frs == 13) frs = 0.47;
            else if (frs == 14) frs = 0.56;

            //temperature=====================================
            if (temp > 26) frs *= Math.Pow(1.17, (temp - 26));
            else if (temp < 26) frs *= Math.Pow(1.12, (-temp + 26));
            //air quality=====================================
            if (pm_2_5 > 96.2) frs *= Math.Pow(1.27, (pm_2_5 - 96.2) / 10);
            if (pm_10 > 115.6 && age >= 65) frs *= Math.Pow(1.012, (pm_10 - 115.6) / 10);
            if (so2 >= 53.21) frs *= Math.Pow(1.01, (so2 - 53.21) / 10);
            if (no2 >= 53.08) frs *= Math.Pow(1.019, (no2 - 53.08) / 10);

            //holiday factor
            if (ishol) frs *= 2.375;
            CrossLocalNotifications.Current.Show("CVD Calculated", "your cvd index is" + frs, 100000);
            return frs;
        }


        [XmlRoot(ElementName = "SevenDaysForecast")]
        public class SevenDaysForecast
        {
            [XmlElement(ElementName = "Custom")]
            public WeatherItem WeatherItem { get; set; }
        }
        public class AirQualityResponse
        {
            [JsonConstructor]
            public AirQualityResponse() { }
            public Pohopolu pohopolu { get; set; }
            public Enhopolu enhopolu { get; set; }
            public Tchopolu tchopolu { get; set; }
            public Tghopolu tghopolu { get; set; }
            public Cdhopolu cdhopolu { get; set; }
            public Khhopolu khhopolu { get; set; }
            public string datetime { get; set; }
        }
        public class Cdhopolu
        {
            public string DDTT { get; set; }
            public string HE_PM10 { get; set; }
            public string HE_PM2_5 { get; set; }
            public string HE_NO2 { get; set; }
            public string HE_O3 { get; set; }
            public string HE_SO2 { get; set; }
            public string HE_CO { get; set; }
        }
        public class Enhopolu
        {
            public string DDTT { get; set; }
            public string HE_PM10 { get; set; }
            public string HE_PM2_5 { get; set; }
            public string HE_NO2 { get; set; }
            public string HE_O3 { get; set; }
            public string HE_SO2 { get; set; }
            public string HE_CO { get; set; }
        }
        public class Khhopolu
        {
            public string DDTT { get; set; }
            public string HE_PM10 { get; set; }
            public string HE_PM2_5 { get; set; }
            public string HE_NO2 { get; set; }
            public string HE_O3 { get; set; }
            public string HE_SO2 { get; set; }
            public string HE_CO { get; set; }
        }
        public class Pohopolu
        {
            public string DDTT { get; set; }
            public string HE_PM10 { get; set; }
            public string HE_PM2_5 { get; set; }
            public string HE_NO2 { get; set; }
            public string HE_CO { get; set; }
            public string HE_O3 { get; set; }
            public string HE_SO2 { get; set; }
        }
        public class Tchopolu
        {
            public string DDTT { get; set; }
            public string HE_PM10 { get; set; }
            public string HE_PM2_5 { get; set; }
            public string HE_NO2 { get; set; }
            public string HE_O3 { get; set; }
            public string HE_SO2 { get; set; }
            public string HE_CO { get; set; }
        }
        public class Tghopolu
        {
            public string DDTT { get; set; }
            public string HE_PM10 { get; set; }
            public string HE_PM2_5 { get; set; }
            public string HE_NO2 { get; set; }
            public string HE_O3 { get; set; }
            public string HE_SO2 { get; set; }
            public string HE_CO { get; set; }
        }
        [XmlRoot(ElementName = "Custom")]
        public class WeatherItem
        {
            [XmlElement(ElementName = "WeatherForecast")]
            public List<WeatherForecast> WeatherForecast { get; set; }
            [XmlElement(ElementName = "IssuedTime")]
            public string IssuedTime { get; set; }
        }

        [XmlRoot(ElementName = "WeatherForecast")]
        public class WeatherForecast
        {
            [XmlElement(ElementName = "AstronomicalTide")]
            public string AstronomicalTide { get; set; }

            [XmlElement(ElementName = "ValidFor")]
            public string ValidFor { get; set; }

            [XmlElement(ElementName = "e_DayOfWeek")]
            public string E_DayOfWeek { get; set; }
            [XmlElement(ElementName = "WeatherStatus")]
            public string WeatherStatus { get; set; }

            [XmlElement(ElementName = "Temperature")]
            public List<Temperature> Temperature { get; set; }
            [XmlElement(ElementName = "WeatherDescription")]
            public string WeatherDescription { get; set; }
        }
        [XmlRoot(ElementName = "Temperature")]
        public class Temperature
        {
            [XmlElement(ElementName = "Type")]
            public string Type { get; set; }
            [XmlElement(ElementName = "MeasureUnit")]
            public string MeasureUnit { get; set; }
            [XmlElement(ElementName = "Value")]
            public string Value { get; set; }
        }
    }
}
