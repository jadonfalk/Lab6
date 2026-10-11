using System;
using System.Diagnostics;

namespace Zork
{
    public static class Assert
    {
        // Calls to this method are included only in Debug builds.
        [Conditional("DEBUG")]
        public static void IsTrue(bool expression, string message = "")
        {
            if (!expression)
            {
                throw new Exception(message);
            }
        }
    }
}
