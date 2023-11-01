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
using Spire.Pdf;
using Newtonsoft.Json;
using System.Data.Common;
using System.Text.Json.Nodes;
using System.Globalization;

namespace yazlab1
{
	public partial class StudentScreen : UserControl
	{

		NpgsqlConnection connection = new NpgsqlConnection("server=localHost; port=5432; Database=yazlab1; user ID=postgres; password=admin");
		public UserControl loginScreen;
		public StudentScreen()
		{
			InitializeComponent();
		}

		static List<string[]> course_data = new List<string[]>();
		void isTranscriptUploaded()
		{
			connection.Open();

			string selectQuery = "SELECT transcript FROM students WHERE student_id = @studentId";

			using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
			{
				command.Parameters.AddWithValue("studentId", 1);/////////////// DÜZELT************************************************

				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
						if (reader.IsDBNull(0))
						{
							foreach (Control control in this.Controls)
							{
								if (control is Button)
								{
									Button button = (Button)control;
									button.Enabled = false;
								}
							}
							button1.Enabled = true;
							reader.Close();
						}
				}
			}
			connection.Close();
		}

		void GetInterestAreas()
		{
			List<string[]> area_data = new List<string[]>();

			comboBox1.Items.Clear();
			comboBox1.Items.Add("Tüm Dersler");
			comboBox1.SelectedIndex = 0;

			string selectQuery = "SELECT interest_areas FROM teachers";

			connection.Open();

			using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
			{
				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						string interestAreas = reader["interest_areas"].ToString();

						var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(interestAreas) as Newtonsoft.Json.Linq.JArray;

						string formattedInterestAreas = "";

						if (jsonArray != null)
						{
							foreach (var item in jsonArray)
							{
								string interestArea = item["interest_area"].ToString();

								if (!area_data.Any(x => x[0] == interestArea))
								{
									area_data.Add(new string[] { interestArea });
									comboBox1.Items.Add(interestArea);
								}
							}
						}
					}
				}
			}

			connection.Close();

		}

		void GetTranscript(string path)
		{


			PdfDocument pdfDocument = new PdfDocument();
			pdfDocument.LoadFromFile(path);

			string textData = "";

			foreach (PdfPageBase page in pdfDocument.Pages)
			{
				string text = page.ExtractText();
				textData += text;
			}

			pdfDocument.Close();

			TranscriptSplit(textData);
		}
		void TranscriptSplit(string text)
		{
			string[] splittedLines = text.Split('\n');
			decimal gpa = 0;
			for (int i = 0; i < splittedLines.Length; i++)
			{
				if (splittedLines[i].EndsWith("(Comment)\r"))
				{
					int x = i + 1;
					while (!splittedLines[x].TrimStart().StartsWith("DNO"))
					{
						List<string> course = new List<string>();
						string[] a = splittedLines[x].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						course.Add(a[0]);
						List<string> courseNameParts = new List<string>();
						for (int j = 1; j < a.Length; j++)
						{
							if (a[j] != "Z" && a[j] != "S")
							{
								courseNameParts.Add(a[j]);
							}
							else
							{
								break;
							}
						}
						string courseName = string.Join(" ", courseNameParts);
						course.Add(courseName);
						course.Add(a[a.Length - 3]);
						course_data.Add(course.ToArray());
						x += 2;
					}

					int startIndex = splittedLines[x].IndexOf("GNO:");
					startIndex += 4;
					int endIndex = splittedLines[x].IndexOf(' ', startIndex);
					gpa = decimal.Parse(splittedLines[x].Substring(startIndex, endIndex - startIndex), new CultureInfo("en-US"));
				}
			}

			if (gpa != 0)
			{
				connection.Open();

				string updateQuery = "UPDATE students SET gpa = @gpa WHERE student_id = @studentId";

				using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
				{
					command.Parameters.Add(new NpgsqlParameter("@gpa", gpa));
					command.Parameters.Add(new NpgsqlParameter("@studentId", 1));
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "PDF Dosyaları|*.pdf|Tüm Dosyalar|*.*";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				GetTranscript(openFileDialog1.FileName);
				string secilenDosyaYolu = openFileDialog1.FileName;
				MessageBox.Show("Seçilen Dosya: " + secilenDosyaYolu);

				connection.Open();

				bool isFirstCourse = true;
				foreach (string[] course in course_data)
				{
					string updateQuery;
					string courseCode = course[0];
					string courseName = course[1];
					string courseGrade = course[2];

					string jsonTranscript = $@"[{{""course_code"": ""{courseCode}"", ""course_name"": ""{courseName}"", ""course_grade"": ""{courseGrade}""}}]";

					if (isFirstCourse)
					{
						updateQuery = @"
						UPDATE students
						SET transcript = @jsonTranscript
						WHERE student_id = @studentId";
						isFirstCourse = false;
					}
					else
					{
						updateQuery = @"
						UPDATE students
						SET transcript = transcript || @jsonTranscript
						WHERE student_id = @studentId";
					}
					using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
					{
						command.Parameters.AddWithValue("jsonTranscript", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonTranscript);
						command.Parameters.AddWithValue("studentId", 1);///////******* girilen id yap DÜZELT*************************************

						command.ExecuteNonQuery();
					}
				}
				connection.Close();

				foreach (Control control in this.Controls)
				{
					if (control is Button)
					{
						Button button = (Button)control;
						button.Enabled = true;
					}
				}
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			GetInterestAreas();
			richTextBox1.Visible = true;
			checkedListBox1.Visible = false;
			connection.Open();

			string selectQuery = "SELECT transcript FROM students WHERE student_id = @studentId";

			using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
			{
				command.Parameters.AddWithValue("studentId", 1);/////////////// DÜZELT************************************************

				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						string transcript = reader["transcript"].ToString();

						var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(transcript) as Newtonsoft.Json.Linq.JArray;

						string formattedTranscript = "";

						if (jsonArray != null)
						{
							foreach (var item in jsonArray)
							{
								string courseCode = item["course_code"].ToString();
								string courseName = item["course_name"].ToString();
								string courseGrade = item["course_grade"].ToString();
								formattedTranscript += $"{courseCode}, {courseName}, {courseGrade}\r\n";
							}
						}
						richTextBox1.Text = formattedTranscript;
					}
				}
			}

			connection.Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			checkedListBox1.Items.Clear();
			checkedListBox1.Visible = true;
			richTextBox1.Visible = false;
			button7.Enabled = false;
			button5.Enabled = true;
			button5.Text = "Ders Ekle";

			if (comboBox1.SelectedItem == "Tüm Dersler")
			{
				connection.Open();

				string selectQuery = "SELECT identification_number, name, surname, lectures FROM teachers ";

				using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
				{
					using (NpgsqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							string identificationNumber = reader["identification_number"].ToString();
							string name = reader["name"].ToString();
							string surname = reader["surname"].ToString();
							string lectures = reader["lectures"].ToString();

							var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(lectures) as Newtonsoft.Json.Linq.JArray;

							string formattedLectures = "";

							if (jsonArray != null)
							{
								foreach (var item in jsonArray)
								{
									string courseCode = item["course_code"].ToString();
									string courseName = item["course_name"].ToString();
									int lectureStatus = int.Parse(item["lecture_status"].ToString());
									if (lectureStatus == 1)
									{
										formattedLectures = $"{identificationNumber} - {name} {surname} - {courseCode} {courseName}";
										checkedListBox1.Items.Add(formattedLectures);
									}
								}
							}
						}
					}
				}
				connection.Close();
			}
			else
			{
				connection.Open();
				string selectedValue = comboBox1.SelectedItem.ToString();

				string selectQuery = "SELECT identification_number, name, surname, lectures FROM teachers WHERE EXISTS (SELECT 1 FROM jsonb_array_elements(interest_areas) AS item WHERE item->>'interest_area' = @selectedValue);";

				using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
				{
					command.Parameters.AddWithValue("selectedValue", selectedValue);

					using (NpgsqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							string identificationNumber = reader["identification_number"].ToString();
							string name = reader["name"].ToString();
							string surname = reader["surname"].ToString();
							string lectures = reader["lectures"].ToString();

							var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(lectures) as Newtonsoft.Json.Linq.JArray;

							string formattedLectures = "";

							if (jsonArray != null)
							{
								foreach (var item in jsonArray)
								{
									string courseCode = item["course_code"].ToString();
									string courseName = item["course_name"].ToString();
									formattedLectures = $"{identificationNumber} - {name} {surname} - {courseCode} {courseName}";
									checkedListBox1.Items.Add(formattedLectures);
								}
							}
						}
					}
				}

				connection.Close();
			}
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (button5.Text == "Ders Ekle")
			{
				connection.Open();

				bool isFirstCourse = true;
				string updateQuery;

				using (NpgsqlCommand command = new NpgsqlCommand("SELECT demanded_lectures FROM students WHERE student_id = @studentId", connection))
				{
					command.Parameters.AddWithValue("studentId", 1); // Student ID'yi doğru bir değerle değiştirin*****************************

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

				foreach (object selectedItem in checkedListBox1.CheckedItems)
				{
					string selectedText = selectedItem.ToString();

					string[] parts = selectedText.Split('-');

					string identificationNumber = parts[0].Trim();
					string teacherName = parts[1].Trim();
					string courseInfo = parts[2].Trim();
					string agreementStatus = "Talep Edildi";
					string demander = "Öğrenci";

					string jsonLecture = $@"[{{""teachers_id"": ""{identificationNumber}"", ""teachers_name"": ""{teacherName}"", ""course_info"": ""{courseInfo}"", ""agreement_status"": ""{agreementStatus}"", ""demander"": ""{demander}""}}]";

					if (isFirstCourse)
					{
						updateQuery = @"
						UPDATE students
						SET demanded_lectures = @jsonLecture
						WHERE student_id = @studentId";
						isFirstCourse = false;
					}
					else
					{
						updateQuery = @"
						UPDATE students
						SET demanded_lectures = demanded_lectures || @jsonLecture
						WHERE student_id = @studentId";
					}
					using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
					{
						command.Parameters.AddWithValue("jsonLecture", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonLecture);
						command.Parameters.AddWithValue("studentId", 1);///////******* girilen id yap DÜZELT*************************************

						command.ExecuteNonQuery();
					}
					MessageBox.Show("Talep Edildi");
				}
				connection.Close();
			}
			else if (button5.Text == "Hoca Talep Onayı")
			{
				/*connection.Open();

				string updateQuery = "UPDATE students SET demanded_lectures = jsonb_set(demanded_lectures, '{agreement_status}', '\"Kabul Edildi\"', false) WHERE demanded_lectures @> '{\"teachers_id\": \"@identificationNumber\", \"teachers_name\": \"@teacherName\", \"course_info\": \"@courseInfo\"}' AND student_id = @studentId;";

				foreach (object selectedItem in checkedListBox1.CheckedItems)
				{
					string selectedText = selectedItem.ToString();

					string[] parts = selectedText.Split('-');

					string identificationNumber = parts[0].Trim();
					string teacherName = parts[1].Trim();
					string courseInfo = parts[2].Trim();
					string demand = parts[3].Trim();

					if (demand != "Hoca Talebi")
						continue;

					using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
					{
						command.Parameters.AddWithValue("studentId", 1);///////******* girilen id yap DÜZELT*************************************
						command.Parameters.AddWithValue("identificationNumber", identificationNumber);
						command.Parameters.AddWithValue("teacherName", teacherName);
						command.Parameters.AddWithValue("courseInfo", courseInfo);
						command.ExecuteNonQuery();
					}

				}

				connection.Close();*/




			}

		}

		private void button6_Click(object sender, EventArgs e)
		{
			checkedListBox1.Items.Clear();
			checkedListBox1.Visible = true;
			richTextBox1.Visible = false;
			button5.Enabled = true;
			button7.Enabled = true;
			button5.Text = "Hoca Talep Onayı";
			connection.Open();

			string selectQuery = "SELECT demanded_lectures FROM students WHERE student_id = @studentId";

			using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
			{
				command.Parameters.AddWithValue("studentId", 1);/////////////// DÜZELT************************************************

				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						string demandedLectures = reader["demanded_lectures"].ToString();

						var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(demandedLectures) as Newtonsoft.Json.Linq.JArray;

						string formattedDemandedLectures = "";

						if (jsonArray != null)
						{
							foreach (var item in jsonArray)
							{
								string courseInfo = item["course_info"].ToString();
								string teachersId = item["teachers_id"].ToString();
								string teachersName = item["teachers_name"].ToString();
								string agreementStatus = item["agreement_status"].ToString();
								string demander = item["demander"].ToString();

								if (agreementStatus == "Talep Edildi" && demander == "Öğrenci")
								{
									formattedDemandedLectures = $"{teachersId} - {teachersName} - {courseInfo} - {agreementStatus}";
									checkedListBox1.Items.Add(formattedDemandedLectures);
								}
								else if (agreementStatus == "Talep Edildi" && demander == "Hoca")
								{
									formattedDemandedLectures = $"{teachersId} - {teachersName} - {courseInfo} - Hoca Talebi";
									checkedListBox1.Items.Add(formattedDemandedLectures);
								}
							}
						}
					}
				}
			}

			connection.Close();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			checkedListBox1.Visible = true;
			richTextBox1.Visible = false;


			string updateQuery;

			foreach (object selectedItem in checkedListBox1.CheckedItems)
			{
				string selectedText = selectedItem.ToString();

				string[] parts = selectedText.Split('-');

				string identificationNumber = parts[0].Trim();
				string teacherName = parts[1].Trim();
				string courseInfo = parts[2].Trim();
				string agreementStatus = parts[3].Trim();

				if (agreementStatus == "Talep Edildi")
				{
					connection.Open();

					string jsonDemandedLectures = $@"[{{""course_info"": ""{courseInfo}"",""teachers_id"": ""{identificationNumber}"", ""teachers_name"": ""{teacherName}"", ""agreement_status"": ""{agreementStatus}""}}]";

					updateQuery = @"UPDATE students
							SET demanded_lectures = (
							SELECT jsonb_agg(item)
							FROM jsonb_array_elements(demanded_lectures) AS item
							WHERE NOT (
							(item->>'course_info' = @courseInfo)
							AND
							(item->>'teachers_id' = @teacherId)))
							WHERE student_id =  @studentId;";////////////////////////ANALİZ ET*********************

					using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
					{
						command.Parameters.AddWithValue("jsonDemandedLectures", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonDemandedLectures);
						command.Parameters.AddWithValue("courseInfo", courseInfo);
						command.Parameters.AddWithValue("teacherId", identificationNumber);
						command.Parameters.AddWithValue("studentId", 1);///////******* girilen id yap DÜZELT*************************************

						command.ExecuteNonQuery();
					}

					connection.Close();
					MessageBox.Show(courseInfo + " İptal Edildi");
				}
				else if (agreementStatus == "Hoca Talebi")
				{
					MessageBox.Show("Hoca Talebi İptal Edilemez");
				}
				else
				{
					return;
				}

			}

			if (checkedListBox1.CheckedItems.Count == 0)
				return;

			button6_Click(sender, e);
		}

		private void button8_Click(object sender, EventArgs e)
		{
			checkedListBox1.Items.Clear();
			checkedListBox1.Visible = true;
			richTextBox1.Visible = false;
			button5.Enabled = false;
			button7.Enabled = false;
			connection.Open();

			string selectQuery = "SELECT demanded_lectures FROM students WHERE student_id = @studentId";


			using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
			{
				command.Parameters.AddWithValue("studentId", 1);/////////////// DÜZELT************************************************

				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						string demandedLectures = reader["demanded_lectures"].ToString();

						var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(demandedLectures) as Newtonsoft.Json.Linq.JArray;

						string formattedDemandedLectures = "";

						if (jsonArray != null)
						{
							foreach (var item in jsonArray)
							{

								string courseInfo = item["course_info"].ToString();
								string teachersId = item["teachers_id"].ToString();
								string teachersName = item["teachers_name"].ToString();
								string agreementStatus = item["agreement_status"].ToString();

								if (agreementStatus == "Kabul Edildi")
								{
									formattedDemandedLectures = $"{teachersId} - {teachersName} - {courseInfo} - {agreementStatus}";
									checkedListBox1.Items.Add(formattedDemandedLectures);
								}
							}
						}
					}
				}
			}

			connection.Close();
		}

		private void button9_Click(object sender, EventArgs e)
		{
			richTextBox1.Clear();
			checkedListBox1.Items.Clear();
			checkedListBox1.Visible = false;
			comboBox1.SelectedItem = null;
			comboBox1.Items.Clear();
			this.Visible = false;
			loginScreen.Visible = true;
		}

		private void StudentScreen_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				GetInterestAreas();
				isTranscriptUploaded();
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{/*
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
		*/
		}
	}
}
