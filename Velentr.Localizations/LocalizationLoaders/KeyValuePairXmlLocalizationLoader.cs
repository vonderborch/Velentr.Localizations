using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;

namespace Velentr.Localizations.LocalizationLoaders
{
    /// <summary>
    /// Loads an XML document in Key/Value pair format of:
    /// 
    /// <Localization>
    ///    <Text Name="key_here">Value</Text>
    /// </Localization>
    ///
    /// The various node keys can be customized in the initializer:
    /// - Localization can be changed through the rootString parameter
    /// - Text can be changed through the rowString parameter
    /// - Name can be changed through the keyString parameter
    /// </summary>
    /// <seealso cref="Velentr.Localizations.LocalizationLoaders.LocalizationLoader" />
    public class KeyValuePairXmlLocalizationLoader : LocalizationLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuePairXmlLocalizationLoader"/> class.
        /// </summary>
        /// <param name="rootString">The root string.</param>
        /// <param name="rowString">The row string.</param>
        /// <param name="keyString">The key string.</param>
        public KeyValuePairXmlLocalizationLoader(string rootString = "Localization", string rowString = "Text", string keyString = "Name")
        {
            RootString = rootString;
            RowString = rowString;
            KeyString = keyString;
        }

        /// <summary>
        /// Gets or sets the root string.
        /// </summary>
        /// <value>
        /// The root string.
        /// </value>
        public string RootString { get; set; }

        /// <summary>
        /// Gets or sets the row string.
        /// </summary>
        /// <value>
        /// The row string.
        /// </value>
        public string RowString { get; set; }

        /// <summary>
        /// Gets or sets the key string.
        /// </summary>
        /// <value>
        /// The key string.
        /// </value>
        public string KeyString { get; set; }

        /// <summary>
        /// Loads the localization from already read file contents.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="conflictResolution">The conflict resolution mode. Defaults to raising an exception.</param>
        /// <returns>
        /// A dictionary of localizations in a key/value pair.
        /// </returns>
        public override Dictionary<string, string> LoadLocalizationFromFile(string fileContents, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            var doc = XDocument.Parse(fileContents);
            return InternalLoadLocalizations(doc, conflictResolution);
        }

        /// <summary>
        /// Loads the localization from a stream.
        /// </summary>
        /// <param name="fileStream">The stream.</param>
        /// <param name="conflictResolution">The conflict resolution mode. Defaults to raising an exception.</param>
        /// <returns>
        /// A dictionary of localizations in a key/value pair.
        /// </returns>
        public override Dictionary<string, string> LoadLocalizationFromFile(Stream fileStream, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            var doc = XDocument.Load(fileStream);
            return InternalLoadLocalizations(doc, conflictResolution);
        }

        /// <summary>
        /// Loads the localization from a file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="conflictResolution">The conflict resolution mode. Defaults to raising an exception.</param>
        /// <returns>
        /// A dictionary of localizations in a key/value pair.
        /// </returns>
        public override Dictionary<string, string> LoadLocalizationFromFilePath(string filePath, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            var doc = XDocument.Load(filePath);
            return InternalLoadLocalizations(doc, conflictResolution);
        }

        /// <summary>
        /// Loads the localizations.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="conflictResolution">The conflict resolution.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to load XML document!</exception>
        /// <exception cref="DuplicateNameException">A localization with the key [{key}] already exists!</exception>
        private Dictionary<string, string> InternalLoadLocalizations(XDocument doc, ConflictResolution conflictResolution)
        {
            var localizations = new Dictionary<string, string>();

            if (doc.Root == null)
            {
                throw new Exception("Unable to load XML document!");
            }

            var rawLocalizations = doc.Root.Elements(RowString);
            foreach (var row in rawLocalizations)
            {
                if (row.Name != RowString)
                {
                    continue;
                }

                var key = (string) row.Attribute(KeyString);
                if (!string.IsNullOrWhiteSpace(key))
                {
                    switch (conflictResolution)
                    {
                        case ConflictResolution.Override:
                            localizations[key] = row.Value;
                            break;
                        case ConflictResolution.RaiseException:
                        case ConflictResolution.Skip:
                            if (localizations.ContainsKey(key))
                            {
                                if (conflictResolution == ConflictResolution.RaiseException)
                                {
                                    throw new DuplicateNameException($"A localization with the key [{key}] already exists!");
                                }
                                else
                                {
                                    break;
                                }
                            }

                            localizations[key] = row.Value;
                            break;
                    }
                }
            }

            return localizations;
        }
    }
}
