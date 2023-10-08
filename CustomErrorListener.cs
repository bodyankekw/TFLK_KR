using Antlr4.Runtime;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tf9ik
{
    internal class CustomErrorListener : IAntlrErrorListener<IToken>
    {
        private RichTextBox textBox;
        private int currentLine;

        public CustomErrorListener(RichTextBox textBox, int currentLine)
        {
            this.textBox = textBox;
            this.currentLine = currentLine;
        }
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            textBox.AppendText($"({currentLine}:{charPositionInLine}): {msg}\n");
            Console.WriteLine($"({line}:{charPositionInLine}): {msg}\n");
        }
    }

}
