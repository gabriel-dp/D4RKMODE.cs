/*--------------------------------------------------
D4RKMODE
Sesi Anísio Teixeira - VCA/BA
--------------------------------------------------*/

const bool console_on = true;
const bool test = false;

import("_setup.cs");
import("_test.cs");
import("1-general/_General.cs");
import("2-track/_Track.cs");
import("3-rescue/_Rescue.cs");
import("4-finish/_Finish.cs");

void Main () {

	Tests();
	Setup();
	Track();
	Rescue();
	Finish();

}
