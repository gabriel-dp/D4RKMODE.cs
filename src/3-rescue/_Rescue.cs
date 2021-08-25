//Rescue - imported files
	import("3-rescue/Variables.cs");
	import("3-rescue/Triangle.cs");
	import("3-rescue/Search.cs");
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		centerQuadrant();

		moveTime(300, 400);
		if (DetectTriangleRight()) sideToSearch = 'L';
	}

	while (local == Local.rescue) {
		forward(300);
		Search();
	}
}
