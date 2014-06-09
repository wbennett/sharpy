using System;
using System.IO;
using System.Text;
using SharpNet.System.Abstract;

namespace SharpNet.System
{
    public class ConsoleWriter
        : TextWriter
    {

        public Action<char> Handler { get; set; }

        public ConsoleWriter(IAcceptChars acceptor)
        {
            Handler = acceptor.Accept;
        }

        public ConsoleWriter(Action<char> handler)
        {

            this.Handler = handler;
        }

        public override void Write(char value)
        {
            Handler(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}