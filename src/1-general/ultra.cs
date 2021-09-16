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
