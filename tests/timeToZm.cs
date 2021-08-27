void Main () {
	float last_ultra = bc.Distance(0);
	int last_time = bc.Millis();
	while (last_ultra - 10 < bc.Distance(0)) {
		bc.Move(300, 300);
	}
	bc.Move(0,0);
	bc.Print(1, bc.Millis() - last_time);
	bc.Print(2, bc.Distance(0) - last_ultra);
	bc.Wait(10000);
}
