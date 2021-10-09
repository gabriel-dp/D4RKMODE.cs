static bool open_actuator = false;

public class Actuator {

	public void Adjust (int ideal_actuator, int ideal_scoop, string operation = "close", int vel = 150) {
		bot.ActuatorSpeed(vel);

		if (operation == "open") bot.OpenActuator();
		else bot.CloseActuator();

		do {
			int angle_actuator = (int) bot.AngleActuator();
			if (angle_actuator > 300) angle_actuator -= 360;
			else if (angle_actuator > 86) angle_actuator = 90;

			if (vel == 150) {
				int vel_actuator = 25*(Math.Abs(ideal_actuator-Math.Abs(angle_actuator)));
				bot.ActuatorSpeed(vel_actuator);
			}

			if (angle_actuator > ideal_actuator) bot.ActuatorDown(15);
			else if (angle_actuator < ideal_actuator)bot.ActuatorUp(15);

			if (angle_actuator == ideal_actuator) break;
		} while (true);

		bot.ActuatorSpeed(150);
		do {
			int angle_scoop = (int) bot.AngleScoop();
			if (angle_scoop > 300) angle_scoop -= 360;

			if (angle_scoop < ideal_scoop) bot.TurnActuatorDown(15);
			else if (angle_scoop > ideal_scoop) bot.TurnActuatorUp(15);

			if (angle_scoop == ideal_scoop) break;
		} while (true);

	}

	public void Up () {
		Adjust(90, 0, "closed");
	}

	public void Down (string state = "open") {
		if (open_actuator) {
			Adjust(0, 0, state);
		} else {
			Adjust(3, 0, "closed");
		}
	}

	public bool isUp () => (bot.AngleActuator() > 80 && bot.AngleActuator() < 300);

	public bool hasVictim () => (bot.HasVictim() && isUp());

	public bool hasKit () => (bot.HasRescueKit());

	public bool isAlive () {
		bot.Wait(100);
		return (bot.Heat() > 35.5);
	}

}

Actuator actuator = new Actuator();

void Dispatch () {
	Retry:

	moveTime(-200, 600);
	int actuator_vel = bot.Heat() > 32 && actuator.hasVictim() ? 30 : 150;
	actuator.Adjust(22, 0, "close", actuator_vel);
	stop(100);

	if (actuator.hasKit()) {
		moveTime(300, 900);
		stop(200);
		actuator.Up();
		moveTime(-300, 200);
	} else  {
		moveTime(300, 650);
		stop(200);
		moveTime(-300, 200);
		actuator.Up();
	}

	CentralizeGyro();
	if (actuator.hasVictim() || actuator.hasKit()) goto Retry;
}
