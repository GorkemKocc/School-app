using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yazlab1
{
	public partial class LoginScreen : UserControl
	{
		public UserControl adminScreen;
		public UserControl studentScreen;
		public LoginScreen()
		{
			InitializeComponent();
		}

		private void LoginScreen_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				adminScreen.Visible = true;
				this.Visible = false;
				checkBox2.Checked = false;
				checkBox3.Checked = false;
			}
			if (checkBox2.Checked)
			{
				//studentScreen.Visible = true;
				//this.Visible = false;
				checkBox1.Checked = false;
				checkBox3.Checked = false;
			}
			if (checkBox3.Checked)
			{
				studentScreen.Visible = true;
				this.Visible = false;
				checkBox1.Checked = false;
				checkBox2.Checked = false;
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				checkBox2.Checked = false;
				checkBox3.Checked = false;
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
			{
				checkBox1.Checked = false;
				checkBox3.Checked = false;
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox3.Checked)
			{
				checkBox2.Checked = false;
				checkBox1.Checked = false;
			}
		}
	}
}
