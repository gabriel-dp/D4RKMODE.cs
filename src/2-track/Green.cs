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
			if (scaleAngle(direction()) < 7) moveTime(300, 30);
			else moveTime(300, 48);
		//

		//centralizes in the line and verifies again
			GreenClassifier();

			if (green_direction == 'B') {
				while (!isFullBlack(2)) left(1000);
			} else if (green_direction == 'L') {
				while (!isFullBlack(3)) right(1000);
			} else {
				while (!isFullBlack(2)) left(1000);
			}
			GreenClassifier();
			if (green_direction == 'n') {
				moveTime(-300, 200);
				return;
			}
		//

		console(2, $"{green_direction}");

		//goes forward and rotate in the axis to make the curve
			moveTime(300, 450);
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
