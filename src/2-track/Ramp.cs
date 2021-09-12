bool flag_ramp = false;
int time_ramp = 0;

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

	if (inclination() > 17 && !flag_ramp) {
		flag_ramp = true;
		open_actuator = true;
		actuator.Down();
		time_ramp = time.millis();

	}

	if (flag_ramp && time.millis() - time_ramp > 20000) {
		bot.CloseActuator();
		flag_ramp = false;
	}
}
