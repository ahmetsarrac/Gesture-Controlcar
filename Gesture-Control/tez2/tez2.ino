
#include "MeAuriga.h"
#include <SoftwareSerial.h>

int d=10;
char btdata;


MeEncoderOnBoard Encoder_1(SLOT1);
MeEncoderOnBoard Encoder_2(SLOT2);

void isr_process_encoder1(void)
{
  if(digitalRead(Encoder_1.getPortB()) == 0)
  {
    Encoder_1.pulsePosMinus();
  }
  else
  {
    Encoder_1.pulsePosPlus();;
  }
}

void isr_process_encoder2(void)
{
  if(digitalRead(Encoder_2.getPortB()) == 0)
  {
    Encoder_2.pulsePosMinus();
  }
  else
  {
    Encoder_2.pulsePosPlus();
  }
}

void setup()
{
  attachInterrupt(Encoder_1.getIntNum(), isr_process_encoder1, RISING);
  attachInterrupt(Encoder_2.getIntNum(), isr_process_encoder2, RISING);
  Serial.begin(115200);
  Serial.print("bluetooth başlatılıyor");

  TCCR1A = _BV(WGM10);
  TCCR1B = _BV(CS11) | _BV(WGM12);

  TCCR2A = _BV(WGM21) | _BV(WGM20);
  TCCR2B = _BV(CS21);
  
}
 
void loop()
{
  
  if(Serial.available())
  {
   btdata = Serial.read();
 
   if (btdata == 'F') {
    Encoder_1.setTarPWM(200);
    Encoder_2.setTarPWM(-200);

   }
   else if (btdata == 'B') {
    Encoder_1.setTarPWM(-200);
    Encoder_2.setTarPWM(200);

   }
   else if (btdata == 'M') {
    Encoder_1.setTarPWM(-100);
    Encoder_2.setTarPWM(-100);

   }
   else if (btdata == 'N') {
    Encoder_1.setTarPWM(100);
    Encoder_2.setTarPWM(100);
   }
   else if (btdata=='S'){
    Encoder_1.setTarPWM(0);
    Encoder_2.setTarPWM(0);
   }
  }
  Encoder_1.loop();
  Encoder_2.loop();
}
