/*--------------------------------------------------
D4RKMODE
Sesi Anísio Teixeira - VCA/BA
--------------------------------------------------*/

const bool console_on = true;
const bool test = false;


enum Local {
	track,
	rescue,
	exit,
	end
};

Local local = Local.track;
void Setup () {
	open_actuator = false;
	if (actuator.hasKit()) {
		actuator.Up();
		alreadyHasKit = true;
	} else actuator.Down();

	//search for the line
		const int angleToSearch = 20;
		if (isWhite(new byte[] {1,2,3,4})) {
			rotate(500, angleToSearch);
			if (isWhite(new byte[] {1,2,3,4})) {
				rotate(500, -(2*angleToSearch));
				if (isWhite(new byte[] {1,2,3,4})) {
					rotate(500, angleToSearch);
					moveTime(-300, 400);
				}
			}
		}
	//
	Centralize();
}

void Tests () {
	if (test) {
		actuator.Up();
		stop(500);
		actuator.Down();
	}
}

//General - imported files

	public class Maths {
	
		const float PI = 3.14f;
	
		public bool interval (float val, float min, float max) => (val >= min && val <= max);
	
		public int map (float number, float min, float max, float minTo, float maxTo) {
			return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
		}
	
		public int hypotenuse (double leg1, double leg2) {
			return (int) (Math.Sqrt(Math.Pow(leg1, 2)+Math.Pow(leg2,2)));
		}
	
		public int ArcTan (double leg1, double leg2) {
			return (int) ((180/PI)*(Math.Atan(leg1/leg2)));
		}
	
	}
	
	Maths maths = new Maths();

	void delay (int ms) => bot.Wait(ms);
	
	public class Time {
	
		public int millis () => bot.Millis();
	
		public int timer () => bot.Timer();
	
		public void reset () => bot.ResetTimer();
	
	}
	
	Time time = new Time();

	//simplified basic commands
	void move (float motor_L, float motor_R) => bot.Move(motor_L, motor_R);
	
	void forward (float motor) => move (motor, motor);
	void back (float motor) => move (-motor, -motor);
	void right (float motor) => move (motor, -motor);
	void left (float motor) => move (-motor, motor);
	
	//More methods
	void rotate (float motor, int angle) {
		int angleToGo = (direction() + angle)%360;
		if (angleToGo < 0) angleToGo = 360 + angleToGo;
		if (angle > 0) while (direction() != angleToGo) right(1000);
		else while (direction() != angleToGo) left(1000);
		stop();
	}
	
	void stop (int ms = 0) {
		move(0, 0);
		delay(ms);
	}
	
	void moveTime (float motor, int ms) {
		time.reset();
		while (time.timer() < ms) forward(motor);
		stop();
	}
	
	const float timePerZm = 16.65f;
	void moveZm (int zm) {
		int timeToMove = (int)(zm*timePerZm);
		if (zm > 0) moveTime(300, timeToMove);
		else moveTime(-300, -timeToMove);
	}
	
	int timeByZm (int ms) => (int)(ms/timePerZm);
	
	void reverse (float motor, int ms = 999999) {
		time.reset();
		while (!bot.Touch(0) && time.timer() < ms) back(motor);
		stop();
	}
	
	//Global Variables
	int error = 0;
	int last_error = 0;
	int turn = 0;

	//Constants of calibration
	const float max_white = 60;
	const byte high_black = 50;
	const byte low_black = 90;
	
	//Main method
	int light (int sensor) {
		float raw = bot.Lightness(sensor - 1);
		int light_data = maths.map(raw, 0, max_white, 0, 100);
		if (light_data > 100) light_data = 100;
		return light_data;
	}
	
	//General methods
		bool isWhite (byte sensor) => (light(sensor) > low_black);
		bool isBlack (byte sensor) => (light(sensor) < low_black);
		bool isFullBlack (byte sensor) => (light(sensor) < high_black);
	
		bool isWhite (byte[] sensors) {
			for (byte i = 0; i < sensors.Length; i++) {
				if (light(sensors[i]) < low_black) return false;
			}
			return true;
		}
	
		bool isBlack (byte[] sensors) {
			for (byte i = 0; i < sensors.Length; i++) {
				if (light(sensors[i]) > low_black) return false;
			}
			return true;
		}
	
		bool isFullBlack (byte[] sensors) {
			for (byte i = 0; i < sensors.Length; i++) {
				if (light(sensors[i]) < low_black) return false;
			}
			return true;
		}
	
		bool anySensorLine () {
			for (byte i = 0; i < 5; i++) {
				if (light(i) < low_black && !isColorized(i)) return true;
			}
			return false;
		}
	//

	public class Colors {
		public float R (byte sensor) => (bot.ReturnRed(sensor-1) + 1);
		public float G (byte sensor) => (bot.ReturnGreen(sensor-1) + 1);
		public float B (byte sensor) => (bot.ReturnBlue(sensor-1) + 1);
	}
	
	Colors colors = new Colors();
	
	//Default colors of simulator
		Dictionary <string, string> colorNames = new Dictionary <string,string> () {
			{"WHITE","BRANCO"},
			{"BLACK","PRETO"},
			{"GREEN","VERDE"},
			{"MAGENTA","MAGENTA"},
			{"BLUE","AZUL"},
			{"CYAN","CIANO"},
			{"YELLOW","AMARELO"},
			{"RED","VERMELHO"},
		};
	
		bool isThatColor (byte sensor, string name) => (bot.ReturnColor(sensor-1) == name || bot.ReturnColor(sensor-1) == colorNames[name]);
	//
	
	//Colors identifier
	bool isColorized (byte sensor) => (colors.R(sensor) != colors.B(sensor));
	
	bool isRed (byte sensor) => ((colors.R(sensor)/colors.B(sensor) > 3.5 && colors.G(sensor) < 20) && isThatColor(sensor, "RED"));
	
	bool isGreen (byte sensor) => ((colors.G(sensor)/colors.R(sensor) > 4 && colors.B(sensor) < 10) && isThatColor(sensor, "GREEN"));
	
	bool isBlue (byte sensor) => ((colors.B(sensor)/colors.R(sensor) > 1.2 && colors.G(sensor) < 75) && isThatColor(sensor, "WHITE"));
	
	bool anySensorColor (string color) {
		for (byte i = 1; i<5; i++) {
			switch (color) {
				case "red":
					if (isRed(i)) return true;
					break;
				case "green":
					if (isGreen(i)) return true;
					break;
				case "blue":
					if (isBlue(i)) return true;
					break;
				default:
					break;
			}
		}
		return false;
	}
	
	bool isColorized (byte[] sensors) {
		for (byte i = 0; i < sensors.Length; i++) {
			if (colors.R(sensors[i]) != colors.B(sensors[i])) return false;
		}
		return true;
	}
	

	int direction () => (int) bot.Compass();
	
	byte Direction () {
		byte quadrant = 0;
		if (maths.interval(direction(), 46, 135)) quadrant = 1;
		else if (maths.interval(direction(), 136, 225)) quadrant = 2;
		else if (maths.interval(direction(), 226, 315)) quadrant = 3;
		return quadrant;
	}
	
	int scaleAngle (int angle) {
		angle = angle%90;
		if (angle >= 45) angle -= 2*(angle-45);
		return angle;
	}
	
	bool isOrtogonal () => (direction() == 0 || direction() == 90 || direction() == 180 || direction() == 270);
	
	//Gyro alignment
		void changeQuadrant (string side) {
			byte last_quadrant = Direction();
	
			if (side == "back") {
				while (Direction() == last_quadrant) right(1000);
				last_quadrant = Direction();
			}
	
			if (side == "left") {
				while (Direction() == last_quadrant) left(1000);
			} else {
				while (Direction() == last_quadrant) right(1000);
			}
	
			stop();
		}
	
		void GoToAngle (int angle) {
			if (maths.interval(direction() - angle, 0, 140) || maths.interval(direction() - angle, -320, -210)) {
				while (direction() != angle) left(1000);
			} else {
				while (direction() != angle) right(1000);
			}
	
			stop();
		}
	
		void centerQuadrant () {
			int ideal_angle = Direction()*90;
			GoToAngle(ideal_angle);
		}
	
		void CentralizeGyro (int operation = 0) {
	
			switch (operation) {
				case 0:
					centerQuadrant();
					break;
				case 90:
					changeQuadrant("right");
					centerQuadrant();
					break;
				case -90:
					changeQuadrant("left");
					centerQuadrant();
					break;
				case 180:
					changeQuadrant("back");
					centerQuadrant();
					break;
				case 45:
					changeQuadrant("right");
					break;
				case -45:
					changeQuadrant("left");
					break;
				default:
					break;
			}
	
		}
	//
	
	const int Kg = 20;
	void FollowerGyro (int angle = 1000) {
		//error to turn
			int direction_ideal = angle;
			if (angle == 1000) direction_ideal = Direction() * 90;
	
			float direction_actual = bot.Compass();
			if ((direction_ideal >= 0 && direction_ideal < 15) && direction_actual > 300) direction_actual -= 360;
			else if ((direction_ideal > 345 && direction_ideal <= 360) && direction_actual < 300) direction_actual += 360;
	
			float direction_error = direction_actual-direction_ideal;
			turn = (int)(direction_error*Kg);
	
			if (turn > 100) turn = 100;
			else if (turn < -100) turn = -100;
		//
	
		//turn to motors
			if (Math.Abs(turn) > 20) {
				if (turn > 0) move(-(300*Math.Abs(turn)*0.02f), 300);
				else move(300, -(300*Math.Abs(turn)*0.02f));
			}
			else {
				forward(300);
			}
		//
	
		//printMotors();
	}

	float ultra (byte sensor) {
		float ultra_data = bc.Distance(sensor - 1);
		ultra_data = ((float)((int)(ultra_data *100)))/100;
		return ultra_data;
	}
	
	bool ultraLimits (byte sensor, int min, int max) => bot.DetectDistance(sensor-1, min, max);
	
	bool ultraInRange (byte sensor) => (ultra(sensor) < 400);
	
	void GoToDistance (int distance) {
		do {
			error = (int) (ultra(1) - distance);
			forward(error*50);
		} while (error != 0);
	}
	
	bool DetectWall () => (ultra(1) < (Math.Pow(scaleAngle(direction()), 2) * 0.006f + 28));

	int inclination () {
		int inclination_data = (int) bot.Inclination();
		if (inclination_data > 300) inclination_data -= 360;
		return inclination_data;
	}

	//colors
	Dictionary <string, string> color = new Dictionary <string,string> () {
		{"white","#FFFFFF"},
		{"black","#000000"},
		{"gray","#696969"},
		{"green","#00CC00"},
		{"pink","#D264D2"},
		{"purple","#915AB4"},
		{"orange","#C88232"},
		{"blue","#465AC8"},
		{"cyan","#00c8c8"},
		{"yellow","#FFFF00"},
		{"green_dark","#009900"},
		{"red","#FF0000"},
		{"comment","#3C455E"}
	};
	
	//led
	string last_led = "";
	void led (string hexcolor) {
		if (last_led != hexcolor) {
			if (hexcolor != "off") bot.TurnLedOn(hexcolor + "DF");		//"DF" is to the translucency
			else bot.TurnLedOff();
			last_led = hexcolor;
		}
	}
	
	//console
	string last_console = "";
	void console (int line, string text, string hexcolor = "") {
		if (console_on && text != last_console) {
			if (hexcolor != "") {
				text = text.Replace("$>", $"<color={hexcolor}>");
				text = text.Replace("<$", "</color>");
			}
			bot.Print(line - 1, "<align=center>" + text + "</align>");
			bot.Print(line, "");
			last_console = text;
		}
	}
	
	void clear (int line = 999) {
		if (line == 999) {
			led(color["white"]);
			bot.ClearConsoleLine(1);
			bot.ClearConsoleLine(2);
		}
		else	bot.ClearConsoleLine(line - 1);
	}
	
	int start_print = 0;
	void printMotors () {
		if (start_print + 63 < time.millis()) {
			string right_data = ((int)bot.GetFrontalRightForce()).ToString();
			string left_data = ((int)bot.GetFrontalLeftForce()).ToString();
	
			string right_spaces = new string (' ', 5 - right_data.Length);
			string left_spaces = new string (' ', 5 - left_data.Length);
	
			console(3, $"$>| {right_spaces+right_data}   |   {left_data+left_spaces} |<$", color["comment"]);
			start_print = time.millis();
		}
	}
	
	//both
	void console_led (int line, string text, string hexcolor, string ledcolor = "") {
		if (ledcolor == "") ledcolor = hexcolor;
		console(line, text, hexcolor);
		led(ledcolor);
	}

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
//

