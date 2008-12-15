// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace AjaxControls
{
    public class i18n
    {
        #region PrivateMembers
        /// <summary>
        ///  Xml file that contains all the global i18N properties like Font names
        /// </summary>
        private XPathDocument i18Nxml;

        #endregion

        #region Constructor
        /// <summary>
        ///  Constructor
        /// </summary>
        public i18n()
        {
            Assembly thisAssembly = this.GetType().Assembly;
            Stream xmlStream = thisAssembly.GetManifestResourceStream("AjaxControls.i18N.xml");
            
            i18Nxml = new XPathDocument(xmlStream);
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///  Get the list of Font Names given the region Code.
        /// </summary>
        /// <param name="languageCode">Language / region code like en-us,ja-JP,zh-CN</param>
        /// <returns>ReadonlyCollection of strings which are the font names supported for this region</returns>
        public ReadOnlyCollection<string> GetFontNames(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException("languageCode"); // param names need not be localized.

            Collection<string> fNames = QueryFonts(languageCode.ToLower());

            // If its not us english then in addition to any language specific font add
            // the english fonts.
            if (string.Compare(languageCode, "en-us",true) != 0)
            {
                Collection<string> englishFonts = QueryFonts("en-us");

                foreach (string ef in englishFonts)
                    fNames.Add(ef);
            }
            return new ReadOnlyCollection<string>(fNames);
        }


        public Hashtable GetFontDescNameMap(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException("languageCode"); // param names need not be localized.

            Hashtable descNameMap = QueryFontDescNameMap(languageCode.ToLower());

            if (string.Compare(languageCode, "en-us", true) != 0)
            {
                Hashtable enDescNameMap = QueryFontDescNameMap("en-us");
                foreach (string desc in enDescNameMap.Keys)
                {
                    if(descNameMap.ContainsKey(desc) == false)
                        descNameMap.Add(desc, enDescNameMap[desc]);
                }
                enDescNameMap = null;
            }

            return descNameMap;
        }

        /// <summary>
        ///  Given the language code get the mapping between font description and the actual font name.
        /// This is needed for the following case:
        ///   When you query IE for the current font settings and if it happens to be a international font name 
        /// like chinese then it returns the name in english instead of the actual font name string as UTF. Hence
        /// we need a way to get the the UTF name so that we can display it correctly on the drop down list
        /// for font name.
        /// </summary>
        /// <param name="languageCode">Language code like en-us</param>
        /// <returns>Hashtable of Description mapping to actual font name</returns>
        public Hashtable QueryFontDescNameMap(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException("languageCode"); // param names need not be localized.

            string selectFontsQuery = "i18N/Languages/Language[@code=\"" + languageCode + "\"]/Fonts/Font";
         
            XPathNavigator fontNav = i18Nxml.CreateNavigator();

            XPathNodeIterator FontNodeIter = fontNav.Select(selectFontsQuery);
            
            Hashtable result = new Hashtable();

            while (FontNodeIter.MoveNext())
            {
                string descKey = FontNodeIter.Current.GetAttribute("Description", FontNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
                string nameValue = FontNodeIter.Current.GetAttribute("Name", FontNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
                if(!result.ContainsKey(descKey))
                    result.Add(descKey, nameValue);
            };

            return result;
        }

        /// <summary>
        ///  Returns you a collection of supported languages in the form en-us,zh-CN etc.
        /// </summary>
        /// <returns>Retruns a collection of supported languages</returns>
        public ReadOnlyCollection<string> GetSupportedLanguages()
        {
            string selectFontsQuery = "i18N/Languages/Language/@code";
            Collection<string> lNames = new Collection<string>();

            XPathNavigator langNav = i18Nxml.CreateNavigator();

            XPathNodeIterator LangNodeIter = langNav.Select(selectFontsQuery);

            while (LangNodeIter.MoveNext())
            {
                lNames.Add(LangNodeIter.Current.Value);
            };

            return new ReadOnlyCollection<string>(lNames);
        }

        /// <summary>
        ///  Query the xml document using Xpath query for font names
        /// </summary>
        /// <param name="languageCode">Language codes like en-us, zh-CN etc</param>
        /// <returns>Collection of font names as string</returns>
        private Collection<string> QueryFonts(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException("languageCode"); // param names need not be localized.

            string selectFontsQuery = "i18N/Languages/Language[@code=\"" + languageCode + "\"]/Fonts/Font/@Name";
            Collection<string> fNames = new Collection<string>();

            XPathNavigator fontsNav = i18Nxml.CreateNavigator();

            XPathNodeIterator FontNodeIter = fontsNav.Select(selectFontsQuery);

            while (FontNodeIter.MoveNext())
            {
                fNames.Add(FontNodeIter.Current.Value);
            };

            return fNames;
        }

        /// <summary>
        ///  Query the xml document using Xpath query for messages defined
        /// </summary>
        /// <param name="languageCode">Language codes like en-us, zh-CN etc</param>
        /// <returns>Collection Message structure</returns>
        public Collection<RteMessage> GetMessages(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException("languageCode"); // param names need not be localized.

            string selectMsgQuery = "i18N/Languages/Language[@code=\"" + languageCode + "\"]/Messages/Message";
            Collection<RteMessage> msgs = new Collection<RteMessage>();

            XPathNavigator msgNav = i18Nxml.CreateNavigator();

            XPathNodeIterator msgNodeIter = msgNav.Select(selectMsgQuery);

            while (msgNodeIter.MoveNext())
            {
                string grp = msgNodeIter.Current.GetAttribute("Group", msgNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));

                if (!String.IsNullOrEmpty(grp))
                    continue;

                RteMessage msg = new RteMessage();
                //msg.Id = Int32.Parse(msgNodeIter.Current.GetAttribute("Id", msgNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance")));
                msg.Id = msgNodeIter.Current.GetAttribute("Id", msgNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
                
                msg.MessageString = msgNodeIter.Current.GetAttribute("Msgstring", msgNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));

                msgs.Add(msg);
            };

            return msgs;
        }

        /// <summary>
        ///  Query the xml document using Xpath query for message defined, filtered by Group
        /// </summary>
        /// <param name="languageCode">Language codes like en-us, zh-CN etc</param>
        /// <returns>Hashtable of Strings</returns>
        public Hashtable GetStringsByGroup(string languageCode,string group)
        {
            if (string.IsNullOrEmpty(languageCode))
                throw new ArgumentNullException("languageCode"); // param names need not be localized.

            string selectMsgQuery = "i18N/Languages/Language[@code=\"" + languageCode + "\"]/Messages/Message[@Group=\"" + group + "\"]";
            Hashtable msgs = new Hashtable();

            XPathNavigator msgNav = i18Nxml.CreateNavigator();

            XPathNodeIterator msgNodeIter = msgNav.Select(selectMsgQuery);

            while (msgNodeIter.MoveNext())
            {
                String Id = msgNodeIter.Current.GetAttribute("Id", msgNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));

                String MessageString = msgNodeIter.Current.GetAttribute("Msgstring", msgNodeIter.Current.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));

                if(msgs.ContainsKey(Id) == false)
                    msgs.Add(Id, MessageString);
            };

            return msgs;
        }

        #endregion
    }

    #region Struct Message
    public struct RteMessage
    {
        public String Id;
        public String MessageString;
    }
    #endregion




}
