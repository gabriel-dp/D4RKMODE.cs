public class Colors {
	public float R (byte sensor) => (bot.ReturnRed(sensor-1) + 1);
	public float G (byte sensor) => (bot.ReturnGreen(sensor-1) + 1);
	public float B (byte sensor) => (bot.ReturnBlue(sensor-1) + 1);
}

Colors colors = new Colors();

//Default colors of simulator
	Dictionary <string, string> colorNames = new Dictionary <string,string> () {
		{"WHITE","BRANCO"},
		{"BLACK","PRETO"},
		{"GREEN","VERDE"},
		{"MAGENTA","MAGENTA"},
		{"BLUE","AZUL"},
		{"CYAN","CIANO"},
		{"YELLOW","AMARELO"},
		{"RED","VERMELHO"},
	};

	bool isThatColor (byte sensor, string name) => (bot.ReturnColor(sensor-1) == name || bot.ReturnColor(sensor-1) == colorNames[name]);
//

//Colors identifier
bool isColorized (byte sensor) => (colors.R(sensor) != colors.B(sensor));

bool isRed (byte sensor) => ((colors.R(sensor)/colors.B(sensor) > 3.5 && colors.G(sensor) < 20) && isThatColor(sensor, "RED"));

bool isGreen (byte sensor) => ((colors.G(sensor)/colors.R(sensor) > 4 && colors.B(sensor) < 10) && isThatColor(sensor, "GREEN"));

bool isBlue (byte sensor) => (colors.B(sensor)/colors.R(sensor) > 1.2 && colors.G(sensor) < 75);

bool anySensorColor (string color) {
	for (byte i = 1; i<5; i++) {
		switch (color) {
			case "red":
				if (isRed(i)) return true;
				break;
			case "green":
				if (isGreen(i)) return true;
				break;
			case "blue":
				if (isBlue(i)) return true;
				break;
			default:
				break;
		}
	}
	return false;
}

bool isColorized (byte[] sensors) {
	for (byte i = 0; i < sensors.Length; i++) {
		if (colors.R(sensors[i]) != colors.B(sensors[i])) return false;
	}
	return true;
}

