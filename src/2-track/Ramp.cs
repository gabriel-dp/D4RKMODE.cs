void Ramp () {
	if (inclination() < -7) {
		led(color["blue"]);

		//lifts the actuator and follows the line for a time then down that
			stop();
			actuator.Up();
			time.reset();
			while (time.timer() < 2000 + 100*scaleAngle(direction())) {
				LineFollower();
			}
			actuator.Down();
			int last_inclination = inclination();
			while (inclination() < -2) LineFollower();
			int last_inclination2 = inclination();
		//

		//if is a seesaw needs to wait that down
			if (last_inclination - last_inclination2 > -12) {
				stop(750);
				moveTime(-300, 250);
			} else {
				stop(200);
			}
		//
	}
}
