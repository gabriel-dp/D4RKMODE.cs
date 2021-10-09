void Tests () {
	if (test) {
		actuator.Up();
		stop(500);
		actuator.Down();
	}
}
