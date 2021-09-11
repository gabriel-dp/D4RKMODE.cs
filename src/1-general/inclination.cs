int inclination () {
	int inclination_data = (int) bot.Inclination();
	if (inclination_data > 300) inclination_data -= 360;
	return inclination_data;
}
