using Npgsql;

namespace yazlab1
{
	public partial class Form1 : Form
	{
		User user = new User();
		public Form1()
		{
			InitializeComponent();
			studentScreen1.Visible = false;
		}

		private void loginScreen1_Load(object sender, EventArgs e)
		{
			loginScreen1.adminScreen = adminScreen1;
			loginScreen1.studentScreen = studentScreen1;
			adminScreen1.loginScreen = loginScreen1;
			studentScreen1.loginScreen = loginScreen1;

			studentScreen1.user = loginScreen1.user;
		}
	}
}