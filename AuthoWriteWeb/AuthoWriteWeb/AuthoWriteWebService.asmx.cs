﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Web.Script.Services;
using log4net;

namespace AuthoWriteWeb
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://www.nefertitiware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class AuthoWriteWebService : WebService
    {


        [WebMethod]
        public string StructuredQuery(string inputText)
        {
            var sb = new StringBuilder();


            try
            {
                var qp = new QueryProcessor(inputText);

                sb.Append(@"<ul>");

                foreach (string searchTerm in qp.SearchTerms)
                {
                    sb.Append(@"<li>" + searchTerm + @"</li>");
                }

                sb.Append(@"</ul>");
            }
            catch (Exception ex)
            {
                ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                Log.Error(ex.StackTrace);
            }
            return sb.ToString();
        }
        [WebMethod]
        public string Fix()
        {
            return "Sad";
        }
    }
}
