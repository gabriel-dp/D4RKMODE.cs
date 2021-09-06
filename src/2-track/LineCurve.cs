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
		Centralize();
		if ((!isWhite(1) && !isColorized(1)) || (!isWhite(4) && !isColorized(4))) {
			moveTime(300, 330);
			if (curve_side == 'R') {
				while (!isFullBlack(3) || isColorized(3)) right(1000);
			} else {
				while (!isFullBlack(2) || isColorized(2)) left(1000);
			}
		} else {
			clear();
			return;
		}
		Centralize();
		clear();
	}
}
