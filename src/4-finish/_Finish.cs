void Finish () {
	while (local == Local.exit) {
		RedEnd();
		LineFollower();
		Obstacle();
		Ramp();
	}
	moveTime(300, 200);
}
