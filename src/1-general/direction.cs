int direction () => (int) bot.Compass();

byte Direction () {
	byte quadrant = 0;
	if (maths.interval(direction(), 46, 135)) quadrant = 1;
	else if (maths.interval(direction(), 136, 225)) quadrant = 2;
	else if (maths.interval(direction(), 226, 315)) quadrant = 3;
	return quadrant;
}

int scaleAngle (int angle) {
	angle = angle%90;
	if (angle >= 45) angle -= 2*(angle-45);
	return angle;
}

//Gyro alignment
	void changeQuadrant (string side) {
		byte last_quadrant = Direction();

		if (side == "back") {
			while (Direction() == last_quadrant) right(1000);
			last_quadrant = Direction();
		}

		if (side == "left") {
			while (Direction() == last_quadrant) left(1000);
		} else {
			while (Direction() == last_quadrant) right(1000);
		}

		stop();
	}

	void GoToAngle (int angle) {
		if (maths.interval(direction() - angle, 0, 140) || maths.interval(direction() - angle, -320, -210)) {
			while (direction() != angle) left(1000);
		} else {
			while (direction() != angle) right(1000);
		}

		stop();
	}

	void centerQuadrant () {
		int ideal_angle = Direction()*90;
		GoToAngle(ideal_angle);
	}

	void CentralizeGyro (int operation = 0) {

		switch (operation) {
			case 0:
				centerQuadrant();
				break;
			case 90:
				changeQuadrant("right");
				centerQuadrant();
				break;
			case -90:
				changeQuadrant("left");
				centerQuadrant();
				break;
			case 180:
				changeQuadrant("back");
				centerQuadrant();
				break;
			case 45:
				changeQuadrant("right");
				break;
			case -45:
				changeQuadrant("left");
				break;
			default:
				break;
		}

	}
//

const int Kg = 20;
void FollowerGyro (int angle = 1000) {
	//error to turn
		int direction_ideal = angle;
		if (angle == 1000) direction_ideal = Direction() * 90;

		int direction_actual = direction();
		if ((direction_ideal >= 0 && direction_ideal < 15) && direction_actual > 300) direction_actual -= 360;
		else if ((direction_ideal > 345 && direction_ideal <= 360) && direction_actual < 300) direction_actual += 360;

		error = direction_actual-direction_ideal;
		turn = error*Kg;

		if (turn > 100) turn = 100;
		else if (turn < -100) turn = -100;
	//

	//turn to motors
		if (Math.Abs(turn) > 20) {
			if (turn > 0) move(-(300*Math.Abs(turn)*0.02f), 300);
			else move(300, -(300*Math.Abs(turn)*0.02f));
		}
		else {
			forward(300);
		}
	//

	printMotors();
}
