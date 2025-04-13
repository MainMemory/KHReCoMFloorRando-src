using KHReCoMFloorRando;
using Point = (int X, int Y);

Point[] dirPt = [(0, 1), (1, 0), (0, -1), (-1, 0)];

Settings settings = new Settings();
if (File.Exists("settings.json"))
	settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json")) ?? new Settings();
int seed;
if (!string.IsNullOrEmpty(settings.Seed))
	seed = settings.Seed.GetHashCode();
else
	seed = (int)DateTime.Now.Ticks;
Random random = new Random(seed);
Archive arc = new Archive("SY0001.BIN");
for (int story = 1; story < 3; story++)
{
	var floors = FloorLayout.ReadFloorData(arc[$"Rooms{story}.BIN"]);
	foreach (var floor in floors)
	{
		if (floor.ID != 100)
		{
			var sprooms = floor.Rooms.Where(r => r.RoomType >= ROOM_TYPE.ROOM_TYPE_EVENT1 && r.RoomType <= ROOM_TYPE.ROOM_TYPE_LAST).OrderBy(r => r.RoomType).ToArray();
			roomstart:
#if DEBUG
			Console.Clear();
			Console.WriteLine("Floor: {0}", floor.ID);
#endif
			floor.Rooms.Clear();
			int numrooms = Math.Max(Math.Min(random.Next(settings.MinRooms, settings.MaxRooms + 1), 30), sprooms.Length + 3);
			int spstart = numrooms - sprooms.Length;
			if (settings.ConnectedRooms)
			{
				RoomSrcInfo[,] tmplayout = new RoomSrcInfo[60, 60];
				var curRoom = new RoomSrcInfo() { RoomType = ROOM_TYPE.ROOM_TYPE_START, ID = 1, X = 30, Y = 30 };
				floor.Rooms.Add(curRoom);
				tmplayout[30, 30] = curRoom;
				curRoom.Doors[(int)DoorDirections.North].DestRoom = 2;
				curRoom = new RoomSrcInfo() { RoomType = ROOM_TYPE.ROOM_TYPE_FIX, FixScene = 1, ID = 2, X = 30, Y = 31 };
				floor.Rooms.Add(curRoom);
				tmplayout[30, 31] = curRoom;
				curRoom.Doors[(int)DoorDirections.South].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
				int minX = 30;
				int maxX = 30;
				int minY = 30;
				int maxY = 31;
				var openDoors = new List<(int X, int Y, int Direction)>()
				{
					(30, 31, (int)DoorDirections.North),
					(30, 31, (int)DoorDirections.East),
					(30, 31, (int)DoorDirections.West)
				};
				for (int i = 2; i < numrooms; i++)
				{
					if (openDoors.Count == 0) // oops no doors left, try again
						goto roomstart;
					curRoom = new RoomSrcInfo() { ID = (short)(i + 1) };
					floor.Rooms.Add(curRoom);
					if (i >= spstart)
					{
						var sp = sprooms[i - spstart];
						curRoom.RoomType = sp.RoomType;
						curRoom.FixScene = sp.FixScene;
					}
					(int X, int Y, int Direction) door;
					do
					{
						door = openDoors[random.Next(openDoors.Count)];
					}
					while (curRoom.RoomType == ROOM_TYPE.ROOM_TYPE_GOAL && door.Direction == (int)DoorDirections.South);
					Point pt = (door.X + dirPt[door.Direction].X, door.Y + dirPt[door.Direction].Y);
					minX = Math.Min(minX, pt.X);
					maxX = Math.Max(maxX, pt.X);
					minY = Math.Min(minY, pt.Y);
					maxY = Math.Max(maxY, pt.Y);
					curRoom.X = (short)pt.X;
					curRoom.Y = (short)pt.Y;
					tmplayout[pt.X, pt.Y] = curRoom;
					switch (curRoom.RoomType)
					{
						case ROOM_TYPE.ROOM_TYPE_CARD:
						case ROOM_TYPE.ROOM_TYPE_LAST:
						case ROOM_TYPE.ROOM_TYPE_FIX:
							curRoom.Doors[(door.Direction + 2) % 4].DestRoom = tmplayout[door.X, door.Y].ID;
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT1:
							curRoom.Doors[(door.Direction + 2) % 4].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
							tmplayout[door.X, door.Y].Doors[door.Direction].DoorType = DOOR_TYPE.DOOR_TYPE_TO_EVENT1;
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT2:
							curRoom.Doors[(door.Direction + 2) % 4].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
							tmplayout[door.X, door.Y].Doors[door.Direction].DoorType = DOOR_TYPE.DOOR_TYPE_TO_EVENT2;
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT3:
							curRoom.Doors[(door.Direction + 2) % 4].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
							tmplayout[door.X, door.Y].Doors[door.Direction].DoorType = DOOR_TYPE.DOOR_TYPE_TO_EVENT3;
							break;
						case ROOM_TYPE.ROOM_TYPE_GOAL:
							curRoom.Doors[(door.Direction + 2) % 4].DestRoom = tmplayout[door.X, door.Y].ID;
							tmplayout[door.X, door.Y].Doors[door.Direction].DoorType = DOOR_TYPE.DOOR_TYPE_TO_GOAL;
							break;
						case ROOM_TYPE.ROOM_TYPE_EXTRA:
							curRoom.Doors[(door.Direction + 2) % 4].DestRoom = tmplayout[door.X, door.Y].ID;
							tmplayout[door.X, door.Y].Doors[door.Direction].DoorType = DOOR_TYPE.DOOR_TYPE_TO_EXTRA;
							break;
					}
					tmplayout[door.X, door.Y].Doors[door.Direction].DestRoom = curRoom.ID;
					openDoors.RemoveAll(d => d.X + dirPt[d.Direction].X == pt.X && d.Y + dirPt[d.Direction].Y == pt.Y);
					switch (curRoom.RoomType)
					{
						case ROOM_TYPE.ROOM_TYPE_CARD:
						case ROOM_TYPE.ROOM_TYPE_FIX:
							for (int j = 0; j < 4; j++)
							{
								int dX = pt.X + dirPt[j].X;
								int dY = pt.Y + dirPt[j].Y;
								if (dX >= 0 && dX < 60 && dY >= 0 && dY < 60)
								{
									if (tmplayout[dX, dY] == null)
										openDoors.Add((pt.X, pt.Y, j));
									else if (curRoom.Doors[j].DestRoom == 0)
										switch (tmplayout[dX,dY].RoomType)
										{
											case ROOM_TYPE.ROOM_TYPE_CARD:
											case ROOM_TYPE.ROOM_TYPE_FIX:
												if (random.Next(100) < settings.ExtraDoorProb)
												{
													tmplayout[dX, dY].Doors[(j + 2) % 4].DestRoom = curRoom.ID;
													curRoom.Doors[j].DestRoom = tmplayout[dX, dY].ID;
												}
												break;
										}
								}
							}
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT1:
						case ROOM_TYPE.ROOM_TYPE_EVENT2:
							{
								int j = random.Next(4);
								int dX = pt.X + dirPt[j].X;
								int dY = pt.Y + dirPt[j].Y;
								if (dX >= 0 && dX < 60 && dY >= 0 && dY < 60)
								{
									if (tmplayout[dX, dY] != null && curRoom.Doors[j].DoorType != DOOR_TYPE.DOOR_TYPE_ONEWAY)
										switch (tmplayout[dX, dY].RoomType)
										{
											case ROOM_TYPE.ROOM_TYPE_CARD:
											case ROOM_TYPE.ROOM_TYPE_FIX:
												tmplayout[dX, dY].Doors[(j + 2) % 4].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
												curRoom.Doors[j].DestRoom = tmplayout[dX, dY].ID;
												break;
										}
								}
							}
							break;
						case ROOM_TYPE.ROOM_TYPE_GOAL:
							for (int j = 1; j < 4; j++)
							{
								int dX = pt.X + dirPt[j].X;
								int dY = pt.Y + dirPt[j].Y;
								if (dX >= 0 && dX < 60 && dY >= 0 && dY < 60)
								{
									if (tmplayout[dX, dY] == null)
										openDoors.Add((pt.X, pt.Y, j));
									else if (curRoom.Doors[j].DestRoom == 0)
										switch (tmplayout[dX, dY].RoomType)
										{
											case ROOM_TYPE.ROOM_TYPE_CARD:
											case ROOM_TYPE.ROOM_TYPE_FIX:
												if (random.Next(100) < settings.ExtraDoorProb)
												{
													tmplayout[dX, dY].Doors[(j + 2) % 4].DestRoom = curRoom.ID;
													tmplayout[dX, dY].Doors[(j + 2) % 4].DoorType = DOOR_TYPE.DOOR_TYPE_TO_GOAL;
													curRoom.Doors[j].DestRoom = tmplayout[dX, dY].ID;
												}
												break;
										}
								}
							}
							break;
					}
#if DEBUG
					for (int y = 0; y < 60; y++)
						for (int x = 0; x < 60; x++)
							if (tmplayout[x, y] != null)
							{
								Console.SetCursorPosition(x * 2, 120 - y * 2);
								Console.ForegroundColor = ConsoleColor.White - (int)tmplayout[x, y].RoomType;
								Console.Write('\u25A0');
								Console.ResetColor();
								for (int j = 0; j < 4; j++)
									if (tmplayout[x, y].Doors[j].DestRoom != 0)
									{
										Console.SetCursorPosition(x * 2 + dirPt[j].X, 120 - y * 2 - dirPt[j].Y);
										if (j % 2 == 0)
											Console.Write('\u2502');
										else
											Console.Write('\u2500');
									}
							}
#endif
				}
				floor.Width = (short)(maxX - minX);
				floor.Depth = (short)(maxY - minY);
				foreach (var rm in floor.Rooms)
				{
					rm.X -= (short)minX;
					rm.Y -= (short)minY;
				}
			}
			else
			{
				RoomSrcInfo[,] tmplayout = new RoomSrcInfo[30, 30];
				var curRoom = new RoomSrcInfo() { RoomType = ROOM_TYPE.ROOM_TYPE_START, ID = 1 };
				floor.Rooms.Add(curRoom);
				Point pt = (random.Next(30), random.Next(30));
				int minX = pt.X;
				int maxX = pt.X;
				int minY = pt.Y;
				int maxY = pt.Y;
				curRoom.X = (short)pt.X;
				curRoom.Y = (short)pt.Y;
				tmplayout[pt.X, pt.Y] = curRoom;
				curRoom.Doors[(int)DoorDirections.North].DestRoom = 2;
				curRoom = new RoomSrcInfo() { RoomType = ROOM_TYPE.ROOM_TYPE_FIX, FixScene = 1, ID = 2 };
				floor.Rooms.Add(curRoom);
				do
				{
					pt = (random.Next(30), random.Next(30));
				}
				while (tmplayout[pt.X, pt.Y] != null);
				minX = Math.Min(minX, pt.X);
				maxX = Math.Max(maxX, pt.X);
				minY = Math.Min(minY, pt.Y);
				maxY = Math.Max(maxY, pt.Y);
				curRoom.X = (short)pt.X;
				curRoom.Y = (short)pt.Y;
				tmplayout[pt.X, pt.Y] = curRoom;
				curRoom.Doors[(int)DoorDirections.South].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
				var openDoors = new List<(RoomSrcInfo Room, DoorSrcInfo Door)>()
				{
					(curRoom, curRoom.Doors[(int)DoorDirections.North]),
					(curRoom, curRoom.Doors[(int)DoorDirections.East]),
					(curRoom, curRoom.Doors[(int)DoorDirections.West])
				};
				for (int i = 2; i < numrooms; i++)
				{
					if (openDoors.Count == 0) // oops no doors left, try again
						goto roomstart;
					curRoom = new RoomSrcInfo() { ID = (short)(i + 1) };
					floor.Rooms.Add(curRoom);
					if (i >= spstart)
					{
						var sp = sprooms[i - spstart];
						curRoom.RoomType = sp.RoomType;
						curRoom.FixScene = sp.FixScene;
					}
					do
					{
						pt = (random.Next(30), random.Next(30));
					}
					while (tmplayout[pt.X, pt.Y] != null);
					minX = Math.Min(minX, pt.X);
					maxX = Math.Max(maxX, pt.X);
					minY = Math.Min(minY, pt.Y);
					maxY = Math.Max(maxY, pt.Y);
					curRoom.X = (short)pt.X;
					curRoom.Y = (short)pt.Y;
					var door = openDoors[random.Next(openDoors.Count)];
					openDoors.Remove(door);
					switch (curRoom.RoomType)
					{
						case ROOM_TYPE.ROOM_TYPE_CARD:
						case ROOM_TYPE.ROOM_TYPE_LAST:
						case ROOM_TYPE.ROOM_TYPE_FIX:
							curRoom.Doors[random.Next(4)].DestRoom = door.Room.ID;
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT1:
							curRoom.Doors[random.Next(4)].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
							door.Door.DoorType = DOOR_TYPE.DOOR_TYPE_TO_EVENT1;
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT2:
							curRoom.Doors[random.Next(4)].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
							door.Door.DoorType = DOOR_TYPE.DOOR_TYPE_TO_EVENT2;
							break;
						case ROOM_TYPE.ROOM_TYPE_EVENT3:
							curRoom.Doors[random.Next(4)].DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
							door.Door.DoorType = DOOR_TYPE.DOOR_TYPE_TO_EVENT3;
							break;
						case ROOM_TYPE.ROOM_TYPE_GOAL:
							curRoom.Doors[random.Next(1, 4)].DestRoom = door.Room.ID;
							door.Door.DoorType = DOOR_TYPE.DOOR_TYPE_TO_GOAL;
							break;
						case ROOM_TYPE.ROOM_TYPE_EXTRA:
							curRoom.Doors[random.Next(1, 4)].DestRoom = door.Room.ID;
							door.Door.DoorType = DOOR_TYPE.DOOR_TYPE_TO_EXTRA;
							break;
					}
					door.Door.DestRoom = curRoom.ID;
					switch (curRoom.RoomType)
					{
						case ROOM_TYPE.ROOM_TYPE_CARD:
						case ROOM_TYPE.ROOM_TYPE_FIX:
							for (int j = 0; j < 4; j++)
								if (curRoom.Doors[j].DestRoom == 0 && curRoom.Doors[j].DoorType != DOOR_TYPE.DOOR_TYPE_ONEWAY)
									openDoors.Add((curRoom, curRoom.Doors[j]));
							break;
						case ROOM_TYPE.ROOM_TYPE_GOAL:
							for (int j = 1; j < 4; j++)
								if (curRoom.Doors[j].DestRoom == 0)
									openDoors.Add((curRoom, curRoom.Doors[j]));
							break;
					}
#if DEBUG
					for (int y = 0; y < 30; y++)
						for (int x = 0; x < 30; x++)
							if (tmplayout[x, y] != null)
							{
								Console.SetCursorPosition(x * 2, 60 - y * 2);
								Console.ForegroundColor = ConsoleColor.White - (int)tmplayout[x, y].RoomType;
								Console.Write('\u25A0');
								Console.ResetColor();
								for (int j = 0; j < 4; j++)
									if (tmplayout[x, y].Doors[j].DestRoom != 0)
									{
										Console.SetCursorPosition(x * 2 + dirPt[j].X, 60 - y * 2 - dirPt[j].Y);
										if (j % 2 == 0)
											Console.Write('\u2502');
										else
											Console.Write('\u2500');
									}
							}
#endif
				}
				if (openDoors.Count > 0)
					foreach (var rm in floor.Rooms)
					{
						switch (rm.RoomType)
						{
							case ROOM_TYPE.ROOM_TYPE_CARD:
							case ROOM_TYPE.ROOM_TYPE_FIX:
								for (int j = 0; j < 4; j++)
									if (rm.Doors[j].DestRoom == 0 && rm.Doors[j].DoorType != DOOR_TYPE.DOOR_TYPE_ONEWAY && random.Next(100) < settings.ExtraDoorProb)
									{
										var door = openDoors[random.Next(openDoors.Count)];
										openDoors.Remove((rm, rm.Doors[j]));
										openDoors.Remove(door);
										rm.Doors[j].DestRoom = door.Room.ID;
										door.Door.DestRoom = rm.ID;
										if (openDoors.Count == 0)
											break;
									}
								break;
							case ROOM_TYPE.ROOM_TYPE_EVENT1:
							case ROOM_TYPE.ROOM_TYPE_EVENT2:
								{
									int dir = random.Next(4);
									if (rm.Doors[dir].DoorType != DOOR_TYPE.DOOR_TYPE_ONEWAY)
									{
										var door = openDoors[random.Next(openDoors.Count)];
										openDoors.Remove(door);
										rm.Doors[dir].DestRoom = door.Room.ID;
										door.Door.DoorType = DOOR_TYPE.DOOR_TYPE_ONEWAY;
									}
								}
								break;
							case ROOM_TYPE.ROOM_TYPE_GOAL:
								for (int j = 1; j < 4; j++)
									if (rm.Doors[j].DestRoom == 0 && rm.Doors[j].DoorType != DOOR_TYPE.DOOR_TYPE_ONEWAY && random.Next(100) < settings.ExtraDoorProb)
									{
										openDoors.Remove((rm, rm.Doors[j]));
										var door = openDoors[random.Next(openDoors.Count)];
										openDoors.Remove(door);
										rm.Doors[j].DestRoom = door.Room.ID;
										door.Door.DestRoom = rm.ID;
										if (openDoors.Count == 0)
											break;
									}
								break;
						}
						if (openDoors.Count == 0)
							break;
					}
				floor.Width = (short)(maxX - minX);
				floor.Depth = (short)(maxY - minY);
				foreach (var rm in floor.Rooms)
				{
					rm.X -= (short)minX;
					rm.Y -= (short)minY;
				}
			}
		}
		if (settings.ProgressiveSize)
		{
			settings.MinRooms = Math.Min(settings.MinRooms + 1, 30);
			settings.MaxRooms = Math.Min(settings.MaxRooms + 1, 30);
		}
	}
	arc[$"Rooms{story}.BIN"] = FloorLayout.WriteFloorData(floors);
}
arc.Save("SY0001.BIN");