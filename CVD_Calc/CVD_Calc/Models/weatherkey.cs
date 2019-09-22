using SQLite;
using System;

namespace CVD_Calc.Models
{
    class weatherkey
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; }
        public DateTime date { get; set; }
        public double SO2_level { get; set; }
        public double CO_level { get; set; }
        public double NO2_level { get; set; }
        public double PM2_5_level { get; set; }
        public double PM_10_level { get; set; }
        public double MinTemp_level { get; set; }
        public double MaxTemp_level { get; set; }
        public double Hum_level { get; set; }
        public double O3_level { get; set; }
        public double CVD_idx { get; set; }
        public double AQI_idx { get; set; }
        public double todaytemp { get; set; }
        public double futuretemp { get; set; }
    }
}
