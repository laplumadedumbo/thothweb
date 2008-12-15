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
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace AjaxControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ToolBarDropDownButton runat=server></{0}:ToolBarDropDownButton>")]
    public class ToolBarDropDownButton : BaseToolBarButton
    {
        public ToolBarDropDownButton(ToolBar owner) : base(owner) 
        {
            items = new Collection<string>();
        }
        
        private int bw;

        private Collection<string> items;
        private bool isFont;

        /// <summary>
        ///  ToolBar Button Width. 
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(150)]
        public int ButtonWidth
        {
            get
            {
                return bw;
            }
            set
            {
                bw = value;
            }
        }

        public bool IsFontItems
        {
            get
            {
                return isFont;
            }
            set
            {
                isFont = value;
            }
        }

        public Collection<string> Items
        {
            get { return items; }
            set { items = value; }
        }

        public virtual string OnClientClick
        {
            get
            {
                string s = (string)ViewState["OnClientClick"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                ViewState["OnClientClick"] = value;
            }
        }

        public virtual string QueryStateCallback
        {
            get
            {
                string s = (string)ViewState["QueryStateCallback"];
                return (s == null) ? "null" : s;
            }
            set
            {
                ViewState["QueryStateCallback"] = value;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Type rstype = this.GetType();

            StringBuilder scriptBlock = new StringBuilder();

            scriptBlock.AppendLine("var ddltbBtni" + this.ClientID + " = new Microsoft.js.ui.DropDownToolbarButton('i" + this.ClientID + "'," + this.ButtonWidth + "," + this.parent.ButtonHeight +  "," + this.QueryStateCallback + ");");
            scriptBlock.AppendLine(this.parent.ClientID + ".AddButton(ddltbBtni" + this.ClientID + ");");

            for(int i=0; i < items.Count; i++)
            {
                string dropDownListItemId = "'i" + ClientID + "_Item_" + i + "'";
                scriptBlock.AppendLine("var select" + i +" = new Microsoft.js.event.EventHandler(document.getElementById(" + dropDownListItemId +"),'click','OnSelect',ddltbBtni" + this.ClientID + ",false);");
                scriptBlock.AppendLine("Microsoft.js.event.EventManager.add(select" + i + ");");
            }

            if (IsSet("OnClientClick") && !string.IsNullOrEmpty(this.OnClientClick))
            {
                scriptBlock.AppendLine("ddltbBtni" + this.ClientID + ".SubscribeToOnChange(" + this.OnClientClick + ");");
                Global.Instance.Resource.RegisterClientImmediateScript(this, "i" + this.ClientID, scriptBlock.ToString());
            }
          
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "i" + ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "tbDropDownBtn,Selectable");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.ButtonWidth.ToString() + "px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "block");
            writer.AddStyleAttribute("float", "left");

            if(String.Compare(parent.Theme,"Classic",true) == 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.images.ClassicDropDownList.png"));
            else
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage,Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.images.DropDownList.png"));
            
            int topmultiplier = 4;

            switch (this.State)
            {
                case ButtonState.Default:
                    topmultiplier = 4;
                    break;
                case ButtonState.Highlighted:
                    topmultiplier = 3;
                    break;
                case ButtonState.Selected:
                    topmultiplier = 2;
                    break;
                case ButtonState.Greyed:
                    topmultiplier = 1;
                    break;
            }

            int top = (parent.ButtonHeight) * topmultiplier;

            if (items.Count > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Value, items[0]);
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Value, "");
            
            writer.AddStyleAttribute("background-position","0px " + top.ToString() + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
            writer.AddAttribute("unselectable", "on");
            writer.RenderBeginTag(HtmlTextWriterTag.Input); //<input>
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "i" + ClientID + "_Children");

            writer.AddAttribute("unselectable", "on");
            writer.AddAttribute(HtmlTextWriterAttribute.Class,"tbDropDownListContainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>

            for(int i=0; i < items.Count; i++)
            {
                string item = items[i];

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0);");

                if(String.Compare(parent.Theme,"Classic",true) == 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "tbClassicDropDownListItem");
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "tbDropDownListItem");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "i" + ClientID + "_Item_" + i);

                if (IsFontItems)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, item);
                
                writer.AddAttribute("unselectable", "on");
                writer.RenderBeginTag(HtmlTextWriterTag.A); // <a>
                writer.Write(item);
                writer.RenderEndTag(); // </a>
            }
            writer.RenderEndTag(); // </div>
        }
    }
}
