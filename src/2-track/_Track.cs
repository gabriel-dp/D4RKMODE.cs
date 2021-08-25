//Track - imported files
	import("2-track/Centralize.cs");
	import("2-track/LineFollower.cs");
	import("2-track/RedEnd.cs");
	import("2-track/TrackEnd.cs");
//

void Track () {
	console(1, "$>--Track--<$", color["comment"]);
	while (local == Local.track) {
		forward(200);
		TrackEnd();
	}
}

