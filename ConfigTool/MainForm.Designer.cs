namespace ConfigTool
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			Label label1;
			Label label2;
			Label label3;
			Label label4;
			Button button1;
			TableLayoutPanel tableLayoutPanel1;
			ToolTip toolTip1;
			extraDoorProb = new NumericUpDown();
			connectedRooms = new CheckBox();
			maxRooms = new NumericUpDown();
			seedName = new TextBox();
			minRooms = new NumericUpDown();
			progressiveSize = new CheckBox();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			label4 = new Label();
			button1 = new Button();
			tableLayoutPanel1 = new TableLayoutPanel();
			toolTip1 = new ToolTip(components);
			tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)extraDoorProb).BeginInit();
			((System.ComponentModel.ISupportInitialize)maxRooms).BeginInit();
			((System.ComponentModel.ISupportInitialize)minRooms).BeginInit();
			SuspendLayout();
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.Left;
			label1.AutoSize = true;
			label1.Location = new Point(3, 7);
			label1.Name = "label1";
			label1.Size = new Size(35, 15);
			label1.TabIndex = 0;
			label1.Text = "Seed:";
			// 
			// label2
			// 
			label2.Anchor = AnchorStyles.Left;
			label2.AutoSize = true;
			label2.Location = new Point(3, 36);
			label2.Name = "label2";
			label2.Size = new Size(103, 15);
			label2.TabIndex = 2;
			label2.Text = "Minimum Rooms:";
			// 
			// label3
			// 
			label3.Anchor = AnchorStyles.Left;
			label3.AutoSize = true;
			label3.Location = new Point(3, 65);
			label3.Name = "label3";
			label3.Size = new Size(105, 15);
			label3.TabIndex = 4;
			label3.Text = "Maximum Rooms:";
			// 
			// label4
			// 
			label4.Anchor = AnchorStyles.Left;
			label4.AutoSize = true;
			label4.Location = new Point(3, 144);
			label4.Name = "label4";
			label4.Size = new Size(125, 15);
			label4.TabIndex = 8;
			label4.Text = "Extra Door Probability:";
			// 
			// button1
			// 
			tableLayoutPanel1.SetColumnSpan(button1, 2);
			button1.Location = new Point(3, 169);
			button1.Name = "button1";
			button1.Size = new Size(356, 23);
			button1.TabIndex = 10;
			button1.Text = "Generate";
			toolTip1.SetToolTip(button1, "Runs the randomizer and generates a new castle layout.");
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.Controls.Add(extraDoorProb, 1, 5);
			tableLayoutPanel1.Controls.Add(connectedRooms, 0, 4);
			tableLayoutPanel1.Controls.Add(maxRooms, 1, 2);
			tableLayoutPanel1.Controls.Add(label3, 0, 2);
			tableLayoutPanel1.Controls.Add(label1, 0, 0);
			tableLayoutPanel1.Controls.Add(seedName, 1, 0);
			tableLayoutPanel1.Controls.Add(label2, 0, 1);
			tableLayoutPanel1.Controls.Add(minRooms, 1, 1);
			tableLayoutPanel1.Controls.Add(progressiveSize, 0, 3);
			tableLayoutPanel1.Controls.Add(label4, 0, 5);
			tableLayoutPanel1.Controls.Add(button1, 0, 6);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 7;
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.Size = new Size(578, 371);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// extraDoorProb
			// 
			extraDoorProb.Location = new Point(134, 140);
			extraDoorProb.Name = "extraDoorProb";
			extraDoorProb.Size = new Size(69, 23);
			extraDoorProb.TabIndex = 9;
			toolTip1.SetToolTip(extraDoorProb, "The probability of extra doors being connected to available rooms, when possible.");
			extraDoorProb.ValueChanged += extraDoorProb_ValueChanged;
			// 
			// connectedRooms
			// 
			connectedRooms.Anchor = AnchorStyles.Left;
			connectedRooms.AutoSize = true;
			connectedRooms.CheckAlign = ContentAlignment.MiddleRight;
			tableLayoutPanel1.SetColumnSpan(connectedRooms, 2);
			connectedRooms.Location = new Point(3, 115);
			connectedRooms.Name = "connectedRooms";
			connectedRooms.Size = new Size(124, 19);
			connectedRooms.TabIndex = 7;
			connectedRooms.Text = "Connected Rooms";
			toolTip1.SetToolTip(connectedRooms, "If checked, rooms will be connected in physical space; else they will be placed completely randomly.");
			connectedRooms.UseVisualStyleBackColor = true;
			connectedRooms.CheckedChanged += connectedRooms_CheckedChanged;
			// 
			// maxRooms
			// 
			maxRooms.Location = new Point(134, 61);
			maxRooms.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			maxRooms.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			maxRooms.Name = "maxRooms";
			maxRooms.Size = new Size(69, 23);
			maxRooms.TabIndex = 5;
			toolTip1.SetToolTip(maxRooms, "The maximum number of rooms on each floor.");
			maxRooms.Value = new decimal(new int[] { 1, 0, 0, 0 });
			maxRooms.ValueChanged += maxRooms_ValueChanged;
			// 
			// seedName
			// 
			seedName.Location = new Point(134, 3);
			seedName.Name = "seedName";
			seedName.Size = new Size(225, 23);
			seedName.TabIndex = 1;
			toolTip1.SetToolTip(seedName, "The name of the seed used to initialize the randomizer. Leave blank for random.");
			seedName.TextChanged += seedName_TextChanged;
			// 
			// minRooms
			// 
			minRooms.Location = new Point(134, 32);
			minRooms.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			minRooms.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			minRooms.Name = "minRooms";
			minRooms.Size = new Size(69, 23);
			minRooms.TabIndex = 3;
			toolTip1.SetToolTip(minRooms, "The minimum number of rooms on each floor.");
			minRooms.Value = new decimal(new int[] { 1, 0, 0, 0 });
			minRooms.ValueChanged += minRooms_ValueChanged;
			// 
			// progressiveSize
			// 
			progressiveSize.Anchor = AnchorStyles.Left;
			progressiveSize.AutoSize = true;
			progressiveSize.CheckAlign = ContentAlignment.MiddleRight;
			tableLayoutPanel1.SetColumnSpan(progressiveSize, 2);
			progressiveSize.Location = new Point(3, 90);
			progressiveSize.Name = "progressiveSize";
			progressiveSize.Size = new Size(109, 19);
			progressiveSize.TabIndex = 6;
			progressiveSize.Text = "Progressive Size";
			toolTip1.SetToolTip(progressiveSize, "If checked, the minimum and maximum room count will increase by 1 for each floor.");
			progressiveSize.UseVisualStyleBackColor = true;
			progressiveSize.CheckedChanged += progressiveSize_CheckedChanged;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			ClientSize = new Size(578, 371);
			Controls.Add(tableLayoutPanel1);
			MaximizeBox = false;
			Name = "MainForm";
			Text = "KH ReCoM Floor Rando";
			FormClosing += Form1_FormClosing;
			Load += Form1_Load;
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)extraDoorProb).EndInit();
			((System.ComponentModel.ISupportInitialize)maxRooms).EndInit();
			((System.ComponentModel.ISupportInitialize)minRooms).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox seedName;
		private NumericUpDown minRooms;
		private NumericUpDown maxRooms;
		private CheckBox progressiveSize;
		private CheckBox connectedRooms;
		private NumericUpDown extraDoorProb;
	}
}
