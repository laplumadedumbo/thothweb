// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace AjaxControls
{
    class TabStripStyle : Style
    {
        public enum Orientation { Horizontal, Vertical };
        // Summary:
        //     Initializes a new instance of the System.Web.UI.WebControls.TableStyle class
        //     using default values.
        public TabStripStyle():base()
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Web.UI.WebControls.TableStyle class
        //     with the specified state bag information.
        //
        // Parameters:
        //   bag:
        //     A System.Web.UI.StateBag that represents the state bag in which to store
        //     style information.
        public TabStripStyle(StateBag bag):base(bag)
        {
        }

        [NotifyParentProperty(true)]
        public virtual string BackImageUrl
        {
            get { return (string) ViewState["BackImageUrl"] ; }
            set { ViewState["BackImageUrl"] = value;}
        }

        [NotifyParentProperty(true)]
        public virtual string ActiveTabImageUrl
        {
            get { return (string) ViewState["ActiveTabImageUrl"]; }
            set { ViewState["ActiveTabImageUrl"] = value; }
        }

        [NotifyParentProperty(true)]
        public virtual string InactiveTabImageUrl
        {
            get { return (string)ViewState["InactiveTabImageUrl"]; }
            set { ViewState["InactiveTabImageUrl"] = value; }
        }

        [NotifyParentProperty(true)]
        public Orientation TabsOrientation
        {
            get { return ViewState["TabsOrientation"] != null ? (Orientation)ViewState["TabsOrientation"] : Orientation.Horizontal; }
            set { ViewState["TabsOrientation"] = value; }
        }

        internal bool IsSet(string key)
        {
            return ViewState[key] != null;
        }

        public override bool IsEmpty
        {
            get
            {
                return base.IsEmpty &&
                    (!IsSet("BackImageUrl") && !IsSet("InactiveTabImageUrl") && !IsSet("ActiveTabImageUrl"));
            }
        }

        public override void Reset()
        {
            base.Reset();

            if (IsEmpty)
                return;

            if (IsSet("BackImageUrl"))
                ViewState.Remove("BackImageUrl");

            if (IsSet("ActiveTabImageUrl"))
                ViewState.Remove("ActiveTabImageUrl");

            if (IsSet("InactiveTabImageUrl"))
                ViewState.Remove("InactiveTabImageUrl");

        }

        protected override void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
        {
            base.FillStyleAttributes(attributes, urlResolver);

            if (IsSet("BackImageUrl"))
                attributes.Add("backgroundImage", this.BackImageUrl);
        }
    }        
}
