void CurveBlack () {
	char curve_side = 'n';
	if (!isWhite(1) && !isColorized(1)) {
		console_led(2, "$>Curva<$ para $>Direita<$", color["gray"], color["black"]);
		console(3, "→");
		curve_side = 'R';
	} else if (!isWhite(4) && !isColorized(4)) {
		console_led(2, "$>Curva<$ para $>Esquerda<$", color["gray"], color["black"]);
		console(3, "←");
		curve_side = 'L';
	}

	if (curve_side != 'n') {

		byte timesLost = 0;

		//verifies if sensor misread green or blue
			moveTime(200, 15);
			if (isWhite(1) && isWhite(4)) {
				reverse(300, 100);
				if (isColorized(1) || isColorized(4)) {
					if ((colors.B(1) > colors.R(1)) || (colors.B(4) > colors.R(4))) {
						CentralizeGyro();
						reverse(300, 200);
					}
				}
				return;
			}
			GreenClassifier();
			if (green_direction != 'n') {
				return;
			}
		//

		//avoids lost the line in the seesaw
			if (Math.Abs(inclination()) > 5) {
				CentralizeGyro();
				stop(500);
			}
		//

		//tries to centralize and avoid "false curves" then goes forward and rotate in the axis
			Centralize();

			if ((!isWhite(1) && !isColorized(1)) || (!isWhite(4) && !isColorized(4))) {
				moveTime(300, 315);

				//avoids lost the line in the seesaw
					if (Math.Abs(inclination()) > 5) {
						CentralizeGyro();
						reverse(300, 300);
						stop(200);
						return;
					}
				//

				int maxTimeToRotate = 4400;
				int timeBackward = 200;
				LostTheLine:
				time.reset();

				if (curve_side == 'R') {
					while (((!isFullBlack(3) || isColorized(3)) && (!isFullBlack(4) || isColorized(4))) && time.timer() < maxTimeToRotate) right(1000);
				} else {
					while (((!isFullBlack(2) || isColorized(2)) && (!isFullBlack(1) || isColorized(1))) && time.timer() < maxTimeToRotate) left(1000);
				}

				//if doesnt find the line
					if (time.timer() > maxTimeToRotate - 50 && timesLost < 2) {
						led(color["orange"]);
						moveTime(-300, timeBackward);
						if (curve_side == 'R') curve_side = 'L';
						else curve_side = 'R';
						timesLost++;
						maxTimeToRotate -= 1000;
						timeBackward = timeBackward/2;
						goto LostTheLine;
					}

					if (timesLost == 2) {
						CentralizeGyro();
					}
				//
			} else return;

			Centralize();
		//

		clear();
	}
}
