void Wall () {
	if (DetectWall()) {
		console_led(2, "$>Parede<$ detectada", color["yellow"]);
		stop();
		if (!actuator.isUp()) actuator.Up();

		CentralizeGyro(90);
		if (!actuator.hasVictim()) {
			reverse(300, 300);
			actuator.Down();
		}

		clear();
	}
}
