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

	bool TriRight () => (bot.GetFrontalLeftForce()-bot.GetFrontalRightForce() > 380 && ultra(1) < 75 && ultra(3) > 50 && ultra(2) < 55);

	bool TriLeft () => (bot.GetFrontalRightForce()-bot.GetFrontalLeftForce() > 380 && ultra(1) < 75 && ultra(2) > 50 && ultra(3) < 55);

	if (TriRight() || TriLeft()) {

		//verify if is the triangle
			time.reset();
			float last_ultra = ultra(1);
			do {
				FollowerGyro();
				if (last_ultra - 2 > ultra(1)) return;
			} while (time.timer() < 150);
		//

		console_led(2, "$>Triângulo<$ detectado", color["gray"], color["black"]);

		stop();
		actuator.Up();

		alignToTriangle(side_triangle);
		reverse(1000);
		stop(9999);


	}

}

