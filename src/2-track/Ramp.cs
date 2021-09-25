bool flag_stuck = false;
int time_stuck = 0;

void StuckObstacle () {
	if (inclination() < -8 && (ultra(1) > 400 || ultra(1) < 35) && !actuator.isUp()) {
		if (!flag_stuck) {
			time_stuck = time.millis();
			flag_stuck = true;
		} else {
			if (time.millis() - time_stuck > 7000) {
				led(color["orange"]);
				stop();
				actuator.Up();
				delay(500);
				moveTime(300, 300);
				Obstacle();
				actuator.Down();
				flag_stuck = false;
			}
		}
	} else {
		flag_stuck = false;
	}
}

void Ramp () {
	if (inclination() < -7) {
		console_led(2, "$>Rampa<$ ou $>Gangorra<$", color["blue"]);

		//lifts the actuator and follows the line for a time then down that
			stop();
			if (!actuator.hasKit()) actuator.Up();
			time.reset();
			while (time.timer() < 2000 + 100*scaleAngle(direction())) {
				LineFollower();
			}
			if (!actuator.hasKit()) {
				actuator.Down();
				stop(75);
			} else {
				moveTime(200, 250);
				stop(75);
			}
			int last_inclination = inclination();
			while (inclination() < -2) {
				LineFollower();
				StuckObstacle();
			}
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

	//avoids be stuck in a speed bump after ramp
		if (inclination() > 8 && ultra(1) < 150 && !actuator.hasKit()) {
			if (!flag_stuck) {
				time_stuck = time.millis();
				flag_stuck = true;
			} else {
				if (time.millis() - time_stuck > 3500) {
					actuator.Up();
					moveTime(300, 100);
					actuator.Down();
					flag_stuck = false;
				}
			}
		} else {
			flag_stuck = false;
		}
	//

}
