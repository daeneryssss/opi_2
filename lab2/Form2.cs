using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lab2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "HTML File(*.html)|*.html";
        }
        private bool isDocumentSaved = false;
        private string fileName = "";

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
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Copy();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Cut();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Paste();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.SelectedText = "";
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's a HTML editor.\n" +
                "Author: Dariia Vasylieva\n" +
                "Course: 3\n" +
                "Group: 6.1210-1pi");
        }

        private void listOfTagsToolStripMenuItem_Click(object sender, EventArgs e)
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
                string fileContent = File.ReadAllText(fileName);
                string list = "\n";
                Regex htmlTagRegex = new Regex("<[^/]+>");
                MatchCollection matches = htmlTagRegex.Matches(fileContent);
                List<string> htmlTags = new List<string>();
                foreach (Match match in matches)
                {
                    htmlTags.Add(match.Value);
                    list += match.Value;
                    list += "\n";
                }
                MessageBox.Show($"List of tags in this HTML file: {list}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tagFound = textBox1.Text;
            string tagReplacing = textBox2.Text;
            string content = richTextBox1.Text;
            string newContent = Regex.Replace(content, "<" + tagFound + ">", "<" + tagReplacing + ">");
            newContent = Regex.Replace(newContent, "</" + tagFound + ">", "</" + tagReplacing + ">");
            richTextBox1.Text = newContent;
        }
    }
}
