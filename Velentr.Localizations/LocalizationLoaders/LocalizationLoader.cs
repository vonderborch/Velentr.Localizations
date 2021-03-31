using System.Collections.Generic;
using System.IO;

namespace Velentr.Localizations.LocalizationLoaders
{
    /// <summary>
    /// Defines a loader for localizations
    /// </summary>
    public abstract class LocalizationLoader
    {

        /// <summary>
        /// Loads the localization from already read file contents.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="conflictResolution">The conflict resolution mode. Defaults to raising an exception.</param>
        /// <returns>A dictionary of localizations in a key/value pair.</returns>
        public abstract Dictionary<string, string> LoadLocalizationFromFile(string fileContents, ConflictResolution conflictResolution = ConflictResolution.RaiseException);

        /// <summary>
        /// Loads the localization from a file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="conflictResolution">The conflict resolution mode. Defaults to raising an exception.</param>
        /// <returns>A dictionary of localizations in a key/value pair.</returns>
        public abstract Dictionary<string, string> LoadLocalizationFromFilePath(string filePath, ConflictResolution conflictResolution = ConflictResolution.RaiseException);

        /// <summary>
        /// Loads the localization from a stream.
        /// </summary>
        /// <param name="fileStream">The stream.</param>
        /// <param name="conflictResolution">The conflict resolution mode. Defaults to raising an exception.</param>
        /// <returns>A dictionary of localizations in a key/value pair.</returns>
        public abstract Dictionary<string, string> LoadLocalizationFromFile(Stream fileStream, ConflictResolution conflictResolution = ConflictResolution.RaiseException);

    }
}
