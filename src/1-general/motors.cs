void move (float motorL, float motorR) => bc.Move(motorL, motorR);

void forward (float motor) => move (motor, motor);
void back (float motor) => move (-motor, -motor);
void right (float motor) => move (motor, -motor);
void left (float motor) => move (-motor, motor);

//Global Variables
int error = 0;

