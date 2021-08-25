float ultra (byte sensor) {
	float ultra_data = bc.Distance(sensor - 1);
	ultra_data = ((float)((int)(ultra_data *100)))/100;
	return ultra_data;
}

bool ultraLimits(byte sensor, int min, int max) => bot.DetectDistance(sensor-1, min, max);
