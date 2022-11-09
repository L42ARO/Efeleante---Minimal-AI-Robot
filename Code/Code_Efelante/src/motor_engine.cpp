#include "motor_engine.h"
#include "Arduino.h"

MotorEngine::MotorEngine(int enB, int enA, int RMotorFwd, int RMotorBck, int LMotorFwd, int LMotorBck) {
    this->enB = enB;
    this->enA = enA;
    this->RMotorFwd = RMotorFwd;
    this->RMotorBck = RMotorBck;
    this->LMotorFwd = LMotorFwd;
    this->LMotorBck = LMotorBck;
}