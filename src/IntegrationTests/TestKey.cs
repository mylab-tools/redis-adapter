using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IntegrationTests
{
    static class TestKey
    {
        public static string New(MethodBase m)
        {
            var tn = m.DeclaringType.FullName;
            int lastPos = tn.LastIndexOf('.');
            int plusPos = tn.IndexOf('+');
            int lessPos = tn.IndexOf('<');
            int gretPos = tn.IndexOf('>');

            var testClassName = tn.Substring(lastPos+1, plusPos - lastPos-1);
            var testMethodName = tn.Substring(lessPos+1, gretPos - lessPos-1);

            return $"tests:{testClassName}:{testMethodName}";
        }
    }
}
