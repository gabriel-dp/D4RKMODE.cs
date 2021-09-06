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
	if (Math.Abs(bot.GetFrontalLeftForce()-bot.GetFrontalRightForce()) > 380 && ultra(1) < 120) {
		time.reset();
		float last_ultra = ultra(1);
		do {
			FollowerGyro();
			if (last_ultra - 1 > ultra(1)) return;
		} while (time.timer() < 100);

		stop();
		actuator.Up();
		if (actuator.victim() != null) led(color["red"]);

		alignToTriangle(side_triangle);
		reverse(1000);
		stop(9999);
	}
}

