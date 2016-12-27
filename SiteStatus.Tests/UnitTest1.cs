using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteStatus.Modules;
using System.Net;
using System.Threading;

namespace SiteStatus.Tests
{
    [TestClass]
    public class UnitTest1
    {     
        [TestMethod]
        public void TestCheck()
        {
            var check = new Checker("www.google.com");
            var autoEvent = new AutoResetEvent(false);                     
            check.StatusChecker(autoEvent);          

            bool result = TestChecker.TestSite();

            Assert.AreEqual(check.Status, result);
        }       
    }

    public class TestChecker
    {
        public static bool TestSite()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("www.google.com");
                request.Timeout = 3000;
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";

                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}   
