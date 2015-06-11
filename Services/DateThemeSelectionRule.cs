using System.Text.RegularExpressions;
using Orchard;
using Orchard.Environment.Extensions;
using Vandelay.Industries.Services;
using MainBit.Themes.Extensions;
using Orchard.Services;
using Orchard.Localization.Services;
using System;
using System.Linq;

namespace MainBit.Themes.Services {
    [OrchardFeature("MainBit.Themes.DateSelectionRule")]
    public class DateThemeSelectionRule : IThemeSelectionRule {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IClock _clock;
        private readonly IDateLocalizationServices _dateLocalizationServices;

        public DateThemeSelectionRule(IWorkContextAccessor workContextAccessor,
            IClock clock,
            IDateLocalizationServices dateLocalizationServices)
        {
            _workContextAccessor = workContextAccessor;
            _clock = clock;
            _dateLocalizationServices = dateLocalizationServices;
        }

        public bool Matches(string name, string criterion) {

            var criterionSegments = criterion
                .Split(new char[] { '-', '.', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            var utcNow = _clock.UtcNow;
            var dateTime = new DateTime(DateTime.MinValue.Year, utcNow.Month, utcNow.Day);
            var dateTimeLocal = _dateLocalizationServices.ConvertToSiteTimeZone(dateTime);
            var from = new DateTime(DateTime.MinValue.Year, criterionSegments[1], criterionSegments[0]);
            var to = new DateTime(DateTime.MinValue.Year, criterionSegments[3], criterionSegments[2]);

            return IsIn(dateTime, from, to);
        }

        private bool IsIn(DateTime dateTime, DateTime from, DateTime to)
        {
            if (from <= to)
            {
                return from <= dateTime && dateTime <= to;
            }
            else
            {
                return from <= dateTime || dateTime <= to;
            }
        }
    }
}