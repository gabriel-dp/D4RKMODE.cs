void DeadVictim (sbyte side_mod) {
	GoToDistance(95);
	CentralizeGyro(90 * side_mod);
	reverse(300, 750);
	moveZm(95);
	CentralizeGyro(-90 * side_mod);

	while (ultra(1) > 40) FollowerGyro();
	stop(250);
	open_actuator = false;
	actuator.Down();
	moveTime(200, 300);

	delay(500);
	moveTime(-300, 150);
	moveTime(300, 50);

	GoToDistance(95);
	CentralizeGyro(90 * side_mod);
	reverse(300, 1500);

	DeadVictimReserved = true;
	open_actuator = true;
}

void DispatchDeadVictim (sbyte side_mod) {
	reverse(300, (time.millis() - timeToFind) - 250);
	moveZm(95);
	CentralizeGyro(-90 * side_mod);

	stop();
	actuator.Down();
	while (!DetectWall()) FollowerGyro();
	stop();
	actuator.Up();

	CentralizeGyro(-90 * side_mod);
	GoToDistance(80);
	Dispatch();

	CentralizeGyro(90 * side_mod);
	GoToDistance(95);
	CentralizeGyro(90 * side_mod);
	reverse(300, 1250);

	DeadVictimReserved = false;
}

void ExpelVictim () {
	if (bot.Heat() > 32 && !DeadVictimReserved) {
		rotate(500, 30);
		rotate(500, -60);
		back(50);
		actuator.Adjust(22, 0);
		rotate(500, 30);
		CentralizeGyro();
		actuator.Up();
	}
}
