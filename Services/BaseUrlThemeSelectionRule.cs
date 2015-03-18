using System.Text.RegularExpressions;
using Orchard;
using Orchard.Environment.Extensions;
using Vandelay.Industries.Services;
using MainBit.Themes.Extensions;

namespace MainBit.Themes.Services {
    [OrchardFeature("MainBit.Themes.BaseUrlSelectonRule")]
    public class BaseUrlThemeSelectionRule : IThemeSelectionRule {
        private readonly IWorkContextAccessor _workContextAccessor;

        public BaseUrlThemeSelectionRule(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public bool Matches(string name, string criterion) {
            var baseUrl = _workContextAccessor.GetContext().HttpContext.Request.GetBaseUrl();
            return new Regex(criterion, RegexOptions.IgnoreCase)
                .IsMatch(baseUrl);
        }
    }
}