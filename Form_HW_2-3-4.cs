using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsForms_HW4_Synchronization_events_critical_sections
{
    public partial class Form_Bus_Stop : Form
    {
        private const int MaxBusCapacity = 50;
        private const int MaxPassengers = 100;
        private int passengersOnStop = 0; //количество пассажиров на остановке
        private List<int> busNumbers;
        private readonly object lockObject = new object();
        private readonly Random random = new Random();
        private Dictionary<int, int> passengerPreferences; // словарь для хранения пассажир (ключ) - номер автобуса (значение) 

        private Thread busThread;

        public Form_Bus_Stop()
        {
            InitializeComponent();
            busNumbers = new List<int>();
            passengerPreferences = new Dictionary<int, int>();
        }

        private void FormBusStop_Load(object sender, EventArgs e)
        {
            // Генерация номера автобуса
            int busNumber;
            for (global::System.Int32 i = 0; i < 5; i++)
            {
                busNumber = random.Next(100, 1000);
                busNumbers.Add(busNumber);
            }

            // запуск потока - работа автобуса
            busThread = new Thread(SimulateBusArrival);
            busThread.Start();
        }

        private void SimulateBusArrival()
        {
            while (true)
            {
                int busNumber = busNumbers[random.Next(5)];

                // считываем текущее количество пассажиров на остановке
                int passengersOnStopBeforeArrival, newPassengersInBus;
                lock (lockObject)
                {
                    passengersOnStopBeforeArrival = passengersOnStop;
                }
                int passengersLeftOnStop = passengersOnStopBeforeArrival;

                // генерация количества пассажиров в автобусе
                int passengersInBus = random.Next(MaxBusCapacity + 1);

                // отправка автобуса
                int passengersOnBus = passengersInBus;
                for (int i = 0; i < passengersOnStopBeforeArrival && passengersOnBus < MaxBusCapacity; i++)
                {

                    if (passengersOnStopBeforeArrival <= 0) break;

                    else if (passengersOnBus == MaxBusCapacity) break;

                    else if (passengerPreferences[i] == busNumber && passengersOnStopBeforeArrival > 0)
                    {
                        passengersOnBus++;
                        passengersLeftOnStop--;
                    }
                }
                if (passengersOnBus - passengersInBus <= 0)
                    newPassengersInBus = 0;
                else newPassengersInBus = passengersOnBus - passengersInBus;

                //обновляем лист бокс
                Invoke(new Action(() =>
                {
                    listBoxLog.Items.Add($"Автобус №{busNumber} прибыл на остановку. Всего пассажиров на остановке: {passengersOnStopBeforeArrival}," +
                        $" в автобусе: {passengersInBus}, Новых пассажиров : {newPassengersInBus}");
                    listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                }));

                // обновляем количество пассажиров на остановке
                lock (lockObject)
                {
                    passengersOnStop = passengersLeftOnStop;
                }

                // пауза перед следующим прибытием автобуса
                Thread.Sleep(random.Next(3000, 6000)); // от 3 до 6 секунд
            }
        }

        private void buttonAddPassengers_Click(object sender, EventArgs e)
        {
            // добавление случайного количества пассажиров на остановку
            int newPassengers = random.Next(100);

            for (int i = passengerPreferences.Count; i < newPassengers; i++)
            {
                int preferredBusNumber = busNumbers[random.Next(5)];
                passengerPreferences.Add(i, preferredBusNumber);
            }

            Invoke(new Action(() =>
            {
                listBoxLog.Items.Add($"На остановке прибыло {passengerPreferences.Count} пассажиров.");
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }));

            // Обновление количества пассажиров на остановке
            lock (lockObject)
            {
                passengersOnStop += passengerPreferences.Count;
            }
        }

        private void FormBusStop_FormClosing(object sender, FormClosingEventArgs e)
        {
            // остановка потока при закрытии формы
            if (busThread != null && busThread.IsAlive)
                busThread.Abort();
        }
    }
}
