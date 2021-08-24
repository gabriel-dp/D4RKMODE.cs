/*--------------------------------------------------
D4RKMODE
Sesi Anísio Teixeira - VCA/BA
--------------------------------------------------*/

const bool console_on = true;

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
	void delay (int time) => bot.Wait(time);
	void move (float motor_L, float motor_R) => bot.Move(motor_L, motor_R);
	
	void forward (float motor) => move (motor, motor);
	void back (float motor) => move (-motor, -motor);
	void right (float motor) => move (motor, -motor);
	void left (float motor) => move (-motor, motor);
	
	void stop (int time = 0) {
		move(0, 0);
		delay(time);
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
//

void Track () {
	console(1, "$>--Track--<$", color["comment"]);
	while (true) {
		LineFollower();
	}
}


void Main () {

	Setup();
	Track();

}
