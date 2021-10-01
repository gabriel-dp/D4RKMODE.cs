void SearchTriangle (byte sensor, bool alreadyInActuator = false) {
	sbyte side_mod = (sbyte) (side_triangle == 'L' ? 1 : -1);
	bool reserved = false;

	if ((ultra(sensor) < 265 && !actuator.hasVictim()) && (((side_triangle == 'R' && sensor == 3) || (side_triangle == 'L' && sensor == 2)) || ((side_triangle == 'L' && sensor == 3 && !DeadVictimReserved) || (side_triangle == 'R' && sensor == 2 && !DeadVictimReserved))) || alreadyInActuator) {
		stop();
		console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);

		timeToFind = time.millis() - timeToFind - 250; //measures the time until find the victim, 250 is the offset

		actuator.Up();
		if (actuator.hasVictim()) {

			//dispatches a victim that already was in the actuator
				reverse(300);
				CentralizeGyro(-90*side_mod);
				if (actuator.isAlive() || AliveVictimsRescued > 1) {
					Dispatch();
					AliveVictimsRescued++;
				} else {
					DeadVictim(side_mod);
					reserved = true;
				}
			//

		} else {

			//align with the ball
				float last_ultra = 0;
				time.reset();
				do {
					last_ultra = ultra(sensor);
					forward(150);
					delay(15);
				} while (ultra(sensor) <= last_ultra && time.timer() < 100);
			//

			if ((side_triangle == 'R' && sensor == 3) || (side_triangle == 'L' && sensor == 2)) { //if the victim is on the complex side

				//triangle calculation
					int zmToMove = (int)(last_ultra+1);

					int twoLegs = timeByZm(timeToFind);
					float prop = last_ultra/50;
					int bigLeg = (int)((prop*twoLegs)/(1+prop));

					int angleToRotate = 180 - maths.ArcTan(bigLeg, last_ultra);
					console(3, $"{twoLegs} | {prop} | {bigLeg} | {angleToRotate}");
				//

				//go rescue
					CentralizeGyro(90*side_mod);
					if (last_ultra < 35) {
						moveTime(-300, 500);
						actuator.Down();
						moveTime(300, 500);
					} else actuator.Down();
					moveZm(zmToMove);
					actuator.Up();
					stop(150);
				//

				//if didn't rescued
					if (!actuator.hasVictim() && !actuator.hasKit()) {
						moveZm(-zmToMove);
						CentralizeGyro(-90*side_mod);
						reverse(300);
						CentralizeGyro();
						actuator.Down();
						timeToFind = time.millis();
						return;
					}
				//

				//dispatch in the triangle
					rotate(500, angleToRotate*side_mod);
					while (!isFullBlack(5)) FollowerGyro(direction());

					if (actuator.isAlive() || AliveVictimsRescued > 1|| actuator.hasKit()) {
						Dispatch();
						AliveVictimsRescued++;
					}

					if (angleToRotate <= 135) rotate(500, (int)((180-Math.Abs(angleToRotate))*side_mod));
					CentralizeGyro();
				//

			} else { //if the victim is on the simple side

				//go rescue
					CentralizeGyro(-90*side_mod);
					if (last_ultra < 30) {
						moveTime(-300, 500);
						actuator.Down();
						moveTime(300, 500);
					} else actuator.Down();
					moveZm((int)last_ultra);
					actuator.Up();
					stop(150);
					if (ultra(1) > 400) moveZm((int)-last_ultra);
					else GoToDistance(95);
					CentralizeGyro(90*side_mod);
				//

				reverse(300);
				CentralizeGyro(-90*side_mod);
				if (actuator.isAlive() || AliveVictimsRescued > 1) {
					Dispatch();
					AliveVictimsRescued++;
				}

			}

		}

		//position the robot in the side of the triangle
			if (actuator.hasVictim() && AliveVictimsRescued < 2) {
				DeadVictim(side_mod);
			} else if (!reserved) {
				GoToDistance(95);
				CentralizeGyro(90*side_mod);
				reverse(300, 750);
			}
			actuator.Down();
			timeToFind = time.millis();
		//

		clear();
	}

}
