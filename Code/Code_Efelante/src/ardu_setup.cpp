#include "ardu_setup.h"
#include "Arduino.h"

void ardu_setup(int enB, int enA, int RMotorFwd, int RMotorBck) {
    Serial.begin(9600);
    pinMode(enB, OUTPUT);
    pinMode(RMotorFwd, OUTPUT);
    pinMode(RMotorBck, OUTPUT);
    pinMode(enA, OUTPUT);

    digitalWrite(RMotorFwd, HIGH);
    digitalWrite(RMotorBck, LOW);
    digitalWrite(LED_BUILTIN, HIGH);
}
void Manage_Time(int* loopIter, float* startTime, float* runTime) {
    if (*loopIter==0) *startTime=millis();
    *runTime=millis()-*startTime;
    loopIter++;
}