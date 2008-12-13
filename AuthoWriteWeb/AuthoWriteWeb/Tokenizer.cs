using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace AuthoWriteWeb
{
    /// <summary>
    /// I have to have many ways to tokenize into searchable terms.  Right now the opencyc is a good option.
    /// </summary>
    interface Tokenizer
    {
        StringDictionary Tokenize(string inputText);
    }

    class DefaultStringTokenizer : Tokenizer
    {
        public StringDictionary Tokenize(string inputText)
        {
            Regex RE = new Regex(@"([\+\-\*\(\)\^\\\s])");

            StringDictionary sd = new StringDictionary();
            foreach (string s in RE.Split(inputText))
            {
                sd.Add(s, "");
            }


            return sd;

        }
    }
}
