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

//both
void console_led (int line, string text, string hexcolor, string ledcolor = "") {
	if (ledcolor == "") ledcolor = hexcolor;
	console(line, text, hexcolor);
	led(ledcolor);
}
