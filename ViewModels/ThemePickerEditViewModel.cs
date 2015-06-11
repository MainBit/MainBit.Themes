using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vandelay.Industries.Models;

namespace MainBit.Themes.ViewModels
{
    [OrchardFeature("MainBit.Themes.ThemePicker")]
    public class ThemePickerEditViewModel
    {
        public ThemePickerSettingsRecord ThemePickerSettings { get; set; }
        public IEnumerable<string> ThemeSelectionRules { get; set; }
        public IEnumerable<string> Themes { get; set; }
    }
}