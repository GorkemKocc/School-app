namespace yazlab1
{
	partial class AdminScreen
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			checkBox1 = new CheckBox();
			checkBox2 = new CheckBox();
			textBox1 = new TextBox();
			label1 = new Label();
			label2 = new Label();
			textBox2 = new TextBox();
			label3 = new Label();
			textBox3 = new TextBox();
			label4 = new Label();
			textBox4 = new TextBox();
			label5 = new Label();
			textBox5 = new TextBox();
			button1 = new Button();
			button2 = new Button();
			comboBox1 = new ComboBox();
			button3 = new Button();
			SuspendLayout();
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(702, 31);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(83, 24);
			checkBox1.TabIndex = 0;
			checkBox1.Text = "Öğrenci";
			checkBox1.UseVisualStyleBackColor = true;
			checkBox1.CheckedChanged += checkBox1_CheckedChanged;
			// 
			// checkBox2
			// 
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(702, 78);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(66, 24);
			checkBox2.TabIndex = 1;
			checkBox2.Text = "Hoca";
			checkBox2.UseVisualStyleBackColor = true;
			checkBox2.CheckedChanged += checkBox2_CheckedChanged;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(112, 28);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(125, 27);
			textBox1.TabIndex = 2;
			textBox1.Visible = false;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(21, 31);
			label1.Name = "label1";
			label1.Size = new Size(28, 20);
			label1.TabIndex = 3;
			label1.Text = "Ad";
			label1.Visible = false;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(21, 79);
			label2.Name = "label2";
			label2.Size = new Size(50, 20);
			label2.TabIndex = 5;
			label2.Text = "Soyad";
			label2.Visible = false;
			// 
			// textBox2
			// 
			textBox2.Location = new Point(112, 75);
			textBox2.Name = "textBox2";
			textBox2.Size = new Size(125, 27);
			textBox2.TabIndex = 4;
			textBox2.Visible = false;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(21, 127);
			label3.Name = "label3";
			label3.Size = new Size(76, 20);
			label3.TabIndex = 7;
			label3.Text = "Kontenjan";
			label3.Visible = false;
			// 
			// textBox3
			// 
			textBox3.Location = new Point(112, 124);
			textBox3.Name = "textBox3";
			textBox3.Size = new Size(125, 27);
			textBox3.TabIndex = 6;
			textBox3.Visible = false;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(286, 82);
			label4.Name = "label4";
			label4.Size = new Size(39, 20);
			label4.TabIndex = 9;
			label4.Text = "Şifre";
			label4.Visible = false;
			label4.Click += label4_Click;
			// 
			// textBox4
			// 
			textBox4.Location = new Point(370, 79);
			textBox4.Name = "textBox4";
			textBox4.Size = new Size(125, 27);
			textBox4.TabIndex = 8;
			textBox4.Visible = false;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(256, 32);
			label5.Name = "label5";
			label5.Size = new Size(96, 20);
			label5.TabIndex = 11;
			label5.Text = "Kulllanıcı Adı";
			label5.Visible = false;
			// 
			// textBox5
			// 
			textBox5.Location = new Point(370, 28);
			textBox5.Name = "textBox5";
			textBox5.Size = new Size(125, 27);
			textBox5.TabIndex = 10;
			textBox5.Visible = false;
			// 
			// button1
			// 
			button1.Location = new Point(702, 116);
			button1.Name = "button1";
			button1.Size = new Size(94, 48);
			button1.TabIndex = 12;
			button1.Text = "Kaydet";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new Point(617, 32);
			button2.Name = "button2";
			button2.Size = new Size(64, 29);
			button2.TabIndex = 13;
			button2.Text = "Göster";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// comboBox1
			// 
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new Point(538, 33);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new Size(73, 28);
			comboBox1.TabIndex = 14;
			// 
			// button3
			// 
			button3.Location = new Point(702, 172);
			button3.Name = "button3";
			button3.Size = new Size(94, 45);
			button3.TabIndex = 15;
			button3.Text = "Güncelle";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// AdminScreen
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(button3);
			Controls.Add(comboBox1);
			Controls.Add(button2);
			Controls.Add(button1);
			Controls.Add(label5);
			Controls.Add(textBox5);
			Controls.Add(label4);
			Controls.Add(textBox4);
			Controls.Add(label3);
			Controls.Add(textBox3);
			Controls.Add(label2);
			Controls.Add(textBox2);
			Controls.Add(label1);
			Controls.Add(textBox1);
			Controls.Add(checkBox2);
			Controls.Add(checkBox1);
			Name = "AdminScreen";
			Size = new Size(833, 500);
			Load += AdminScreen_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private CheckBox checkBox1;
		private CheckBox checkBox2;
		private TextBox textBox1;
		private Label label1;
		private Label label2;
		private TextBox textBox2;
		private Label label3;
		private TextBox textBox3;
		private Label label4;
		private TextBox textBox4;
		private Label label5;
		private TextBox textBox5;
		private Button button1;
		private Button button2;
		private ComboBox comboBox1;
		private Button button3;
	}
}
