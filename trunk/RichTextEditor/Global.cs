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
    ///  Capture all Global interfaces that are needed across the custom controls.
    /// </summary>
    public sealed class Global
    {
        /// <summary>
        ///  Instance of global embedded resource class.
        /// </summary>
        private static volatile EmbeddedResource resource;

        /// <summary>
        ///  Instance of global properties.
        /// </summary>
        private static volatile Properties properties;

        /// <summary>
        ///  i18N instance that would support getting font names for the region.
        /// </summary>
        private static volatile i18n i18n;
        /// <summary>
        ///  Self pointer. 
        /// </summary>
        private static volatile AjaxControls.Global self;

        /// <summary>
        ///  Lock object.
        /// </summary>
        private static object syncRoot = new Object();

        /// <summary>
        ///  Don't allow to be created on stack.
        /// </summary>
        private Global() { resource = new EmbeddedResource(); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Global Instance
        {
            get
            {
                if (self == null)
                {
                    lock (syncRoot)
                    {
                        if (self == null)
                        {
                            self = new AjaxControls.Global();
                            resource = new EmbeddedResource();
                            properties = new Properties();
                            i18n = new i18n();
                        }
                    }
                }

                return self;
            }
        }

        /// <summary>
        ///  Embedded Resource that wraps all the apis like GetUrl,RegisterClientScript taking handling
        /// any html encoding internally.
        /// </summary>
        public EmbeddedResource Resource
        {
            get
            {
                return resource;
            }
        }

        public Properties Properties
        {
            get
            {
                return properties;
            }
        }

        public i18n i18N
        {
            get
            {
                return i18n;
            }
        }

    }
}
