byte side_sensor = 3;

void Search () {
	if (sideToSearch == 'R') side_sensor = 2;

	if (ultra(side_sensor) < 150) {

		//align with the ball
			float last_ultra = ultra(side_sensor);
			do {
				last_ultra = ultra(side_sensor);
				forward(150);
				delay(15);
			} while (ultra(side_sensor) <= last_ultra);
		//

		moveTime(-300, 500);
		int angle_rotate = (int) ((180/Math.PI)*(Math.Atan(last_ultra/23)));
		if (sideToSearch == 'L') angle_rotate = -angle_rotate;
		rotate(500, angle_rotate);

		stop(9999);
	}

}