//Track - imported files

	const int Kc = 15;
	
	void Centralize (int timeout = 2000) {
	
		//If only the external sensors are above the line
			checkpoint_centralize:
			time.reset();
	
			if (isWhite(new byte[] {1,2,3}) && isBlack(4)) {
				moveTime(200, 15);
				while (!isFullBlack(3) && time.timer() < 500) left(1000);
			} else if (isBlack(1) && isWhite(new byte[] {2,3,4})) {
				moveTime(200, 15);
				while (!isFullBlack(2) && time.timer() < 500) right(1000);
			}
	
			if (time.timer() > 450) {
				moveTime(300, 30);
				goto checkpoint_centralize;
			}
		//
	
		time.reset();
		do {
			error = (light(2) - light(3)) * Kc;
			left(error);
		} while ((Math.Abs(error) > Kc*2 && time.timer() > timeout) || time.timer() < 150);
	
		last_zero = time.millis();
		stop();
	}

	char green_direction = 'n';
	void GreenClassifier () {
		green_direction = 'n';
		if ((isGreen(1) || isGreen(2)) && (isGreen(3) || isGreen(4))) {
			green_direction = 'B';
			console_led(2, "$>Beco sem saída<$", color["green"]);
			console(3, "↓");
		} else if (isGreen(1) || isGreen(2)) {
			green_direction = 'R';
			console_led(2, "$>Intersecção<$ para $>Direita<$", color["green"]);
			console(3, "→");
		} else if (isGreen(3) || isGreen(4)){
			green_direction = 'L';
			console_led(2, "$>Intersecção<$ para $>Esquerda<$", color["green"]);
			console(3, "←");
		}
	}
	
	void Green () {
		if (anySensorColor("green")) {
			led(color["green"]);
	
			//goes a bit forward to detect dead ends
				int timeGreen = 15;
				if (scaleAngle(direction()) > 20) timeGreen = 45;
				else if (scaleAngle(direction()) > 7) timeGreen = 30;
	
				time.reset();
				while (time.timer() < timeGreen) Follower(false);
	
				if (isFullBlack(new byte[] {1, 4})) moveTime(-300, 16);
			//
	
			//centralizes in the line and verifies again
				GreenClassifier();
				if (green_direction == 'B') {
					while (!isFullBlack(2)) left(1000);
					rotate(500, 2);
				} else if (green_direction == 'L') {
					while (!isFullBlack(3)) right(1000);
					rotate(500, -2);
				} else {
					while (!isFullBlack(2)) left(1000);
					rotate(500, 2);
				}
				stop(50);
				GreenClassifier();
	
				if (green_direction == 'n') {
					led(color["orange"]);
					reverse(300, 175);
					return;
				}
			//
	
			//goes forward and rotate in the axis to make the curve
				moveTime(300, 425);
				if (green_direction == 'B') {
					CentralizeGyro(180);
				} else {
					if (green_direction == 'R') {
						if (scaleAngle(direction()) < 25) CentralizeGyro(45);
						while (!isFullBlack(3) && !isFullBlack(4) && !isBlack(4)) right(1000);
					} else {
						if (scaleAngle(direction()) < 25) CentralizeGyro(-45);
						while (!isFullBlack(2) && !isFullBlack(1) && !isBlack(1)) left(1000);
					}
				}
				Centralize();
			//
	
			clear();
		}
	}

	int last_ramp = 0;
	
	void CurveBlack () {
		char curve_side = 'n';
		if (!isWhite(1) && !isColorized(1)) {
			console_led(2, "$>Curva<$ para $>Direita<$", color["gray"], color["black"]);
			console(3, "→");
			curve_side = 'R';
		} else if (!isWhite(4) && !isColorized(4)) {
			console_led(2, "$>Curva<$ para $>Esquerda<$", color["gray"], color["black"]);
			console(3, "←");
			curve_side = 'L';
		}
	
		if (curve_side != 'n') {
	
			byte timesLost = 0;
	
			//verifies if sensor misread green or blue
				moveTime(200, 15);
				if (isWhite(1) && isWhite(4)) {
					reverse(300, 100);
					if (isColorized(1) || isColorized(4)) {
						if ((colors.B(1) > colors.R(1)) || (colors.B(4) > colors.R(4))) {
							CentralizeGyro();
							reverse(300, 200);
						}
					}
					return;
				}
				GreenClassifier();
				if (green_direction != 'n') {
					return;
				}
			//
	
			//avoids lost the line in the seesaw
				if (Math.Abs(inclination()) > 3) {
					CentralizeGyro();
					stop(800);
					if (time.millis() - last_ramp < 1250) return;
					last_ramp = time.millis();
				}
			//
	
			//tries to centralize and avoid "false curves" then goes forward and rotate in the axis
				Centralize();
	
				if ((!isWhite(1) && !isColorized(1)) || (!isWhite(4) && !isColorized(4))) {
					moveTime(300, 315);
	
					int maxTimeToRotate = 4400;
					int timeBackward = 200;
					LostTheLine:
					time.reset();
	
					if (curve_side == 'R') {
						while (((!isFullBlack(3) || isColorized(3)) && (!isFullBlack(4) || isColorized(4))) && time.timer() < maxTimeToRotate) right(1000);
					} else {
						while (((!isFullBlack(2) || isColorized(2)) && (!isFullBlack(1) || isColorized(1))) && time.timer() < maxTimeToRotate) left(1000);
					}
	
					//if doesnt find the line
						if (time.timer() > maxTimeToRotate - 50 && timesLost < 2) {
							led(color["orange"]);
							moveTime(-300, timeBackward);
							if (curve_side == 'R') curve_side = 'L';
							else curve_side = 'R';
							timesLost++;
							maxTimeToRotate -= 1000;
							timeBackward = timeBackward/2;
							goto LostTheLine;
						}
	
						if (timesLost == 2) {
							CentralizeGyro();
						}
					//
				} else return;
	
				Centralize();
			//
	
			clear();
		}
	}

	const float Kp = 1;
	const float Kd = 20;
	
	const int vel_front = 300;
	const int vel_axis = 1000;
	const int motor_limit = 185;
	
	const int turn_axis = 50;
	const int turn_normal = 15;
	const float turn_coefficient = 0.01f;
	
	int last_zero = 0;
	
	void Follower (bool led_on = true) {
	
		//error to turn
			error = light(2)-light(3);
	
			int P = (int) (error * Kp);
			int D = (int) ((last_error - error) * Kd);
			if (last_error != error) last_error = error;
			turn = P + D;
	
			if (turn > 100) turn = 100;
			else if (turn < -100) turn = -100;
		//
	
		//turn to motors
			if (Math.Abs(turn) > turn_axis) {
				left(vel_axis*turn);
				last_zero = time.millis();
			}
			else if (Math.Abs(turn) > turn_normal) {
				if (turn > 0) move(-(vel_front*Math.Abs(turn)*turn_coefficient), vel_front);
				else move(vel_front, -(vel_front*Math.Abs(turn)*turn_coefficient));
				last_zero = time.millis();
			}
			else {
				forward(motor_limit);
			}
		//
	
		//maybe lost the line
			if (time.millis() - last_zero > 1000 && scaleAngle(direction()) > 2) {
				CentralizeGyro();
				last_zero = time.millis();
			}
		//
	
		if (led_on) led(color["white"]);
		//printMotors();
	
	}
	
	void LineFollower () {
	
		CurveBlack();
		Green();
		Follower();
	
	}

	byte timesFailed = 0;
	
	void Obstacle () {
		if (DetectWall()) {
			console_led(2, "$>Obstáculo<$   Possível", color["pink"]);
			reverse(300, 50);
			stop();
			if (!actuator.hasKit()) actuator.Up();
	
			//after lifting the actuator, follows the line for a time to confirm that's a obstacle
				const int timeObstacle = 2000;
				time.reset();
				while (time.timer() < timeObstacle && ultra(1) > 15) {
					LineFollower();
				}
	
				//if isn't a obstacle, downs the actuator
				if (time.timer() > timeObstacle - 50) {
					stop();
					if (!actuator.hasKit()) actuator.Down();
	
					//avoids the bug of up and down
						timesFailed++;
						if (timesFailed >= 2) {
							bot.ActuatorDown(16);
						}
					//
	
					return;
				}
			//
	
			//if is a obstacle, starts avoid it
				console_led(2, "$>Obstáculo<$ Confirmado", color["purple"]);
				bool surpassed = false;
				bool obstructed = false;
	
				const int timeout = 1250;
				void GoForward (bool first = true, bool second = true) {
					obstructed = false;
					while (ultra(3) > 50 && first) FollowerGyro(direction());
					time.reset();
					while (ultra(3) < 50 && second && time.timer() < timeout) FollowerGyro(direction());
					if (time.timer() > timeout-50) {
						led(color["orange"]);
						moveTime(-300, 50);
						rotate(500, -25);
						moveTime(300, 200);
						rotate(500, -25);
						moveTime(300, 400);
						CentralizeGyro();
						obstructed = true;
					}
				}
	
				void AfterAvoid () {
					CentralizeGyro(90);
					reverse(300, 100);
					Centralize();
					if (!actuator.hasKit()) actuator.Down();
					surpassed = true;
					clear();
				}
	
				//	|-> /
					CentralizeGyro(45);
					rotate(500, 20);
					GoForward();
	
				//	/ -> |
					if (!obstructed) CentralizeGyro(-90);
					//search for the line in a 90 degress obstacle
						while (ultra(3) > 50 && !surpassed) {
							forward(190);
							if (!isWhite(new byte[] {3,4})) {
								led(color["pink"]);
								moveTime(300, 400);
								AfterAvoid();
							}
						}
						if (surpassed) return;
					//
					GoForward(false, true);
					moveTime(300, 100);
	
				//	| -> \
					CentralizeGyro(-45);
					rotate(500, -20);
					GoForward(true, false);
					//search for the line in a normal obstacle
						while (ultra(3) < 50 && !surpassed) {
							forward(190);
							if (!isWhite(new byte[] {1,2})) {
								led(color["pink"]);
								GoForward();
								moveTime(-300, 150);
								AfterAvoid();
							}
						}
						if (surpassed) return;
					//
	
				//	\ -> /
					CentralizeGyro(0);
					GoForward();
					CentralizeGyro(-45);
					rotate(500, -20);
					GoForward();
					moveTime(-300, 150);
					AfterAvoid();
	
			//
	
		}
	}

	int start_ramp = 0;
	
	bool flag_stuck = false;
	int time_stuck = 0;
	
	void StuckObstacle () {
		if (inclination() < -8 && (ultra(1) > 400 || ultra(1) < 35) && !actuator.isUp()) {
			if (!flag_stuck) {
				time_stuck = time.millis();
				flag_stuck = true;
			} else {
				if (time.millis() - time_stuck > 7000) {
					led(color["orange"]);
					stop();
					actuator.Up();
					delay(500);
					moveTime(300, 300);
					Obstacle();
					actuator.Down();
					flag_stuck = false;
				}
			}
		} else {
			flag_stuck = false;
		}
	}
	
	void Ramp () {
		if (inclination() < -7) {
			console_led(2, "$>Rampa<$ ou $>Gangorra<$", color["blue"]);
			console(3, "↑");
	
			//lifts the actuator and follows the line for a time then down that
				stop();
				if (!actuator.hasKit()) actuator.Up();
				time.reset();
				while (time.timer() < 2000 + 100*scaleAngle(direction())) {
					LineFollower();
				}
				if (!actuator.hasKit()) {
					stop();
					actuator.Down();
				}
	
				start_ramp = time.millis();
				int last_inclination = inclination();
				while (inclination() < -2) {
					LineFollower();
					StuckObstacle();
				}
				stop(200);
				int last_inclination2 = inclination();
			//
	
			//if is a seesaw needs to wait that down
				if (last_inclination - last_inclination2 < -11 && time.millis() - start_ramp < 4000 && Math.Abs(last_inclination2) > 2) {
					stop(500);
					moveTime(-300, 250);
					if (scaleAngle(direction()) > 20) CentralizeGyro();
				}
			//
		}
	
		//avoids be stuck in a speed bump after ramp
			if (inclination() > 8 && ultra(1) < 150 && !actuator.hasKit()) {
				if (!flag_stuck) {
					time_stuck = time.millis();
					flag_stuck = true;
				} else {
					if (time.millis() - time_stuck > 4500) {
						actuator.Up();
						moveTime(300, 100);
						actuator.Down();
						flag_stuck = false;
					}
				}
			} else {
				flag_stuck = false;
			}
		//
	
	}

	bool alreadyHasKit = false;
	
	void Kit () {
		if (actuator.hasKit() && !alreadyHasKit) {
			open_actuator = true;
			actuator.Down();
			moveTime(300, 800);
			actuator.Up();
			if (actuator.hasKit()) {
				alreadyHasKit = true;
				led(color["blue"]);
			} else {
				led(color["red"]);
				actuator.Down();
			}
	
			if (isWhite(new byte[] {1,2,3,4})) {
				moveTime(-300, 350);
				if (isWhite(new byte[] {1,2,3,4})) moveTime(-300, 450);
			}
			Centralize();
		}
	}

	void RedEnd () {
		if (anySensorColor("red")) {
			local = Local.end;
		}
	}

	void TrackEnd () {
		if (isBlue(2) && isBlue(3)) {
			CentralizeGyro();
			time.reset();
			do {
				forward(300);
				if (ultra(2) < 60 && ultra(3) < 60) {
					local = Local.rescue;
					return;
				}
			} while (time.timer() < 500);
		}
	}
	
