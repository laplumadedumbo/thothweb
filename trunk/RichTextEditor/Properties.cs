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
    ///  These are the global properties across AjaxControls
    /// </summary>
    public class Properties
    {
        /// <summary>
        ///  This when associated with a control denotes if its in client mode (dom mode) or in server mode. 
        /// </summary>
        private bool domMode = false;

        public bool DomMode
        {
            get { return domMode; }
            set { domMode = value; }
        }
    }
}
