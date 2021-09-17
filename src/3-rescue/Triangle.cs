char side_triangle = 'n';

void Triangle () {

	bool TriRight () {
		if (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 75 && ultra(2) < 55) {
			side_triangle = 'R';
			return true;
		} else return false;
	}

	bool TriLeft () {
		if (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 75 && ultra(3) < 55) {
			side_triangle = 'L';
			return true;
		} else return false;
	}

	if (TriRight() || TriLeft()) {

		//verify if is the triangle
			time.reset();
			float last_ultra = ultra(1);
			do {
				FollowerGyro();
				if (last_ultra - 2 > ultra(1)) return;
			} while (time.timer() < 150);
		//

		console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);

		stop();
		actuator.Up();

		if (actuator.hasVictim()) {
			moveTime(-200, 600);
			actuator.Adjust(20, 0);
			stop(100);
			moveTime(300, 600);
			stop(150);
			moveTime(-300, 200);
			actuator.Up();
			CentralizeGyro();
		}

		GoToDistance(95);
		if (side_triangle == 'R') CentralizeGyro(-90);
		else CentralizeGyro(90);
		reverse(300, 750);
		actuator.Down();

		stop(9999);


	}

}

