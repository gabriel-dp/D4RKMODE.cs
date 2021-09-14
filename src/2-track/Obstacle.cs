void Obstacle () {
	if (DetectWall()) {
		led(color["pink"]);
		reverse(300, 50);
		stop();
		actuator.Up();

		//after lifting the actuator, follows the line for a time to confirm that's a obstacle
			const int timeObstacle = 1750;
			time.reset();
			while (time.timer() < timeObstacle && ultra(1) > 15) {
				LineFollower();
			}

			//if isn't a obstacle, downs the actuator
			if (time.timer() > timeObstacle - 50) {
				stop();
				actuator.Down();
				return;
			}
		//

		//if is a obstacle, starts avoid it
			led(color["purple"]);
			bool surpassed = false;

			void GoForward (bool first = true, bool second = true) {
				while (ultra(3) > 50 && first) FollowerGyro(direction());
				while (ultra(3) < 50 && second) FollowerGyro(direction());
			}

			void AfterAvoid () {
				CentralizeGyro(90);
				reverse(300, 100);
				Centralize();
				actuator.Down();
				surpassed = true;
				clear();
			}

			//	|-> /
				CentralizeGyro(45);
				rotate(500, 20);
				GoForward();

			//	/ -> |
				CentralizeGyro(-90);
				//search for the line in a 90 degress obstacle
					while (ultra(3) > 50 && !surpassed) {
						forward(200);
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
						forward(200);
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