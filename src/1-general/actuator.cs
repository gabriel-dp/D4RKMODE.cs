static bool open_actuator = false;

public class Actuator {

	public void Adjust (int ideal_actuator, int ideal_scoop, string operation = "close", int vel = 150) {
		bot.ActuatorSpeed(vel);

		if (operation == "open") bot.OpenActuator();
		else bot.CloseActuator();

		int start = bot.Millis();
		int max_time = 112500/vel;
		int angle_actuator = 0;
		do {
			angle_actuator = (int) bot.AngleActuator();
			if (angle_actuator > ideal_actuator) bot.ActuatorDown(16);
			else if (angle_actuator < ideal_actuator)bot.ActuatorUp(16);
		} while ((!(angle_actuator > ideal_actuator-2 && angle_actuator < ideal_actuator+2)) && bot.Millis() - start < max_time);

		int angle_scoop = 0;
		do {
			angle_scoop = (int) bot.AngleScoop();
			if (angle_scoop < ideal_scoop) bot.TurnActuatorDown(16);
			else if (angle_scoop > ideal_scoop) bot.TurnActuatorUp(16);
		} while ((!(angle_scoop > ideal_scoop-2 && angle_scoop < ideal_scoop+2)) && bot.Millis() - start < max_time*2);

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
