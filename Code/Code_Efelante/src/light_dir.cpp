#include "Arduino.h"
#include "light_dir.h"

int LightDirection(int FrontEye, int LeftEye, int RightEye, int BackEye) {
  // put your main code here, to run repeatedly:
  float eyes[4] = {
    analogRead(FrontEye),
    analogRead(LeftEye),
    analogRead(RightEye),
    analogRead(BackEye)
  };
  float maxEye[2]={0, eyes[0]}; // First is the index of max value then it's the max value itself
  for (int i = 1; i < 4; i++) {
    if(eyes[i] > maxEye[1]) {
      maxEye[0] = (float)i;
      maxEye[1] = eyes[i];
    }
  }
  String dir=(maxEye[0]==0)?"Front":(maxEye[0]==1)?"Left":(maxEye[0]==2)?"Right":(maxEye[0]==3)?"Back":"";
  String txtToPrint=dir+": {"+String(eyes[0],2)+","+String(eyes[1],2)+","+String(eyes[2],2)+","+String(eyes[3],2)+"}";
  // Serial.println(txtToPrint);
  return  int(maxEye[0]);
}