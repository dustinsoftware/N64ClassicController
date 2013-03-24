N64ClassicController
=============

Connect any input device to your N64 using just an Arduino and a computer.

### Requirements

* Arduino
* N64 controller cord that you can sacrifice
* A computer to translate your desired input into serial I/O for the Arduino

You will need to know what PORTB corresponds to on your Arduino.  On standard models this should be pin 8.  On the Mega, it's port 53.

I used a Logitech gamepad for my input method.  Visual Studio 2010 Express will work fine for this.  You could use a bluetooth-connected Wii-mote, GlovePIE, and PPJoy to simulate a gamepad.

### Connecting the Arduino
1. The white wire should be connected to PORTB and the black wire should be connected to GND.  **Do not connect the red wire to anything.**
2. Plug in the Arduino to your computer and turn on the N64.  If it complains about no controller, try pressing the reset button on the N64.  If it still isn't working, check your wiring.
3. Build and run the N64Controller application.  Change the COM port in the app.config to the port assigned to your Arduino.  After mapping the buttons, you should be good to go.

### How This Works
The program uploaded to the Arduino simulates a controller by responding to the N64's queries.  The C# program interprits your gamepad's state and turns it into 4 bytes (2 for button state, 2 for analog stick state), which it sends over the serial port when the Arduino asks for it.  Those 4 bytes are then sent to the N64 when it queries the Arduino for controller state.

### Helpful Reading
* https://github.com/brownan/Gamecube-N64-Controller
* https://github.com/maxpowel/Wii-N64-Controller/blob/master/README.rst
