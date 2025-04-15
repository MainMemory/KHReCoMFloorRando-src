using Newtonsoft.Json;
using System.Diagnostics;

namespace ConfigTool
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		Settings settings = new Settings();
		bool loading;

		private void Form1_Load(object sender, EventArgs e)
		{
			if (File.Exists("settings.json"))
				settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json")) ?? new Settings();
			loading = true;
			seedName.Text = settings.Seed ?? string.Empty;
			minRooms.Value = settings.MinRooms;
			maxRooms.Value = settings.MaxRooms;
			progressiveSize.Checked = settings.ProgressiveSize;
			connectedRooms.Checked = settings.ConnectedRooms;
			extraDoorProb.Value = settings.ExtraDoorProb;
			loading = false;
		}

		private void seedName_TextChanged(object sender, EventArgs e)
		{
			settings.Seed = seedName.Text;
		}

		private void minRooms_ValueChanged(object sender, EventArgs e)
		{
			settings.MinRooms = (int)minRooms.Value;
			if (!loading && maxRooms.Value < minRooms.Value)
				maxRooms.Value = minRooms.Value;
		}

		private void maxRooms_ValueChanged(object sender, EventArgs e)
		{
			settings.MaxRooms = (int)maxRooms.Value;
			if (!loading && minRooms.Value > maxRooms.Value)
				minRooms.Value = maxRooms.Value;
		}

		private void progressiveSize_CheckedChanged(object sender, EventArgs e)
		{
			settings.ProgressiveSize = progressiveSize.Checked;
		}

		private void connectedRooms_CheckedChanged(object sender, EventArgs e)
		{
			settings.ConnectedRooms = connectedRooms.Checked;
		}

		private void extraDoorProb_ValueChanged(object sender, EventArgs e)
		{
			settings.ExtraDoorProb = (int)extraDoorProb.Value;
		}

		private void Save()
		{
			File.WriteAllText("settings.json", JsonConvert.SerializeObject(settings));
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Save();
			Process.Start("KHReCoMFloorRando.exe");
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Save();
		}
	}
}
