static bool open_actuator = false;

public class Actuator {

	public void Adjust (int ideal_actuator, int ideal_scoop, string operation = "close") {
		bot.ActuatorSpeed(150);

		if (operation == "open") bot.OpenActuator();
		else bot.CloseActuator();

		int start = bot.Millis();
		int angle_actuator = 0;
		do {
			angle_actuator = (int) bot.AngleActuator();
			if (angle_actuator > ideal_actuator) bot.ActuatorDown(16);
			else if (angle_actuator < ideal_actuator)bot.ActuatorUp(16);
		} while ((!(angle_actuator > ideal_actuator-2 && angle_actuator < ideal_actuator+2)) && bot.Millis() - start < 750);

		int angle_scoop = 0;
		do {
			angle_scoop = (int) bot.AngleScoop();
			if (angle_scoop < ideal_scoop) bot.TurnActuatorDown(16);
			else if (angle_scoop > ideal_scoop) bot.TurnActuatorUp(16);
		} while ((!(angle_scoop > ideal_scoop-2 && angle_scoop < ideal_scoop+2)) && bot.Millis() - start < 1500);

	}

	public void Up () {
		Adjust(89, 0);
	}

	public void Down (string state = "open") {
		if (open_actuator) Adjust(1, 0, state);
		else Adjust(3, 0);
	}

	public bool isUp () => (bot.AngleActuator() > 80);

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
	actuator.Adjust(20, 0);
	stop(100);
	if (actuator.hasKit()) moveTime(300, 1000);
	else moveTime(300, 600);
	stop(200);
	moveTime(-300, 200);
	actuator.Up();
	CentralizeGyro();
	if (actuator.hasVictim()) goto Retry;
}
