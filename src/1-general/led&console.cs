//colors
Dictionary <string, string> color = new Dictionary <string,string> () {
	{"white","#FFFFFF"},
	{"black","#000000"},
	{"gray","#696969"},
	{"green","#00CC00"},
	{"pink","#D264D2"},
	{"purple","#915AB4"},
	{"orange","#C88232"},
	{"blue","#465AC8"},
	{"cyan","#00c8c8"},
	{"yellow","#FFFF00"},
	{"green_dark","#009900"},
	{"red","#FF0000"},
	{"comment","#3C455E"}
};

//led
string last_led = "";
void led (string hexcolor) {
	if (last_led != hexcolor) {
		if (hexcolor != "off") bot.TurnLedOn(hexcolor + "DF");		//"DF" is to the translucency
		else bot.TurnLedOff();
		last_led = hexcolor;
	}
}

//console
string last_console = "";
void console (int line, string text, string hexcolor = "") {
	if (console_on && text != last_console) {
		if (hexcolor != "") {
			text = text.Replace("$>", $"<color={hexcolor}>");
			text = text.Replace("<$", "</color>");
		}
		bot.Print(line - 1, "<align=center>" + text + "</align>");
		bot.Print(line, "");
		last_console = text;
	}
}

void clear (int line = 2) {
	bot.ClearConsoleLine(line - 1);
	if (line == 2) led(color["white"]);
}

int start_print = 0;
void printMotors () {
	if (start_print + 63 < time.millis()) {
		string right_data = ((int)bot.GetFrontalRightForce()).ToString();
		string left_data = ((int)bot.GetFrontalLeftForce()).ToString();

		string right_spaces = new string (' ', 5 - right_data.Length);
		string left_spaces = new string (' ', 5 - left_data.Length);

		console(3, $"$>| {right_spaces+right_data}   |   {left_data+left_spaces} |<$", color["comment"]);
		start_print = time.millis();
	}
}

//both
void console_led (int line, string text, string hexcolor, string ledcolor = "") {
	if (ledcolor == "") ledcolor = hexcolor;
	console(line, text, hexcolor);
	led(ledcolor);
}