//

void Track () {
	console(1, "$>--Pista--<$", color["comment"]);

	while (local == Local.track) {
		LineFollower();
		Obstacle();
		Ramp();
		Kit();
		TrackEnd();
		RedEnd();
	}
}

//Rescue - imported files


	void Exit (sbyte side_mod) {
	
		//verifies if already is in front of a empty space
			for (byte i = 0; i < 4; i++) {
				if (ultra(i) > 360) {
					while (ultra(i) > 400) left(1000 * side_mod);
				}
			}
		//
	
		FindTheExitAgain:
		//goes to the exit
			led(color["white"]);
	
			//identify which sensor reads more than the limit
				byte whichSensor = 0;
				while (whichSensor == 0) {
					right(1000 * side_mod);
					for (byte i = 0; i < 4; i++) {
						if (ultra(i) > 400) {
							whichSensor = i;
							break;
						}
					}
				}
				console(2, $"{whichSensor}");
			//
	
			//centralizes with the empty space
				AlreadyFindedTheExit:
	
				time.reset();
				while (ultra(whichSensor) > 360) right(1000*side_mod);
				int timeDetecting = time.timer();
	
				time.reset();
				while (time.timer() < timeDetecting/1.8) left(1000*side_mod);
	
				if (whichSensor == 2) rotate(500, 90);
				else if (whichSensor == 3) rotate(500, -90);
	
				if (ultra(1) < 400) {
					goto FindTheExitAgain;
				}
	
				stop();
				open_actuator = false;
				actuator.Down();
			//
	
			//goes to the empty space
				time.reset();
				while (isWhite(new byte[] {2,3})) FollowerGyro(direction());
				int timeToBack = time.timer();
				moveTime(300, 50);
			//
	
			//verifies if that is the entrance or the exit
				if (anySensorColor("blue") && !isThatColor(1, "GREEN") && !isThatColor(2, "GREEN") && !isThatColor(3, "GREEN") && !isThatColor(4, "GREEN") && !isThatColor(2, "CYAN") && !isThatColor(3, "CYAN")) {
					led(color["orange"]);
					moveTime(-300, timeToBack);
					if (whichSensor == 2) rotate(500, -90*side_mod);
					else if (whichSensor == 3) rotate(500, 90*side_mod);
					while (ultra(whichSensor) > 360) {
						right(1000*side_mod);
						for (byte i = 0; i < 4; i++) {
							if (ultra(i) > 400 && i != whichSensor) {
								whichSensor = i;
								goto AlreadyFindedTheExit;
							}
						}
					}
					right(1000*side_mod);
					delay(350);
					goto FindTheExitAgain;
				} else {
					led(color["green_dark"]);
	
					//goes forward until is in the line
						while (!isFullBlack(1) && !isFullBlack(2) && !isFullBlack(3) && !isFullBlack(4)) forward(300);
						while (isThatColor(1, "GREEN") || isThatColor(2, "GREEN") || isThatColor(3, "GREEN") || isThatColor(4, "GREEN") || isThatColor(1, "CYAN") || isThatColor(2, "CYAN") || isThatColor(3, "CYAN") || isThatColor(4, "CYAN")) forward(200);
						moveTime(300, 100);
					//
	
	
					//Centralize in the empty space
						stop();
						actuator.Up();
						while ((!isThatColor(2, "GREEN") || !isThatColor(2, "CYAN")) && (!isThatColor(3, "GREEN") || isThatColor(3, "CYAN"))) right(1000);
						CentralizeGyro();
	
						if (ultra(1) > 60) {
							rotate(500, -10);
						}
						GoToDistance(23);
					//
	
					//returns to the line
						rotate(500, -45);
						while (!isWhite(4)) left(1000);
						while (isWhite(new byte[] {2,3}) && !isOrtogonal()) left(1000);
						if (scaleAngle(direction()) > 25) {
							moveTime(300, 75);
							if (isWhite(new byte[] {1,2,3,4})) {
								rotate(500, -30);
							}
						}
						Centralize();
						stop();
						actuator.Down();
					//
	
					local = Local.exit;
				}
			//
		//
	
	}

	float last_R = 0;
	float last_T_R = 10000;
	float last_L = 0;
	float last_T_L = 10000;
	
	int start_ultra = 0;
	const int ultra_interval = 110;
	
	void DetectTriangle (char side, bool reset = false) {
	
		if (time.millis() - start_ultra > ultra_interval) {
			byte sensor = (byte) (side == 'R' ? 2 : 3);
			float last_T = side == 'R' ? last_T_R : last_T_L ;
	
			if (maths.interval(Math.Abs(last_T - ultra(sensor)), 8, 11) && ultra(sensor) > 70) {
				moveTime(300, 300);
				actuator.Up();
				if (side == 'R') CentralizeGyro(90);
				else CentralizeGyro(-90);
				if (!actuator.hasVictim() && !actuator.hasKit()) actuator.Down();
			} else {
				if (side == 'R') last_T_R = (int) last_R;
				else last_T_L = (int) last_L;
	
				if (reset) start_ultra = time.millis();
			}
		}
	
	}
	
	void DetectVictim (byte sensor, float last, string operation) {
	
		if (maths.interval(last - ultra(sensor), 5, maxReadVictim) && !actuator.isUp()) {
			//align with the ball
				float last_ultra = ultra(sensor);
				moveTime(300, 50);
				if (ultra(sensor) > 1000 || last_ultra - ultra(sensor) < 1) return;
			//
	
			if (operation == "normal") Search(sensor);
			else SearchTriangle(sensor);
		}
	
	}
	
	void Ultras (bool victims = true, bool triangle = false, string operation = "normal") {
		last_R = ultra(2);
		last_L = ultra(3);
	
		FollowerGyro();
		delay(16);
	
		if (victims) {
			DetectVictim(2, last_R, operation);
			DetectVictim(3, last_L, operation);
		}
	
		if (triangle) {
			DetectTriangle('R');
			DetectTriangle('L', true);
		}
	}

	void Search (byte sensor) {
		sbyte side_mod = (sbyte) (sensor == 2 ? 1 : -1);
	
		if (ultra(sensor) < 265 && !actuator.hasVictim() && !actuator.hasKit()) {
			stop();
			console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);
	
			actuator.Up();
			if (actuator.hasVictim() || actuator.hasKit()) return;
	
			//align with the ball
				float last_ultra = 0;
				time.reset();
				do {
					last_ultra = ultra(sensor);
					forward(150);
					delay(15);
				} while (ultra(sensor) <= last_ultra && time.timer() < 500);
				if (time.timer() > 450) {
					actuator.Down();
					return;
				}
			//
	
			//triangle calculation
				moveTime(-300, 500);
				int angleToRotate = (int) ((180/Math.PI)*(Math.Atan(last_ultra/23)));
				int zmToMove = maths.hypotenuse(last_ultra, 23) + 1;
			//
	
			//go rescue
				rotate(500, angleToRotate*side_mod);
				actuator.Down();
				moveZm(zmToMove);
				actuator.Up();
				stop(150);
				moveZm(-zmToMove);
				rotate(500, -angleToRotate*side_mod);
				centerQuadrant();
			//
	
			if (!actuator.hasVictim() && !actuator.hasKit()) actuator.Down();
	
			clear();
		}
	
	}

	int maxReadVictim = 10000;
	
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
					reverse(300, timeToFind);
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
						if (angleToRotate == 135) angleToRotate++;
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
	
					//if dont rescue
						if (!actuator.hasVictim()) {
							maxReadVictim = 400;
						}
					//
	
					//dispatch in the triangle
						rotate(500, angleToRotate*side_mod);
						while (!isFullBlack(5)) FollowerGyro(direction());
	
						if (actuator.isAlive() || AliveVictimsRescued > 1 || actuator.hasKit()) {
							Dispatch();
							AliveVictimsRescued++;
						}
	
						if (angleToRotate <= 137) {
							if (angleToRotate > 130) rotate(500, 10*side_mod);
							else rotate(500, (int)((180-Math.Abs(angleToRotate))*side_mod));
						}
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
	
					reverse(300, timeToFind);
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
	
				if (AliveVictimsRescued >= 2) {
					DispatchDeadVictim(side_mod);
				}
				actuator.Down();
				timeToFind = time.millis();
			//
	
			clear();
		}
	
	}

	char side_triangle = 'n';
	int timeToFind = 0;
	
	void Triangle () {
	
		bool TriRight () {
			if (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 97 && ultra(2) < 55) {
				side_triangle = 'R';
				return true;
			} else return false;
		}
	
		bool TriLeft () {
			if (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 97 && ultra(3) < 55) {
				side_triangle = 'L';
				return true;
			} else return false;
		}
	
		if (TriRight() || TriLeft()) {
	
			//verifies if is the triangle
				time.reset();
				float last_ultra = ultra(1);
				do {
					FollowerGyro();
					if (last_ultra - 2 > ultra(1)) return;
				} while (time.timer() < 150);
	
				console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);
				sbyte side_mod = (sbyte) (side_triangle == 'L' ? 1 : -1);
			//
	
			//lifts the actuator and dispatches if it has a victim
				stop();
				actuator.Up();
				if ((actuator.hasVictim() && actuator.isAlive()) || actuator.hasKit()) {
					bool wasKit = actuator.hasKit();
					Dispatch();
					if (!wasKit) AliveVictimsRescued++;
				} else if (actuator.hasVictim()) {
					DeadVictim(side_mod);
				}
			//
	
			//position the robot in the side of the triangle
				if (!DeadVictimReserved) {
					GoToDistance(95);
					CentralizeGyro(90 * side_mod);
					reverse(300, 750);
				}
				actuator.Down();
			//
	
			//search for victims
				byte timesSearched = 0;
	
				VictimInEnd:
				bool wall_ahead = (ultra(1) < 400);
				timeToFind = time.millis();
				while (((wall_ahead && !DetectWall()) || (!wall_ahead && isWhite(new byte[] {1,2,3,4}))) && time.millis() - timeToFind < 10000) {
					FollowerGyro();
					Ultras(true, false, "triangle");
				}
				int mid_arena = (int)((time.millis()-timeToFind)/2.5);
				if (time.millis() - timeToFind > 9950) {
					mid_arena = (time.millis()-timeToFind)/4;
				}
	
				//wall or line
					stop();
					actuator.Up();
					CentralizeGyro();
					if (actuator.hasVictim() && timesSearched < 3) {
						led(color["red"]);
						SearchTriangle(2, true);
						timesSearched++;
						goto VictimInEnd;
					}
				//
	
			//
	
			//search for the exit
				console(2, $"{time.millis() - timeToFind} | {mid_arena}");
	
				if (DeadVictimReserved) { //rescue the remanescent dead victim
					DispatchDeadVictim(side_mod);
					moveTime(300, mid_arena);
				} else moveTime(-300, mid_arena);
	
				Exit(side_mod);
			//
	
		}
	
	}
	

	void DeadVictim (sbyte side_mod) {
		GoToDistance(95);
		CentralizeGyro(90 * side_mod);
		reverse(300, 750);
		moveZm(95);
		CentralizeGyro(-90 * side_mod);
	
		while (ultra(1) > 40) FollowerGyro();
		stop(250);
		open_actuator = false;
		actuator.Down();
		moveTime(200, 300);
	
		delay(500);
		moveTime(-300, 150);
		moveTime(300, 50);
	
		GoToDistance(95);
		CentralizeGyro(90 * side_mod);
		reverse(300, 1500);
	
		DeadVictimReserved = true;
		open_actuator = true;
	}
	
	void DispatchDeadVictim (sbyte side_mod) {
		reverse(300, (time.millis() - timeToFind) - 250);
		moveZm(95);
		CentralizeGyro(-90 * side_mod);
	
		stop();
		actuator.Down();
		while (!DetectWall()) FollowerGyro();
		stop();
		actuator.Up();
	
		CentralizeGyro(-90 * side_mod);
		GoToDistance(80);
		Dispatch();
	
		CentralizeGyro(90 * side_mod);
		GoToDistance(95);
		CentralizeGyro(90 * side_mod);
		reverse(300, 1250);
	
		DeadVictimReserved = false;
	}
