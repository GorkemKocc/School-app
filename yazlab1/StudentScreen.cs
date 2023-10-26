using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Pdf;

namespace yazlab1
{
	public partial class StudentScreen : UserControl
	{
		public StudentScreen()
		{
			InitializeComponent();
		}


		static List<string[]> course_data = new List<string[]>();

		static void GetTranscript(string path)
		{


			PdfDocument pdfDocument = new PdfDocument();
			pdfDocument.LoadFromFile(path);

			string textData = ""; // Metin verilerini saklamak için boş bir string

			foreach (PdfPageBase page in pdfDocument.Pages)
			{
				string text = page.ExtractText();
				textData += text; // Her sayfanın metnini birleştir
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
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			foreach (string[] course in course_data)
			{
				string courseText = string.Join(", ", course) + Environment.NewLine; // Her dersin sonuna yeni satır eklenir
				richTextBox1.AppendText(courseText);
			}
		}
	}
}
