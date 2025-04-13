using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHReCoMFloorRando
{
	internal class FloorLayout
	{
		public short ID { get; set; }
		public short Width { get; set; }
		public short Depth { get; set; }
		public short StartScene { get; set; }
		public short GoalScene { get; set; }
		public List<RoomSrcInfo> Rooms { get; } = [];

		private const int Signature = 0x004C5246; // "FRL\0"
		private const ushort Version = 4;
		public const int Size = 0x10;

		public FloorLayout() { }

		public FloorLayout(byte[] data, int offset)
		{
			ID = BitConverter.ToInt16(data, offset);
			short roomcnt = BitConverter.ToInt16(data, offset + 2);
			Width = BitConverter.ToInt16(data, offset + 4);
			Depth = BitConverter.ToInt16(data, offset + 6);
			StartScene = BitConverter.ToInt16(data, offset + 8);
			GoalScene = BitConverter.ToInt16(data, offset + 10);
			Rooms = new List<RoomSrcInfo>(roomcnt);
			offset += Size;
			for (int i = 0; i < roomcnt; i++)
			{
				Rooms.Add(new RoomSrcInfo(data, offset));
				offset += RoomSrcInfo.Size;
			}
		}

		public byte[] GetBytes()
		{
			byte[] data = new byte[Size + Rooms.Count * RoomSrcInfo.Size];
			BitConverter.GetBytes(ID).CopyTo(data, 0);
			BitConverter.GetBytes((short)Rooms.Count).CopyTo(data, 2);
			BitConverter.GetBytes(Width).CopyTo(data, 4);
			BitConverter.GetBytes(Depth).CopyTo(data, 6);
			BitConverter.GetBytes(StartScene).CopyTo(data, 8);
			BitConverter.GetBytes(GoalScene).CopyTo(data, 10);
			int offset = Size;
			for (int i = 0; i < Rooms.Count; i++)
			{
				Rooms[i].GetBytes().CopyTo(data, offset);
				offset += RoomSrcInfo.Size;
			}
			return data;
		}

		public static List<FloorLayout> ReadFloorData(byte[] data)
		{
			if (data.Length >= 0x10 && BitConverter.ToInt32(data, 0) == Signature && BitConverter.ToUInt16(data, 4) == Version)
			{
				ushort floorcnt = BitConverter.ToUInt16(data, 6);
				List<FloorLayout> floors = new List<FloorLayout>(floorcnt);
				int headoff = BitConverter.ToInt32(data, 8);
				int dataoff = BitConverter.ToInt32(data, 12);
				for (int i = 0; i < floorcnt; i++)
				{
					floors.Add(new FloorLayout(data, BitConverter.ToInt32(data, headoff) + dataoff));
					headoff += 4;
				}
				return floors;
			}
			throw new FormatException("Invalid Rooms file!");
		}

		public static byte[] WriteFloorData(IList<FloorLayout> floors)
		{
			byte[] data = new byte[0x10 + floors.Count * (4 + Size) + floors.Sum(f => f.Rooms.Count) * RoomSrcInfo.Size];
			BitConverter.GetBytes(Signature).CopyTo(data, 0);
			BitConverter.GetBytes(Version).CopyTo(data, 4);
			BitConverter.GetBytes((ushort)floors.Count).CopyTo(data, 6);
			int headoff = 0x10; // offset table always immediately follows header
			int dataoff = 0x10 + floors.Count * 4;
			BitConverter.GetBytes(headoff).CopyTo(data, 8);
			BitConverter.GetBytes(dataoff).CopyTo(data, 12);
			for (int i = 0; i < floors.Count; i++)
			{
				BitConverter.GetBytes(dataoff).CopyTo(data, headoff);
				byte[] tmp = floors[i].GetBytes();
				tmp.CopyTo(data, dataoff);
				headoff += 4;
				dataoff += tmp.Length;
			}
			return data;
		}
	}

	internal class RoomSrcInfo
	{
		public short ID { get; set; }
		public ROOM_TYPE RoomType { get; set; }
		public short FixScene { get; set; }
		public short X { get; set; }
		public short Y { get; set; }
		public DoorSrcInfo[] Doors { get; } = new DoorSrcInfo[4];

		public const int Size = 0x1C;

		public RoomSrcInfo()
		{
			for (int i = 0; i < Doors.Length; i++)
				Doors[i] = new DoorSrcInfo();
		}

		public RoomSrcInfo(byte[] data, int offset)
		{
			ID = BitConverter.ToInt16(data, offset);
			RoomType = (ROOM_TYPE)BitConverter.ToInt16(data, offset + 2);
			FixScene = BitConverter.ToInt16(data, offset + 4);
			X = BitConverter.ToInt16(data, offset + 8);
			Y = BitConverter.ToInt16(data, offset + 10);
			offset += 12;
			for (int i = 0; i < Doors.Length; i++)
			{
				Doors[i] = new DoorSrcInfo(data, offset);
				offset += DoorSrcInfo.Size;
			}
		}

		public byte[] GetBytes()
		{
			byte[] data = new byte[Size];
			BitConverter.GetBytes(ID).CopyTo(data, 0);
			BitConverter.GetBytes((short)RoomType).CopyTo(data, 2);
			BitConverter.GetBytes(FixScene).CopyTo(data, 4);
			BitConverter.GetBytes(X).CopyTo(data, 8);
			BitConverter.GetBytes(Y).CopyTo(data, 10);
			int offset = 12;
			for (int i = 0; i < Doors.Length; i++)
			{
				Doors[i].GetBytes().CopyTo(data, offset);
				offset += DoorSrcInfo.Size;
			}
			return data;
		}
	}

	internal enum ROOM_TYPE
	{
		ROOM_TYPE_CARD = 0x0,
		ROOM_TYPE_START = 0x1,
		ROOM_TYPE_EVENT1 = 0x2,
		ROOM_TYPE_EVENT2 = 0x3,
		ROOM_TYPE_EVENT3 = 0x4,
		ROOM_TYPE_GOAL = 0x5,
		ROOM_TYPE_EXTRA = 0x6,
		ROOM_TYPE_LAST = 0x7,
		ROOM_TYPE_FIX = 0x8,
	}

	internal class DoorSrcInfo
	{
		public short DestRoom { get; set; }
		public DOOR_TYPE DoorType { get; set; }
		public byte CheckEventRoom { get; set; }

		public const int Size = 4;

		public DoorSrcInfo() { }

		public DoorSrcInfo(byte[] data, int offset)
		{
			DestRoom = BitConverter.ToInt16(data, offset);
			DoorType = (DOOR_TYPE)data[offset + 2];
			CheckEventRoom = data[offset + 3];
		}

		public byte[] GetBytes()
		{
			byte[] data = new byte[Size];
			BitConverter.GetBytes(DestRoom).CopyTo(data, 0);
			data[2] = (byte)DoorType;
			data[3] = CheckEventRoom;
			return data;
		}
	}

	enum DOOR_TYPE
	{
		DOOR_TYPE_NORMAL = 0x0,
		DOOR_TYPE_TO_EVENT1 = 0x1,
		DOOR_TYPE_TO_EVENT2 = 0x2,
		DOOR_TYPE_TO_EVENT3 = 0x3,
		DOOR_TYPE_OPEN_AFTER_EVENT = 0x4,
		DOOR_TYPE_TO_GOAL = 0x5,
		DOOR_TYPE_TO_EXTRA = 0x6,
		DOOR_TYPE_ONEWAY = 0x7,
		DOOR_TYPE_WORLD_ENTRANCE = 0x8,
		DOOR_TYPE_SELECT_WORLD = 0x9,
		DOOR_TYPE_FLOOR_CHANGE = 0xA,
		DOOR_TYPE_TO_LAST = 0xB,
		DOOR_TYPE_EVENTROOM = 0xC,
		_DOOR_TYPE_DEBUG = 0xD,
		_DOOR_TYPE_MAX = 0xE,
	}

	enum DoorDirections
	{
		North,
		East,
		South,
		West
	}
}
