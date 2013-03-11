using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace N64Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sender = new SerialSender(Settings.Default.COMPort);
                var controller = new Controller(Settings.Default.Deadzone);
                var reader = new JoypadReader(controller);

                Log.Write("App startup completed.  Now running.");
                byte[] lastBytes = null;
                while (true)
                {
                    reader.Read();
                    byte[] bytes = controller.GetBytes();
                    if (!bytes.IsEquivalentTo(lastBytes))
                    {
                        sender.SendBytes(bytes);
                        lastBytes = bytes;
                    } 
                }
            }
            catch (InvalidCastException ex)
            {
                Log.Write(ex.ToString());
                throw;
            }
        }
    }
}
