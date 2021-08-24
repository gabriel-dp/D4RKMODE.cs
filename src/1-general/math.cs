public class Maths {

	public int map (float number, float min, float max, float minTo, float maxTo) {
		return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
	}

	public bool interval (float val, float min, float max) => (val >= min && val <= max);

}

Maths maths = new Maths();
