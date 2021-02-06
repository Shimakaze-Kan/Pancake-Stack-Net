using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IDE
{
    public class ValidateSourceCode
    {
        private string[] _sourceCode;

        public string SourceCode
        {
            get { return string.Join(Environment.NewLine, _sourceCode); }
        }


        public ValidateSourceCode(string rawSourceCode)
        {
            _sourceCode = RemoveWhiteCharactersAndComments(rawSourceCode);
        }

        private string[] RemoveWhiteCharactersAndComments(string rawSourceCode)
        {
            Regex matchMultipleSpaces = new Regex(@"[ ]{2,}", RegexOptions.None);

            rawSourceCode = Regex.Replace(rawSourceCode.Replace('\t', ' '), @" +", " ");
            return rawSourceCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(x => Regex.Replace(x, @"\/\/.*", string.Empty).Trim()).ToArray();
        }

        public Tuple<bool, int> ValidateInstructions()
        {
            var sourceCode = _sourceCode.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < sourceCode.Length; i++)
            {
                switch (sourceCode[i])
                {
                    case var word when new Regex(@"Put this ([^ ]*?) pancake on top!").IsMatch(word):
                        break;
                    case "Eat the pancake on top!":
                        break;
                    case "Put the top pancakes together!":
                        break;
                    case "Give me a pancake!":
                        break;
                    case "How about a hotcake?":
                        break;
                    case "Show me a pancake!":
                        break;
                    case "Take from the top pancakes!":
                        break;
                    case "Flip the pancakes on top!":
                        break;
                    case "Put another pancake on top!":
                        break;
                    case var label when new Regex(@"\[(.*)\]").IsMatch(label):
                        break;
                    case var label when new Regex(@"If the pancake isn't tasty, go over to ""(.*)"".").IsMatch(label):
                        break;
                    case var label when new Regex(@"If the pancake is tasty, go over to ""(.*)"".").IsMatch(label):
                        break;
                    case "Put syrup on the pancakes!":
                        break;
                    case "Put butter on the pancakes!":
                        break;
                    case "Take off the syrup!":
                        break;
                    case "Take off the butter!":
                        break;
                    case "Show me a numeric pancake!":
                        break;
                    case "Eat all of the pancakes!":
                        break;
                    default:
                        return new Tuple<bool, int>(false, i);
                }
            }

            return new Tuple<bool, int>(true, 0);
        }

        public Dictionary<int,int> MapRealLineNumbersWithRaw()
        {
            int lineNumer = 0;
            Dictionary<int, int> mapCollection = new Dictionary<int, int>();

            for (int i = 0; i < _sourceCode.Length; i++)
            {
                if (!string.IsNullOrEmpty(_sourceCode[i]))
                    mapCollection[lineNumer++] = i;
            }

            return mapCollection;
        }
    }
}
