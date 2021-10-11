//Rescue - imported files
	import("3-rescue/Variables.cs");
	import("3-rescue/Exit.cs");
	import("3-rescue/Ultrasonics.cs");
	import("3-rescue/Search.cs");
	import("3-rescue/SearchTriangle.cs");
	import("3-rescue/Triangle.cs");
	import("3-rescue/DeadVictim.cs");
//

bool DeadVictimReserved = false;
public static byte AliveVictimsRescued = 0;

void Rescue () {
	if (local == Local.rescue) {
		clear();
		console(1, "$>--Resgate--<$", color["comment"]);
		moveTime(300, 300);

		open_actuator = true;
		if (!actuator.isUp()) actuator.Down();
	}

	while (local == Local.rescue) {
		Ultras(true, true);
		Triangle();
	}
}
