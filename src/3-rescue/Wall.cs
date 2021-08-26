void Wall () {
	if (DetectWall()) {
		actuator.Up();
		CentralizeGyro(90);
		reverse(300, 300);
		if (!has_victim) actuator.Down();
	}
}
