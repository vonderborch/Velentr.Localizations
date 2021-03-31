using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Velentr.Localizations.LocalizationLoaders;

namespace Velentr.Localizations
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizationSystem
    {

        /// <summary>
        /// The locales
        /// </summary>
        private Dictionary<string, Language> _locales;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationSystem"/> class.
        /// </summary>
        /// <param name="defaultLocalization">The default localization.</param>
        /// <param name="loader">The loader.</param>
        public LocalizationSystem(string defaultLocalization, LocalizationLoader loader = null)
        {
            _locales = new Dictionary<string, Language>();
            DefaultLocalization = defaultLocalization;
            CurrentLocalization = defaultLocalization;
            LocalizationLoader = loader;

            AddLanguage(defaultLocalization);
        }

        /// <summary>
        /// Gets or sets the default localization.
        /// </summary>
        /// <value>
        /// The default localization.
        /// </value>
        public string DefaultLocalization { get; set; }

        /// <summary>
        /// Gets or sets the current localization.
        /// </summary>
        /// <value>
        /// The current localization.
        /// </value>
        public string CurrentLocalization { get; set; }

        /// <summary>
        /// Gets or sets the localization loader.
        /// </summary>
        /// <value>
        /// The localization loader.
        /// </value>
        public LocalizationLoader LocalizationLoader { get; set; }

        /// <summary>
        /// Adds a new language.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <param name="defaultLocale">The default locale. Defaults to the DefaultLocale.</param>
        /// <param name="conflictResolution">The conflict resolution.</param>
        /// <returns>Whether we were able to successfully add the locale.</returns>
        /// <exception cref="DuplicateNameException">The locale [{locale}] already exists!</exception>
        public bool AddLanguage(string locale, string defaultLocale = null, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            if (_locales.ContainsKey(locale))
            {
                switch (conflictResolution)
                {
                    case ConflictResolution.Override:
                        _locales.Remove(locale);
                        break;
                    case ConflictResolution.RaiseException:
                        throw new DuplicateNameException($"The locale [{locale}] already exists!");
                    case ConflictResolution.Skip:
                        return false;
                }
            }

            _locales.Add(locale, new Language(this, locale, DefaultLocalization == locale ? null : (defaultLocale ?? DefaultLocalization)));
            return true;
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <param name="loadMode">The load mode. Defaults to LocalizationLoadMode.Truncate.</param>
        /// <param name="conflictResolution">The conflict resolution. Defaults to ConflictResolution.RaiseException.</param>
        /// <exception cref="KeyNotFoundException">The locale [{actualLocale}] does not exist!</exception>
        public void LoadLocalization(string fileContents, string locale = null, bool autoAddLocale = false, LocalizationLoadMode loadMode = LocalizationLoadMode.Truncate, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            if (LocalizationLoader == null)
            {
                throw new Exception("Localization loader is not configured! Please create a localization loader when initializing the LocalizationSystem or attach it to the LocalizationSystem.Localization property.");
            }

            // let's make sure we have a locale to load to...
            var actualLocale = GetAndCheckActualLocale(locale, !autoAddLocale);
            if (!actualLocale.Item2 && autoAddLocale)
            {
                AddLanguage(actualLocale.Item1, DefaultLocalization, ConflictResolution.Skip);
            }

            // run the loader against the file contents
            var localizations = LocalizationLoader.LoadLocalizationFromFile(fileContents);

            // assign the loaded localizations as required
            AddLocalizations(localizations, actualLocale.Item1, loadMode, conflictResolution);
        }

        /// <summary>
        /// Loads the localization from file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <param name="loadMode">The load mode. Defaults to LocalizationLoadMode.Truncate.</param>
        /// <param name="conflictResolution">The conflict resolution. Defaults to ConflictResolution.RaiseException.</param>
        /// <exception cref="KeyNotFoundException">The locale [{actualLocale}] does not exist!</exception>
        public void LoadLocalizationFromFilePath(string filePath, string locale = null, bool autoAddLocale = false, LocalizationLoadMode loadMode = LocalizationLoadMode.Truncate, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            if (LocalizationLoader == null)
            {
                throw new Exception("Localization loader is not configured! Please create a localization loader when initializing the LocalizationSystem or attach it to the LocalizationSystem.Localization property.");
            }

            // let's make sure we have a locale to load to...
            var actualLocale = GetAndCheckActualLocale(locale, !autoAddLocale);
            if (!actualLocale.Item2 && autoAddLocale)
            {
                AddLanguage(actualLocale.Item1, DefaultLocalization, ConflictResolution.Skip);
            }

            // run the loader against the file contents
            var localizations = LocalizationLoader.LoadLocalizationFromFilePath(filePath);

            // assign the loaded localizations as required
            AddLocalizations(localizations, actualLocale.Item1, loadMode, conflictResolution);
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <param name="loadMode">The load mode. Defaults to LocalizationLoadMode.Truncate.</param>
        /// <param name="conflictResolution">The conflict resolution. Defaults to ConflictResolution.RaiseException.</param>
        /// <exception cref="KeyNotFoundException">The locale [{actualLocale}] does not exist!</exception>
        public void LoadLocalization(Stream file, string locale = null, bool autoAddLocale = false, LocalizationLoadMode loadMode = LocalizationLoadMode.Truncate, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            if (LocalizationLoader == null)
            {
                throw new Exception("Localization loader is not configured! Please create a localization loader when initializing the LocalizationSystem or attach it to the LocalizationSystem.Localization property.");
            }

            // let's make sure we have a locale to load to...
            var actualLocale = GetAndCheckActualLocale(locale, !autoAddLocale);
            if (!actualLocale.Item2 && autoAddLocale)
            {
                AddLanguage(actualLocale.Item1, DefaultLocalization, ConflictResolution.Skip);
            }

            // run the loader against the file contents
            var localizations = LocalizationLoader.LoadLocalizationFromFile(file);

            // assign the loaded localizations as required
            AddLocalizations(localizations, actualLocale.Item1, loadMode, conflictResolution);
        }

        /// <summary>
        /// Adds the localizations.
        /// </summary>
        /// <param name="localizations">The localizations.</param>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <param name="loadMode">The load mode. Defaults to LocalizationLoadMode.Truncate.</param>
        /// <param name="conflictResolution">The conflict resolution. Defaults to ConflictResolution.RaiseException.</param>
        /// <exception cref="KeyNotFoundException">The locale [{actualLocale}] does not exist!</exception>
        public void AddLocalizations(Dictionary<string, string> localizations, string locale = null, LocalizationLoadMode loadMode = LocalizationLoadMode.Truncate, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            // let's make sure we have a locale to load to...
            var actualLocale = GetAndCheckActualLocale(locale);

            // assign the loaded localizations as required
            _locales[actualLocale.Item1].AddLocalizations(localizations, loadMode, conflictResolution);
        }

        /// <summary>
        /// Gets the and check actual locale.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <param name="raiseException">if set to <c>true</c> [raise exception].</param>
        /// <returns>Item1 -> the actual locale, Item2 -> whether the locale exists</returns>
        /// <exception cref="KeyNotFoundException">The locale [{actualLocale}] does not exist!</exception>
        private (string, bool) GetAndCheckActualLocale(string locale, bool raiseException = true)
        {
            var actualLocale = locale ?? CurrentLocalization;
            var contained = _locales.ContainsKey(actualLocale);
            if (!contained && raiseException)
            {
                throw new KeyNotFoundException($"The locale [{actualLocale}] does not exist!");
            }

            return (actualLocale, contained);
        }

        /// <summary>
        /// Adds the localization.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="localizedText">The localized text.</param>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <param name="conflictResolution">The conflict resolution.</param>
        /// <exception cref="KeyNotFoundException">The locale [{actualLocale}] does not exist!</exception>
        public void AddLocalization(string key, string localizedText, string locale = null, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            var actualLocale = GetAndCheckActualLocale(locale);

            _locales[actualLocale.Item1].AddLocalization(key, localizedText, conflictResolution);
        }

        /// <summary>
        /// Gets the localization.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <returns></returns>
        public string GetLocalization(string key, string locale = null)
        {
            var actualLocale = GetAndCheckActualLocale(locale);

            return _locales.TryGetValue(actualLocale.Item1, out var language) ? language.GetLocalization(key) : key;
        }

        /// <summary>
        /// Determines whether the specified locale exists in the system.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>
        ///   <c>true</c> if the specified locale exists in the system; otherwise, <c>false</c>.
        /// </returns>
        public bool HasLocale(string locale)
        {
            return _locales.ContainsKey(locale);
        }

        /// <summary>
        /// Gets a new localization fetcher.
        /// </summary>
        /// <param name="locale">The locale. Defaults to the CurrentLocale.</param>
        /// <param name="prependedLocalizationKey">The prepended localization key. Defaults to an empty string.</param>
        /// <returns>The localization fetcher.</returns>
        public LocalizationFetcher GetLocalizationFetcher(string locale = null, string prependedLocalizationKey = "")
        {
            var actualLocale = GetAndCheckActualLocale(locale);

            return new LocalizationFetcher(this, actualLocale.Item1, prependedLocalizationKey);
        }
    }
}
