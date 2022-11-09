#ifndef MOTOR_ENGINE_H
#define MOTOR_ENGINE_H
class MotorEngine{
    public:
        MotorEngine(int enB, int enA, int RMotorFwd, int RMotorBck, int LMotorFwd, int LMotorBck);
        void stop();
    private:
        int enB;
        int enA;
        int RMotorFwd;
        int RMotorBck;
        int LMotorFwd;
        int LMotorBck;
};
#endif