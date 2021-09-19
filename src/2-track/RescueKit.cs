bool alreadyHasKit = false;

void Kit () {
	if (actuator.hasKit() && !alreadyHasKit) {
		open_actuator = true;
		actuator.Down();
		moveTime(300, 750);
		actuator.Up();
		if (actuator.hasKit()) alreadyHasKit = true;
	}
}
