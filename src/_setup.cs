enum Local {
	track,
	rescue,
	exit,
	end
};

Local local = Local.track;
void Setup () {
	open_actuator = false;
	has_victim = false;

	actuator.Down();
	Centralize();
}
