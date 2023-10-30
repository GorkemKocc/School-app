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

namespace yazlab1
{
	public partial class StudentScreen : UserControl
	{

		NpgsqlConnection connection = new NpgsqlConnection("server=localHost; port=5432; Database=yazlab1; user ID=postgres; password=admin");

		public StudentScreen()
		{
			InitializeComponent();

		}

		static List<string[]> course_data = new List<string[]>();

		static void GetTranscript(string path)
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
		static void TranscriptSplit(string text)
		{
			string[] splittedLines = text.Split('\n');
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
				}
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
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
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

			connection.Open();

			string selectQuery = "SELECT identification_number, name, surname, lectures FROM teachers ";// ORDER BY identification_number ASC";

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
								formattedLectures = $"{identificationNumber} - {name} {surname} - {courseCode} {courseName}";
								checkedListBox1.Items.Add(formattedLectures);
							}
						}
					}
				}
			}

			connection.Close();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			connection.Open();

			bool isFirstCourse = true;
			string updateQuery;

			using (NpgsqlCommand command = new NpgsqlCommand("SELECT agreement_status FROM students WHERE student_id = @studentId", connection))
			{
				command.Parameters.AddWithValue("studentId", 1); // Student ID'yi doğru bir değerle değiştirin



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

				//string identificationNumber = selectedText.Substring(0, selectedText.IndexOf('-')).Trim();
				string[] parts = selectedText.Split('-');

				string identificationNumber = parts[0].Trim();
				string teacherName = parts[1].Trim();
				string courseInfo = parts[2].Trim();


				string jsonLecture = $@"[{{""teachers_id"": ""{identificationNumber}"", ""teachers_name"": ""{teacherName}"", ""course_info"": ""{courseInfo}""}}]";

				if (isFirstCourse)
				{
					updateQuery = @"
						UPDATE students
						SET agreement_status = @jsonLecture
						WHERE student_id = @studentId";
					isFirstCourse = false;
				}
				else
				{
					updateQuery = @"
						UPDATE students
						SET agreement_status = agreement_status || @jsonLecture
						WHERE student_id = @studentId";
				}
				using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
				{
					command.Parameters.AddWithValue("jsonLecture", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonLecture);
					command.Parameters.AddWithValue("studentId", 1);///////******* girilen id yap DÜZELT*************************************

					command.ExecuteNonQuery();
				}

			}
			connection.Close();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			checkedListBox1.Items.Clear();
			checkedListBox1.Visible = true;
			richTextBox1.Visible = false;

			connection.Open();

			string selectQuery = "SELECT agreement_status FROM students WHERE student_id = @studentId";

			using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
			{
				command.Parameters.AddWithValue("studentId", 1);/////////////// DÜZELT************************************************

				using (NpgsqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						string agreementStatus = reader["agreement_status"].ToString();

						var jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(agreementStatus) as Newtonsoft.Json.Linq.JArray;

						string formattedAgreementStatus = "";

						if (jsonArray != null)
						{
							foreach (var item in jsonArray)
							{
								string courseInfo = item["course_info"].ToString();
								string teachersId = item["teachers_id"].ToString();
								string teachersName = item["teachers_name"].ToString();
								formattedAgreementStatus = $"{teachersId} - {teachersName} - {courseInfo}";
								checkedListBox1.Items.Add(formattedAgreementStatus);

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

			connection.Open();

			string updateQuery;

			foreach (object selectedItem in checkedListBox1.CheckedItems)
			{
				string selectedText = selectedItem.ToString();

				//string identificationNumber = selectedText.Substring(0, selectedText.IndexOf('-')).Trim();
				string[] parts = selectedText.Split('-');

				string identificationNumber = parts[0].Trim();
				string teacherName = parts[1].Trim();
				string courseInfo = parts[2].Trim();


				string jsonAgreementStatus = $@"[{{""course_info"": ""{courseInfo}"",""teachers_id"": ""{identificationNumber}"", ""teachers_name"": ""{teacherName}""}}]";


				updateQuery = @"UPDATE students
							SET agreement_status = (
							SELECT jsonb_agg(item)
							FROM jsonb_array_elements(agreement_status) AS item
							WHERE NOT (
							(item->>'course_info' = @courseInfo)
							AND
							(item->>'teachers_id' = @teacherId)))
							WHERE student_id =  @studentId;";////////////////////////ANALİZ ET*********************

				using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
				{
					command.Parameters.AddWithValue("jsonAgreementStatus", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonAgreementStatus);
					command.Parameters.AddWithValue("courseInfo", courseInfo);
					command.Parameters.AddWithValue("teacherId", identificationNumber);
					command.Parameters.AddWithValue("studentId", 1);///////******* girilen id yap DÜZELT*************************************

					command.ExecuteNonQuery();
				}

			}
			connection.Close();

			button6_Click(sender, e);
		}
	}
}
