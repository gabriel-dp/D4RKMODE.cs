void Search (byte sensor) {
	sbyte side_mod = (sbyte) (sensor == 2 ? 1 : -1);

	if (ultra(sensor) < 265 && !actuator.isUp()) {
		stop();
		console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);

		actuator.Up();
		if (actuator.hasVictim() || actuator.hasKit()) return;

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
			moveTime(-300, 500);
			int angleToRotate = (int) ((180/Math.PI)*(Math.Atan(last_ultra/23)));
			int zmToMove = maths.hypotenuse(last_ultra, 23) + 1;
		//

		//go rescue
			rotate(500, angleToRotate*side_mod);
			actuator.Down();
			moveZm(zmToMove);
			actuator.Up();
			stop(150);
			ExpelVictim(false);
			moveZm(-zmToMove);
			rotate(500, -angleToRotate*side_mod);
			centerQuadrant();
		//

		if (!actuator.hasVictim() && !actuator.hasKit()) actuator.Down();

		clear();
	}

}
