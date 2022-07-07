#include<Uduino.h>
#include<Encoder.h>

struct Button {
  byte pin;
  unsigned long lastPressed;
  
  public:  
    Button (byte buttonPin) {
      pin = buttonPin;
      pinMode(buttonPin, INPUT_PULLUP);
      lastPressed = 0;
    }

    bool read_down() {
      bool pressed = false;
      if (digitalRead(pin) == LOW) {
        pressed = (millis() - lastPressed > 50);
        lastPressed = millis();
      }
      return pressed;
    }

    bool read() {
      return digitalRead(pin) == LOW;
    }
};

// ====================================================== //

Uduino uduino("Uno");

Encoder encoders[] = {Encoder(2, 4), Encoder(3, 5)};
Button buttons[] = {Button(6), Button(7), Button(A2), Button(A3), Button(A4)};

#define Joy_XAXIS A1
#define Joy_YAXIS A0
#define Joy_LIMIT 344.5

byte LEDPins[] = {9, 10, 11};

// ====================================================== //

void setup() {
  Serial.begin(9600);
  Serial.println("Buttons:,Analog_X:,Analog_Y:,Encoder_1:,Encoder_2:");
  
  uduino.addCommand("SetLED", set_led);
  uduino.addCommand("SetLEDs", set_leds);
  for (int LEDPin : LEDPins) pinMode(LEDPin, OUTPUT);
}

void loop() {
  uduino.update();

  byte buttonStates = 0;
  for (int i = 0; i < 5; i++) buttonStates += (1 << i) * buttons[i].read();
  
  Serial.print(buttonStates);
  Serial.print(',');
  Serial.print(analogRead(Joy_XAXIS) / Joy_LIMIT - 1.0, 8);
  Serial.print(',');
  Serial.print(analogRead(Joy_YAXIS) / Joy_LIMIT - 1.0, 8);
  Serial.print(',');
  Serial.print(-encoders[0].read() / 4);
  Serial.print(',');
  Serial.print(-encoders[1].read() / 4);

  Serial.println("");
}

void set_led() {
  int parameters = uduino.getNumberOfParameters();
  if(parameters >= 2) {
    int valueOne = uduino.charToInt(uduino.nextParameter());
    int valueTwo = uduino.charToInt(uduino.nextParameter());
    analogWrite(LEDPins[valueOne], valueTwo);
  }
}

void set_leds() {
  int parameters = uduino.getNumberOfParameters();
  if(parameters >= 3) {
    analogWrite(LEDPins[0], uduino.charToInt(uduino.nextParameter()));
    analogWrite(LEDPins[1], uduino.charToInt(uduino.nextParameter()));
    analogWrite(LEDPins[2], uduino.charToInt(uduino.nextParameter()));
  }
}
