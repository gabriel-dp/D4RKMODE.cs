/*--------------------------------------------------
D4RKMODE
Sesi Anísio Teixeira - VCA/BA
--------------------------------------------------*/

const bool console_on = true;

enum Local {
	track,
	rescue,
	exit,
	end
};

Local local = Local.track;
void Setup () {
	open_actuator = false;
	actuator.Down();

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

//General - imported files
	public class Maths {
	
		const float PI = 3.14f;
	
		public int map (float number, float min, float max, float minTo, float maxTo) {
			return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
		}
	
		public bool interval (float val, float min, float max) => (val >= min && val <= max);
	
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
	//
	public class Colors {
		public int R (byte sensor) => (int) (bot.ReturnRed(sensor-1) + 1);
		public int G (byte sensor) => (int) (bot.ReturnGreen(sensor-1) + 1);
		public int B (byte sensor) => (int) (bot.ReturnBlue(sensor-1) + 1);
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
	
			int direction_actual = direction();
			if ((direction_ideal >= 0 && direction_ideal < 15) && direction_actual > 300) direction_actual -= 360;
			else if ((direction_ideal > 345 && direction_ideal <= 360) && direction_actual < 300) direction_actual += 360;
	
			error = direction_actual-direction_ideal;
			turn = error*Kg;
	
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
	
		printMotors();
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
	
	void clear (int line = 2) {
		bot.ClearConsoleLine(line - 1);
		if (line == 2) led(color["white"]);
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
	
		public void Adjust (int ideal_actuator, int ideal_scoop, string operation = "close") {
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
	
		public bool isAlive () {
			Adjust(45, 0);
			bot.Wait(500);
			if (bot.Heat() > 34 && bot.Heat() < 37) {
				Up();
				bot.Wait(500);
				return (bot.Heat() > 35.5);
			}
			return false;
		}
	
	}
	
	Actuator actuator = new Actuator();
	
	void Dispatch () {
		Retry:
		moveTime(-200, 600);
		actuator.Adjust(20, 0);
		stop(100);
		moveTime(300, 600);
		stop(200);
		moveTime(-300, 200);
		actuator.Up();
		CentralizeGyro();
		if (actuator.hasVictim()) goto Retry;
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
		} else if (isGreen(1) || isGreen(2)) {
			green_direction = 'R';
		} else if (isGreen(3) || isGreen(4)){
			green_direction = 'L';
		}
	}
	
	void Green () {
		if (isGreen(1) || isGreen(2) || isGreen(3) || isGreen(4)) {
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
	
			console(2, $"{green_direction}");
	
			//goes forward and rotate in the axis to make the curve
				moveTime(300, 425);
				if (green_direction == 'B') {
					CentralizeGyro(180);
				} else {
					if (green_direction == 'R') {
						if (scaleAngle(direction()) < 25) CentralizeGyro(45);
						while (!isFullBlack(3) && !isFullBlack(4)) right(1000);
					} else {
						if (scaleAngle(direction()) < 25) CentralizeGyro(-45);
						while (!isFullBlack(2) && !isFullBlack(1)) left(1000);
					}
				}
				Centralize();
			//
	
			clear();
		}
	}
	void CurveBlack () {
		char curve_side = 'n';
		if (!isWhite(1) && !isColorized(1)) {
			console_led(2, "Curva para Direita →", color["black"]);
			curve_side = 'R';
		} else if (!isWhite(4) && !isColorized(4)) {
			console_led(2, "Curva para Esquerda ←", color["black"]);
			curve_side = 'L';
		}
	
		if (curve_side != 'n') {
	
			//verifies if sensor misread green
				moveTime(300, 15);
				if (isWhite(1) && isWhite(4)) {
					reverse(300, 100);
					return;
				}
				GreenClassifier();
				if (green_direction != 'n') {
					return;
				}
			//
	
			//tries to centralize and avoid "false curves" then goes forward and rotate in the axis
				Centralize();
	
				if ((!isWhite(1) && !isColorized(1)) || (!isWhite(4) && !isColorized(4))) {
					moveTime(300, 315);
	
					int maxTimeToRotate = 4400;
					LostTheLine:
					time.reset();
	
					if (curve_side == 'R') {
						while (((!isFullBlack(3) || isColorized(3)) && (!isFullBlack(4) || isColorized(4))) && time.timer() < maxTimeToRotate) right(1000);
					} else {
						while (((!isFullBlack(2) || isColorized(2)) && (!isFullBlack(1) || isColorized(1))) && time.timer() < maxTimeToRotate) left(1000);
					}
	
					//if doesnt find the line
						if (time.timer() > maxTimeToRotate - 50) {
							led(color["orange"]);
							moveTime(-300, 200);
							if (curve_side == 'R') curve_side = 'L';
							else curve_side = 'R';
							maxTimeToRotate += maxTimeToRotate;
							goto LostTheLine;
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
	const int motor_limit = 190;
	
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
				CentralizeGyro(0);
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
	void Obstacle () {
		if (DetectWall()) {
			led(color["pink"]);
			reverse(300, 50);
			stop();
			actuator.Up();
	
			//after lifting the actuator, follows the line for a time to confirm that's a obstacle
				const int timeObstacle = 1750;
				time.reset();
				while (time.timer() < timeObstacle && ultra(1) > 15) {
					LineFollower();
				}
	
				//if isn't a obstacle, downs the actuator
				if (time.timer() > timeObstacle - 50) {
					stop();
					actuator.Down();
					return;
				}
			//
	
			//if is a obstacle, starts avoid it
				led(color["purple"]);
				bool surpassed = false;
	
				void GoForward (bool first = true, bool second = true) {
					while (ultra(3) > 50 && first) FollowerGyro(direction());
					while (ultra(3) < 50 && second) FollowerGyro(direction());
				}
	
				void AfterAvoid () {
					CentralizeGyro(90);
					reverse(300, 100);
					Centralize();
					actuator.Down();
					surpassed = true;
					clear();
				}
	
				//	|-> /
					CentralizeGyro(45);
					rotate(500, 20);
					GoForward();
	
				//	/ -> |
					CentralizeGyro(-90);
					//search for the line in a 90 degress obstacle
						while (ultra(3) > 50 && !surpassed) {
							forward(200);
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
							forward(200);
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
	void Ramp () {
		if (inclination() < -7) {
			led(color["blue"]);
	
			//lifts the actuator and follows the line for a time then down that
				stop();
				actuator.Up();
				time.reset();
				while (time.timer() < 2000 + 100*scaleAngle(direction())) {
					LineFollower();
				}
				actuator.Down();
				int last_inclination = inclination();
				while (inclination() < -2) LineFollower();
				int last_inclination2 = inclination();
			//
	
			//if is a seesaw needs to wait that down
				if (last_inclination - last_inclination2 > -12) {
					stop(750);
					moveTime(-300, 250);
				} else {
					stop(200);
				}
			//
		}
	}
	void RedEnd () {
		for (byte i = 1; i<5; i++) {
			if (isRed(i)) local = Local.end;
		}
	}
	void TrackEnd () {
		/* ESSE ERA O ANTIGO
		if (isWhite(new byte[] {1,2,3,4}) && (ultraLimits(1, 350, 390) || ultraLimits(1, 250, 290)) && (ultra(2) < 45 && ultra(3) < 45)) {
			local = Local.rescue;
		}
		*/
	
		if (isWhite(new byte[] {1,2,3,4}) && (ultra(2) < 45 && ultra(3) < 45)) {
			local = Local.rescue;
		}
	}
//

void Track () {
	console(1, "$>--Track--<$", color["comment"]);

	while (local == Local.track) {
		LineFollower();
		Obstacle();
		Ramp();
		TrackEnd();
		RedEnd();
	}
}

//Rescue - imported files
	char sideToSearch = 'R';
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
				moveTime(300, 200);
				actuator.Up();
				if (side == 'R') CentralizeGyro(90);
				else CentralizeGyro(-90);
				if (!actuator.hasVictim()) actuator.Down();
			} else {
				if (side == 'R') last_T_R = (int) last_R;
				else last_T_L = (int) last_L;
	
				if (reset) start_ultra = time.millis();
			}
		}
	
	}
	
	void DetectVictim (byte sensor, float last, string operation) {
	
		if (last - ultra(sensor) > 5 && !actuator.isUp()) {
			//align with the ball
				float last_ultra = 0;
				time.reset();
				do {
					last_ultra = ultra(sensor);
					forward(200);
					delay(30);
				} while (ultra(sensor) <= last_ultra && time.timer() < 125);
				if (time.timer() < 100 || ultra(sensor) > 1000) return;
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
	
		if (ultra(sensor) < 265 && !actuator.hasVictim()) {
			stop();
			console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);
	
			actuator.Up();
			if (actuator.hasVictim()) return;
	
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
				if (sideToSearch == 'L') angleToRotate = -angleToRotate;
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
	
			if (!actuator.hasVictim()) actuator.Down();
	
			clear();
		}
	
	}
	void SearchTriangle (byte sensor) {
		sbyte side_mod = (sbyte) (sensor == 2 ? 1 : -1);
	
		if (ultra(sensor) < 265 && !actuator.hasVictim()) {
			stop();
			console_led(2, $"$>Vítima<$ detectada a $>{(int)ultra(sensor)}<$ zm", color["cyan"]);
	
			timeToFind = time.millis() - timeToFind - 250;
	
			actuator.Up();
			if (actuator.hasVictim()) return;
	
			//align with the ball
				float last_ultra = 0;
				time.reset();
				do {
					last_ultra = ultra(sensor);
					forward(150);
					delay(15);
				} while (ultra(sensor) <= last_ultra && time.timer() < 1000);
				if (time.timer() > 950) return;
			//
	
			//triangle calculation
				int zmToMove = (int)(last_ultra+1);
	
				int twoLegs = timeByZm(timeToFind);
				float prop = last_ultra/50;
				int bigLeg = (int)((prop*twoLegs)/(1+prop));
	
				int angleToRotate = 180 - (int) ((180/Math.PI)*(Math.Atan(bigLeg/last_ultra)));
				console(2, $"{twoLegs} | {prop} | {bigLeg} | {angleToRotate}");
			//
	
			//go rescue
				CentralizeGyro(90*side_mod);
				actuator.Down();
				moveZm(zmToMove);
				actuator.Up();
				stop(150);
			//
	
			//dispatch in the triangle
				rotate(500, angleToRotate*side_mod);
				while (!isFullBlack(5)) FollowerGyro(direction());
				Dispatch();
	
				stop(9999);
			//
	
			if (!actuator.hasVictim()) actuator.Down();
	
			clear();
		}
	
	}
	char side_triangle = 'n';
	int timeToFind = 0;
	
	void Triangle () {
	
		bool TriRight () {
			if (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 90 && ultra(2) < 55) {
				side_triangle = 'R';
				return true;
			} else return false;
		}
	
		bool TriLeft () {
			if (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 90 && ultra(3) < 55) {
				side_triangle = 'L';
				return true;
			} else return false;
		}
	
		if (TriRight() || TriLeft()) {
	
			//verify if is the triangle
				time.reset();
				float last_ultra = ultra(1);
				do {
					FollowerGyro();
					if (last_ultra - 2 > ultra(1)) return;
				} while (time.timer() < 150);
			//
	
			console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);
	
			stop();
			actuator.Up();
	
			if (actuator.hasVictim()) {
				Dispatch();
			}
	
			GoToDistance(95);
			if (side_triangle == 'R') CentralizeGyro(-90);
			else CentralizeGyro(90);
			reverse(300, 750);
			actuator.Down();
	
			bool wall_ahead = (ultra(1) < 400);
			timeToFind = time.millis();
			while ((wall_ahead && !DetectWall()) || (!wall_ahead && isWhite(new byte[] {1,2,3,4}))) {
				FollowerGyro();
				Ultras(true, false, "triangle");
			}
	
			actuator.Up();
			stop(9999);
	
	
		}
	
	}
	
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		centerQuadrant();
		moveTime(300, 400);

		open_actuator = true;
		actuator.Down();
	}

	while (local == Local.rescue) {
		Ultras(true, true);
		Triangle();
	}
}
void Finish () {
	while (local == Local.exit) {
		RedEnd();
		LineFollower();
		Obstacle();
		Ramp();
	}
	moveTime(300, 200);
}

void Main () {

	Setup();
	Track();
	Rescue();
	Finish();

}
