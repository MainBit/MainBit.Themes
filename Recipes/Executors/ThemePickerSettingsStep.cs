using System;
using System.Collections.Generic;
using Orchard.Data;
using Orchard.Logging;
using Orchard.Recipes.Models;
using Orchard.Recipes.Services;
using Vandelay.Industries.Models;

namespace MainBit.Themes.Recipes.Executors {
    public class ThemePickerSettingsStep : RecipeExecutionStep {
        private readonly IRepository<ThemePickerSettingsRecord> _repository;

        public ThemePickerSettingsStep(
            IRepository<ThemePickerSettingsRecord> repository,
            RecipeExecutionLogger logger) : base(logger) {
            _repository = repository;
        }

        public override string Name {
            get { return "ThemePickerSettings"; }
        }

        public override void Execute(RecipeExecutionContext context) {
            foreach (var xmlRecord in context.RecipeStep.Step.Elements()) {
                var name = xmlRecord.Attribute("Name").Value;
                Logger.Information("Importing theme picker settings '{0}'.", name);

                try {
                    var record = GetThemePickerSettingsRecord(name);
                    record.RuleType = xmlRecord.Attribute("RuleType").Value;
                    record.Criterion = xmlRecord.Attribute("Criterion").Value;
                    record.Theme = xmlRecord.Attribute("Theme").Value;
                    record.Priority = int.Parse(xmlRecord.Attribute("Priority").Value);
                    record.Zone = xmlRecord.Attribute("Zone").Value;
                    record.Position = xmlRecord.Attribute("Position").Value;
                    UpdateOrCreateThemePickerSettingsRecord(record);
                }
                catch (Exception ex) {
                    Logger.Error(ex, "Error while importing theme picker settings '{0}'.", name);
                    throw;
                }
            }
        }

        private ThemePickerSettingsRecord GetThemePickerSettingsRecord(string name) {
            var record = _repository.Get(x => x.Name == name);

            if (record == null) {
                record = new ThemePickerSettingsRecord {
                    Name = name
                };
            }

            return record;
        }

        private void UpdateOrCreateThemePickerSettingsRecord(ThemePickerSettingsRecord record)
        {
            if (record.Id > 0)
                _repository.Update(record);
            else
                _repository.Create(record);
        }
    }
}
