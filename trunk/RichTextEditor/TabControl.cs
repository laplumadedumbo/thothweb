// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AjaxControls
{
    internal class Tab : WebControl , INamingContainer
    {
        private LinkButton aElement;
        protected TabStrip parent;
      

        /// <summary>
        ///  Text to be display inside the Tab
        /// </summary>
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        ///  Script Callback Function to Invoke on Click
        /// </summary>
        public string OnClientClick
        {
            get
            {
                String s = (String)ViewState["OnClientClick"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["OnClientClick"] = value;
            }
        }

        /// <summary>
        ///  Property to get / set Active state of the Tab. If this is true the Tab is Active.
        /// </summary>
        public bool Active
        {
            get
            {
                bool ia = (bool)ViewState["IsActive"];
                return ia;
            }
            set
            {
                ViewState["IsActive"] = value;
            }
        }

        public Tab(TabStrip owner)
        {
            parent = owner;
        }


        /// <summary>
        ///  Check to see if a Property is set on the Server Control Tag in the calling page.
        /// </summary>
        /// <param name="key">Property key name</param>
        /// <returns>True if this propery is present</returns>
        internal bool IsSet(string key)
        {
            return ViewState[key] != null;
        }

        /// <summary>
        ///  On initializing the Control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            aElement = new LinkButton();
            
            bool isActive = IsSet("IsActive") ? this.Active : false;

            if (isActive)
                aElement.CssClass = "ActiveTab";
            else
                aElement.CssClass = "InActiveTab";
        }


        /// <summary>
        ///  Render the Control as HTML
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderContents(writer);
            RenderEndTag(writer);
        }

        /// <summary>
        ///  Start HTML Tag for the Tab.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Li; }
        }

        /// <summary>
        ///  Render the Start tag for the HTML.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (TagKey != HtmlTextWriterTag.Unknown)
                writer.RenderBeginTag(TagKey);
            else
                writer.RenderBeginTag(this.TagName);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            aElement.ID = this.ClientID;

            Type rstype = this.GetType();

            bool isActive = IsSet("IsActive") ? this.Active : false;

            StringBuilder ScriptBuilder = new StringBuilder();
            
            ScriptBuilder.Append("var " + this.ClientID + " = new Microsoft.js.ui.Tab(" + this.parent.ClientID + ",'" + this.ClientID+ "','" + this.Text + "','" + isActive.ToString() + "');");
            ScriptBuilder.Append(parent.ClientID + ".AddTab(" + this.ClientID + ");");
            ScriptBuilder.Append("this." + this.ClientID + "Click = " + "new Microsoft.js.event.EventHandler(" + this.ClientID + ".uiElement,'click','" + this.OnClientClick + "',this,false);");
            ScriptBuilder.Append("Microsoft.js.event.EventManager.add(this." + this.ClientID + "Click);");

            Global.Instance.Resource.RegisterClientImmediateScript(this, this.ClientID, ScriptBuilder.ToString());

        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            try
            {
                aElement.RenderBeginTag(writer); //<a>

                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Span); // <span>

                writer.RenderBeginTag(HtmlTextWriterTag.Em); // <em>

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "tabTxt");
                writer.RenderBeginTag(HtmlTextWriterTag.Span); // <span>

                writer.Write(this.Text);

                writer.RenderEndTag(); // </span>
                writer.RenderEndTag(); // </em>
                writer.RenderEndTag(); // </span>
            
                aElement.RenderEndTag(writer);
            }
            catch (System.NullReferenceException e)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write(e.StackTrace + "<br/>" + e.Message);
            }
        }
    }
}
