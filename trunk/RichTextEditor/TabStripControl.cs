// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI.HtmlControls;


namespace AjaxControls
{
    /// <summary>
    ///  ASP.NET 2.0 Custrom TabStrip (a container) control to hold Tabs.
    /// </summary>
    public class TabStrip : CompositeControl
    {
        #region Class Members
        /// <summary>
        ///  List of Tab Names
        /// </summary>
        private Collection<Pair> tabNames;
        #endregion

        #region Constructor
        /// <summary>
        /// TabStrip Constructor
        /// </summary>
        public TabStrip()
        {
            tabNames = new Collection<Pair>();
        }
        #endregion

        #region Public Methods

        /// <summary>
        ///  BackgroundImage url
        /// </summary>

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Localizable(true)]
        public string BackgroundImage
        {
            get
            {
                String s = (String)ViewState["BackgroundImage"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["BackgroundImage"] = value;
            }
        }


        /// <summary>
        ///  Public interface to Add Tab to the TabStrip
        /// </summary>
        /// <param name="tabCaption">Text to be display inside the tab</param>
        /// <param name="scriptClickCallBackFunction">Client function to be invoked when the tab is clicked.</param>
        public void AddTab(string tabCaption,string onClientClickFunction)
        {
            if (string.IsNullOrEmpty(tabCaption))
                throw new Exception(Resource.MissingTabCaptionError);

            if (string.IsNullOrEmpty(onClientClickFunction))
                throw new Exception(Resource.MissingClientClickFunctionError);

            Pair tabProp = new Pair(tabCaption, onClientClickFunction);
            tabNames.Add(tabProp);
        }

        /// <summary>
        ///  Override the RenderBeginTag to implement the one for this custom control. This method renders the begin tag and adds any attributes to it.
        /// </summary>
        /// <param name="writer">Instance of HtmlTextWriter to output the HTML tag</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);

            if (TagKey != HtmlTextWriterTag.Unknown)
                writer.RenderBeginTag(TagKey);
            else
                writer.RenderBeginTag(this.TagName);
        }

        #endregion

        #region Protected Methods
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.TabStrip.js");

            HtmlLink cssLink = new HtmlLink();
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            cssLink.Href = Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.TabStrip.css");
            this.Page.Header.Controls.Add(cssLink);

            StringBuilder ScriptBuilder = new StringBuilder();

            ScriptBuilder.Append("var " + this.ClientID + " = new Microsoft.js.ui.TabStrip('" + this.ClientID + "');");
            Global.Instance.Resource.RegisterClientImmediateScript(this, "CreateTabStrip", ScriptBuilder.ToString());
        }

        protected override void  Render(HtmlTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderContents(writer);
            RenderEndTag(writer);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Clear();
           
            for(int tabIndex=0; tabIndex < tabNames.Count; tabIndex++)
            {
                Tab tab = new Tab(this);

                if (tabIndex > 0)
                    tab.CssClass = "InActiveTab";
                else
                {
                    tab.CssClass = "ActiveTab";
                    tab.Active = true;
                }

                tab.Text = tabNames[tabIndex].First.ToString();
                tab.OnClientClick = tabNames[tabIndex].Second.ToString();
                Controls.Add(tab);
            }
            
            ChildControlsCreated = true;
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "tabstrip");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);
            if (IsSet("BackgroundImage"))
            {

                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url('" + this.BackgroundImage + "')");
            }
            else
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,Global.Instance.Resource.GetUrl(this.Page,"AjaxControls.images.BlueCaptionBar.png"));
            }

            if (!this.Enabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);

            ControlStyle.AddAttributesToRender(writer, this);


            System.Web.UI.AttributeCollection collection = this.Attributes;
            IEnumerator iter = Attributes.Keys.GetEnumerator();

            while (iter.MoveNext())
            {
                string key = (string)iter.Current;
                writer.AddAttribute(key, Attributes[key]);
            }
            
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

        protected override Style CreateControlStyle()
        {
            return new TabStripStyle(ViewState);
        }
        
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Ul); // <ul>

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "Hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "prevTabId");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "Tab_" + tabNames[0].First);
            
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            this.RenderChildren(writer);
            writer.RenderEndTag(); // </ul>
        }
        #endregion
    }
}
