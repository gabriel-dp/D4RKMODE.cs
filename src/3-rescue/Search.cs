byte side_sensor = 2;

void Search () {

	if (ultra(side_sensor) < 150 && !actuator.hasVictim()) {
		stop();
		console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(side_sensor)}<$ zm", color["cyan"]);

		actuator.Up();
		if (actuator.hasVictim()) return;

		//align with the ball
			float last_ultra = 0;
			time.reset();
			do {
				last_ultra = ultra(side_sensor);
				forward(150);
				delay(15);
			} while (ultra(side_sensor) <= last_ultra && time.timer() < 1500);
			if (time.timer() > 1450) return;
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

		if (!actuator.hasVictim()) actuator.Down();

		clear();
	}

}
