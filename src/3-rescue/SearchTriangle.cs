void SearchTriangle (byte sensor) {
	sbyte side_mod = (sbyte) (sensor == 2 ? 1 : -1);

	if (ultra(sensor) < 265 && !actuator.hasVictim()) {
		stop();
		console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);

		timeToFind = time.millis() - timeToFind - 250;

		actuator.Up();
		if (actuator.hasVictim()) return;

		//align with the ball
			float last_ultra = 0;
			time.reset();
			do {
				last_ultra = ultra(sensor);
				forward(150);
				delay(15);
			} while (ultra(sensor) <= last_ultra && time.timer() < 1000);
			if (time.timer() > 950) return;
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
			actuator.Down();
			moveZm(zmToMove);
			actuator.Up();
			stop(150);
		//

		//dispatch in the triangle
			rotate(500, angleToRotate*side_mod);
			while (!isFullBlack(5)) FollowerGyro(direction());
			Dispatch();

			stop(9999);
		//

		if (!actuator.hasVictim()) actuator.Down();

		clear();
	}

}
