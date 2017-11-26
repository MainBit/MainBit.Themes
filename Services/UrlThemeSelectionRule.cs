using System.Text.RegularExpressions;
using Orchard;
using Orchard.Environment.Extensions;
using Vandelay.Industries.Services;
using MainBit.Themes.Extensions;

namespace MainBit.Themes.Services {
    [OrchardFeature("MainBit.Themes.UrlSelectonRule")]
    public class UrlThemeSelectionRule : IThemeSelectionRule {
        private readonly IWorkContextAccessor _workContextAccessor;

        public UrlThemeSelectionRule(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public bool Matches(string name, string criterion) {
            var url = _workContextAccessor.GetContext().HttpContext.Request.RawUrl;
            return new Regex(criterion, RegexOptions.IgnoreCase)
                .IsMatch(url);
        }
    }
}