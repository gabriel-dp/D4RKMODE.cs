string[] rainbow = {"#FF0000", "#FF8800", "#FFFF00", "#00FF00", "#00DDDD", "#00AAFF", "#0044FF", "#AA00FF", "#FF00FF", "#FF0088"};

string[] fade = {"#43434E", "#52525C", "#676770", "#77777F", "#8C8C93", "#A6A6AB", "#C3C3C6", "#E2E2E4", "#EEEEEE", "#FFFFFF"};

void Celebrate (string word, string[] colors) {

	string word_final = "";
	int colors_index = 0;

	string colorize (char text, string hex) => $"<color={hex}>{text}</color>";

	while (true) {
		colors_index++;

		word_final = "";
		for (byte i = 0;  i < word.Length; i++) {
			word_final += colorize(word[i], colors[colors_index % colors.Length]);
			led(colors[colors_index % colors.Length]);
			colors_index++;
		}

		console(1, $"<b><size=60><align=center>{word_final}</align></size></b>");
		delay(55);
	}

}
