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

		//verifies if sensor misread green
			moveTime(300, 15);
			if (isWhite(1) && isWhite(4)) {
				reverse(300, 100);
				return;
			}
			GreenClassifier();
			if (green_direction != 'n') {
				return;
			}
		//

		//tries to centralize and avoid "false curves" then goes forward and rotate in the axis
			Centralize();

			if ((!isWhite(1) && !isColorized(1)) || (!isWhite(4) && !isColorized(4))) {
				moveTime(300, 315);

				//avoids lost the line in the seesaw
					if (inclination() > 8) {
						CentralizeGyro();
						return;
					}
				//

				int maxTimeToRotate = 4400;
				LostTheLine:
				time.reset();

				if (curve_side == 'R') {
					while (((!isFullBlack(3) || isColorized(3)) && (!isFullBlack(4) || isColorized(4))) && time.timer() < maxTimeToRotate) right(1000);
				} else {
					while (((!isFullBlack(2) || isColorized(2)) && (!isFullBlack(1) || isColorized(1))) && time.timer() < maxTimeToRotate) left(1000);
				}

				//if doesnt find the line
					if (time.timer() > maxTimeToRotate - 50) {
						led(color["orange"]);
						moveTime(-300, 200);
						if (curve_side == 'R') curve_side = 'L';
						else curve_side = 'R';
						maxTimeToRotate += maxTimeToRotate;
						goto LostTheLine;
					}
				//
			} else return;

			Centralize();
		//

		clear();
	}
}
