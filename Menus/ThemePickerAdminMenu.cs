using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace MainBit.Themes.Menus {
    [OrchardFeature("MainBit.Themes.ThemePicker")]
    public class ThemePickerAdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.AddImageSet("themes")
                .Add(
                    T("Themes"), "10",
                    menu => menu.Action("Index", "Admin", new {area = "Orchard.Themes"})
                                .Permission(Orchard.Themes.Permissions.ApplyTheme)
                                .Add(
                                    T("MainBit Picker"), "5",
                                    item => item.Action("Index", "ThemePickerAdmin", new { area = "MainBit.Themes" })
                                                .Permission(Orchard.Themes.Permissions.ApplyTheme).LocalNav()));
        }
    }
}