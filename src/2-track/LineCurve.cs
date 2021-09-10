void CurveBlack () {
	char curve_side = 'n';
	if (!isWhite(1) && !isColorized(1)) {
		console_led(2, "Curva para Direita →", color["black"]);
		curve_side = 'R';
	} else if (!isWhite(4) && !isColorized(4)) {
		console_led(2, "Curva para Esquerda ←", color["black"]);
		curve_side = 'L';
	}

	if (curve_side != 'n') {

		//verifies if is a green square
			moveTime(300, 15);
			GreenClassifier();
			if (green_direction != 'n') {
				clear();
				return;
			}
		//

		Centralize();
		if ((!isWhite(1) && !isColorized(1)) || (!isWhite(4) && !isColorized(4))) {
			moveTime(300, 315);

			int maxTimeToRotate = 4400;
			LostTheLine:
			time.reset();

			if (curve_side == 'R') {
				while ((!isFullBlack(3) || isColorized(3)) && time.timer() < maxTimeToRotate) right(1000);
			} else {
				while ((!isFullBlack(2) || isColorized(2)) && time.timer() < maxTimeToRotate) left(1000);
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
		} else {
			clear();
			return;
		}
		Centralize();
		clear();

	}
}
