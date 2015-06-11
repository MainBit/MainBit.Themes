using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Orchard;
using Orchard.Environment.Descriptor.Models;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Localization;
using Orchard.UI.Admin;
using Vandelay.Industries.Services;
using Vandelay.Industries.ViewModels;
using MainBit.Themes.ViewModels;
using Vandelay.Industries.Models;

namespace MainBit.Themes.Controllers {
    [Admin]
    [OrchardFeature("MainBit.Themes.ThemePicker")]
    [ValidateInput(false)]
    public class ThemePickerAdminController : Controller {
        private readonly ISettingsService _settingsService;
        private readonly IExtensionManager _extensionManager;
        private readonly ShellDescriptor _shellDescriptor;
        private readonly IEnumerable<IThemeSelectionRule> _rules;

        public ThemePickerAdminController(
            IOrchardServices orchardServices,
            ISettingsService settingsService,
            IExtensionManager extensionManager,
            ShellDescriptor shellDescriptor,
            IEnumerable<IThemeSelectionRule> rules) {

            Services = orchardServices;
            _settingsService = settingsService;
            _extensionManager = extensionManager;
            _shellDescriptor = shellDescriptor;
            _rules = rules;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index() {
            if (!Services.Authorizer.Authorize(Orchard.Themes.Permissions.ApplyTheme, T("Cannot manage themes")))
                return new HttpUnauthorizedResult();

            var viewModel = _settingsService.Get().OrderByDescending(p => p.Priority);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            if (!Services.Authorizer.Authorize(Orchard.Themes.Permissions.ApplyTheme, T("Cannot manage themes")))
                return new HttpUnauthorizedResult();

            var viewModel = GetEditViewModel();

            return View("Edit", viewModel);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(ThemePickerEditViewModel viewModel)
        {
            if (!Services.Authorizer.Authorize(Orchard.Themes.Permissions.ApplyTheme, T("Cannot manage themes")))
                return new HttpUnauthorizedResult();

            CreateThemePickerSettings(viewModel);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (!Services.Authorizer.Authorize(Orchard.Themes.Permissions.ApplyTheme, T("Cannot manage themes")))
                return new HttpUnauthorizedResult();

            var viewModel = GetEditViewModel(id);

            return View(viewModel);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST(ThemePickerEditViewModel viewModel)
        {
            if (!Services.Authorizer.Authorize(Orchard.Themes.Permissions.ApplyTheme, T("Cannot manage themes")))
                return new HttpUnauthorizedResult();

            CreateThemePickerSettings(viewModel);
            _settingsService.Remove(viewModel.ThemePickerSettings.Id);

            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            if (!Services.Authorizer.Authorize(Orchard.Themes.Permissions.ApplyTheme, T("Cannot manage themes")))
                return new HttpUnauthorizedResult();
            
            _settingsService.Remove(id);

            return RedirectToAction("Index");
        }

        private ThemePickerEditViewModel GetEditViewModel(int id = 0)
        {
            var viewModel = new ThemePickerEditViewModel
            {
                ThemePickerSettings = id > 0 ? _settingsService.Get().First(s => s.Id == id) : new ThemePickerSettingsRecord(),
                ThemeSelectionRules = _rules.Select(r => r.GetType().Name),
                Themes = _extensionManager.AvailableExtensions()
                    .Where(d => DefaultExtensionTypes.IsTheme(d.ExtensionType) &&
                                _shellDescriptor.Features.Any(sf => sf.Name == d.Id))
                    .Select(d => d.Id)
                    .OrderBy(n => n)
            };

            return viewModel;
        }

        private void CreateThemePickerSettings(ThemePickerEditViewModel viewModel)
        {
            _settingsService.Add(viewModel.ThemePickerSettings.Name,
                viewModel.ThemePickerSettings.RuleType,
                viewModel.ThemePickerSettings.Criterion,
                viewModel.ThemePickerSettings.Theme,
                viewModel.ThemePickerSettings.Priority,
                viewModel.ThemePickerSettings.Zone ?? string.Empty,
                viewModel.ThemePickerSettings.Position ?? string.Empty);
        }
    }
}