// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AjaxControls
{
    public class BaseToolBarButton : WebControl, INamingContainer
    {
        protected ToolBar parent;
        public enum ButtonState { Default, Highlighted, Selected, Greyed };

        
        public ButtonState State
        {
            get
            {
                ButtonState st = (ButtonState)ViewState["ButtonState"];
                return st;
            }
            set
            {
                ViewState["ButtonState"] = value;
            }

        }
        public BaseToolBarButton(ToolBar owner):base()
        {
            parent = owner;
        }

        

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
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

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Name
        {
            get
            {
                String s = (String)ViewState["Name"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Name"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.State = ButtonState.Default;
        }

        /// <summary>
        ///  Start HTML Tag for the button
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Span; }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "cell");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);
            
            writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "top");

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

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);

            if (TagKey != HtmlTextWriterTag.Unknown)
                writer.RenderBeginTag(TagKey);
            else
                writer.RenderBeginTag(this.TagName);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            RenderBeginTag(writer); 
        }
    }
}
