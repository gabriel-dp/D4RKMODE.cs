int direction () => (int) bot.Compass();

byte Direction () {
	byte quadrant = 0;
	if (maths.interval(direction(), 46, 135)) quadrant = 1;
	else if (maths.interval(direction(), 136, 225)) quadrant = 2;
	else if (maths.interval(direction(), 226, 315)) quadrant = 3;
	return quadrant;
}

int scaleAngle (int angle) {
	angle = angle%90;
	if (angle >= 45) angle -= 2*(angle-45);
	return angle;
}
