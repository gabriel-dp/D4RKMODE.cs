bool alreadyHasKit = false;

void Kit () {
	if (actuator.hasKit() && !alreadyHasKit) {
		open_actuator = true;
		actuator.Down();
		moveTime(300, 750);
		actuator.Up();
		if (actuator.hasKit()) alreadyHasKit = true;
		else {
			open_actuator = true;
			actuator.Down();
		}

		if (isWhite(new byte[] {1,2,3,4})) {
			moveTime(-300, 350);
		}
		Centralize();
	}
}
