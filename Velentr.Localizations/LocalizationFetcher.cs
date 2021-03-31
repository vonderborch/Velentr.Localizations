using System;
using System.Collections.Generic;
using System.Text;

namespace Velentr.Localizations
{
    /// <summary>
    /// A helper class to make fetching localizations for a specific locale easier.
    /// </summary>
    public class LocalizationFetcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationFetcher"/> class.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="prependedLocalizationKey">The prepended localization key.</param>
        /// <exception cref="System.ArgumentException">The locale [{locale}] is not a valid locale for the LocalizationSystem!</exception>
        public LocalizationFetcher(LocalizationSystem system, string locale, string prependedLocalizationKey = "")
        {
            LocalizationSystem = system;
            Locale = locale;
            PrependedLocalizationKey = prependedLocalizationKey;

            if (!system.HasLocale(locale))
            {
                throw new ArgumentException($"The locale [{locale}] is not a valid locale for the LocalizationSystem!");
            }
        }

        /// <summary>
        ///     Gets the localization system.
        /// </summary>
        /// <value>
        ///     The localization system.
        /// </value>
        public LocalizationSystem LocalizationSystem { get; }

        /// <summary>
        ///     Gets the locale.
        /// </summary>
        /// <value>
        ///     The locale.
        /// </value>
        public string Locale { get; }

        /// <summary>
        ///     Gets the prepended localization key. This can be used to make fetching localization easier. For example:
        ///          - PrependedLocalizationKey: "addresses.home."
        ///          - If we're looking for a key such as "addresses.home.street", we can now just search for "street" when calling the GetLocalization method and the system will look for the "addresses.home.street" localized text.
        /// </summary>
        /// <value>
        ///     The prepended localization key.
        /// </value>
        public string PrependedLocalizationKey { get; }

        /// <summary>
        /// Gets the localization.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The localized text.</returns>
        public string GetLocalization(string key)
        {
            key = $"{PrependedLocalizationKey}{key}";
            return LocalizationSystem.GetLocalization(key, Locale);
        }
    }
}
