using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpNet.System.Abstract;

namespace SharpNet.System
{
    public class ConsoleBuffer
        : 
        TextWriter
    {

        public ConsoleWriter ConsoleOut { get; set; }
        public ConsoleWriter ConsoleError { get; set; }
        private StreamWriter _streamWriterOut;
        private MemoryStream _memoryStreamOut;
        private StreamWriter _streamWriterError;
        private MemoryStream _memoryStreamError;
        private IList<IDisposable> _theDisposables = new List<IDisposable>();  

        public ConsoleBuffer()
        {
            //initialize the memory stream
            _memoryStreamOut = new MemoryStream();
            _theDisposables.Add(_memoryStreamOut);
            _streamWriterOut = new StreamWriter(_memoryStreamOut);
            _theDisposables.Add(_streamWriterOut);

            _memoryStreamError = new MemoryStream();
            _theDisposables.Add(_memoryStreamError);
            _streamWriterError = new StreamWriter(_memoryStreamError);
            _theDisposables.Add(_streamWriterError);

            this.ConsoleOut = new ConsoleWriter(this.AcceptOut);
            _theDisposables.Add(this.ConsoleOut);
            this.ConsoleError = new ConsoleWriter(this.AcceptError);
            _theDisposables.Add(this.ConsoleError);
            //bind to the console
            Console.SetOut(this.ConsoleOut);
            Console.SetError(this.ConsoleError);
        }

        public void AcceptOut(char val)
        {
            this._streamWriterOut.Write(val);
        }

        public void AcceptError(char val)
        {
            this._streamWriterError.Write(val); 
        }

        private string _flush(MemoryStream ms, StreamWriter sw)
        {
            sw.Flush();
            var data = ms.ToArray();
            var returndata = new byte[data.Length];
            //copy data so it doesn't get collected prematurely
            data.CopyTo(returndata, 0);
            var val = Encoding.Default
                .GetString(returndata);
            //reset the stream
            ms.Seek(0, SeekOrigin.Begin);
            ms.SetLength(0);
            return val;
        }

        public string FlushError()
        {
            return _flush(this._memoryStreamError,
                this._streamWriterError);
        }

        public string FlushOut()
        {
            return _flush(this._memoryStreamOut,
                this._streamWriterOut);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            //dispose of everything else
            if (_theDisposables == null ||
                !_theDisposables.Any())
                return;

            foreach (var theDisposable in _theDisposables)
            {
                try
                {
                    theDisposable.Dispose();
                }
                catch
                {
                }
            }
        }

        public override Encoding Encoding
        {
            get { return ConsoleOut.Encoding; }
        }

        private static object lockobj = new object();

        private static Dictionary<string,ConsoleBuffer>  
            _instances = new Dictionary<string, ConsoleBuffer>();

        public static void Activate(string key)
        {
            if (_instances.ContainsKey(key))
                return;
            lock (lockobj)
            {
                if (_instances.ContainsKey(key))
                    return;

                _instances[key] = new ConsoleBuffer();
            }
        }

        public static void Deactivate(string key)
        {
            if (!_instances.ContainsKey(key))
                return;

            _instances.Remove(key);
        }

        public static ConsoleBuffer Instance(string key)
        {
            return !_instances.ContainsKey(key) ? null : _instances[key];
        }
    }
}
