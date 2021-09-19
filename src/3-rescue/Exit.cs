void Exit () { //pura gamiarra
	CentralizeGyro(90);
	while (ultra(3) < 200) forward(300);
	moveTime(-300, 600);
	CentralizeGyro(-90);
	open_actuator = false;
	actuator.Down();
	while (isWhite(new byte[] {1,2,3,4})) FollowerGyro();
	moveTime(300, 500);
	Centralize();
	local = Local.exit;
}
