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
    ///  ToolBar Composite web control.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ToolBar runat=server></{0}:ToolBar>")]
    public class ToolBar : CompositeControl
    {
        /// <summary>
        ///  Enumeration of ButtonTypes. 
        /// </summary>
        public enum ToolbarItemType { ImageButton= 0, ToggleButton=1, DropDownList = 2 };

        /// <summary>
        ///  List of ToolBarButton properties used for creation of the controls.
        /// </summary>
        protected Collection<BaseToolBarItemData> childControlsInfo;

        private int bw;
        private int bh;

        /// <summary>
        ///  ToolBarImage url
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Localizable(true)]
        public string ToolBarImage
        {
            get
            {
                String s = (String)ViewState["ToolBarImage"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ToolBarImage"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("Blue")]
        [Localizable(false)]
        public string Theme
        {
            get
            {
                String s = (String)ViewState["Theme"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Theme"] = value;
            }
        }

        /// <summary>
        ///  ToolBar Button Width. 
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(24)]
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

        /// <summary>
        ///  ToolBar Button Height.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(24)]
        public int ButtonHeight
        {
            get
            {
                return bh;
            }
            set
            {
                bh = value;
            }
        }


        /// <summary>
        ///  ToolBar constructor
        /// </summary>
        public ToolBar()
        {
            childControlsInfo = new Collection<BaseToolBarItemData>();
        }


        /// <summary>
        ///  Initialize the childControlsInfo Collection.
        /// </summary>
        /// <param name="e">Event Arguments supplied</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        ///  Public interface to Add a new button to the ToolBar.
        /// </summary>
        /// <param name="type">Type of the Button</param>
        /// <param name="text">Text display as tooltip or title for the button</param>
        /// <param name="imageIndex">Index of the image to be used within the ToolBarImage</param>
        public void AddButton(ToolbarItemType type,string text,int imageIndex,string onClientClick,string queryStateCallback,string name)
        {
            if (text == null)
                text = string.Empty;

            
            if(type == ToolbarItemType.ImageButton || type == ToolbarItemType.ToggleButton)
            {
                    ToolBarButtonItemData ctrlInfo = new ToolBarButtonItemData();
                    ctrlInfo.ButtonType = type;
                    ctrlInfo.Text = text;
                    ctrlInfo.ImageIndex = imageIndex;
                    ctrlInfo.OnClientClick = onClientClick;
                    ctrlInfo.QueryStateCallback = queryStateCallback;
                    ctrlInfo.Name = name;
                    childControlsInfo.Add(ctrlInfo);
            }
                    
        }

        public void AddDropDownButton(Collection<string> items,int width, string onClientClick,bool isFontNames,string queryStateCallback)
        {
            ToolBarDropDownItemData data = new ToolBarDropDownItemData();
            data.Items = items;
            data.OnClientClick = onClientClick;
            data.Width = width;
            data.IsFontItems = isFontNames;
            data.QueryStateCallback = queryStateCallback;
            childControlsInfo.Add(data);
        }


        /// <summary>
        ///  Setup Client Javascripts during prerender event.
        /// </summary>
        /// <param name="e">Instance of EventArgs</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            Type rstype = this.GetType();

            HtmlLink cssLink = new HtmlLink();
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";

            cssLink.Href = Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.Toolbar.css");
            this.Page.Header.Controls.Add(cssLink);

            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.Microsoft.js");
            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.Event.js");
            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.ToolBar.js");

            Global.Instance.Resource.RegisterClientImmediateScript(this, this.ClientID, "var " + this.ClientID + "= new Microsoft.js.ui.Toolbar('" + this.ClientID + "');");

        }

        /// <summary>
        ///  The start HTML Tag for the ToolBar
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        ///  Add Atributes to the start HTML Tag for the Toolbar.
        /// </summary>
        /// <param name="writer">HtmlTextWriter used to output the HTML tags to the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "toolbar");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);

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
        ///  Create the Child controls for the ToolBar based on used supplied childControlsInfo.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            base.CreateChildControls();

            for (int ctrlIndex = 0; ctrlIndex < this.childControlsInfo.Count; ctrlIndex++)
            {

                switch (this.childControlsInfo[ctrlIndex].ButtonType)
                {
                    case ToolbarItemType.ImageButton:
                        ToolBarButton imgBtn = new ToolBarButton(this);
                        imgBtn.Text = this.childControlsInfo[ctrlIndex].Text;
                        imgBtn.ImageIndex = ((ToolBarButtonItemData)this.childControlsInfo[ctrlIndex]).ImageIndex;
                        imgBtn.Toggle = false;
                        imgBtn.OnClientClick = this.childControlsInfo[ctrlIndex].OnClientClick;
                        imgBtn.Name = this.childControlsInfo[ctrlIndex].Name;
                        Controls.Add(imgBtn);
                        break;
                    case ToolbarItemType.ToggleButton:
                        ToolBarButton toggleBtn = new ToolBarButton(this);
                        toggleBtn.Text = this.childControlsInfo[ctrlIndex].Text;
                        toggleBtn.ImageIndex =  ((ToolBarButtonItemData)this.childControlsInfo[ctrlIndex]).ImageIndex;
                        toggleBtn.Toggle = true;
                        toggleBtn.OnClientClick = this.childControlsInfo[ctrlIndex].OnClientClick;
                        toggleBtn.QueryStateCallback = this.childControlsInfo[ctrlIndex].QueryStateCallback;
                        toggleBtn.Name = this.childControlsInfo[ctrlIndex].Name;
                        Controls.Add(toggleBtn);
                        break;
                    case ToolbarItemType.DropDownList:
                        ToolBarDropDownButton ddBtn = new ToolBarDropDownButton(this);
                        ddBtn.Items = ((ToolBarDropDownItemData) this.childControlsInfo[ctrlIndex]).Items;
                        ddBtn.ButtonWidth = ((ToolBarDropDownItemData)this.childControlsInfo[ctrlIndex]).Width;
                        ddBtn.IsFontItems = ((ToolBarDropDownItemData)this.childControlsInfo[ctrlIndex]).IsFontItems;
                        ddBtn.OnClientClick = this.childControlsInfo[ctrlIndex].OnClientClick;
                        ddBtn.QueryStateCallback = this.childControlsInfo[ctrlIndex].QueryStateCallback;
                        Controls.Add(ddBtn);
                        break;
                }
            }
            ChildControlsCreated = true;
        }

        /// <summary>
        ///  Begins rendering the ToolBar control.
        /// </summary>
        /// <param name="writer">HtmlTextWriter instance to output the HTML tags to client</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);

            if (TagKey != HtmlTextWriterTag.Unknown)
                writer.RenderBeginTag(TagKey);
            else
                writer.RenderBeginTag(this.TagName);
        }

        /// <summary>
        ///  Main Render method that invokes other render methods.
        /// </summary>
        /// <param name="writer">HtmlTextWriter instance to output the HTML tags to client</param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderContents(writer);
            RenderEndTag(writer);
        }

        /// <summary>
        ///  This portion renders any additional tag other than the main Tag for the toolbar 
        /// and invokes render on its child controls.
        /// </summary>
        /// <param name="writer">HtmlTextWriter instance to output the HTML tags to client</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            this.RenderChildren(writer);
            writer.RenderEndTag();
        }
    }
}
