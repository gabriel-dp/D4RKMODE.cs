void TrackEnd () {
	if (isBlue(2) && isBlue(3) && isBlack(new byte[] {2,3})) {
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

