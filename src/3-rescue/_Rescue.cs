//Rescue - imported files
	import("3-rescue/Variables.cs");
	import("3-rescue/Triangle.cs");
	import("3-rescue/Wall.cs");
	import("3-rescue/Search.cs");
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		centerQuadrant();
		open_actuator = true;

		moveTime(300, 500);
		if (DetectTriangleRight()) sideToSearch = 'L';

		actuator.Down();
	}

	while (local == Local.rescue) {
		forward(300);
		Search();
		Wall();
	}
}
