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
    public class ToolBarButtonItemData : BaseToolBarItemData
    {
        /// <summary>
        ///  Index of the image from 0 in the full image of the ToolBar.
        /// </summary>
        private int imageIndex;

     
        /// <summary>
        ///  Index of the image give the image size in the ToolBarImage.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return imageIndex;
            }
            set
            {
                imageIndex = value;
            }
        }
    }
}
