const float Kp = 1;
const float Kd = 20;

const int vel_front = 300;
const int vel_axis = 1000;
const int motor_limit = 185;

const int turn_axis = 50;
const int turn_normal = 15;
const float turn_coefficient = 0.01f;

int last_zero = 0;

void Follower (bool led_on = true) {

	//error to turn
		error = light(2)-light(3);

		int P = (int) (error * Kp);
		int D = (int) ((last_error - error) * Kd);
		if (last_error != error) last_error = error;
		turn = P + D;

		if (turn > 100) turn = 100;
		else if (turn < -100) turn = -100;
	//

	//turn to motors
		if (Math.Abs(turn) > turn_axis) {
			left(vel_axis*turn);
			last_zero = time.millis();
		}
		else if (Math.Abs(turn) > turn_normal) {
			if (turn > 0) move(-(vel_front*Math.Abs(turn)*turn_coefficient), vel_front);
			else move(vel_front, -(vel_front*Math.Abs(turn)*turn_coefficient));
			last_zero = time.millis();
		}
		else {
			forward(motor_limit);
		}
	//

	//maybe lost the line
		if (time.millis() - last_zero > 1000 && scaleAngle(direction()) > 2) {
			CentralizeGyro();
			last_zero = time.millis();
		}
	//

	if (led_on) led(color["white"]);
	//printMotors();

}

void LineFollower () {

	CurveBlack();
	Green();
	Follower();

}
