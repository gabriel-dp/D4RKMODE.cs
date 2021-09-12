const int Kc = 15;

void Centralize (int timeout = 2000) {

	//If only the external sensors are above the line
		checkpoint_centralize:
		time.reset();

		if (isWhite(new byte[] {1,2,3}) && isBlack(4)) {
			moveTime(200, 15);
			while (!isFullBlack(3) && time.timer() < 500) left(1000);
		} else if (isBlack(1) && isWhite(new byte[] {2,3,4})) {
			moveTime(200, 15);
			while (!isFullBlack(2) && time.timer() < 500) right(1000);
		}

		if (time.timer() > 450) {
			moveTime(300, 30);
			goto checkpoint_centralize;
		}
	//

	time.reset();
	do {
		error = (light(2) - light(3)) * Kc;
		left(error);
	} while ((Math.Abs(error) > Kc*2 && time.timer() > timeout) || time.timer() < 150);

	last_zero = time.millis();
	stop();
}
