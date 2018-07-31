using System;

namespace WhiteRaven.Shared.Basics
{
    public static class ExceptionExtentions
    {
        /// <summary>
        /// Creates a human-readable message from the exception containing its type,
        /// message and inner exceptions (recursively)
        /// </summary>
        /// <param name="ex">The exception to convert</param>
        /// <returns>The exception converted to a readable message</returns>
        public static string ToShortStringWithInnerExceptions(this Exception ex)
        {
            var inner = ex.InnerException != null
                ? $" (inner exception: {ToShortStringWithInnerExceptions(ex.InnerException)})"
                : string.Empty;

            return $"{ex.GetType().Name}: {ex.Message}{inner}";
        }

        /// <summary>
        /// Creates a human-readable message from the exception containing its
        /// message and inner exceptions (recursively)
        /// </summary>
        /// <param name="ex">The exception to convert</param>
        /// <returns>The exception converted to a readable message</returns>
        public static string ToMessageWithInnerExceptions(this Exception ex)
        {
            var inner = ex.InnerException != null
                ? $" (inner exception: {ToShortStringWithInnerExceptions(ex.InnerException)})"
                : string.Empty;

            return $"{ex.Message}{inner}";
        }
    }
}