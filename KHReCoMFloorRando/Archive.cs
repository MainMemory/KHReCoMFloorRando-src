using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHReCoMFloorRando
{
	internal class Archive
	{
		private readonly Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

		public Archive() { }

		public Archive(string filename)
		{
			byte[] data = File.ReadAllBytes(filename);
			int addr = 0;
			while (true)
			{
				int sz = BitConverter.ToInt32(data, addr + 0x1C);
				if ((sz & 0x80000000) == 0)
					break;
				byte[] tmp = new byte[sz & 0x7FFFFFFF];
				Array.Copy(data, BitConverter.ToInt32(data, addr), tmp, 0, tmp.Length);
				files.Add(Encoding.ASCII.GetString(data, addr + 4, 0x18).TrimEnd('\0'), tmp);
				addr += 0x20;
			}
		}

		public void Save(string filename)
		{
			byte[] buffer = new byte[files.Count * 0x20 + files.Values.Sum(v => v.Length)];
			int headoff = 0;
			int datoff = files.Count * 0x20;
			foreach (var file in files)
			{
				BitConverter.GetBytes(datoff).CopyTo(buffer, headoff);
				Encoding.ASCII.GetBytes(file.Key).CopyTo(buffer, headoff + 4);
				BitConverter.GetBytes(file.Value.Length | int.MinValue).CopyTo(buffer, headoff + 0x1C);
				headoff += 0x20;
				file.Value.CopyTo(buffer, datoff);
				datoff += file.Value.Length;
			}
			File.WriteAllBytes(filename, buffer);
		}

		private static string CheckNameLength(string name)
		{
			if (name.Length > 0x18)
				name = name.Remove(0x18);
			return name;
		}

		public void AddFile(string name, byte[] buffer)
		{
			files.Add(CheckNameLength(name), buffer);
		}

		public void AddFile(string filename) => AddFile(Path.GetFileName(filename), File.ReadAllBytes(filename));

		public void RemoveFile(string name) => files.Remove(name);

		public void Clear() => files.Clear();

		public byte[] this[string name]
		{

			get => files[name];
			set => files[CheckNameLength(name)] = value;
		}
	}
}
