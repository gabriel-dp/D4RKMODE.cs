byte side_sensor = 3;

void Search () {
	if (sideToSearch == 'R') side_sensor = 2;

	if (ultra(side_sensor) < 150) {
		stop(9999);
	}

}
