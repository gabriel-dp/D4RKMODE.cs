const float Kp = 1;
const float Kd = 20;

const int vel_max_front = 300;
const int vel_max_axis = 1000;

const int turn_divider = 10;
const int motor_limit = 100;
const int motor_axis_min = 150;



void LineFollower () {

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

	//

	//move(motorR, motorL);
	led(color["white"]);

}
