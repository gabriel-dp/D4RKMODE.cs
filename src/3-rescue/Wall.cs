void Wall () {
	if (DetectWall()) {
		stop();
		console_led(2, "$>Parede<$ detectada", color["yellow"]);

		if (!actuator.isUp()) {
			actuator.Up();
			stop(150);
			if (actuator.victim() == "alive") first_check_alive = true; led(color["green"]);
		}
		CentralizeGyro(90);
		if (actuator.victim() == null) {
			reverse(300, 300);
			actuator.Down();
		}

		clear();
	}
}
