void TrackEnd () {
	if (isBlue(2) && isBlue(3)) {
		CentralizeGyro();
		time.reset();
		do {
			forward(300);
			if (ultra(2) < 60 && ultra(3) < 60) {
				local = Local.rescue;
				return;
			}
		} while (time.timer() < 500);
	}
}

