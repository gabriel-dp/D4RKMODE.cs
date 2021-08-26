byte side_sensor = 3;

void Search () {
	if (sideToSearch == 'R') side_sensor = 2;

	if (ultra(side_sensor) < 150) {

		stop();
		actuator.Up();
		if (has_victim) return;

		//align with the ball
			float last_ultra = ultra(side_sensor);
			do {
				last_ultra = ultra(side_sensor);
				forward(150);
				delay(15);
			} while (ultra(side_sensor) <= last_ultra);
		//

		moveTime(-300, 500);
		int angleToRotate = (int) ((180/Math.PI)*(Math.Atan(last_ultra/23)));
		int timeToMove = (int) (Math.Pow(angleToRotate,2)*0.3);
		if (sideToSearch == 'L') angleToRotate = -angleToRotate;


		rotate(500, angleToRotate);
		moveTime(300, timeToMove);

		console(2, timeToMove.ToString());

		stop(9999);
	}

}
