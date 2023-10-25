using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace yazlab1
{
	public partial class AdminScreen : UserControl
	{
		NpgsqlConnection connection = new NpgsqlConnection("server=localHost; port=5432; Database=yazlab1; user ID=postgres; password=admin");
		public AdminScreen()
		{
			InitializeComponent();
		}

		private void AdminScreen_Load(object sender, EventArgs e)
		{
			textBox1.Text = null;
			textBox2.Text = null;
			textBox3.Text = null;
			textBox4.Text = null;
			textBox5.Text = null;
			
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
			{
				button3.Enabled = false;

				label1.Visible = true;
				label2.Visible = true;
				label3.Visible = true;
				label4.Visible = true;
				label5.Visible = true;
				textBox1.Visible = true;
				textBox2.Visible = true;
				textBox3.Visible = true;
				textBox4.Visible = true;
				textBox5.Visible = true;
				checkBox1.Checked = false;

				comboBox1.Items.Clear();
				comboBox1.Text = null;

				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT identification_number FROM teachers", connection))
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						comboBox1.Items.Add(reader["identification_number"]);
					}
				}
				connection.Close();
			}


		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) //student
		{
			if (checkBox1.Checked)
			{
				button3.Enabled = false;

				label1.Visible = true;
				label2.Visible = true;
				label3.Visible = false;
				label4.Visible = true;
				label5.Visible = true;
				textBox1.Visible = true;
				textBox2.Visible = true;
				textBox3.Visible = false;
				textBox4.Visible = true;
				textBox5.Visible = true;
				checkBox2.Checked = false;


				comboBox1.Items.Clear();
				comboBox1.Text = null;

				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT student_id FROM students", connection))
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						comboBox1.Items.Add(reader["student_id"]);
					}
				}
				connection.Close();

			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (checkBox1.Checked && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //student
			{

				connection.Open();

				using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO students (name, surname, username, password) VALUES (@name, @surname, @username, @password)"))
				{
					cmd.Connection = connection;

					cmd.Parameters.AddWithValue("@name", textBox1.Text);
					cmd.Parameters.AddWithValue("@surname", textBox2.Text);
					cmd.Parameters.AddWithValue("@username", textBox4.Text);
					cmd.Parameters.AddWithValue("@password", textBox5.Text);

					cmd.ExecuteNonQuery();
				}
				connection.Close();

				MessageBox.Show("Kayıt Başarılı");
			}
			if (checkBox2.Checked && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && int.TryParse(textBox3.Text, out int n) && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //teacher
			{
				connection.Open();

				using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO teachers (name, surname, quota, username, password) VALUES (@name, @surname, @quota, @username, @password)"))
				{
					cmd.Connection = connection;

					cmd.Parameters.AddWithValue("@name", textBox1.Text);
					cmd.Parameters.AddWithValue("@surname", textBox2.Text);
					cmd.Parameters.AddWithValue("@quota", int.Parse(textBox3.Text));
					cmd.Parameters.AddWithValue("@username", textBox4.Text);
					cmd.Parameters.AddWithValue("@password", textBox5.Text);

					cmd.ExecuteNonQuery();
				}
				connection.Close();

				MessageBox.Show("Kayıt Başarılı");
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			button3.Enabled = true;
			if (checkBox1.Checked && comboBox1.SelectedItem != null)
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT name, surname, username, password FROM students WHERE student_id = @id", connection))
				{
					cmd.Parameters.AddWithValue("@id", comboBox1.SelectedItem);
					using (NpgsqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							textBox1.Text = reader["name"].ToString();
							textBox2.Text = reader["surname"].ToString();
							textBox4.Text = reader["username"].ToString();
							textBox5.Text = reader["password"].ToString();
						}
					}
				}
				connection.Close();
			}
			if (checkBox2.Checked && comboBox1.SelectedItem != null)
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT name, surname, quota, username, password FROM teachers WHERE identification_number = @id", connection))
				{
					cmd.Parameters.AddWithValue("@id", comboBox1.SelectedItem);
					using (NpgsqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							textBox1.Text = reader["name"].ToString();
							textBox2.Text = reader["surname"].ToString();
							textBox3.Text = reader["quota"].ToString();
							textBox4.Text = reader["username"].ToString();
							textBox5.Text = reader["password"].ToString();
						}
					}
				}
				connection.Close();
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (checkBox1.Checked && comboBox1.SelectedItem != null && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //student
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE students SET name = @name, surname = @surname, username = @username, password = @password WHERE student_id = @id", connection))
				{
					cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, comboBox1.SelectedItem);
					cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, textBox1.Text);
					cmd.Parameters.AddWithValue("@surname", NpgsqlDbType.Text, textBox2.Text);
					cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Text, textBox4.Text);
					cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, textBox5.Text);

					cmd.ExecuteNonQuery();

				}
				connection.Close();
			}

			if (checkBox2.Checked && comboBox1.SelectedItem != null && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && int.TryParse(textBox3.Text, out int n) && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //teacher
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE teachers SET name = @name, surname = @surname, quota = @quota, username = @username, password = @password WHERE identification_number = @id", connection))
				{
					cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, comboBox1.SelectedItem);
					cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, textBox1.Text);
					cmd.Parameters.AddWithValue("@surname", NpgsqlDbType.Text, textBox2.Text);
					cmd.Parameters.AddWithValue("@quota", NpgsqlDbType.Integer, int.Parse(textBox3.Text));
					cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Text, textBox4.Text);
					cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, textBox5.Text);

					cmd.ExecuteNonQuery();

				}
				connection.Close();
			}
		}
	}
}
