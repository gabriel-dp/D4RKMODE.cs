byte timesFailed = 0;

void Obstacle () {
	if (DetectWall()) {
		console_led(2, "$>Obstáculo<$   Possível", color["pink"]);
		reverse(300, 50);
		stop();
		if (!actuator.hasKit()) actuator.Up();

		//after lifting the actuator, follows the line for a time to confirm that's a obstacle
			const int timeObstacle = 2000;
			time.reset();
			while (time.timer() < timeObstacle && ultra(1) > 15) {
				LineFollower();
			}

			//if isn't a obstacle, downs the actuator
			if (time.timer() > timeObstacle - 50) {
				stop();
				if (!actuator.hasKit()) actuator.Down();

				//avoids the bug of up and down
					timesFailed++;
					if (timesFailed >= 2) {
						bot.ActuatorDown(16);
					}
				//

				return;
			}
		//

		//if is a obstacle, starts avoid it
			console_led(2, "$>Obstáculo<$ Confirmado", color["purple"]);
			bool surpassed = false;
			bool obstructed = false;

			const int timeout = 1250;
			void GoForward (bool first = true, bool second = true) {
				obstructed = false;
				while (ultra(3) > 50 && first) FollowerGyro(direction());
				time.reset();
				while (ultra(3) < 50 && second && time.timer() < timeout) FollowerGyro(direction());
				if (time.timer() > timeout-50) {
					led(color["orange"]);
					moveTime(-300, 50);
					rotate(500, -25);
					moveTime(300, 200);
					rotate(500, -25);
					moveTime(300, 400);
					CentralizeGyro();
					obstructed = true;
				}
			}

			void AfterAvoid () {
				CentralizeGyro(90);
				reverse(300, 100);
				Centralize();
				if (!actuator.hasKit()) actuator.Down();
				surpassed = true;
				clear();
			}

			//	|-> /
				CentralizeGyro(45);
				rotate(500, 20);
				GoForward();

			//	/ -> |
				if (!obstructed) CentralizeGyro(-90);
				//search for the line in a 90 degress obstacle
					while (ultra(3) > 50 && !surpassed) {
						forward(190);
						if (!isWhite(new byte[] {3,4})) {
							led(color["pink"]);
							moveTime(300, 400);
							AfterAvoid();
						}
					}
					if (surpassed) return;
				//
				GoForward(false, true);
				moveTime(300, 100);

			//	| -> \
				CentralizeGyro(-45);
				rotate(500, -20);
				GoForward(true, false);
				//search for the line in a normal obstacle
					while (ultra(3) < 50 && !surpassed) {
						forward(190);
						if (!isWhite(new byte[] {1,2})) {
							led(color["pink"]);
							GoForward();
							moveTime(-300, 150);
							AfterAvoid();
						}
					}
					if (surpassed) return;
				//

			//	\ -> /
				CentralizeGyro(0);
				GoForward();
				CentralizeGyro(-45);
				rotate(500, -20);
				GoForward();
				moveTime(-300, 150);
				AfterAvoid();

		//

	}
}
