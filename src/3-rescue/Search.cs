byte side_sensor = 3;

void Search () {
	if (sideToSearch == 'R') side_sensor = 2;

	if (ultra(side_sensor) < 150 && actuator.victim() == null) {

		stop();
		actuator.Up();
		if (actuator.victim() != null) return;

		//align with the ball
			float last_ultra = 0;
			do {
				last_ultra = ultra(side_sensor);
				forward(150);
				delay(15);
			} while (ultra(side_sensor) <= last_ultra);
		//

		//triangle calculation
			moveTime(-300, 500);
			int angleToRotate = (int) ((180/Math.PI)*(Math.Atan(last_ultra/23)));
			int zmToMove = maths.hypotenuse(last_ultra, 23) + 1;
			if (sideToSearch == 'L') angleToRotate = -angleToRotate;
		//

		//go rescue
			rotate(500, angleToRotate);
			actuator.Down();
			moveZm(zmToMove);
			actuator.Up();
			stop(150);
			moveZm(-zmToMove);
			rotate(500, -angleToRotate);
			centerQuadrant();
		//

		if (actuator.victim() == null) actuator.Down();
		else if (actuator.victim() == "alive") first_check_alive = true;

	}

}
