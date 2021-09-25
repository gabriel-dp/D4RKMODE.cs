//Finish - imported files
	import("4-finish/Celebrate.cs");
//

void Finish () {
	while (local == Local.exit) {
		RedEnd();
		LineFollower();
		Obstacle();
		Ramp();
	}
	moveTime(300, 200);
	Celebrate("D4RKMODE", rainbow);
}
