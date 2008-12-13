using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace AuthoWriteWeb
{
    public class QueryProcessor
    {

        public QueryProcessor(string inputText)
        {
            this.inputText = Strip(inputText);
            Tokenizer tokenizer = new DefaultStringTokenizer();
            searchTerms = tokenizer.Tokenize(this.inputText);
        }
        public string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        private StringDictionary searchTerms;

        public StringDictionary SearchTerms
        {
            get { return searchTerms; }
            set { searchTerms = value; }
        }

        private string inputText;
    }
}
