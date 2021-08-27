//simplified basic commands
void move (float motor_L, float motor_R) => bot.Move(motor_L, motor_R);

void forward (float motor) => move (motor, motor);
void back (float motor) => move (-motor, -motor);
void right (float motor) => move (motor, -motor);
void left (float motor) => move (-motor, motor);

//More methods
void rotate (float motor, int angle) {
	int angleToGo = (direction() + angle)%360;
	if (angleToGo < 0) angleToGo = 360 + angleToGo;
	if (angle > 0) while (direction() != angleToGo) right(1000);
	else while (direction() != angleToGo) left(1000);
	stop();
}

void stop (int ms = 0) {
	move(0, 0);
	delay(ms);
}

void moveTime (float motor, int ms) {
	time.reset();
	while (time.timer() < ms) forward(motor);
	stop();
}

const float timePerZm = 16.65f;
void moveZm (int zm) {
	int timeToMove = (int)(zm*timePerZm);
	if (zm > 0) moveTime(300, timeToMove);
	else moveTime(-300, -timeToMove);
}

void reverse (float motor, int ms = 999999) {
	time.reset();
	while (!bot.Touch(0) && time.timer() < ms) back(motor);
	stop();
}

//Global Variables
int error = 0;
int last_error = 0;
int turn = 0;
int motorR = 0;
int motorL = 0;
