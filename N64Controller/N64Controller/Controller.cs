using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N64Controller
{
    internal sealed class Controller
    {
        public Controller(int deadZone)
        {
            m_deadZone = deadZone;
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[4];
            
            if (ButtonA)
                bytes[0] |= 0x80;
            if (ButtonB)
                bytes[0] |= 0x40;
            if (ButtonZ)
                bytes[0] |= 0x20;
            if (ButtonStart)
                bytes[0] |= 0x10;
            if (ButtonDUp)
                bytes[0] |= 0x08;
            if (ButtonDDown)
                bytes[0] |= 0x04;
            if (ButtonDLeft)
                bytes[0] |= 0x02;
            if (ButtonDRight)
                bytes[0] |= 0x01;

            if (ButtonL)
                bytes[1] |= 0x20;
            if (ButtonR)
                bytes[1] |= 0x10;
            if (ButtonCUp)
                bytes[1] |= 0x08;
            if (ButtonCDown)
                bytes[1] |= 0x04;
            if (ButtonCLeft)
                bytes[1] |= 0x02;
            if (ButtonCRight)
                bytes[1] |= 0x01;

            bytes[2] = (byte)StickX;
            bytes[3] = (byte)StickY;

            return bytes;
        }

        public bool ButtonA { get; set; }
        public bool ButtonB { get; set; }
        public bool ButtonZ { get; set; }
        public bool ButtonL { get; set; }
        public bool ButtonR { get; set; }
        public bool ButtonCUp { get; set; }
        public bool ButtonCDown { get; set; }
        public bool ButtonCLeft { get; set; }
        public bool ButtonCRight { get; set; }
        public bool ButtonDUp { get; set; }
        public bool ButtonDDown { get; set; }
        public bool ButtonDLeft { get; set; }
        public bool ButtonDRight { get; set; }
        public bool ButtonStart { get; set; }
        public int StickX
        {
            get
            {
                return m_stickX;
            }
            set
            {
                if (Math.Abs(value) > 128)
                    throw new ArgumentException("value must be between -128 and 128");
                m_stickX = value;
            }
        }
        public int StickY
        {
            get
            {
                return m_stickY;
            }
            set
            {
                if (Math.Abs(value) > 128)
                    throw new ArgumentException("value must be between -128 and 128");
                m_stickY = value;
            }
        }
        
        int m_stickX;
        int m_stickY;
        int m_deadZone;
    }    
}