//

bool DeadVictimReserved = false;
public static byte AliveVictimsRescued = 0;

void Rescue () {
	if (local == Local.rescue) {
		clear();
		console(1, "$>--Resgate--<$", color["comment"]);
		moveTime(300, 300);

		open_actuator = true;
		if (!actuator.hasKit()) actuator.Down();
	}

	while (local == Local.rescue) {
		Ultras(true, true);
		Triangle();
	}
}

//Finish - imported files

	string[] rainbow = {"#FF0000", "#FF8800", "#FFFF00", "#00FF00", "#00DDDD", "#00AAFF", "#0044FF", "#AA00FF", "#FF00FF", "#FF0088"};
	
	string[] fade = {"#43434E", "#52525C", "#676770", "#77777F", "#8C8C93", "#A6A6AB", "#C3C3C6", "#E2E2E4", "#EEEEEE", "#FFFFFF"};
	
	void Celebrate (string word, string[] colors) {
	
		string word_final = "";
		int colors_index = 0;
	
		string colorize (char text, string hex) => $"<color={hex}>{text}</color>";
	
		while (true) {
			colors_index++;
	
			word_final = "";
			for (byte i = 0;  i < word.Length; i++) {
				word_final += colorize(word[i], colors[colors_index % colors.Length]);
				led(colors[colors_index % colors.Length]);
				colors_index++;
			}
	
			console(1, $"<b><size=60><align=center>{word_final}</align></size></b>");
			delay(55);
		}
	
	}
//

void Finish () {
	console(1, "$>--Saída--<$", color["comment"]);
	while (local == Local.exit) {
		RedEnd();
		LineFollower();
		Obstacle();
		Ramp();
	}
	moveTime(300, 200);
	Celebrate("D4RKMODE", fade);
}

void Main () {

	Tests();
	Setup();
	Track();
	Rescue();
	Finish();

}
