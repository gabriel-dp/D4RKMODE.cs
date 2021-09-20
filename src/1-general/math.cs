public class Maths {

	const float PI = 3.14f;

	public bool interval (float val, float min, float max) => (val >= min && val <= max);

	public int map (float number, float min, float max, float minTo, float maxTo) {
		return (int)((((number - min) * (maxTo - minTo)) / (max - min)) + minTo);
	}

	public int hypotenuse (double leg1, double leg2) {
		return (int) (Math.Sqrt(Math.Pow(leg1, 2)+Math.Pow(leg2,2)));
	}

	public int ArcTan (double leg1, double leg2) {
		return (int) ((180/PI)*(Math.Atan(leg1/leg2)));
	}

}

Maths maths = new Maths();
