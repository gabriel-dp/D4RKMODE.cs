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
		moveTime(300, 750);
		CentralizeGyro(90);
		while (ultra(2) > 70) FollowerGyro();
		/*
		open_actuator = true;

		if (DetectTriangleRight()) {
			sideToSearch = 'L';
			side_triangle = 2;
			side_sensor = 3;
		}
		*/
	}

	while (local == Local.rescue) {
		FollowerGyro();
		//Search();
		Wall();
		Triangle();
		if (ultra(2) > 70) {
			moveTime(300, 650);
			CentralizeGyro(90);
			while (isWhite(3)) forward(200);
			moveTime(300, 300);
			Centralize();
			local = Local.exit;
			return;
		}
	}
}
