using ServiceModels.Abstractions;
using System;
using System.Collections.Generic;

namespace Ingestion.Api.Handlers
{
    /// <summary>
    /// Provides a comparer for <see cref="BaseSignal"/> objects based on their DateTime property.
    /// </summary>
    public class SignalDateTimeComparer : IComparer<BaseSignal>
    {
        /// <summary>
        /// Compares two <see cref="BaseSignal"/> objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="BaseSignal"/> object to compare.</param>
        /// <param name="y">The second <see cref="BaseSignal"/> object to compare.</param>
        /// <returns>An integer that indicates the relative order of the objects being compared.</returns>
        public int Compare(BaseSignal? x, BaseSignal? y)
        {
            if (x != null && y != null)
            {
                return DateTime.Compare(x.DateTimeUtc, y.DateTimeUtc);
            }

            if (x == null && y == null)
            {
                return 0;
            }

            return x == null ? -1 : 1;
        }
    }
}

