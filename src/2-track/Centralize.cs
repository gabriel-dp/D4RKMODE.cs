const int Kc = 15;

void Centralize () {

	//If only the external sensors are above the line
	if (isWhite(new byte[] {1,2,3}) && isBlack(4)) {
		while (!isFullBlack(3)) left(1000);
	} else if (isBlack(1) && isWhite(new byte[] {2,3,4})) {
		while (!isFullBlack(2)) right(1000);
	}

	do {
		error = (light(2) - light(3)) * Kc;
		left(error);
	} while (Math.Abs(error) > Kc*2);

	stop();
}
