void Wall () {
	if (DetectWall()) {
		console_led(2, "$>Parede<$ detectada", color["yellow"]);
		stop();

		//temporary
			if (ultra(3) > 50) {
				CentralizeGyro(-90);
				open_actuator = false;
				actuator.Down();
				while (isWhite(3)) forward(200);
				moveTime(300, 300);
				Centralize();
				local = Local.exit;
				return;
			}
		//

		if (!actuator.isUp()) actuator.Up();

		CentralizeGyro(90);
		if (!actuator.hasVictim()) {
			reverse(300, 300);
			actuator.Down();
		}

		clear();
	}
}
