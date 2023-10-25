namespace yazlab1
{
	partial class Form1
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
			adminScreen1 = new AdminScreen();
			loginScreen1 = new LoginScreen();
			SuspendLayout();
			// 
			// adminScreen1
			// 
			adminScreen1.Location = new Point(0, 0);
			adminScreen1.Name = "adminScreen1";
			adminScreen1.Size = new Size(808, 469);
			adminScreen1.TabIndex = 0;
			// 
			// loginScreen1
			// 
			loginScreen1.Location = new Point(0, 0);
			loginScreen1.Name = "loginScreen1";
			loginScreen1.Size = new Size(808, 469);
			loginScreen1.TabIndex = 1;
			loginScreen1.Load += loginScreen1_Load;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(loginScreen1);
			Controls.Add(adminScreen1);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
		}

		#endregion

		private AdminScreen adminScreen1;
		private LoginScreen loginScreen1;
	}
}