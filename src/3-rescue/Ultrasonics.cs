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
