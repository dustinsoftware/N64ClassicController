using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace N64Controller
{
    internal sealed class SerialSender
    {
        public SerialSender(string port)
        {
            m_port = new SerialPort(port, 115200);            
            m_port.ReadTimeout = 5000;
            m_port.Open();

            return;
        }

        public void SendBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (bytes.Length != 4)
                throw new ArgumentException("exactly 4 bytes must be sent.");

            try
            {
                m_port.DiscardInBuffer();
                m_port.ReadByte();
                //Log.Write(bytes);
                m_port.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Log.Write(e.ToString());
            }
        }

        SerialPort m_port;
    }
}
