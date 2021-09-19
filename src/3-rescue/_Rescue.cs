//Rescue - imported files
	import("3-rescue/Variables.cs");
	import("3-rescue/Ultrasonics.cs");
	import("3-rescue/Search.cs");
	import("3-rescue/SearchTriangle.cs");
	import("3-rescue/Triangle.cs");
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		moveTime(300, 300);

		open_actuator = true;
		if (actuator.hasKit()) actuator.Up();
		else actuator.Down();

	}

	while (local == Local.rescue) {
		Ultras(true, true);
		Triangle();
	}
}
