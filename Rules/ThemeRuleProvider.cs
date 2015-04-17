using System;
using System.Globalization;
using System.Linq;
using Orchard.Events;
using Orchard;

namespace MainBit.Themes.RuleEngine
{
    public interface IRuleProvider : IEventHandler {
        void Process(dynamic ruleContext);
    }

    public class ThemeRuleProvider : IRuleProvider {
        private readonly IWorkContextAccessor _wca;

        public ThemeRuleProvider(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        public void Process(dynamic ruleContext) {
            if (String.Equals(ruleContext.FunctionName, "theme", StringComparison.OrdinalIgnoreCase)) {
                ProcessTheme(ruleContext);
            }
        }

        private void ProcessTheme(dynamic ruleContext)
        {
            var currentTheme = _wca.GetContext().CurrentTheme;
            ruleContext.Result = ((object[])ruleContext.Arguments)
                .Cast<string>()
                .Any(t => string.Equals(t, currentTheme.Id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}