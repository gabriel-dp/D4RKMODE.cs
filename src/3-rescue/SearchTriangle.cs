void SearchTriangle (byte sensor, bool alreadyInActuator = false) {
	sbyte side_mod = (sbyte) (side_triangle == 'L' ? 1 : -1);

	if ((ultra(sensor) < 265 && !actuator.hasVictim()) || alreadyInActuator) {
		stop();
		console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);

		timeToFind = time.millis() - timeToFind - 250;

		actuator.Up();
		if (actuator.hasVictim()) {
			reverse(300);
			CentralizeGyro(-90*side_mod);
			Dispatch();
		} else {

			//align with the ball
				float last_ultra = 0;
				time.reset();
				do {
					last_ultra = ultra(sensor);
					forward(150);
					delay(15);
				} while (ultra(sensor) <= last_ultra && time.timer() < 500);
				if (time.timer() > 450) {
					actuator.Down();
					return;
				}
			//

			//triangle calculation
				int zmToMove = (int)(last_ultra+1);

				int twoLegs = timeByZm(timeToFind);
				float prop = last_ultra/50;
				int bigLeg = (int)((prop*twoLegs)/(1+prop));

				int angleToRotate = 180 - (int) ((180/Math.PI)*(Math.Atan(bigLeg/last_ultra)));
				console(2, $"{twoLegs} | {prop} | {bigLeg} | {angleToRotate}");
			//

			//go rescue
				CentralizeGyro(90*side_mod);
				if (last_ultra < 30) {
					moveTime(-300, 500);
					actuator.Down();
					moveTime(300, 500);
				} else actuator.Down();
				moveZm(zmToMove);
				actuator.Up();
				stop(150);
			//

			//dispatch in the triangle
				rotate(500, angleToRotate*side_mod);
				while (!isFullBlack(5)) FollowerGyro(direction());
				Dispatch();

				if (angleToRotate < 135) rotate(500, (180-angleToRotate)*side_mod);
				CentralizeGyro();
			//
		}

		GoToDistance(95);
		if (side_triangle == 'R') CentralizeGyro(-90);
		else CentralizeGyro(90);
		reverse(300, 750);
		actuator.Down();
		timeToFind = time.millis();

		clear();
	}

}
