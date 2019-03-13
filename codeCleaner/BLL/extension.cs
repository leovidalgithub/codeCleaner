using System;

namespace codeCleaner.ExtensionMethods {
    public static class DateExtensions {
        /// <summary>
        /// DateTime extension method.
        /// It removes Milliseconds in order to compare DateTimes
        /// </summary>
        public static DateTime TrimMilliseconds(this DateTime dt) {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
        }
    }
}
