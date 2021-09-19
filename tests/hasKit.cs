void Main () {
	while (!bot.HasRescueKit()) {
		bot.Move(200, 200);
	}
	bot.Move(200, 200);
	bot.Wait(1000);
}
