//Rescue - imported files
	import("3-rescue/Variables.cs");
	import("3-rescue/Ultrasonics.cs");
	import("3-rescue/Triangle.cs");
	import("3-rescue/Wall.cs");
	import("3-rescue/Search.cs");
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		centerQuadrant();
		moveTime(300, 500);

		open_actuator = true;
		actuator.Down();
	}

	while (local == Local.rescue) {
		FollowerGyro();
		Ultras();
	}
}
