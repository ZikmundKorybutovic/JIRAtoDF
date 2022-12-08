using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIRAtoDF
{
    static class Utils
    {
        private static readonly List<string> relevantStatuses = new List<string>() { "analysis completed", "pending", "rejected" };
        public static bool IsStatusRelevant(string status) => relevantStatuses.Contains(status.ToLower());
        public static readonly bool Debugging = false;

    }
}
