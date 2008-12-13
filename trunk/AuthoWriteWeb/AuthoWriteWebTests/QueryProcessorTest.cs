using AuthoWriteWeb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Specialized;

namespace AuthoWriteWebTests
{
    
    
    /// <summary>
    ///This is a test class for QueryProcessorTest and is intended
    ///to contain all QueryProcessorTest Unit Tests
    ///</summary>
    [TestClass]
    public class QueryProcessorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SearchTerms
        ///</summary>
        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\Lenworth\\Documents\\Visual Studio 2008\\Projects\\AuthoWriteWeb\\AuthoWriteWeb", "/")]
        [UrlToTest("http://localhost:53459/")]
        public void SearchTermsTest()
        {
            string inputText = string.Empty; // TODO: Initialize to an appropriate value
            QueryProcessor target = new QueryProcessor(inputText); // TODO: Initialize to an appropriate value
            StringDictionary expected = null; // TODO: Initialize to an appropriate value
            StringDictionary actual;
            target.SearchTerms = expected;
            actual = target.SearchTerms;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Strip
        ///</summary>
        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\Lenworth\\Documents\\Visual Studio 2008\\Projects\\AuthoWriteWeb\\AuthoWriteWeb", "/")]
        [UrlToTest("http://localhost:53459/")]
        public void StripTest()
        {
            string inputText = string.Empty; // TODO: Initialize to an appropriate value
            QueryProcessor target = new QueryProcessor(inputText); // TODO: Initialize to an appropriate value
            string text = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Strip(text);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for QueryProcessor Constructor
        ///</summary>
        [TestMethod]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\Lenworth\\Documents\\Visual Studio 2008\\Projects\\AuthoWriteWeb\\AuthoWriteWeb", "/")]
        [UrlToTest("http://localhost:53459/")]
        public void QueryProcessorConstructorTest()
        {
            string inputText = string.Empty; // TODO: Initialize to an appropriate value
            QueryProcessor target = new QueryProcessor(inputText);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
