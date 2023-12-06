// Inclure la librairie M5 (version pour M5Atom) :
// https://github.com/m5stack/M5Atom
#include <M5Atom.h>

// Inclure la librairie FastLED qui va gérer le pixel :
// https://github.com/FastLED/FastLED
#include <FastLED.h>

// Un tableau qui contient une variable de type CRGB.
// Il y a un seul pixel, mais il doit être dans un tableau.
// CRGB est un type de couleur défini par la lirairie FastLed :
// https://github.com/FastLED/FastLED/wiki/Pixel-reference#crgb-reference
CRGB mesPixels[1];

#include <M5_PbHub.h>
M5_PbHub myPbHub;

#include "Unit_Encoder.h"
Unit_Encoder myEncoder;
int myEncoderPreviousRotation;

#include <MicroOscSlip.h>
MicroOscSlip<1024> myMicroOsc(&Serial);

//<MicroOscUdp.h>
//MicroOscUdp<1024> myMicroOsc(&myUdp,myDestinationIp,myDestinationPort);

#define KEY_UNIT_CHANNEL 4
#define ANGLE_UNIT_CHANNEL 3
#define PIR_UNIT_CHANNEL 2
#define LIGHT_UNIT_CHANNEL 1

//#include <SPI.h>
//#include <Ethernet.h>
// An EthernetUDP instance to let us send and receive packets over UDP
//EthernetUDP myUdp;

//IPAddress myDestinationIp(192, 168, 157, 42);
//unsigned int myDestinationPort = 7001;

//IPAddress myIp(192, 168, 157, 119);
//unsigned int myPort = 7000;


unsigned long monChronoDepart;

void setup() {
  // Démarrer la libraire M5 avec toutes les options de pré-configuration désactivées :
  M5.begin(false, false, false);

  // Démarrer la connexion sérielle :
  Serial.begin(115200);

  // Ajouter le pixel (il y en a un seul) du M5Atom à la librairie FastLED :
  FastLED.addLeds<WS2812, DATA_PIN, GRB>(mesPixels, 1);

  myPbHub.begin();

  myPbHub.setPixelCount(KEY_UNIT_CHANNEL, 1);

  Wire.begin();

  myEncoder.begin();

  // ANIMATION DE DÉMARRAGE
  while (millis() < 5000) {
    mesPixels[0] = CHSV((millis() / 5) % 255, 255, 255 - (millis() * 255 / 5000));
    FastLED.show();
    delay(50);
  }
  mesPixels[0] = CRGB(0, 0, 0);
  FastLED.show();
}

void myOscMessageParser(MicroOscMessage& receivedOscMessage) {
  if (receivedOscMessage.checkOscAddress("/pixel")) {
    int lumiere = receivedOscMessage.nextAsInt();

    myPbHub.setPixelColor(KEY_UNIT_CHANNEL, 0, lumiere, lumiere, lumiere);
 
  } else {
    myPbHub.setPixelColor(KEY_UNIT_CHANNEL, 0, 0, 0, 0);
  }
}


void loop() {
  // TOUJOURS inclure M5.update() au début de loop() :
  M5.update();

  myMicroOsc.onOscMessageReceived(myOscMessageParser);

  if (millis() - monChronoDepart >= 50) {  // SI LE TEMPS ÉCOULÉ DÉPASSE 50 MS...
    monChronoDepart = millis();            // ...REDÉMARRER LE CHRONOMÈTRE...
    //Serial.print(maValeurKey);
    //Serial.print("KEY: ");
    int maValeurKey = myPbHub.digitalRead(KEY_UNIT_CHANNEL);
    // Allumer le pixel du KEY si son bouton est appuyé
    if (maValeurKey == 0) {
      myMicroOsc.sendInt("/key", 1);
    } else {
      myMicroOsc.sendInt("/key", 0);
    }

    //Serial.print(maValeurAngle);
    //Serial.print(" ANGLE: ");
    int maValeurAngle = myPbHub.analogRead(ANGLE_UNIT_CHANNEL);
    myMicroOsc.sendInt("/pot", maValeurAngle);

    //Serial.print(maValeurPir);
    //Serial.print(" PIR: ");
    int maValeurPir = myPbHub.digitalRead(PIR_UNIT_CHANNEL);
    myMicroOsc.sendInt("/motion", maValeurPir);


    //Serial.print(maValeurLight);
    //Serial.print(" LIGHT: ");
    int maValeurLight = myPbHub.analogRead(LIGHT_UNIT_CHANNEL);
    myMicroOsc.sendInt("/light", maValeurLight);


    int encoderRotation = myEncoder.getEncoderValue();
    int encoderRotationChange = encoderRotation - myEncoderPreviousRotation;
    myEncoderPreviousRotation = encoderRotation;
    myMicroOsc.sendInt("/encoder", encoderRotationChange);

    int encoderButton = myEncoder.getButtonStatus();
    // myMicroOsc.sendInt("/buttonEncoder", encoderButton);
    
    if(encoderButton == 1){
      myMicroOsc.sendInt("/buttonEncoder",0);
    }else{
      myMicroOsc.sendInt("/buttonEncoder", 1);
    }

    uint32_t myColorOn = 0xfb6f92;
    uint32_t myColorOff = 0x000000;

    if (encoderButton == 0) {
      myEncoder.setLEDColor(2, myColorOn);
      myEncoder.setLEDColor(1, myColorOn);
    } else {
      if (encoderRotationChange > 0) {
        myEncoder.setLEDColor(1, myColorOn);
        myEncoder.setLEDColor(2, myColorOff);
      } else if (encoderRotationChange < 0) {
        myEncoder.setLEDColor(1, myColorOff);
        myEncoder.setLEDColor(2, myColorOn);
      } else {
        myEncoder.setLEDColor(1, myColorOff);
        myEncoder.setLEDColor(2, myColorOff);
      }
    }
  }
  //Serial.println();
}