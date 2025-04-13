namespace KHReCoMFloorRando
{
	internal class Settings
	{
		// A string which is hashed and used as the starting point for the randomization.
		public string? Seed { get; set; }
		// The minimum number of rooms on a floor. Will be capped based on number of event rooms for each floor.
		public int MinRooms { get; set; } = 5;
		// The maximum number of rooms on a floor. Capped at 30.
		public int MaxRooms { get; set; } = 20;
		// If true, the number of minimum and maximum rooms will increase by 1 for each floor.
		public bool ProgressiveSize { get; set; } = false;
		// If true, rooms will be connected in physical space, otherwise they will be placed totally randomly.
		public bool ConnectedRooms { get; set; } = true;
		// The probability of extra doors being connected to available rooms, when possible. Range 0-100.
		public int ExtraDoorProb { get; set; } = 50;
	}
}
