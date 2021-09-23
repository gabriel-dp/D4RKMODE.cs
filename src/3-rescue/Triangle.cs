char side_triangle = 'n';
int timeToFind = 0;

void Triangle () {

	bool TriRight () {
		if (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 97 && ultra(2) < 55) {
			side_triangle = 'R';
			return true;
		} else return false;
	}

	bool TriLeft () {
		if (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 97 && ultra(3) < 55) {
			side_triangle = 'L';
			return true;
		} else return false;
	}

	if (TriRight() || TriLeft()) {

		//verifies if is the triangle
			time.reset();
			float last_ultra = ultra(1);
			do {
				FollowerGyro();
				if (last_ultra - 2 > ultra(1)) return;
			} while (time.timer() < 150);
		//

		console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);
		sbyte side_mod = (sbyte) (side_triangle == 'L' ? 1 : -1);

		//lifts the actuator and dispatches if it has a victim
			stop();
			actuator.Up();
			if (actuator.hasVictim() || actuator.hasKit()) {
				Dispatch();
			}
		//

		//position the robot in the side of the triangle
			GoToDistance(95);
			CentralizeGyro(90 * side_mod);
			reverse(300, 750);
			actuator.Down();
		//

		//search for victims
			VictimInEnd:
			bool wall_ahead = (ultra(1) < 400);
			timeToFind = time.millis();
			while ((wall_ahead && !DetectWall()) || (!wall_ahead && isWhite(new byte[] {1,2,3,4}))) {
				FollowerGyro();
				Ultras(true, false, "triangle");
			}

			//wall or line
				actuator.Up();
				if (actuator.hasVictim()) {
					SearchTriangle(2, true);
					goto VictimInEnd;
				}
			//

		//

		//search for the exit
			int mid_arena = (time.millis()-500-timeToFind)/2;
			console(2, $"{time.millis() - timeToFind} | {mid_arena}");

			if (DeadVictimReserved) { //rescue the remanescent dead victim
				reverse(300);
				moveZm(95);
				CentralizeGyro(-90 * side_mod);

				while (!DetectWall()) FollowerGyro();
				stop();
				actuator.Up();
				CentralizeGyro(-90 * side_mod);
				Dispatch();

				CentralizeGyro(90 * side_mod);
				GoToDistance(95);
				reverse(300, 1250);

				moveTime(300, mid_arena);
			} else moveTime(-300, mid_arena);


			FindTheExitAgain:

			byte whichSensor = 0;
			while (true) {
				right(1000 * side_mod);
				for (byte i = 0; i < 4; i++) {
					if (ultra(i) > 400) {
						whichSensor = i;
						break;
					}
				}
				if (whichSensor != 0) break;
			}

			time.reset();
			while (ultra(whichSensor) > 400) right(1000*side_mod);
			int timeDetecting = time.timer();
			time.reset();
			while (time.timer() < timeDetecting/2) left(1000*side_mod);

			if (whichSensor != 1) rotate(500, 90*side_mod);
			stop();
			open_actuator = false;
			actuator.Down();

			time.reset();
			while (isWhite(new byte[] {2,3})) FollowerGyro(direction());
			int timeToBack = time.timer();

			moveTime(300, 50);
			if (anySensorColor("blue") && !isThatColor(2, "GREEN") && !isThatColor(3, "GREEN")) {
				led(color["orange"]);
				moveTime(-300, timeToBack);
				if (whichSensor != 1) rotate(500, -90*side_mod);
				while (ultra(whichSensor) > 400) right(1000*side_mod);
				right(1000*side_mod);
				delay(250);
				goto FindTheExitAgain;
			} else {
				led(color["green_dark"]);
				moveTime(300, 300);
				Centralize();
				local = Local.exit;
			}
		//

	}

}

