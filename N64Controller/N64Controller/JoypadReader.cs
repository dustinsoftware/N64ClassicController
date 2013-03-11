using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.DirectInput;
using System.Threading;
using System.Xml.Linq;
using System.Configuration;
using Newtonsoft.Json;

namespace N64Controller
{
    internal sealed class JoypadReader
    {
        public JoypadReader(Controller controller)
        {
            m_controller = controller;
            
            var directInput = new DirectInput();
            Guid joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            if (joystickGuid == Guid.Empty)
            {
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;
            }

            if (joystickGuid == Guid.Empty)
                throw new InvalidOperationException("Gamepad not found.");                

            m_joystick = new Joystick(directInput, joystickGuid);
            m_joystick.Properties.BufferSize = 128;
            m_joystick.Acquire();

            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            m_mappings = new Dictionary<string, List<ButtonState>>();
            if (configManager.AppSettings.Settings["ButtonsJson"] == null)
            {
                var buttons = new[] { "ButtonA", "ButtonB", "ButtonZ", "ButtonL", "ButtonR", "ButtonCUp", "ButtonCDown", "ButtonCLeft", "ButtonCRight", "ButtonDUp", "ButtonDDown", "ButtonDLeft", "ButtonDRight", "ButtonStart" };
                foreach (var button in buttons)
                {
                    Log.Write("Please press the button for " + button);
                    string buttonPress = GetButtonEvent(m_joystick);
                    Log.Write("Please release the button for " + button);
                    string buttonRelease = GetButtonEvent(m_joystick);

                    if (!m_mappings.ContainsKey(buttonPress))
                        m_mappings.Add(buttonPress, new List<ButtonState>());
                    m_mappings[buttonPress].Add(new ButtonState { Name = button, State = true });

                    if (!m_mappings.ContainsKey(buttonRelease))
                        m_mappings.Add(buttonRelease, new List<ButtonState>());
                    m_mappings[buttonRelease].Add(new ButtonState { Name = button, State = false });
                }

                configManager.AppSettings.Settings.Add("ButtonsJson", JsonConvert.SerializeObject(m_mappings));
                configManager.Save(ConfigurationSaveMode.Modified);
            }
            
            m_mappings = JsonConvert.DeserializeObject<Dictionary<string, List<ButtonState>>>(configManager.AppSettings.Settings["ButtonsJson"].Value.ToString());            
        }

        public void Read()
        {
            m_joystick.Poll();
            var data = m_joystick.GetBufferedData();
            foreach (var state in data)
            {
                List<ButtonState> states;
                m_mappings.TryGetValue(state.Offset.ToString() + state.Value.ToString(), out states);

                if (states != null)
                {
                    foreach (var button in states)
                        m_controller.GetType().GetProperty(button.Name).SetValue(m_controller, button.State, null);
                }

                else if (state.Offset.ToString() == Settings.Default.StickX)
                    m_controller.StickX = Math.Abs(state.Value - 32768) < Settings.Default.Deadzone ? 0 : (state.Value / 256) - 128;
                else if (state.Offset.ToString() == Settings.Default.StickY)
                    m_controller.StickY = Math.Abs(state.Value - 32768) < Settings.Default.Deadzone ? 0 : 127 - (state.Value / 256);
                else if (state.Offset.ToString() == Settings.Default.CStickX)
                {
                    m_controller.ButtonCLeft = state.Value - 32768 < -Settings.Default.CButtonDeadzone;
                    m_controller.ButtonCRight = state.Value - 32768 > Settings.Default.CButtonDeadzone;
                }
                else if (state.Offset.ToString() == Settings.Default.CStickY)
                {
                    m_controller.ButtonCUp = state.Value - 32768 < -Settings.Default.CButtonDeadzone;
                    m_controller.ButtonCDown = state.Value - 32768 > Settings.Default.CButtonDeadzone;
                }
                else
                    Log.Write("Warning: " + state.Offset + " is not mapped to any controller button.");
            }
        }

        private sealed class ButtonState
        {
            public string Name { get; set; }
            public Boolean State { get; set; }
        }

        private static string GetButtonEvent(Joystick joystick)
        {
            while (true)
            {
                joystick.Poll();
                var data = joystick.GetBufferedData();
                if (data.Count(x => x.Offset != JoystickOffset.X && x.Offset != JoystickOffset.Y && x.Offset != JoystickOffset.Z && x.Offset != JoystickOffset.RotationZ) != 0)
                {
                    var buttonId = data.First(x => x.Offset != JoystickOffset.X && x.Offset != JoystickOffset.Y && x.Offset != JoystickOffset.Z && x.Offset != JoystickOffset.RotationZ);
                    return buttonId.Offset.ToString() + buttonId.Value.ToString();
                }
                Thread.Sleep(10);
            }
        }

        Joystick m_joystick;
        Controller m_controller;
        Dictionary<string, List<ButtonState>> m_mappings;
    }
}
