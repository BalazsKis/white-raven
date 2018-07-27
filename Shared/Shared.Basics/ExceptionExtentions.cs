using System;

namespace WhiteRaven.Shared.Basics
{
    public static class ExceptionExtentions
    {
        public static string ToShortStringWithInnerExceptions(this Exception ex)
        {
            var inner = ex.InnerException != null
                ? $" (inner exception: {ToShortStringWithInnerExceptions(ex.InnerException)})"
                : string.Empty;

            return $"{ex.GetType().Name}: {ex.Message}{inner}";
        }

        public static string ToMessageWithInnerExceptions(this Exception ex)
        {
            var inner = ex.InnerException != null
                ? $" (inner exception: {ToShortStringWithInnerExceptions(ex.InnerException)})"
                : string.Empty;

            return $"{ex.Message}{inner}";
        }
    }
}