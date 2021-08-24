//Track - imported files
	import("2-track/Centralize.cs");
	import("2-track/LineFollower.cs");
	import("2-track/RedEnd.cs");
//

void Track () {
	console(1, "$>--Track--<$", color["comment"]);
	while (local == Local.track) {
		RedEnd();
	}
	led(color["red"]);
}

