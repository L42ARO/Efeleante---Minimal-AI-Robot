#include <Arduino.h>
#include <ardu_setup.h>
#include <light_dir.h>

#define FrontEye A7
#define LeftEye A0
#define RightEye A6
#define BackEye A1
#define enB 5
#define enA 6
#define RMotorFwd 2
#define RMotorBck 4
#define LMotorFwd 7
#define LMotorBck 8

void setup() {
  ardu_setup(enB, enA, RMotorFwd, RMotorBck);  
}


void move(int power, String dir, int power2=NULL, String dir2="*"){
  if (power2==NULL) {
    power2=power;
  }
  if (dir2=="*") {
    dir2=dir;
  }
  analogWrite(enB, power);
  analogWrite(enA, power2);
  if (dir=="FWD"){
    digitalWrite(RMotorFwd, HIGH);
    digitalWrite(RMotorBck, LOW);
  }else if (dir=="BKW"){
    digitalWrite(RMotorFwd, LOW);
    digitalWrite(RMotorBck, HIGH);
  }
  if (dir2=="FWD"){
    digitalWrite(LMotorFwd, HIGH);
    digitalWrite(LMotorBck, LOW);
  }else if (dir2=="BKW"){
    digitalWrite(LMotorFwd, LOW);
    digitalWrite(LMotorBck, HIGH);
  }
}
void stop(){
  digitalWrite(RMotorFwd, LOW);
  digitalWrite(RMotorBck, LOW);
  digitalWrite(LMotorFwd, LOW);
  digitalWrite(LMotorBck, LOW);
}

int loopIter=0;
float startTime=0;
float runTime=0;
void loop() {
  // TIME SETUP
  Manage_Time(&loopIter, &startTime, &runTime);
  //GETTING LIGHT DIRECTION
  int dir = LightDirection(FrontEye, LeftEye, RightEye, BackEye);
  switch (dir)
  {
  case 0:
    stop();
    Serial.println("LOOKING FRONT!");
    break;
  case 1:
    move(255, "FWD", 255, "BKW");
    Serial.println("MOVING LEFT!");
    break;
  case 2:
    move(255, "BKW", 255, "FWD");
    Serial.println("MOVING RIGHT!");
    break;
  case 3:
    move(255, "BKW", 255, "FWD");
    Serial.println("MOVING 360!");
  default:
    break;
  }
  delay(20);
}