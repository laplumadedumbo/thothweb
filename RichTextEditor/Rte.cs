// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Collections.Specialized;

namespace AjaxControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Rte runat=server></{0}:Rte>")]
    public class RichTextEditor : WebControl
    {
        string[] emoticons = {"smile_regular.gif", "smile_teeth.gif", "smile_wink.gif", "smile_omg.gif", "smile_tongue.gif", "smile_shades.gif", "smile_angry.gif", "smile_confused.gif", "smile_embaressed.gif", "smile_sad.gif", "smile_cry.gif", "smile_sniff.gif", "smile_angel.gif", "smile_baringteeth.gif", "smile_nerd.gif", "smile_sick.gif", "smile_party.gif", "smile_yawn.gif", "smile_thinking.gif", "smile_zipit.gif", "smile_secret.gif", "smile_sarcastic.gif", "smile_eyeroll.gif", "heart.gif", "heart_broken.gif", "messenger.gif", "cat.gif", "dog.gif", "snail.gif", "blacksheep.gif", "moon.gif", "star.gif", "sun.gif", "rainbow.gif", "hug_dude.gif", "hug_girl.gif", "kiss.gif", "rose.gif", "rose_wilted.gif", "clock.gif", "present.gif", "cake.gif", "camera.gif", "lightbulb.gif", "coffee.gif", "phone.gif", "mobile.gif", "car.gif", "airplane.gif", "computer.gif", "money.gif", "film.gif", "music_note.gif", "pizza.gif", "soccerball.gif", "envelope.gif", "guy_handsacrossamerica.gif", "girl_handsacrossamerica.gif", "island.gif", "umbrella.gif" };
        protected ToolBar toolbar;
        protected HtmlGenericControl textEditorClient;
        protected TextBox textEditorServer;
        protected TabStrip tabstrip;
        protected HtmlGenericControl iFrame;
        private string text = string.Empty;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                EnsureChildControls();
                
                {
                    if (this.textEditorClient != null)
                        this.text = this.Page.Request.Form[this.TextEditorClientID];
                    else if(this.Page != null && this.Page.Request.Form[this.TextEditorClientID] != null)
                        this.text = this.Page.Request.Form[this.TextEditorClientID];
                    else
                        this.text = this.textEditorServer.Text;
                }

                return this.text;
            }
            set
            {
                EnsureChildControls();
                this.text = value;
                if (this.textEditorClient != null)
                {
                    this.textEditorClient.InnerText = this.text;
                }
                else if (this.textEditorServer != null)
                {
                    this.textEditorServer.Text = this.text;
                }
            }
        }

        public string Culture
        {
            get
            {
                EnsureChildControls();
                String s = (String)ViewState["Culture"];
                return ((s == null || string.Compare(s,"neutral",true,System.Globalization.CultureInfo.CurrentCulture) == 0) ? "en-us" : s);
            }
            set
            {
                EnsureChildControls();
                ViewState["Culture"] = value;
            }

        }

        public string TextEditorClientID
        {
            get
            {
               EnsureChildControls();
               String s = (String)ViewState["TextEditorClientID"];
               return ((s == null) ? String.Empty : s);
            }
            set
            {
                EnsureChildControls();
                ViewState["TextEditorClientID"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Unit), "740")]
        [Localizable(false)]
        public override Unit Width
        {
            get
            {
                Unit s = Unit.Empty;
                if (ViewState["ControlWidth"] != null)
                {
                    s = (Unit)ViewState["ControlWidth"];
                }
                else
                {
                    if (IsSet("Theme") && String.Compare(this.Theme, "Classic") == 0)
                        s = Unit.Pixel(726);
                    else
                        s = Unit.Pixel(744);

                }
                return s;
            }
            set
            {
                ViewState["ControlWidth"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Unit), "400")]
        [Localizable(false)]
        public override Unit Height
        {
            get
            {
                Unit s = Unit.Empty;
                if (ViewState["ControlHeight"] != null)
                {
                    s = (Unit)ViewState["ControlHeight"];
                }
                else
                {
                    s = Unit.Pixel(400);

                }
                return s;
            }
            set
            {
                ViewState["ControlHeight"] = value;
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
                EnsureChildControls(); // DEV NOTE: I think this call is requiring me to set Height and Width before Theme. Does it need to be here?
                String s = (String)ViewState["Theme"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                EnsureChildControls(); // DEV NOTE: I think this call is requiring me to set Height and Width before Theme. Does it need to be here?
                ViewState["Theme"] = value;
            }
        }

        public bool DomMode
        {
            get
            {
                return Global.Instance.Properties.DomMode;
            }

            set
            {
                Global.Instance.Properties.DomMode = value;
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


        /// <summary>
        ///  The start HTML Tag for the Rte
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.Page.IsPostBack)
            {

            }
            EnsureChildControls();
        }


        /// <summary>
        ///  Add Atributes to the start HTML Tag for the Toolbar.
        /// </summary>
        /// <param name="writer">HtmlTextWriter used to output the HTML tags to the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Rte");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);

            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());

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
        ///  Create the Child controls like Toolbar, editor, tabs.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            base.CreateChildControls();

            toolbar = new ToolBar();
            toolbar.ID = "toolbar";

            if (DomMode)
            {
                textEditorClient = new HtmlGenericControl("textarea");
                textEditorClient.ID = "editor";
                textEditorClient.InnerText = this.Text;
            }
            else
            {
                textEditorServer = new TextBox();
                textEditorServer.TextMode = TextBoxMode.MultiLine;
                textEditorServer.ID = "editor";
            
                textEditorServer.Text = this.Text;
            }

            Controls.Add(toolbar);
            if (DomMode)
            {
                Controls.Add(textEditorClient);
            }
            else
            {
                Controls.Add(textEditorServer);
            }

            iFrame = new HtmlGenericControl("iframe");
            iFrame.ID = "RichEdit";
            iFrame.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#ffffff");

            iFrame.Style.Add(HtmlTextWriterStyle.Width, this.Width.ToString());
            iFrame.Style.Add(HtmlTextWriterStyle.Height, this.Height.ToString());

            iFrame.Attributes.Add("frameborder", "0");

            Controls.Add(iFrame);

            tabstrip = new TabStrip();
            tabstrip.ID = "tabstrip";

            Controls.Add(tabstrip);

            ChildControlsCreated = true;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Type rstype = this.GetType();

            if (IsSet("Theme") && string.Compare(Theme, "Classic",true) == 0)
            {
                toolbar.ToolBarImage = Global.Instance.Resource.GetUrl(this.Page,"AjaxControls.images.ClassicToolBarImage.png");
                toolbar.ButtonHeight = 24;
                toolbar.ButtonWidth = 24;
                tabstrip.BackgroundImage = Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.images.ClassicCaptionBar.png");
                toolbar.Theme = this.Theme;
            }
            else
            {
                //Default to blue.
                toolbar.ToolBarImage = Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.images.BlueToolBarImage.png");
                toolbar.ButtonHeight = 24;
                toolbar.ButtonWidth = 25;
                toolbar.Theme = "Blue";
            }

             
            Collection<string> dditems= new Collection<string>();
            string lang;
            ReadOnlyCollection<string> supportedLanguages = Global.Instance.i18N.GetSupportedLanguages();
           
            if (supportedLanguages.Contains(this.Culture.ToLower()) == true)
                lang = this.Culture.ToLower();
            else
                lang = "en-us";

            Hashtable ToolTips = Global.Instance.i18N.GetStringsByGroup(lang, "ToolTip");

            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Cut"].ToString(), 0, "OnCut",null,"RteCut");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Paste"].ToString(), 1, "OnPaste",null,"RtePaste");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Copy"].ToString(), 2, "OnCopy",null,"RteCopy");

            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["Bold"].ToString(), 3, "OnBold", "QueryCommandState('bold')","RteBold");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["Italics"].ToString(), 4, "OnItalics", "QueryCommandState('italic')","RteItalics");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["Underline"].ToString(), 5, "OnUnderline","QueryCommandState('underline')","RteUnderline");

            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Indent"].ToString(), 8, "OnIndent",null,"RteIndent");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Outdent"].ToString(), 7, "OnOutdent",null,"RteOutdent");

           
            foreach (string fontName in Global.Instance.i18N.GetFontNames(lang))
            {
                if(!dditems.Contains(fontName))
                dditems.Add(fontName);
            }

            //.SKN
            Hashtable FontDescNameMap = Global.Instance.i18N.GetFontDescNameMap(lang);
            Global.Instance.Resource.RegisterClientImmediateScript(this, "fontDescName", "var fontDescNameMap = new Array();");

            foreach (string desc in FontDescNameMap.Keys)
            {
                Global.Instance.Resource.RegisterClientImmediateScript(this,desc,"fontDescNameMap['" + desc + "'] = '" + FontDescNameMap[desc] + "' ;\n") ;
            }

            foreach (RteMessage msg in Global.Instance.i18N.GetMessages(lang.ToLower()))
            {
              Global.Instance.Resource.RegisterClientImmediateScript(this,"RteMsg" + msg.Id.ToString(), "RteMsg" + msg.Id + "='" +  msg.MessageString + "' ;");
            }

            toolbar.AddDropDownButton(dditems, 141, "OnFontChange",true,"QueryCommandValue('fontname')");

            Global.Instance.Resource.RegisterClientImmediateScript(this, "lastFontName", "lastFontName = '" + dditems[0] + "' ;");

            Collection<string> ddfontSizes = new Collection<string>();
            ddfontSizes.Add("10pt");
            ddfontSizes.Add("12pt");
            ddfontSizes.Add("14pt");
            ddfontSizes.Add("18pt");
            ddfontSizes.Add("24pt");

            Global.Instance.Resource.RegisterClientImmediateScript(this, "lastFontSize", "lastFontSize = '" + ddfontSizes[0] + "' ;");

            // Add javascript associative array to map from pt size to browser size.
            StringBuilder script = new StringBuilder();
            script.AppendLine("fontsizes = new Array();");
            for(int fz=0; fz < ddfontSizes.Count; fz++)
                script.AppendLine("fontsizes['" + ddfontSizes[fz] + "'] = " + (fz + 2) + ";");
            
            Global.Instance.Resource.RegisterClientImmediateScript(this, "fontsizemap", script.ToString());

            toolbar.AddDropDownButton(ddfontSizes, 141, "OnFontSizeChange", false, "QueryCommandValue('fontsize')");

            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["LeftJustify"].ToString(), 9, "OnLeftJustify", "QueryCommandState('JustifyLeft')","RteJustifyLeft");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["CenterJustify"].ToString(), 10, "OnCenterJustify", "QueryCommandState('JustifyCenter')","RteJustifyCenter");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["RightJustify"].ToString(), 11, "OnRightJustify", "QueryCommandState('JustifyRight')","RteJustifyRight");

            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["BulletedList"].ToString(), 12, "OnUnOrderedList", "QueryCommandState('InsertUnorderedList')", "RteUnorderedList");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ToggleButton, ToolTips["NumberedList"].ToString(), 13, "OnOrderedList", "QueryCommandState('InsertOrderedList')", "RteOrderedList");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Hyperlink"].ToString(), 14, "OnCreateLink", null, "RteHyperLink");

            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["MCodeBlock"].ToString(), 15, "OnMarkCode",null,"RteMarkCode");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["FgColor"].ToString(), 6, "OnFgColor",null,"RteFgColor");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["BgColor"].ToString(), 16, "OnBgColor",null,"RteBgColor");
            toolbar.AddButton(AjaxControls.ToolBar.ToolbarItemType.ImageButton, ToolTips["Emoticons"].ToString(), 17, "OnEmoticon",null,"RteEmoticon");

            if (DomMode)
            {
                textEditorClient.Style.Add(HtmlTextWriterStyle.Display, "none");
                textEditorClient.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
                textEditorClient.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");

                textEditorClient.Attributes["id"] = textEditorClient.ClientID;
                textEditorClient.Attributes["name"] = textEditorClient.ID;
                TextEditorClientID = textEditorClient.ClientID;
            }
            else
            {
                textEditorServer.Style.Add(HtmlTextWriterStyle.Display, "none");
                textEditorServer.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
                textEditorServer.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");

                TextEditorClientID = textEditorServer.ClientID;
            }
            tabstrip.AddTab("TextView", "OnTextView");
            tabstrip.AddTab("HtmlView", "OnHtmlView");

            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.Microsoft.js");
            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.RichTextEditor.js");

            Global.Instance.Resource.RegisterClientImmediateScript(this, "emoticons", "var emoticonsArray = new Array();");

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine();
            foreach (string emoticon in emoticons)
            {
                string imgUrl = Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.images.emoticons." + emoticon);
                scriptBuilder.AppendLine("emoticonsArray.push('" + imgUrl + "');");
            }
            
            Global.Instance.Resource.RegisterClientImmediateScript(this, "emoticonsadd", scriptBuilder.ToString());
            Global.Instance.Resource.RegisterClientImmediateScript(this, "editorLinks", "var editCssUrl = \'" + Global.Instance.Resource.GetUrl(this.Page, "AjaxControls.edit.css") + "\';");
            Global.Instance.Resource.RegisterClientImmediateScript(this, "editorIdinit", "var editorId = '" + this.TextEditorClientID + "' ;");
            Global.Instance.Resource.RegisterClientImmediateScript(this, "RichEditIdinit", "var RichEditId = '" + this.iFrame.ClientID + "' ;");
            Global.Instance.Resource.RegisterClientImmediateScript(this, "RteIdinit", "var RteId = '" + this.ClientID + "' ;");
            Global.Instance.Resource.RegisterClientImmediateScript(this, "RteToolbarId", "var RteToolbarId = '" + toolbar.ClientID + "' ;");

            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.ColorPicker.js");
            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.EmoticonPicker.js");
            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.HyperLinkDialog.js");
            Global.Instance.Resource.RegisterClientScript(this.Page, this.Controls, "AjaxControls.RteStartup.js");
        }

        /// <summary>
        ///  Begins rendering the editor
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
        protected override void RenderContents(HtmlTextWriter writer)
        {
            this.RenderChildren(writer);

            Global.Instance.Resource.RegisterClientImmediateScript(this, "RteInit", "InitRte();");

            // if not using server form we need to add the script here 
            if (DomMode)
            {
                Global.Instance.Resource.RenderClientImmediateScript(writer);
            }
        }
    }
}
