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
        public bool isDocumentSaved = false;
        public string fileName = "";
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text File(*.txt)|*.txt";
            richTextBox1.KeyDown += richTextBox1_KeyDown;
        }
        // основні функції
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
            SaveTextToFile();
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
        // лабораторна 4
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = Singleton<LogEventLogger>.GetInstance(0);
                instance.LogEvent(DateTime.Now, "Last clicked change", richTextBox3);
            }
            catch (Exception ex)
            {
                richTextBox3.AppendText($"Error: {ex.Message}\r\n");
            }
        }

        public class LogEventLogger : Singleton<LogEventLogger>
        {
            private List<LogEvent> _logs;

            protected LogEventLogger()
            {
                _logs = new List<LogEvent>();
            }

            public void LogEvent(DateTime timeStamp, string text, RichTextBox richTextBox)
            {
                _logs.Add(new LogEvent(timeStamp, text, richTextBox));
                string logText = $"{timeStamp} - {text}\r\n";
                richTextBox.AppendText(logText);
            }
        }

        public class LogEvent
        {
            public DateTime TimeStamp { get; }
            public string Text { get; }
            public RichTextBox RichTextBox { get; }

            public LogEvent(DateTime timeStamp, string text, RichTextBox richTextBox)
            {
                TimeStamp = timeStamp;
                Text = text;
                RichTextBox = richTextBox;
            }
        }

        public class Singleton<T> where T : class
        {
            private static readonly Lazy<T>[] instances_;
            const int maxInstances = 1;
            static Singleton()
            {
                instances_ = new Lazy<T>[maxInstances];
                for (int i = 0; i < maxInstances; i++)
                {
                    instances_[i] = new Lazy<T>(() => CreateInstance());
                }
            }
            protected Singleton() { }

            private static T CreateInstance()
            {
                System.Reflection.ConstructorInfo cInfo = typeof(T).GetConstructor(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null,
                    new Type[0],
                    new System.Reflection.ParameterModifier[0]);
                return (T)cInfo.Invoke(null);
            }

            public static T GetInstance(int index)
            {
                if (index < 0 || index >= instances_.Length)
                    throw new ArgumentOutOfRangeException(nameof(index), "Invalid instance index");

                return instances_[index].Value;
            }
        }
        // лабораторна 5
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
                    htmlText = document.Text;
                }
                else
                {
                    htmlText = "<html>\n<body>\n";
                    foreach (string paragraph in document.Text.Split('\n'))
                    {
                        htmlText += "<p>" + paragraph + "</p>";
                    }
                    htmlText += "\n</body>\n</html>";
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
                    fileName += ".bin";
                }
                byte[] bytes = Encoding.UTF8.GetBytes(document.Text);
                File.WriteAllBytes(fileName, bytes);
            }
        }
        public static class DocumentWriterFactory
        {
            public static DocumentWriter CreateDocumentWriter(string fileExtension)
            {
                switch (fileExtension)
                {
                    case ".html":
                        return new HtmlDocumentWriter();
                    case ".txt":
                        return new TextDocumentWriter();
                    case ".bin":
                        return new BinaryDocumentWriter();
                    default:
                        throw new ArgumentException("Unsupported file extension: " + fileExtension);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Document document = new Document(richTextBox1.Text);
            string fileExtension;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (radioButton1.Checked)
            {
                fileExtension = ".txt";
                saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            }
            else if (radioButton2.Checked)
            {
                fileExtension = ".html";
                saveFileDialog1.Filter = "HTML Files (*.html)|*.html";
            }
            else if (radioButton3.Checked)
            {
                fileExtension = ".bin";
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
                DocumentWriter writer = DocumentWriterFactory.CreateDocumentWriter(fileExtension);
                writer.Write(document, saveFileDialog1.FileName);
                MessageBox.Show("File saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message);
            }
        }

        // лабораторна 6
        public abstract class DocumentReader
        {
            public abstract Document Read(string fileName);
        }

        public class HtmlDocumentReader : DocumentReader
        {
            public override Document Read(string fileName)
            {
                string htmlText = File.ReadAllText(fileName);
                htmlText = Regex.Replace(htmlText, @"<(?!p|br)[^>]*>", "");
                htmlText = htmlText.Replace("<br>", "\n");
                htmlText = htmlText.Replace("<p>", "\n");
                htmlText = htmlText.Replace("</p>", "");
                return new Document(htmlText);
            }
        }
        public class TextDocumentReader : DocumentReader
        {
            public override Document Read(string fileName)
            {
                string text = File.ReadAllText(fileName);
                return new Document(text);
            }
        }
        public class BinaryDocumentReader : DocumentReader
        {
            public override Document Read(string fileName)
            {
                byte[] bytes = File.ReadAllBytes(fileName);
                string text = Encoding.UTF8.GetString(bytes);
                return new Document(text);
            }
        }
        public abstract class DocumentReaderFactory
        {
            public abstract DocumentReader CreateDocumentReader();
        }
        public class HtmlDocumentReaderFactory : DocumentReaderFactory
        {
            public override DocumentReader CreateDocumentReader()
            {
                return new HtmlDocumentReader();
            }
        }
        public class TextDocumentReaderFactory : DocumentReaderFactory
        {
            public override DocumentReader CreateDocumentReader()
            {
                return new TextDocumentReader();
            }
        }
        public class BinaryDocumentReaderFactory : DocumentReaderFactory
        {
            public override DocumentReader CreateDocumentReader()
            {
                return new BinaryDocumentReader();
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            try
            {
                DocumentReaderFactory readerFactory;
                if (radioButton1.Checked)
                {
                    openFileDialog1.Filter = "Text Files (*.txt)|*.txt";
                    readerFactory = new TextDocumentReaderFactory();
                }
                else if (radioButton2.Checked)
                {
                    openFileDialog1.Filter = "HTML Files (*.html)|*.html";
                    readerFactory = new HtmlDocumentReaderFactory();
                }
                else if (radioButton3.Checked)
                {
                    openFileDialog1.Filter = "Binary Files (*.bin)|*.bin";
                    readerFactory = new BinaryDocumentReaderFactory();
                }
                else
                {
                    MessageBox.Show("Unsupported file format.");
                    return;
                }
                openFileDialog1.Title = "Open file";
                openFileDialog1.ShowDialog();
                string fileName = openFileDialog1.FileName;
                string fileExtension = Path.GetExtension(fileName);
                DocumentReader reader = readerFactory.CreateDocumentReader();
                Document document = reader.Read(fileName);
                richTextBox1.Text = document.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening file: " + ex.Message);
            }
        }
        // лабораторна 7 (автосейв + спостерігач)
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            var text = richTextBox1.Text;
            var lines = text.Split('\n');
            var linkObserver = new LinkObserver(richTextBox2);
            foreach (var line in lines)
            {
                linkObserver.Update(line);
            }
            if (e.KeyCode == Keys.Enter)
            {
                SaveTextToFile();
            }
        }
        private void SaveTextToFile()
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
        public interface IObserver
        {
            void Update(string data);
        }

        public class LinkObserver : IObserver
        {
            private readonly RichTextBox _output;

            public LinkObserver(RichTextBox output)
            {
                _output = output;
                _output.Text = "";
            }

            public void Update(string data)
            {
                var regex = new Regex(@"\S+\.(edu.ua|net.ua|com.ua|in.ua|org.ua)", RegexOptions.IgnoreCase);
                var matches = regex.Matches(data);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        _output.AppendText(match.Value + "\n");
                    }
                }
            }
        }
    }
}
