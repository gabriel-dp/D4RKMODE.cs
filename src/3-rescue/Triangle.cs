bool DetectTriangleRight() {
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

