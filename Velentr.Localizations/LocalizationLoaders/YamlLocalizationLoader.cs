using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace Velentr.Localizations.LocalizationLoaders
{
    public class YamlLocalizationLoader : LocalizationLoader
    {

        public const string StringType = "String";

        public override Dictionary<string, string> LoadLocalizationFromFile(string fileContents, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            return InternalLoadLocalization(fileContents, conflictResolution);
        }

        public override Dictionary<string, string> LoadLocalizationFromFilePath(string filePath, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            var fileContents = File.ReadAllText(filePath);

            return InternalLoadLocalization(fileContents, conflictResolution);
        }

        public override Dictionary<string, string> LoadLocalizationFromFile(Stream fileStream, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            var lines = new StringBuilder();
            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    lines.AppendLine(reader.ReadLine());
                }
            }

            return InternalLoadLocalization(lines.ToString(), conflictResolution);
        }

        private Dictionary<string, string> InternalLoadLocalization(string fileContents, ConflictResolution conflictResolution)
        {
            var deserializer = new Deserializer();
            var deserializedLocalizations = deserializer.Deserialize<Dictionary<string, object>>(fileContents);

            return ParseDeserializedYaml(deserializedLocalizations, string.Empty, conflictResolution);
        }

        private Dictionary<string, string> ParseDeserializedYaml(Dictionary<string, object> deserializedLocalizations, string keyPath, ConflictResolution conflictResolution)
        {
            var localizations = new Dictionary<string, string>();

            foreach (var deserializedLocalization in deserializedLocalizations)
            {
                var type1 = deserializedLocalization.Value.GetType();

                if (type1.Name == StringType)
                {
                    localizations.Add($"{keyPath}{deserializedLocalization.Key}", deserializedLocalization.Value.ToString());
                }
                else
                {
                    var locs = ParseDeserializedYaml(ObjectToDictionary<string, object>(deserializedLocalization.Value), $"{keyPath}{deserializedLocalization.Key}.", conflictResolution);
                    foreach (var loc in locs)
                    {
                        localizations.Add(loc.Key, loc.Value);
                    }
                }
            }

            return localizations;
        }

        private static Dictionary<T, V> ObjectToDictionary<T, V>(object obj)
        {
            var dicCurrent = new Dictionary<T, V>();
            foreach (DictionaryEntry dicData in (obj as IDictionary))
            {
                dicCurrent.Add((T)dicData.Key, (V)dicData.Value);
            }
            return dicCurrent;
        }
    }
}
