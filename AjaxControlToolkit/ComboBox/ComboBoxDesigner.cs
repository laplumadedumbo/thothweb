﻿using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.Design.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AjaxControlToolkit
{
    public class ComboBoxDesigner : ListControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            // first, save current items before the base method resets them
            ListControl control = (ListControl)base.ViewControl;
            ListItem[] originalItems = new ListItem[control.Items.Count];
            control.Items.CopyTo(originalItems, 0);

            // now, let the base method reset them
            string baseHtml = base.GetDesignTimeHtml();

            // repopulate with original items
            control.Items.Clear();
            control.Items.AddRange(originalItems);

            // try to render as much resourced CSS as possible in the designer
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream cssStream = assembly.GetManifestResourceStream("AjaxControlToolkit.ComboBox.ComboBox.css");
            StreamReader cssReader = new StreamReader(cssStream);
            String cssString = cssReader.ReadToEnd();

            // perform CSS substitution for the designer 
            const string SUBSTITUTION_PATTERN = @"(<%=)\s*(WebResource\("")(?<resourceName>.+)\s*(""\)%>)";
            Regex regex = new Regex(SUBSTITUTION_PATTERN);
            cssString = regex.Replace(cssString, new MatchEvaluator(PerformWebResourceSubstitution));

            return "<style>" + cssString + "</style>" + baseHtml;
        }

        protected virtual string PerformWebResourceSubstitution(Match m)
        {
            string replacedString = m.ToString();
            replacedString = replacedString.Replace(m.Value, ViewControl.Page.ClientScript.GetWebResourceUrl(
                this.GetType(), m.Groups["resourceName"].Value));
            return replacedString;
        }

        public override bool AllowResize
        {
            get { return false; }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection actionLists = new DesignerActionListCollection();
                actionLists.AddRange(base.ActionLists);
                actionLists.Add(new ComboBoxDesignerActionList(Component));
                return actionLists;
            }
        }

    }
}
