using Npgsql;

namespace yazlab1
{
	public partial class Form1 : Form
	{
		NpgsqlConnection Connection = new NpgsqlConnection("server=localHost; port=5432; Database=yazlab1; user Id=Postgers; password=admin");
		public Form1()
		{
			InitializeComponent();
		}

		private void loginScreen1_Load(object sender, EventArgs e)
		{
			loginScreen1.adminScreen = adminScreen1;
		}
	}
}