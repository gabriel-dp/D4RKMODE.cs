enum Local {
	track,
	rescue,
	exit,
	end
};

Local local = Local.track;
void Setup () {
	Centralize();
	actuator.Down();
}
