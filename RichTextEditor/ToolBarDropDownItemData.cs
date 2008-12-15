// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AjaxControls
{
    /// <summary>
    ///  Class that captures all the user supplied properties of the ToolBarItem that needs 
    /// to be added to the toolbar.
    /// </summary>
    public class ToolBarDropDownItemData : BaseToolBarItemData
    {
        private Collection<string> items;
        private int width;
        bool isFontNames;

        public ToolBarDropDownItemData()
            : base()
        {
            this.ButtonType = ToolBar.ToolbarItemType.DropDownList;
            items = new Collection<string>();
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public Collection<string> Items
        {
            get { return items; }
            set { items = value; }
        }

        public bool IsFontItems
        {
            get { return isFontNames; }
            set { isFontNames = value; }
        }
    }
}
