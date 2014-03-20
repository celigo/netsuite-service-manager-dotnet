/*
 * If the ServiceManager.dll has not arleady been built, the compiler will throw an error due to the VolatileAssembly
 * directive in the T4 templates. In which case Excelude the SuiteTalk folder and buld with FISRTBUILD directive.
 */

#if FIRSTBUILD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.celigo.net.ServiceManager.SuiteTalk
{
    public interface INetSuiteService
    {
        string Url { get; set; }

        System.Net.CookieContainer CookieContainer { get; set; }

        SessionResponse login(Passport passport);

        SessionResponse logout();

        INetSuiteService Clone();

        Passport passport { get; set; }

        int Timeout { get; set; }

        Preferences preferences { get; set; }

        SearchPreferences searchPreferences { get; set; }

        ApplicationInfo applicationInfo { get; set; }
    }

    public partial class NetSuiteService : INetSuiteService
    {
        public INetSuiteService Clone() { throw new NotImplementedException(); }
    }

    public static class Extensions
    {
        // Dummy methods

        public static SearchPreferences Duplicate(this SearchPreferences pref) { throw new NotImplementedException(); }

        public static string GetInternalId(this CustomFieldRef cfr){ throw new NotImplementedException(); }
    }
}
#endif
