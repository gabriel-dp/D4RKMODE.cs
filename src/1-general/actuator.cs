static bool open_actuator = false;

public class Actuator {

	public void ActuatorAdjust (int ideal_actuator, int ideal_scoop) {
		bot.ActuatorSpeed(150);

		int angle_actuator = 0;
		do {
			angle_actuator = (int) bot.AngleActuator();
			if (angle_actuator > ideal_actuator) bot.ActuatorDown(16);
			else if (angle_actuator < ideal_actuator)bot.ActuatorUp(16);
		} while (!(angle_actuator > ideal_actuator-2) || !(angle_actuator < ideal_actuator+2));

		int angle_scoop = 0;
		do {
			angle_scoop = (int) bot.AngleScoop();
			if (angle_scoop < ideal_scoop) bot.TurnActuatorDown(16);
			else if (angle_scoop > ideal_scoop) bot.TurnActuatorUp(16);
		} while (!(angle_scoop > ideal_scoop-2) || !(angle_scoop < ideal_scoop+2));

		if (open_actuator) bot.OpenActuator();
		else bot.CloseActuator();
	}

	public void Up () {
		ActuatorAdjust(87, 0);
	}

	public void Down () {
		ActuatorAdjust(4, 0);
	}

}

Actuator actuator = new Actuator();
