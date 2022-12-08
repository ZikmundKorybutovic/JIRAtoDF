using System;

namespace JIRAtoDF
{
    public static class Logger
    {
        public static void DebugLog(string text)
        {
            if (Utils.Debugging)
            {
                Console.WriteLine(text);
            }
        }

        public static void DebugLog(bool text)
        {
            if (Utils.Debugging)
            {
                Console.WriteLine(text);
            }
        }

        public static void DebugLog(int text)
        {
            if (Utils.Debugging)
            {
                Console.WriteLine(text);
            }
        }

        public static void Log(string text)
        {
           
            Console.WriteLine(text);
          
        }

        public static void Log(bool text)
        {
            Console.WriteLine(text);
        }

        public static void Log(int text)
        {
            Console.WriteLine(text);
        }
    }
}
