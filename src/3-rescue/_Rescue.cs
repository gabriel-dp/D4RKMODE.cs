//Rescue - imported files
	import("3-rescue/Triangle.cs")
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		centerQuadrant();

		moveTime(300, 400);
		DetectTriangleRight();
	}

	while (local == Local.rescue) {

	}
}
