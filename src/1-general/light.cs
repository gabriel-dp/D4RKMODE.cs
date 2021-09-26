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
		for (byte i = 0; i < 4; i++) {
			if (light(i) < low_black && !isColorized(i)) return true;
		}
		return false;
	}
//
