using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForms_HW4_Synchronization_events_critical_sections
{
    public partial class Form_HW_1 : Form
    {
        private static readonly object lockObject = new object();
        private static readonly Random random = new Random();
        private static readonly string fileName = "numbers.txt";
        private ManualResetEvent generationCompletedEvent = new ManualResetEvent(false);
        private ManualResetEvent sumCompletedEvent = new ManualResetEvent(false);
        private ManualResetEvent productCompletedEvent = new ManualResetEvent(false);

        public Form_HW_1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Text = "Выполняется...";
            buttonStart.Enabled = false;
            lblStatus.Text = "Выполняется генерация\nчисел и их запись в файл";
            
            if (File.Exists(fileName))
                File.Delete(fileName);
            if (File.Exists("sums.txt"))
                File.Delete("sums.txt");
            if (File.Exists("products.txt"))
                File.Delete("products.txt");

            // создаем и запускаем поток для генерации чисел и записи их в файл
            Thread generatorThread = new Thread(GenerateNumbersToFile);
            generatorThread.Start();

            //создаем и запускаем поток для подсчета суммы пар чисел
            Thread sumThread = new Thread(CalculateSum);
            sumThread.Start();

            //Создаем и запускаем поток для произведения пар чисел
            Thread productThread = new Thread(CalculateProduct);
            productThread.Start();


        }

        private void GenerateNumbersToFile()
        {
            lock (lockObject)
            {
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    for (int i = 0; i < 10; i++) 
                    {
                        int num1 = random.Next(1, 101); 
                        int num2 = random.Next(1, 101); 
                        writer.WriteLine($"{num1} {num2}");

                        // изменением цвета фона формы для создания визуального эффекта
                        Invoke(new Action(() => { BackColor = System.Drawing.Color.LightBlue; }));
                        Thread.Sleep(500); // задержка для визуального эффекта
                        Invoke(new Action(() => { BackColor = System.Drawing.Color.LightGray; }));
                        Thread.Sleep(500); // задержка для визуального эффекта
                    }
                }

                // обновляем интерфейс в главном потоке
                Invoke(new Action(() => {
                    lblStatus.Text = "Генерация чисел завершена.";
                    MessageBox.Show("Числа записаны в файл numbers.txt", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));

                generationCompletedEvent.Set();
            }
        }

        private void CalculateSum()
        {
            lock (lockObject)
            {
                generationCompletedEvent.WaitOne(); // ожидание завершения генерации чисел
                lblStatus.Text = "Выполняется подсчет суммы\nчисел и запись суммы в файл.";
                string[] lines = File.ReadAllLines(fileName);
                using (StreamWriter writer = File.AppendText("sums.txt"))
                {
                    foreach (string line in lines)
                    {
                        string[] numbers = line.Split(' ');
                        int num1 = int.Parse(numbers[0]);
                        int num2 = int.Parse(numbers[1]);
                        int sum = num1 + num2;
                        writer.WriteLine($"Сумма чисел {num1} и {num2} равна {sum}");

                        Invoke(new Action(() => { BackColor = System.Drawing.Color.LightBlue; }));
                        Thread.Sleep(500);
                        Invoke(new Action(() => { BackColor = System.Drawing.Color.LightGray; }));
                        Thread.Sleep(500);
                    }
                }
                //обновляем интерфейс в главном потоке
                Invoke(new Action(() =>
                {
                    lblStatus.Text = "Подсчет сумм завершен.";
                    MessageBox.Show("Суммы записаны в файл sums.txt", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));

                sumCompletedEvent.Set(); // устанавливаем сигнал завершения
            }
        }

        private void CalculateProduct()
        {
            lock (lockObject)
            {
                generationCompletedEvent.WaitOne();
                lblStatus.Text = "Выполняется подсчет произведения\nчисел и запись произведения в файл.";
                string[] lines = File.ReadAllLines(fileName);
                using (StreamWriter writer = File.AppendText("products.txt"))
                {
                    foreach (string line in lines)
                    {
                        string[] numbers = line.Split(' ');
                        int num1 = int.Parse(numbers[0]);
                        int num2 = int.Parse(numbers[1]);
                        int product = num1 * num2;
                        writer.WriteLine($"Произведение чисел {num1} и {num2} равно {product}");

                        Invoke(new Action(() => { BackColor = System.Drawing.Color.LightBlue; }));
                        Thread.Sleep(500);
                        Invoke(new Action(() => { BackColor = System.Drawing.Color.LightGray; }));
                        Thread.Sleep(500);

                    }
                }
                //обновляем интерфейс в главном потоке
                Invoke(new Action(() =>
                {
                    Thread.Sleep(1000);
                    MessageBox.Show("Произведения записаны в файл products.txt", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Операции успешно завершены.";
                    buttonStart.Text = "Выполнено";
                }));

                productCompletedEvent.Set(); // устанавливаем сигнал завершения
            }
        }

    }

}
