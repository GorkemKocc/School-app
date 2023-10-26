namespace yazlab1
{
	partial class StudentScreen
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
			openFileDialog1 = new OpenFileDialog();
			button1 = new Button();
			richTextBox1 = new RichTextBox();
			button2 = new Button();
			button3 = new Button();
			button4 = new Button();
			comboBox1 = new ComboBox();
			checkedListBox1 = new CheckedListBox();
			button5 = new Button();
			SuspendLayout();
			// 
			// openFileDialog1
			// 
			openFileDialog1.FileName = "openFileDialog1";
			// 
			// button1
			// 
			button1.Location = new Point(692, 142);
			button1.Name = "button1";
			button1.Size = new Size(94, 60);
			button1.TabIndex = 0;
			button1.Text = "Transkript Ekle";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// richTextBox1
			// 
			richTextBox1.Location = new Point(3, 41);
			richTextBox1.Name = "richTextBox1";
			richTextBox1.Size = new Size(676, 443);
			richTextBox1.TabIndex = 1;
			richTextBox1.Text = "";
			// 
			// button2
			// 
			button2.Location = new Point(692, 76);
			button2.Name = "button2";
			button2.Size = new Size(94, 60);
			button2.TabIndex = 2;
			button2.Text = "Transkript Görüntüle";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new Point(692, 10);
			button3.Name = "button3";
			button3.Size = new Size(94, 60);
			button3.TabIndex = 3;
			button3.Text = "Mesaj";
			button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			button4.Location = new Point(692, 208);
			button4.Name = "button4";
			button4.Size = new Size(94, 60);
			button4.TabIndex = 4;
			button4.Text = "Açılan Dersler";
			button4.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new Point(3, 7);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new Size(676, 28);
			comboBox1.TabIndex = 5;
			// 
			// checkedListBox1
			// 
			checkedListBox1.FormattingEnabled = true;
			checkedListBox1.Location = new Point(3, 41);
			checkedListBox1.Name = "checkedListBox1";
			checkedListBox1.Size = new Size(676, 400);
			checkedListBox1.TabIndex = 6;
			// 
			// button5
			// 
			button5.Location = new Point(692, 274);
			button5.Name = "button5";
			button5.Size = new Size(94, 60);
			button5.TabIndex = 7;
			button5.Text = "Ders Ekle";
			button5.UseVisualStyleBackColor = true;
			// 
			// StudentScreen
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(button5);
			Controls.Add(checkedListBox1);
			Controls.Add(comboBox1);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(richTextBox1);
			Controls.Add(button1);
			Name = "StudentScreen";
			Size = new Size(800, 500);
			ResumeLayout(false);
		}

		#endregion

		private OpenFileDialog openFileDialog1;
		private Button button1;
		private RichTextBox richTextBox1;
		private Button button2;
		private Button button3;
		private Button button4;
		private ComboBox comboBox1;
		private CheckedListBox checkedListBox1;
		private Button button5;
	}
}
