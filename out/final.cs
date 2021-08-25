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
	Centralize();
	actuator.Down();
}

//General - imported files
	public class Maths {
	
		public int map (float number, float min, float max, float minTo, float maxTo) {
			return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
		}
	
		public bool interval (float val, float min, float max) => (val >= min && val <= max);
	
	}
	
	Maths maths = new Maths();
	void delay (int ms) => bot.Wait(ms);
	
	public class Time {
	
		public int millis () => bot.Millis();
	
		public int timer () => bot.Timer();
	
		public void reset () => bot.ResetTimer();
	
	}
	
	Time time = new Time();
	void move (float motor_L, float motor_R) => bot.Move(motor_L, motor_R);
	
	void forward (float motor) => move (motor, motor);
	void back (float motor) => move (-motor, -motor);
	void right (float motor) => move (motor, -motor);
	void left (float motor) => move (-motor, motor);
	
	void rotate (float motor, float angle) => bot.MoveFrontalAngles(motor, angle);
	
	//More methods
	void stop (int ms = 0) {
		move(0, 0);
		delay(ms);
	}
	
	void moveTime (float motor, int ms) {
		time.reset();
		while (time.timer() < ms) forward(motor);
		stop();
	}
	
	void reverse (float motor, int ms = 999999) {
		time.reset();
		while (!bot.Touch(0) && time.timer() < ms) back(motor);
		stop();
	}
	
	//Global Variables
	int error = 0;
	int last_error = 0;
	int turn = 0;
	int motorR = 0;
	int motorL = 0;
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
		public int R (byte sensor) => (int) bot.ReturnRed(sensor-1);
		public int G (byte sensor) => (int) bot.ReturnGreen(sensor-1);
		public int B (byte sensor) => (int) bot.ReturnBlue(sensor-1);
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
				default:
					break;
			}
	
		}
	//
	float ultra (byte sensor) {
		float ultra_data = bc.Distance(sensor - 1);
		ultra_data = ((float)((int)(ultra_data *100)))/100;
		return ultra_data;
	}
	
	bool ultraLimits(byte sensor, int min, int max) => bot.DetectDistance(sensor-1, min, max);
	
	void GoToDistance (int distance) {
		do {
			error = (int) (ultra(1) - distance);
			forward(error*50);
		} while (error != 0);
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
	
	//both
	void console_led (int line, string text, string hexcolor, string ledcolor = "") {
		if (ledcolor == "") ledcolor = hexcolor;
		console(line, text, hexcolor);
		led(ledcolor);
	}
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
//

//Track - imported files
	const int Kc = 15;
	
	void Centralize () {
	
		//If only the external sensors are above the line
		if (isWhite(new byte[] {1,2,3}) && isBlack(4)) {
			while (!isFullBlack(3)) left(1000);
		} else if (isBlack(1) && isWhite(new byte[] {2,3,4})) {
			while (!isFullBlack(2)) right(1000);
		}
	
		do {
			error = (light(2) - light(3)) * Kc;
			left(error);
		} while (Math.Abs(error) > Kc*2);
	
		stop();
	}
	const float Kp = 1;
	const float Kd = 20;
	
	const int vel_max_front = 300;
	const int vel_max_axis = 1000;
	
	const int turn_divider = 10;
	const int motor_limit = 100;
	const int motor_axis_min = 150;
	
	
	
	void LineFollower () {
	
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
	
		//
	
		//move(motorR, motorL);
		led(color["white"]);
	
	}
	void RedEnd () {
		for (byte i = 1; i<5; i++) {
			if (isRed(i)) local = Local.end;
		}
	}
	void TrackEnd () {
		if (isWhite(new byte[] {1,2,3,4}) && (ultraLimits(1, 350, 390) || ultraLimits(1, 250, 290)) && (ultra(2) < 45 && ultra(3) < 45)) {
			local++;
		}
	}
//

void Track () {
	console(1, "$>--Track--<$", color["comment"]);
	while (local == Local.track) {
		forward(200);
		TrackEnd();
	}
}

//Rescue - imported files
	bool DetectTriangleRight() {
		if (ultra(2) > 400) return false;
	
		int last_frontal = (int) ultra(1);
		int last_lateral = (int) ultra(2);
		GoToDistance(last_frontal-10);
		if (maths.interval(ultra(2) - last_lateral, 9, 11)) led(color["red"]);
		return maths.interval(ultra(2) - last_lateral, 9, 11);
	}
	
//

void Rescue () {
	if (local == Local.rescue) {
		console(1, "$>--Rescue--<$", color["comment"]);
		centerQuadrant();

		moveTime(300, 400);
		DetectTriangleRight();
	}

	while (local == Local.rescue) {

	}
}

void Main () {

	Setup();
	Track();
	Rescue();

}
