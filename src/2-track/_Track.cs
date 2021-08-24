//Track - imported files
	import("2-track/Centralize.cs");
	import("2-track/LineFollower.cs");
//

void Track () {
	console(1, "$>--Track--<$", color["comment"]);
	while (true) {
		LineFollower();
	}
}

