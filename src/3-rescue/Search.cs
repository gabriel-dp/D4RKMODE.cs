byte side_sensor = 3;

void Search () {
	if (sideToSearch == 'R') side_sensor = 2;

	if (ultra(side_sensor) < 150 && !has_victim) {

		stop();
		actuator.Up();
		if (has_victim) return;

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
			int timeToMove = (int) (Math.Pow(angleToRotate,2)*0.3);
			if (sideToSearch == 'L') angleToRotate = -angleToRotate;
		//

		//go rescue
			rotate(500, angleToRotate);
			actuator.Down();
			moveTime(300, timeToMove);
			actuator.Up();
			stop(150);
			moveTime(-300, timeToMove);
			rotate(500, -angleToRotate);
			centerQuadrant();
		//

		if (!has_victim) actuator.Down();

	}

}
