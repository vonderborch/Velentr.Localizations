using System.Collections.Generic;
using System.Data;

namespace Velentr.Localizations
{

    /// <summary>
    ///     Defines a language (a set of localized text and the locale it is a part of)
    /// </summary>
    internal class Language
    {

        /// <summary>
        ///     The cache
        /// </summary>
        private readonly Dictionary<string, string> _cache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Language" /> class.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="defaultLocale">The default locale.</param>
        internal Language(LocalizationSystem system, string locale, string defaultLocale = null)
        {
            LocalizationSystem = system;
            Locale = locale;
            DefaultLocale = defaultLocale;

            _cache = new Dictionary<string, string>();
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

            return LocalizationSystem.GetLocalization(key, DefaultLocale);
        }

    }

}
