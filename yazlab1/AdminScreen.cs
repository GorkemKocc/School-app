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
		public UserControl loginScreen;
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
				button3.Visible = true;
				button4.Visible = true;

				label1.Visible = true;
				label2.Visible = true;
				label3.Visible = true;
				label4.Visible = true;
				label5.Visible = true;
				label6.Visible = false;
				textBox1.Visible = true;
				textBox2.Visible = true;
				textBox3.Visible = true;
				textBox4.Visible = true;
				textBox5.Visible = true;
				checkBox1.Checked = false;
				checkBox3.Checked = false;

				comboBox1.Items.Clear();
				comboBox1.Text = null;
				comboBox1.Items.Add("Kayıt");
				label1.Text = "Ad";
				label5.Text = "Kullanıcı Adı";
				comboBox1.SelectedIndex = 0;

				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT identification_number, name, surname FROM teachers", connection))
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string formattedTeacher = $"{reader["identification_number"]} {reader["name"]} {reader["surname"]}";
						comboBox1.Items.Add(formattedTeacher);
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
				button3.Visible = true;
				button4.Visible = true;

				label1.Visible = true;
				label2.Visible = true;
				label3.Visible = false;
				label4.Visible = true;
				label5.Visible = true;
				label6.Visible = false;
				textBox1.Visible = true;
				textBox2.Visible = true;
				textBox3.Visible = false;
				textBox4.Visible = true;
				textBox5.Visible = true;
				checkBox2.Checked = false;
				checkBox3.Checked = false;

				comboBox1.Items.Clear();
				comboBox1.Text = null;
				comboBox1.Items.Add("Kayıt");
				label1.Text = "Ad";
				label5.Text = "Kullanıcı Adı";
				comboBox1.SelectedIndex = 0;

				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT student_id, name, surname FROM students", connection))
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string formattedStudent = $"{reader["student_id"]} {reader["name"]} {reader["surname"]}";
						comboBox1.Items.Add(formattedStudent);
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
					cmd.Parameters.AddWithValue("@username", textBox5.Text);
					cmd.Parameters.AddWithValue("@password", textBox4.Text);

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
					cmd.Parameters.AddWithValue("@username", textBox5.Text);
					cmd.Parameters.AddWithValue("@password", textBox4.Text);

					cmd.ExecuteNonQuery();
				}
				connection.Close();

				MessageBox.Show("Kayıt Başarılı");
			}
			if (checkBox3.Checked && textBox1.Text.Length > 0 && textBox5.Text.Length > 0) //lectures
			{
				string[] parts = comboBox1.SelectedItem.ToString().Split(' ');

				if (parts.Length >= 1)
				{
					if (int.TryParse(parts[0], out int teacherId))
					{
						connection.Open();

						bool isFirstCourse = true;
						string updateQuery;

						using (NpgsqlCommand command = new NpgsqlCommand("SELECT lectures FROM teachers WHERE identification_number = @teacherid", connection))
						{
							command.Parameters.AddWithValue("teacherId", teacherId);

							using (NpgsqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									if (reader.IsDBNull(0))
									{
										isFirstCourse = true;
									}
									else
										isFirstCourse = false;
								}
								reader.Close();
							}
						}

						string couseCode = textBox1.Text;
						string courseName = textBox5.Text;
						string lectureStatus = "0";


						string jsonLecture = $@"[{{""course_code"": ""{couseCode}"", ""course_name"": ""{courseName}"", ""lecture_status"": ""{lectureStatus}""}}]";

						if (isFirstCourse)
						{
							updateQuery = @"
						UPDATE teachers
						SET lectures = @jsonLecture
						WHERE identification_number = @teacherId";
							isFirstCourse = false;
						}
						else
						{
							updateQuery = @"
						UPDATE teachers
						SET lectures = lectures || @jsonLecture
						WHERE identification_number = @teacherId";
						}
						using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
						{
							command.Parameters.AddWithValue("jsonLecture", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonLecture);
							command.Parameters.AddWithValue("teacherId", teacherId);

							command.ExecuteNonQuery();
						}

						connection.Close();
						MessageBox.Show("Kayıt Başarılı");
						return;
					}
				}
			}
			comboBox1.SelectedIndex = 0;
			checkBox1_CheckedChanged(sender, new EventArgs());
			checkBox2_CheckedChanged(sender, new EventArgs());
		}

		private void button3_Click(object sender, EventArgs e)
		{
			string[] parts;
			int selectedIndex = comboBox1.SelectedIndex;

			if (checkBox1.Checked && comboBox1.SelectedItem != null && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //student
			{
				parts = comboBox1.SelectedItem.ToString().Split(' ');

				if (parts.Length >= 1)
				{
					if (int.TryParse(parts[0], out int studentId))
					{
						connection.Open();
						using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE students SET name = @name, surname = @surname, username = @username, password = @password WHERE student_id = @id", connection))
						{
							cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, studentId);
							cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, textBox1.Text);
							cmd.Parameters.AddWithValue("@surname", NpgsqlDbType.Text, textBox2.Text);
							cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Text, textBox5.Text);
							cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, textBox4.Text);

							cmd.ExecuteNonQuery();

						}
						connection.Close();
					}
				}

				if (checkBox2.Checked && comboBox1.SelectedItem != null && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && int.TryParse(textBox3.Text, out int n) && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //teacher
				{
					parts = comboBox1.SelectedItem.ToString().Split(' ');

					if (parts.Length >= 1)
					{
						if (int.TryParse(parts[0], out int teacherId))
						{
							connection.Open();
							using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE teachers SET name = @name, surname = @surname, quota = @quota, username = @username, password = @password WHERE identification_number = @id", connection))
							{
								cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, teacherId);
								cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, textBox1.Text);
								cmd.Parameters.AddWithValue("@surname", NpgsqlDbType.Text, textBox2.Text);
								cmd.Parameters.AddWithValue("@quota", NpgsqlDbType.Integer, int.Parse(textBox3.Text));
								cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Text, textBox5.Text);
								cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, textBox4.Text);

								cmd.ExecuteNonQuery();

							}
							connection.Close();
						}
					}
				}
			}
			checkBox1_CheckedChanged(sender, new EventArgs());
			checkBox2_CheckedChanged(sender, new EventArgs());
			comboBox1.SelectedIndex = selectedIndex;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			string[] parts;
			if (checkBox1.Checked && comboBox1.SelectedItem != null && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //student
			{
				parts = comboBox1.SelectedItem.ToString().Split(' ');

				if (parts.Length >= 1)
				{
					if (int.TryParse(parts[0], out int studentId))
					{
						connection.Open();
						using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM students WHERE student_id = @id", connection))
						{
							cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, studentId);
							cmd.ExecuteNonQuery();
						}
						connection.Close();
					}
				}
			}

			if (checkBox2.Checked && comboBox1.SelectedItem != null && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && int.TryParse(textBox3.Text, out int n) && textBox4.Text.Length > 0 && textBox5.Text.Length > 0) //teacher
			{
				parts = comboBox1.SelectedItem.ToString().Split(' ');

				if (parts.Length >= 1)
				{
					if (int.TryParse(parts[0], out int teacherId))
					{
						connection.Open();
						using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM teachers WHERE identification_number = @id", connection))
						{
							cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, teacherId);
							cmd.ExecuteNonQuery();
						}
						connection.Close();
					}
				}
			}
			comboBox1.SelectedIndex = 0;
			checkBox1_CheckedChanged(sender, new EventArgs());
			checkBox2_CheckedChanged(sender, new EventArgs());
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			button3.Enabled = true;
			if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Kayıt")
			{
				textBox1.Text = "";
				textBox2.Text = "";
				textBox3.Text = "";
				textBox5.Text = "";
				textBox4.Text = "";
			}
			else if (comboBox1.SelectedItem != null && checkBox1.Checked)
			{
				string[] parts = comboBox1.SelectedItem.ToString().Split(' ');

				if (parts.Length >= 1)
				{
					if (int.TryParse(parts[0], out int studentId))
					{
						connection.Open();
						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT name, surname, username, password FROM students WHERE student_id = @id", connection))
						{
							cmd.Parameters.AddWithValue("@id", studentId);
							using (NpgsqlDataReader reader = cmd.ExecuteReader())
							{
								if (reader.Read())
								{
									textBox1.Text = reader["name"].ToString();
									textBox2.Text = reader["surname"].ToString();
									textBox5.Text = reader["username"].ToString();
									textBox4.Text = reader["password"].ToString();
								}
							}
						}
						connection.Close();
					}
				}


			}
			else if (comboBox1.SelectedItem != null && checkBox2.Checked)
			{
				string[] parts = comboBox1.SelectedItem.ToString().Split(' ');

				if (parts.Length >= 1)
				{
					if (int.TryParse(parts[0], out int teacherId))
					{
						connection.Open();
						using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT name, surname, quota, username, password FROM teachers WHERE identification_number = @id", connection))
						{
							cmd.Parameters.AddWithValue("@id", teacherId);
							using (NpgsqlDataReader reader = cmd.ExecuteReader())
							{
								if (reader.Read())
								{
									textBox1.Text = reader["name"].ToString();
									textBox2.Text = reader["surname"].ToString();
									textBox3.Text = reader["quota"].ToString();
									textBox5.Text = reader["username"].ToString();
									textBox4.Text = reader["password"].ToString();
								}
							}
						}
						connection.Close();
					}
				}
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox3.Checked)
			{
				button3.Visible = false;
				button4.Visible = false;

				label1.Visible = true;
				label2.Visible = false;
				label3.Visible = false;
				label4.Visible = false;
				label5.Visible = true;
				textBox1.Visible = true;
				textBox2.Visible = false;
				textBox3.Visible = false;
				textBox4.Visible = false;
				textBox5.Visible = true;
				checkBox1.Checked = false;
				checkBox2.Checked = false;

				label1.Text = "Ders Kodu";
				label5.Text = "Ders Adı";
				label6.Visible = true;

				comboBox1.Items.Clear();
				comboBox1.Text = null;

				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT identification_number, name, surname FROM teachers", connection))
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string formattedTeacher = $"{reader["identification_number"]} {reader["name"]} {reader["surname"]}";
						comboBox1.Items.Add(formattedTeacher);
					}
				}
				connection.Close();
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			foreach (Control control in this.Controls)
			{
				if (control is Label)
				{
					Label label = (Label)control;
					label.Visible = false;
				}
				else if (control is TextBox)
				{
					TextBox textBox = (TextBox)control;
					textBox.Visible = false;
				}
			}

			checkBox1.Checked = false;
			checkBox2.Checked = false;
			checkBox3.Checked = false;
			button1.Visible = true;
			button2.Visible = true;
			button3.Visible = true;
			button4.Visible = true;
			comboBox1.SelectedItem = null;
			comboBox1.Items.Clear();
			loginScreen.Visible = true;
			this.Visible = false;
		}
	}
}
