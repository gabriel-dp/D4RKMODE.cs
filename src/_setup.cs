enum Local {
	track,
	rescue,
	exit,
	end
};

Local local = Local.track;
void Setup () {
	open_actuator = false;

	actuator.Down();
	Centralize();
}
