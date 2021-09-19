void TrackEnd () {
	if (anySensorColor("blue") && isBlack(new byte[] {2,3})) {
		CentralizeGyro();
		time.reset();
		do {
			forward(300);
			if (ultra(2) < 50 && ultra(3) < 50) {
				local = Local.rescue;
				return;
			}
		} while (time.timer() < 500);
	}
}

