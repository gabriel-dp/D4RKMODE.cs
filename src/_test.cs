void Tests () {
	if (test) {
		actuator.Up();
		if (bot.Heat() > 32) {
			rotate(500, 30);
			rotate(500, -60);
			back(50);
			actuator.Adjust(22, 0);
			rotate(500, 30);
		}
		stop(9999);
	}
}
