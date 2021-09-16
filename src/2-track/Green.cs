char green_direction = 'n';
void GreenClassifier () {
	green_direction = 'n';
	if ((isGreen(1) || isGreen(2)) && (isGreen(3) || isGreen(4))) {
		green_direction = 'B';
	} else if (isGreen(1) || isGreen(2)) {
		green_direction = 'R';
	} else if (isGreen(3) || isGreen(4)){
		green_direction = 'L';
	}
}

void Green () {
	if (isGreen(1) || isGreen(2) || isGreen(3) || isGreen(4)) {
		led(color["green"]);

		//goes a bit forward to detect dead ends
			int timeGreen = 15;
			if (scaleAngle(direction()) > 20) timeGreen = 45;
			else if (scaleAngle(direction()) > 7) timeGreen = 30;

			time.reset();
			while (time.timer() < timeGreen) Follower(false);

			if (isFullBlack(new byte[] {1, 4})) moveTime(-300, 16);
		//

		//centralizes in the line and verifies again
			GreenClassifier();
			if (green_direction == 'B') {
				while (!isFullBlack(2)) left(1000);
				rotate(500, 2);
			} else if (green_direction == 'L') {
				while (!isFullBlack(3)) right(1000);
				rotate(500, -2);
			} else {
				while (!isFullBlack(2)) left(1000);
				rotate(500, 2);
			}
			stop(50);
			GreenClassifier();

			if (green_direction == 'n') {
				led(color["orange"]);
				reverse(300, 175);
				return;
			}
		//

		console(2, $"{green_direction}");

		//goes forward and rotate in the axis to make the curve
			moveTime(300, 425);
			if (green_direction == 'B') {
				CentralizeGyro(180);
			} else {
				if (green_direction == 'R') {
					if (scaleAngle(direction()) < 25) CentralizeGyro(45);
					while (!isFullBlack(3) && !isFullBlack(4)) right(1000);
				} else {
					if (scaleAngle(direction()) < 25) CentralizeGyro(-45);
					while (!isFullBlack(2) && !isFullBlack(1)) left(1000);
				}
			}
			Centralize();
		//

		clear();
	}
}
