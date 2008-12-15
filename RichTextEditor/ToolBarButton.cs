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

namespace AjaxControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ToolBarImageButton runat=server></{0}:ToolBarImageButton>")]
    public class ToolBarButton : BaseToolBarButton
    {

        public ToolBarButton(ToolBar owner) : base(owner) { } 

        public int ImageIndex
        {
            get
            {
                int i = (int)ViewState["ButtonImageIndex"];
                return i;
            }
            set
            {
                ViewState["ButtonImageIndex"] = value;
            }
        }

        public bool Toggle
        {
            get
            {
                bool t = (bool)ViewState["Toggle"];
                return t;
            }
            set
            {
                ViewState["Toggle"] = value;
            }
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

            ToolBar.ToolbarItemType itemType;
            bool toggle = IsSet("Toggle") ? this.Toggle : false;

            itemType = toggle ? ToolBar.ToolbarItemType.ToggleButton : ToolBar.ToolbarItemType.ImageButton;


            string jsVarName;

            if (!String.IsNullOrEmpty(this.Name))
                jsVarName = this.Name;
            else
                jsVarName = this.ClientID;

            string jsStmt = "var " + jsVarName + " = new Microsoft.js.ui.ToolbarButton('" + this.ClientID + "','" + itemType.ToString() + "','" + this.State + "'," + this.parent.ButtonWidth + "," + this.parent.ButtonHeight + "," + this.QueryStateCallback + ");";
            scriptBlock.AppendLine(jsStmt);

            scriptBlock.AppendLine(this.parent.ClientID + ".AddButton(" + jsVarName + ");");

            if (IsSet("OnClientClick") && !string.IsNullOrEmpty(this.OnClientClick))
            {
                scriptBlock.AppendLine("this.tbBtnClick" + jsVarName + " = " + "new Microsoft.js.event.EventHandler(" + jsVarName + ".uiElement,'click','" + this.OnClientClick + "(" + jsVarName + ")',this,false);");
                scriptBlock.AppendLine("Microsoft.js.event.EventManager.add(this.tbBtnClick" + jsVarName + ");");
            }

            Global.Instance.Resource.RegisterClientImmediateScript(this, this.ClientID, scriptBlock.ToString());
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_a");
           
            writer.AddAttribute("unselectable", "on"); // This is need for the following reason.
            // In IE when you select items outside the iframe, it looses focus from iframe and any selection
            // you made is also lost. In order to avoid that behavior, you need to add the attribute
            // unselectable and set its value to 'on'

            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "block");
            writer.AddStyleAttribute("float", "left");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, parent.ButtonWidth + "px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, parent.ButtonHeight + "px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, parent.ToolBarImage);
            writer.AddAttribute(HtmlTextWriterAttribute.Title, this.Text);
            
            int left = (parent.ButtonWidth) * (this.ImageIndex > 0 ? this.ImageIndex * -1 : this.ImageIndex);
            int topmultiplier = 0;

            switch (this.State)
            {
                case ButtonState.Default:
                    topmultiplier = 0;
                    break;
                case ButtonState.Highlighted:
                    topmultiplier = 1;
                    break;
                case ButtonState.Selected:
                    topmultiplier = 2;
                    break;
                case ButtonState.Greyed:
                    topmultiplier = 3;
                    break;
            }

            int top = (parent.ButtonHeight) * topmultiplier;

            writer.AddStyleAttribute("background-position",left + "px " + top + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.RenderEndTag(); // </span>
        }
    }
}
