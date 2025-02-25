using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp001
{
    public partial class Form1 : Form
    {

        string lastFilePath = "";

        public Form1()
        {
            InitializeComponent();
            richTextBox1.AcceptsTab = true;
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void собратьИЗапуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastFilePath == "")
            {
                textBox1.Clear();
                textBox1.AppendText("Сохраните файл cpp куда-нибудь\r\n");
                MessageBox.Show("Сохраните файл cpp куда-нибудь", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            try {
                richTextBox1.SaveFile(lastFilePath, RichTextBoxStreamType.PlainText);
                textBox1.Clear();
                textBox1.AppendText("Начинается сборка...\r\n");
                string cppFilePath = lastFilePath;
                string directoryPath = Path.GetDirectoryName(lastFilePath);
                string executablePath = Path.Combine(directoryPath, "main.exe");

                // Путь к vcvarsall.bat (настройка окружения Visual Studio)
                string vcvarsallPath = @"C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat";

                // Аргументы для vcvarsall.bat (x64 или x86)
                string vcvarsallArgs = "x64";

                // Команда для компиляции main.cpp
                string compileCommand = $"cl /EHsc /Fe:\"{executablePath}\" \"{lastFilePath}\"";

                // Создаем процесс для настройки окружения и компиляции
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c \"\"{vcvarsallPath}\" {vcvarsallArgs} && {compileCommand}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;


                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866); // OEM Russian
                process.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(866);  // OEM Russian

                // Запуск процесса
                process.Start();

                // Чтение вывода (для отладки)
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                textBox1.AppendText("Идет сборка...\r\n");

                // Ожидание завершения процесса
                process.WaitForExit();

                // Проверка успешности завершения
                if (process.ExitCode == 0)
                {
                    textBox1.AppendText("Сборка завершена\r\n");
                    //MessageBox.Show("Сборка завершена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("\"" + executablePath + "\"");
                }
                else
                {
                    textBox1.AppendText("Ошибка сборки\r\n");
                    MessageBox.Show($"Ошибка сборки:\n{error}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText("Произошло исключение\r\n");
                MessageBox.Show($"Исключение: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "C++ Files (*.cpp)|*.cpp|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
            }

            lastFilePath = saveFileDialog.FileName;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "C++ Files (*.cpp)|*.cpp|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
            }

            lastFilePath = openFileDialog.FileName;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Vladislav IDE.\nВерсия 1.0\n© 2025", // Текст сообщения
                "О программе",                            // Заголовок окна
                MessageBoxButtons.OK,                     // Кнопка "OK"
                MessageBoxIcon.Information               // Иконка информации
            );
        }
    }
}
