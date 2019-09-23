using CVD_Calc.Resx;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVD_Calc.ViewModels
{
    class ProfileViewModel
    {
        public string[] genditems { get => new string[] {
            AppResources.Profile_PickerOption_0,
            AppResources.Profile_PickerOption_1,
            AppResources.Profile_PickerOption_2
        }; }
        public string[] YNitems
        {
            get => new string[] {
            AppResources.Profile_PickerOption_0,
            AppResources.Profile_PickerOption_3,
            AppResources.Profile_PickerOption_4
        };
        }
    }
}
