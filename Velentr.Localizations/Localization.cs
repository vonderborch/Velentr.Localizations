using System;
using System.Collections.Generic;
using System.Data;

namespace Velentr.Localizations
{

    /// <summary>
    ///     Defines a language (a set of localized text and the locale it is a part of)
    /// </summary>
    internal class Localization
    {

        /// <summary>
        ///     The cache
        /// </summary>
        private Dictionary<string, string> _cache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Localization" /> class.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="defaultLocale">The default locale.</param>
        /// <param name="enableMachineTranslation">Whether to enable machine translations.</param>
        internal Localization(LocalizationSystem system, string locale, string defaultLocale = null, bool enableMachineTranslation = false)
        {
            LocalizationSystem = system;
            Locale = locale;
            DefaultLocale = defaultLocale;
            MachineTranslationEnabled = enableMachineTranslation;

            _cache = new Dictionary<string, string>();

            if (enableMachineTranslation)
            {
                // validate that the language pairs are valid!
                if (!LocalizationSystem.Apertium.IsValidPair(locale, defaultLocale))
                {
                    throw new Exception($"Invalid language pair! Valid Pairs: {system.Apertium.GetValidPairsString()}");
                }
            }
        }

        /// <summary>
        ///     Gets the localization system.
        /// </summary>
        /// <value>
        ///     The localization system.
        /// </value>
        internal LocalizationSystem LocalizationSystem { get; }

        /// <summary>
        ///     Gets the locale.
        /// </summary>
        /// <value>
        ///     The locale.
        /// </value>
        internal string Locale { get; }

        /// <summary>
        ///     Gets the default locale.
        /// </summary>
        /// <value>
        ///     The default locale.
        /// </value>
        internal string DefaultLocale { get; }

        /// <summary>
        ///     Gets a value indicating whether the machine translation is enabled.
        /// </summary>
        ///
        /// <value>
        ///     True if machine translation enabled, false if not.
        /// </value>
        internal bool MachineTranslationEnabled { get; }

        /// <summary>
        ///     Clears the cache.
        /// </summary>
        internal void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        ///     Adds the localization.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="text">The text.</param>
        internal void AddLocalization(string key, string text)
        {
            _cache[key] = text;
        }

        /// <summary>
        ///     Adds the localizations.
        /// </summary>
        /// <param name="localizations">The localizations.</param>
        /// <param name="loadMode">The load mode.</param>
        /// <param name="conflictResolution">The conflict resolution.</param>
        /// <exception cref="DuplicateNameException">A localization already exists in [{Locale}] for the key [{localization.Key}]!</exception>
        internal void AddLocalizations(Dictionary<string, string> localizations, LocalizationLoadMode loadMode = LocalizationLoadMode.Truncate, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            if (loadMode == LocalizationLoadMode.Truncate)
            {
                ClearCache();
            }

            foreach (var localization in localizations)
            {
                switch (conflictResolution)
                {
                    case ConflictResolution.Override:
                        _cache[localization.Key] = localization.Value;
                        break;
                    case ConflictResolution.RaiseException:
                    case ConflictResolution.Skip:
                        if (_cache.ContainsKey(localization.Key))
                        {
                            if (conflictResolution == ConflictResolution.RaiseException)
                            {
                                throw new DuplicateNameException($"A localization already exists in [{Locale}] for the key [{localization.Key}]!");
                            }

                            break;
                        }

                        _cache[localization.Key] = localization.Value;
                        break;
                }
            }
        }

        /// <summary>
        ///     Adds the localization.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="text">The text.</param>
        /// <param name="conflictResolution">The conflict resolution.</param>
        /// <exception cref="DuplicateNameException">A localization already exists in [{Locale}] for the key [{key}]!</exception>
        internal void AddLocalization(string key, string text, ConflictResolution conflictResolution = ConflictResolution.RaiseException)
        {
            if (_cache.ContainsKey(key))
            {
                switch (conflictResolution)
                {
                    case ConflictResolution.RaiseException:
                        throw new DuplicateNameException($"A localization already exists in [{Locale}] for the key [{key}]!");
                    case ConflictResolution.Skip:
                        return;
                }
            }

            _cache[key] = text;
        }

        /// <summary>
        ///     Gets a localization.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The localization.</returns>
        internal string GetLocalization(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                return value;
            }

            if (MachineTranslationEnabled)
            {
                var defaultLocaleLocalization = LocalizationSystem.GetLocalization(key, DefaultLocale);
                var translation = LocalizationSystem.Apertium.Translate(defaultLocaleLocalization);
                _cache.Add(key, translation);
                return translation;
            }

            return LocalizationSystem.GetLocalization(key, DefaultLocale);
        }

    }

}
