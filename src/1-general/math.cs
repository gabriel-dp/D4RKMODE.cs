public class Maths {

	public int map (float number, float min, float max, float minTo, float maxTo) {
		return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
	}

}

Maths maths = new Maths();
