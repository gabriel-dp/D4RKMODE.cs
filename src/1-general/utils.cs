void delay (int ms) => bot.Wait(ms);

public class Time {

	public int millis () => bot.Millis();

	public int timer () => bot.Timer();

	public void reset () => bot.ResetTimer();

}

Time time = new Time();
