using System.Linq;
using System.Xml.Linq;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Recipes.Services;
using Vandelay.Industries.Models;

namespace MainBit.Themes.Recipes.Builders {
    public class ThemePickerSettingsStep : RecipeBuilderStep {
        private readonly IRepository<ThemePickerSettingsRecord> _repository;

        public ThemePickerSettingsStep(IRepository<ThemePickerSettingsRecord> repository) {
            _repository = repository;
        }

        public override string Name {
            get { return "ThemePickerSettings"; }
        }

        public override LocalizedString DisplayName {
            get { return T("Theme Picker Settings"); }
        }

        public override LocalizedString Description {
            get { return T("Exports theme picker settings."); }
        }

        public override void Build(BuildContext context) {
            var records = _repository.Table.ToList();

            if (!records.Any())
                return;

            var root = new XElement("ThemePickerSettings");
            context.RecipeDocument.Element("Orchard").Add(root);

            foreach (var record in records) {
                root.Add(new XElement("Record",
                    new XAttribute("Name", record.Name),
                    new XAttribute("RuleType", record.RuleType),
                    new XAttribute("Criterion", record.Criterion),
                    new XAttribute("Theme", record.Theme),
                    new XAttribute("Priority", record.Priority),
                    new XAttribute("Zone", record.Zone ?? ""),
                    new XAttribute("Position", record.Position ?? "")));
            }
        }
    }
}

