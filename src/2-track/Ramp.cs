void Ramp () {
	if (inclination() < -7) {
		led(color["blue"]);
		stop();

		actuator.Up();
		time.reset();
		const int timeToRamp = 2000;

		while (time.timer() < timeToRamp) {
			LineFollower();
		}
		actuator.Down();
		int last_inclination = inclination();

		while (inclination() < -5) LineFollower();
		int last_inclination2 = inclination();

		if (last_inclination - last_inclination2 > -5) {
			stop(750);
			moveTime(-300, 250);
		} else {
			stop(200);
		}
	}
}
