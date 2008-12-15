// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxControls
{
    /// <summary>
    ///  Class that captures all the user supplied properties of the ToolBarItem that needs 
    /// to be added to the toolbar.
    /// </summary>
    public class BaseToolBarItemData
    {
        /// <summary>
        ///  Type of the ToolBar Button.
        /// </summary>
        private ToolBar.ToolbarItemType type;

        /// <summary>
        ///  The text that is display as title or tooltip for the Button.
        /// </summary>
        private string text;

        /// <summary>
        ///  Name that will be used by the control to create the client side js variable.
        /// </summary>
        private string name;

        /// <summary>
        ///  Callback function to invoke on client click.
        /// </summary>
        private string onClientClick;

        /// <summary>
        ///  Callback to query and get the state from an external callback.
        /// This is needed when you need to update the toolbar button from external
        /// events not direct ui events to the button.
        /// </summary>
        private string queryStateCallback;

        /// <summary>
        ///  ButtonType property
        /// </summary>
        public ToolBar.ToolbarItemType ButtonType
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
            }
        }

        /// <summary>
        ///  Text property 
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        ///  Client Callback
        /// </summary>
        public string OnClientClick
        {
            get
            {
                return onClientClick;
            }
            set
            {
                onClientClick = value;
            }
        }

        /// <summary>
        ///  Query State Callback
        /// </summary>
        public string QueryStateCallback
        {
            get
            {
                return queryStateCallback;
            }
            set
            {
                queryStateCallback = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value == null ? string.Empty : value;
            }
        }
    }
}
