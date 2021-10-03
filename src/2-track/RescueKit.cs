bool alreadyHasKit = false;

void Kit () {
	if (actuator.hasKit() && !alreadyHasKit) {
		open_actuator = true;
		actuator.Down();
		moveTime(300, 800);
		actuator.Up();
		if (actuator.hasKit()) {
			alreadyHasKit = true;
			led(color["blue"]);
		} else {
			led(color["red"]);
			actuator.Down();
		}

		if (isWhite(new byte[] {1,2,3,4})) {
			moveTime(-300, 350);
			if (isWhite(new byte[] {1,2,3,4})) moveTime(-300, 450);
		}
		Centralize();
	}
}
