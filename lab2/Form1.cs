using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text File(*.txt)|*.txt";
        }
        public bool isDocumentSaved = false;
        public string fileName = "";

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            fileName = saveFileDialog1.FileName;
            File.WriteAllText(fileName, richTextBox1.Text);
            MessageBox.Show("File has been saved!");
            isDocumentSaved = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            fileName = openFileDialog1.FileName;
            string fileText = File.ReadAllText(fileName);
            richTextBox1.Text = fileText;
            isDocumentSaved = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDocumentSaved == true)
                richTextBox1.SaveFile(fileName, RichTextBoxStreamType.PlainText);
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                fileName = saveFileDialog1.FileName;
                File.WriteAllText(fileName, richTextBox1.Text);
            }
            MessageBox.Show("File has been saved!");
            isDocumentSaved = true;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.TextLength > 0)
            {
                richTextBox1.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Paste();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Cut();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.SelectedText = "";
            }
        }

        private void deleteAllExtraSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Text = richTextBox1.Text;
            Text = Regex.Replace(Text, @"\n+", "\n");
            Text = Regex.Replace(Text, @"\s{2,}", " ");
            DialogResult result = MessageBox.Show("Your input text:\n" +
                $"{richTextBox1.Text}\n" +
                "Output text without empty strings and double spaces:\n" +
                $"{Text}\n" +
                $"Do you want to replace input text with output?", "Yes, replace it", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                richTextBox1.Text = Text;
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Error: No file is currently opened.");
                return;
            }
            else if (isDocumentSaved == false)
            {
                MessageBox.Show("Error: Please open a file or save changes.");
                return;
            }
            else
            {
                //розмір у Кб
                long fileSizeBytes = new FileInfo(fileName).Length;
                double fileSizeKB = (double)fileSizeBytes / 1024;
                //кількість символів
                string fileContent = File.ReadAllText(fileName);
                int symbols = fileContent.Length;
                //кількість абзаців
                string[] paragraphs = fileContent.Split(new string[] { "\n" }, StringSplitOptions.None);
                int paragraphCount = paragraphs.Count(p => !string.IsNullOrWhiteSpace(p));
                //кількість пустих рядків
                int emptyCount = paragraphs.Length - paragraphCount;
                //кількість авторських сторінок
                int pagesCount = symbols / 1800;
                //кількість голосних, приголосних, цифр, спеціальних символів, знаків пунктуації
                int vowelCount = 0, consonantsCount = 0, numbersCount = 0, specialSymbolsCount = 0;
                foreach(char symbol in fileContent.ToLower())
                {
                    if (char.IsLetter(symbol))
                    {
                        if (symbol == 'a' || symbol == 'e' || symbol == 'i' || symbol == 'o' || symbol == 'u')
                            vowelCount++;
                        else
                            consonantsCount++;
                    }
                    else if (char.IsDigit(symbol))
                        numbersCount++;
                    else if (!char.IsWhiteSpace(symbol))
                        specialSymbolsCount++;
                }
                MessageBox.Show($"\nDon't forget to save changes, because it's the statistic for saved file!" +
                    $"\nFile name: {Path.GetFileName(openFileDialog1.FileName)}" +
                    $"\nFile size: {fileSizeKB.ToString("0.00")} KB" +
                    $"\nNumber of symbols is: { symbols}" +
                    $"\nNumber of paragraphs: { paragraphCount}" +
                    $"\nNumber of empty lines: { emptyCount}" +
                    $"\nNumber of author pages (1 author page = 1800 symbols): { pagesCount}" +
                    $"\nNumber of vowels: { vowelCount}" +
                    $"\nNumber of consonants: { consonantsCount}" +
                    $"\nNumber of digits: { numbersCount}" +
                    $"\nNumber of special symbols: { specialSymbolsCount}");
            }
        }

        private void infoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's a text editor.\n" +
                "Author: Dariia Vasylieva\n" +
                "Course: 3\n" +
                "Group: 6.1210-1pi");
        }
        private void findToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Error: No file is currently opened.");
                return;
            }
            else if (isDocumentSaved == false)
            {
                MessageBox.Show("Error: Please open a file or save changes.");
                return;
            }
            else
            {
                MessageBox.Show("Now we will find the first appearence of pair of words, that both begin with capital letters and have only one space between them!");
                string pattern = @"\b[A-Z][a-z]*\s[A-Z][a-z]*\b";
                string fileContent = File.ReadAllText(fileName);
                Match match = Regex.Match(fileContent, pattern);
                if (match.Success)
                    MessageBox.Show($"Success! Match found: {match.Value}");
                else
                    MessageBox.Show("Failure... Match not found.");
            }
        }

        private void hTMLEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int amountNews = (int)numericUpDown1.Value;
            int start = 0;
            int page = 1;
            string baseUrl = "https://www.znu.edu.ua/cms/index.php?action=news/view&start={0}&site_id=27&lang=ukr";
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            List<string> headers = new List<string>();
            List<string> annotations = new List<string>();
            Regex headerRegex = new Regex("<h4>(.+?)</h4>");
            Regex annotationRegex = new Regex("<div class=\"text\">(.+?)</div>");
            while (headers.Count < amountNews)
            {
                string url = string.Format(baseUrl, start);
                string htmlContent = client.DownloadString(url);
                MatchCollection headerMatches = headerRegex.Matches(htmlContent);
                MatchCollection annotationMatches = annotationRegex.Matches(htmlContent);
                for (int i = 0; i < headerMatches.Count && i < annotationMatches.Count; i++)
                {
                    headers.Add(headerMatches[i].Groups[1].Value);
                    string annotation = annotationMatches[i].Groups[1].Value;
                    annotation = Regex.Replace(annotation, "<.*?>", string.Empty);
                    annotations.Add(annotation);
                    if (headers.Count >= amountNews)
                    {
                        break;
                    }
                }
                start += 20;
                page++;
            }
            richTextBox1.Clear();
            for (int i = 0; i < headers.Count && i < annotations.Count; i++)
            {
                richTextBox1.AppendText(headers[i] + Environment.NewLine + "\n");
                richTextBox1.AppendText(annotations[i] + Environment.NewLine + "\n");
                richTextBox1.AppendText(Environment.NewLine + "\n" + "--------------------------------------------------------------------------------" + "\n");
            }
        }
        public class Document
        {
            public string Text { get; set; }

            public Document(string text)
            {
                Text = text;
            }
        }

        public abstract class DocumentWriter
        {
            public abstract void Write(Document document, string fileName);
        }

        public class HtmlDocumentWriter : DocumentWriter
        {
            public override void Write(Document document, string fileName)
            {
                string htmlText = "";
                if (Path.GetExtension(fileName) != ".html")
                {
                    // Not an HTML file, write as plain text
                    htmlText = document.Text;
                }
                else
                {
                    // HTML file, add tags
                    htmlText = "<html><body>";
                    foreach (string paragraph in document.Text.Split('\n'))
                    {
                        htmlText += "<p>" + paragraph + "</p>";
                    }
                    htmlText += "</body></html>";
                }

                File.WriteAllText(fileName, htmlText);
            }
        }

        public class TextDocumentWriter : DocumentWriter
        {
            public override void Write(Document document, string fileName)
            {
                if (Path.GetExtension(fileName) != ".txt")
                {
                    // Not a text file, append .txt extension
                    fileName += ".txt";
                }
                File.WriteAllText(fileName, document.Text);
            }
        }

        public class BinaryDocumentWriter : DocumentWriter
        {
            public override void Write(Document document, string fileName)
            {
                if (Path.GetExtension(fileName) != ".bin")
                {
                    // Not a binary file, append .bin extension
                    fileName += ".bin";
                }
                byte[] bytes = Encoding.UTF8.GetBytes(document.Text);
                File.WriteAllBytes(fileName, bytes);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Document document = new Document(richTextBox1.Text);

            DocumentWriter writer;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (radioButton1.Checked)
            {
                writer = new TextDocumentWriter();
                saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            }
            else if (radioButton2.Checked)
            {
                writer = new HtmlDocumentWriter();
                saveFileDialog1.Filter = "HTML Files (*.html)|*.html";
            }
            else if (radioButton3.Checked)
            {
                writer = new BinaryDocumentWriter();
                saveFileDialog1.Filter = "Binary Files (*.bin)|*.bin";
            }
            else
            {
                MessageBox.Show("Please select a file format.");
                return;
            }

            
            
            saveFileDialog1.Title = "Save file as";
            saveFileDialog1.ShowDialog();

            try
            {
                writer.Write(document, saveFileDialog1.FileName);
                MessageBox.Show("File saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message);
            }
        }

    }
}
