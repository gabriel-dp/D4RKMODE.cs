public class Colors {
	public int R (byte sensor) => (int) bot.ReturnRed(sensor-1);
	public int G (byte sensor) => (int) bot.ReturnGreen(sensor-1);
	public int B (byte sensor) => (int) bot.ReturnBlue(sensor-1);
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

