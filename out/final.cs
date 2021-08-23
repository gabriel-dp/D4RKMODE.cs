/*--------------------------------------------------
D4RKMODE
Sesi Anísio Teixeira - VCA/BA
--------------------------------------------------*/

void Setup () {
	Centralize();
}
//imported files
	public class Maths {
	
		public int map (float number, float min, float max, float minTo, float maxTo) {
			return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
		}
	
	}
	
	Maths maths = new Maths();
	void move (float motorL, float motorR) => bc.Move(motorL, motorR);
	
	void forward (float motor) => move (motor, motor);
	void back (float motor) => move (-motor, -motor);
	void right (float motor) => move (motor, -motor);
	void left (float motor) => move (-motor, motor);
	
	//Global Variables
	int error = 0;
	
	//Constants of calibration
	const float max_white = 60;
	const byte high_black = 50;
	const byte low_black = 90;
	
	//Main method
	int light (int sensor) {
		float raw = bc.Lightness(sensor - 1);
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
//imported files
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
		} while (Math.Abs(error) > Kc);
	}

void Main () {

	Setup();

}
