using System;
using System.Collections.Generic;
using System.Text;

namespace Velentr.Localizations
{
    /// <summary>
    /// Defines the various means with which we can load localizations
    /// </summary>
    public enum LocalizationLoadMode
    {
        /// <summary>
        /// Truncate the existing language's localization
        /// </summary>
        Truncate,

        /// <summary>
        /// Append to the existing localization for the language
        /// </summary>
        Append,
    }
}
