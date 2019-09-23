using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SQLite;
using CVD_Calc.Models;

namespace CVD_Calc.ViewModels
{
    class HomeViewModel
    {
        public bool HolidayVisible
        {
            get=>App.holiday[DateTime.Now.Month - 1].Contains(DateTime.Now.Day);            
        }
        public bool shownormalaq {get => normalaq;}
        public bool shownotsuitaq{ get => notsuitaq;}
        public bool showbadaq{get => badaq;}
        public bool showuptempalert{get => uptempalert;}
        public bool showdowntempalert{get => downtempalert;}
        public double scvd { get => seevd; }
        public static double seevd
        {
            get
            {
                using (SQLiteConnection aqdata = new SQLiteConnection(App.everydayinfo))
                {
                    aqdata.CreateTable<weatherkey>();
                    if (aqdata.Table<weatherkey>().ToList().Count == 0) return 0;
                    double currentcvd = aqdata.Table<weatherkey>().ToList().Last().CVD_idx;
                    currentcvd = Math.Round(currentcvd * 10000) / 100;
                    return currentcvd;
                }                
            }
        }
        public bool lowcvd { get => showlowcvd; }
        public static bool showlowcvd
        {
            get
            {
                if (seevd>0 && seevd < 10) return true;
                else return false;
            }
        }
        public bool bettercvd { get => showbettercvd; }
        public static bool showbettercvd
        {
            get
            {
                if (seevd >= 10&&seevd<16) return true;
                else return false;
            }
        }
        public bool medcvd { get => showmedcvd; }
        public static bool showmedcvd
        {
            get
            {
                if (seevd>=16 &&seevd < 20) return true;
                else return false;
            }
        }
        public bool highcvd { get => showhighcvd; }
        public static bool showhighcvd
        {
            get
            {
                if (seevd >=20) return true;
                else return false;
            }
        }
        public static bool normalaq {
            get
            {
                if (uptempalert) return false;
                if (downtempalert) return false;
                using (SQLiteConnection aqdata = new SQLiteConnection(App.everydayinfo))
                {
                    aqdata.CreateTable<weatherkey>();
                    if (aqdata.Table<weatherkey>().ToList().Count == 0) return false;
                    double currentaqi = aqdata.Table<weatherkey>().ToList().Last().AQI_idx;
                    if (currentaqi <= 100) return true;
                    else return false;
                }
            }
        }
        public static bool notsuitaq
        {
            get
            {
                if (uptempalert) return false;
                if (downtempalert) return false;
                using (SQLiteConnection aqdata = new SQLiteConnection(App.everydayinfo))
                {
                    aqdata.CreateTable<weatherkey>();
                    if (aqdata.Table<weatherkey>().ToList().Count == 0) return false;
                    double currentaqi = aqdata.Table<weatherkey>().ToList().Last().AQI_idx;
                    if (currentaqi > 100 && currentaqi <= 200) return true;
                    else return false;
                }                    
            }
        }
        public static bool badaq
        {
            get
            {
                if (uptempalert) return false;
                if (downtempalert) return false;
                using (SQLiteConnection aqdata = new SQLiteConnection(App.everydayinfo))
                {
                    aqdata.CreateTable<weatherkey>();
                    if (aqdata.Table<weatherkey>().ToList().Count == 0) return false;
                    double currentaqi = aqdata.Table<weatherkey>().ToList().Last().AQI_idx;
                    if (currentaqi >= 200) return true;
                    else return false;
                }                    
            }
        }
        public static bool uptempalert
        {
            get
            {
                using (SQLiteConnection aqdata = new SQLiteConnection(App.everydayinfo))
                {
                    aqdata.CreateTable<weatherkey>();
                    if (aqdata.Table<weatherkey>().ToList().Count == 0) return false;
                    double futureaqi = aqdata.Table<weatherkey>().ToList().Last().futuretemp;
                    if (futureaqi <= 28) return false;
                    double currenttemp = aqdata.Table<weatherkey>().ToList().Last().todaytemp;
                    if ((futureaqi - currenttemp) >= 5) return true;
                    else return false;
                }               
            }
        }
        public static bool downtempalert
        {
            get
            {
                using (SQLiteConnection aqdata = new SQLiteConnection(App.everydayinfo))
                {
                    aqdata.CreateTable<weatherkey>();
                    if (aqdata.Table<weatherkey>().ToList().Count == 0) return false;
                    double futureaqi = aqdata.Table<weatherkey>().ToList().Last().futuretemp;
                    if (futureaqi >= 15) return false;
                    double currenttemp = aqdata.Table<weatherkey>().ToList().Last().todaytemp;
                    if ((futureaqi - currenttemp) <= -5) return true;
                    else return false;
                }
                   
            }
        }
    }
}
