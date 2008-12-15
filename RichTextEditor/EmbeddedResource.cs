// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

namespace AjaxControls
{
    /// <summary>
    ///  Helper that encapsulates encoding and registering client scripts for all the controls.
    /// </summary>
    public class EmbeddedResource
    {
        /// <summary>
        ///  Variable for immediate script block
        /// </summary>
        private StringBuilder immediateScriptBlock = new StringBuilder();
        
        /// <summary>
        ///  Given an embedded resource name get its html encoded url
        /// </summary>
        /// <param name="page">Requesting Page instance</param>
        /// <param name="resourceName">Requested Resource Name</param>
        /// <returns>Web Url string</returns>
        public string GetUrl(System.Web.UI.Page page,string resourceName)
        {
            DebugAssert(resourceName);
            return HttpUtility.HtmlEncode(page.ClientScript.GetWebResourceUrl(this.GetType(), resourceName));
        }

        /// <summary>
        ///  Given an embedded resource name Register it as a client script on the page.
        /// </summary>
        /// <param name="page">Requesting Page instance</param>
        /// <param name="resourceName">Requested Resource Name</param>
        public void RegisterClientScript(System.Web.UI.Page page, ControlCollection controls, string resourceName)
        {
            DebugAssert(resourceName);
            if (Global.Instance.Properties.DomMode)
            {
                Literal domScriptInclude = new Literal();
                domScriptInclude.Mode = LiteralMode.Transform;
                // It was reported that a single encode in GetUrl didn't resolve the issue with url not being encoded in this 
                // particular case. Need to followup with ASP.NET team on this and get it resolved.
                domScriptInclude.Text = "<script src=\"" + HttpUtility.HtmlEncode(Global.Instance.Resource.GetUrl(page, resourceName)) + "\" type=\"text/javascript\" ></script>";

                // add to header if possible; current control if not 
                // note: control includes must be prior to any html element that references them or client script error will result
                if (page.Header != null)
                {
                    page.Header.Controls.Add(domScriptInclude);
                }
                else
                {
                    if (controls != null)
                    {
                        controls.Add(domScriptInclude);
                    }
                }
            }
            else
            {
                page.ClientScript.RegisterClientScriptInclude(this.GetType(), resourceName, this.GetUrl(page, resourceName));
            }
        }

        
        public void RegisterClientImmediateScript(System.Web.UI.Control hostControl, string scriptToken, string scriptFragment)
        {
            if (Global.Instance.Properties.DomMode)
            {
                this.immediateScriptBlock.AppendLine(scriptFragment);
            }
            else
            {
                hostControl.Page.ClientScript.RegisterStartupScript(hostControl.GetType(), scriptToken, scriptFragment, true);
            }
        }

        public void RenderClientImmediateScript(HtmlTextWriter writer)
        {
            writer.Write("\n<script>" + immediateScriptBlock.ToString() + "</script>");
            immediateScriptBlock = new StringBuilder();
        }

        /// <summary>
        ///  Make sure that the resource name does exist in the DLL as embedded resource.
        ///  Do the assert only in Debug mode.
        /// </summary>
        /// <param name="resourceName">Requested Resource Name</param>
        public void DebugAssert(string resourceName)
        {
            Debug.Assert(null != this.GetType().Assembly.GetManifestResourceInfo(resourceName));
        }
    }
}
