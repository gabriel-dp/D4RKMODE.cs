bool DetectTriangleRight () {
	if (ultra(2) > 400) return false;

	int last_frontal = (int) ultra(1);
	int last_lateral = (int) ultra(2);
	GoToDistance(last_frontal-10);

	if (maths.interval(ultra(2) - last_lateral, 9, 11)) {
		CentralizeGyro(90);
		return true;
	}

	return false;
}

const int triangle_hypotenuse = 120;
byte side_triangle = 3;

void alignToTriangle (byte side) {
	if (side == 3) {
		CentralizeGyro(45);
		int ideal_ultra = (int)((triangle_hypotenuse/2)+ultra(side));
		GoToDistance(ideal_ultra);
		centerQuadrant();
		int degress = -10;
		if (ultra(1) < 280) degress = 10;
		CentralizeGyro(45);
		rotate(500, degress);
	} else {
		CentralizeGyro(-45);
		int ideal_ultra = (int)((triangle_hypotenuse/2)+ultra(side));
		GoToDistance(ideal_ultra);
		centerQuadrant();
		int degress = 10;
		if (ultra(1) < 280) degress = -10;
		CentralizeGyro(-45);
		rotate(500, degress);
	}
}

void Triangle () {
	if (sideToSearch == 'L') side_triangle = 2;
	alignToTriangle(3);
}

