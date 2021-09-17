float last_R = 0;
float last_T_R = 10000;
float last_L = 0;
float last_T_L = 10000;

int start_ultra = 0;
const int ultra_interval = 120;

void DetectTriangle (char side, bool reset = false) {

	if (time.millis() - start_ultra > ultra_interval) {
		byte sensor = (byte) (side == 'R' ? 2 : 3);
		float last_T = side == 'R' ? last_T_R : last_T_L ;

		if (maths.interval(Math.Abs(last_T - ultra(sensor)), 8, 11) && ultra(sensor) > 70) {
			moveTime(300, 250);
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

void DetectVictim (byte sensor, float last) {

	if (maths.interval(last - ultra(sensor), 5, 400)) {
		Search(sensor);
	}

}

void Ultras (bool victims = true, bool triangle = false) {
	last_R = ultra(2);
	last_L = ultra(3);

	FollowerGyro();
	delay(16);

	if (victims) {
		DetectVictim(2, last_R);
		DetectVictim(3, last_L);
	}

	if (triangle) {
		DetectTriangle('R');
		DetectTriangle('L', true);
	}
}
