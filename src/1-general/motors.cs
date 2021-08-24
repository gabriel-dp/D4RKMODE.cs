void move (float motor_L, float motor_R) => bot.Move(motor_L, motor_R);

void forward (float motor) => move (motor, motor);
void back (float motor) => move (-motor, -motor);
void right (float motor) => move (motor, -motor);
void left (float motor) => move (-motor, motor);

void stop (int time = 0) {
	move(0, 0);
	delay(time);
}

//Global Variables
int error = 0;
int last_error = 0;
int turn = 0;
int motorR = 0;
int motorL = 0;
