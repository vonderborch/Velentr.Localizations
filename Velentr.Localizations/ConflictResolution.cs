using System;
using System.Collections.Generic;
using System.Text;

namespace Velentr.Localizations
{
    /// <summary>
    /// Defines the various means with which we can handle conflict resolutions
    /// </summary>
    public enum ConflictResolution
    {
        /// <summary>
        /// When a conflict occurs, raise an exception.
        /// </summary>
        RaiseException,

        /// <summary>
        /// When a conflict occurs, override the existing value.
        /// </summary>
        Override,

        /// <summary>
        /// When a conflict occurs, move on
        /// </summary>
        Skip,
    }
}
