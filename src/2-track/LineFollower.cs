const float Kp = 1;
const float Kd = 20;

const int vel_front = 300;
const int vel_axis = 1000;
const int motor_limit = 190;

const int turn_axis = 50;
const int turn_normal = 15;
const float turn_coefficient = 0.01f;

void LineFollower () {

	CurveBlack();

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
		}
		else if (Math.Abs(turn) > turn_normal) {
			if (turn > 0) move(-(vel_front*Math.Abs(turn)*turn_coefficient), vel_front);
			else move(vel_front, -(vel_front*Math.Abs(turn)*turn_coefficient));
		}
		else {
			forward(motor_limit);
		}
	//

	led(color["white"]);
	//printMotors();

}
