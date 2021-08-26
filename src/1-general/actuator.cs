static bool open_actuator = false;
static bool has_victim = false;

public class Actuator {

	public void ActuatorAdjust (int ideal_actuator, int ideal_scoop, string operation = "close") {
		bot.ActuatorSpeed(150);

		if (operation == "open") bot.OpenActuator();
		else bot.CloseActuator();

		int angle_actuator = 0;
		do {
			angle_actuator = (int) bot.AngleActuator();
			if (angle_actuator > ideal_actuator) bot.ActuatorDown(16);
			else if (angle_actuator < ideal_actuator)bot.ActuatorUp(16);
		} while (!(angle_actuator > ideal_actuator-2 && angle_actuator < ideal_actuator+2));

		int angle_scoop = 0;
		do {
			angle_scoop = (int) bot.AngleScoop();
			if (angle_scoop < ideal_scoop) bot.TurnActuatorDown(16);
			else if (angle_scoop > ideal_scoop) bot.TurnActuatorUp(16);
		} while (!(angle_scoop > ideal_scoop-2 && angle_scoop < ideal_scoop+2));

		has_victim = bot.HasVictim();
	}

	public void Up () {
		ActuatorAdjust(89, 0);
	}

	public void Down () {
		if (open_actuator) ActuatorAdjust(1, 0, "open");
		else ActuatorAdjust(4, 0);
	}

}

Actuator actuator = new Actuator();
