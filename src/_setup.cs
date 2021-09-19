enum Local {
	track,
	rescue,
	exit,
	end
};

Local local = Local.track;
void Setup () {
	open_actuator = false;
	if (actuator.hasKit()) {
		actuator.Up();
		alreadyHasKit = true;
	} else actuator.Down();

	//search for the line
		const int angleToSearch = 20;
		if (isWhite(new byte[] {1,2,3,4})) {
			rotate(500, angleToSearch);
			if (isWhite(new byte[] {1,2,3,4})) {
				rotate(500, -(2*angleToSearch));
				if (isWhite(new byte[] {1,2,3,4})) {
					rotate(500, angleToSearch);
					moveTime(-300, 400);
				}
			}
		}
	//
	Centralize();
}
