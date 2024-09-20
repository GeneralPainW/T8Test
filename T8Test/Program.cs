namespace T8Test
{
    internal class Program
    {
        static double DbmToMilliwatts(double dbm)
        {
            return Math.Pow(10, dbm / 10);
        }

        static double MilliwattsToDbm(double milliwatts)
        {
            return 10 * Math.Log10(milliwatts);
        }
        static void Main(string[] args)
        {
            bool uncorrectInput = true;
            var attenuationList = new List<double>();                                   // Затухание
            var gainList = new List<double>();                                          // Усиление
            var noiseList = new List<double>();                                         // Шум
            Console.Write("Введите количество пролетов: ");
            int numberOfLine = int.Parse(Console.ReadLine());

            while (uncorrectInput)
            {
                Console.Write(  "Идентичные сегменты линии связи?" +
                                "\n 1 - Да" +
                                "\n 2 - Нет" +
                                "\nВаш ввод: ");
                int.TryParse(Console.ReadLine(), out int input);
                if (input == 1)
                {
                    Console.Write($"Введите затухание (в дБ): ");
                    double attenuation = double.Parse(Console.ReadLine());              // Затухание в каждом сегменте линии 
                    Console.Write($"Введите усиление (в дБ): ");
                    double gain = double.Parse(Console.ReadLine());                     // Усиление в каждом сегменте линии
                    Console.Write($"Введите шум усилителя (в дБм): ");
                    double noise = double.Parse(Console.ReadLine());                    // Шум в каждом сегменте линии

                    for (int i = 0; i < numberOfLine; i++)
                    {
                        attenuationList.Add(attenuation);
                        gainList.Add(gain);
                        noiseList.Add(noise);
                    }
                    uncorrectInput = false;
                }
                else if (input == 2)
                {
                    for (int i = 0; i < numberOfLine; i++)
                    {
                        Console.WriteLine($"Заполняется {i + 1} пролет из {numberOfLine}");
                        Console.Write($"Введите затухание на {i + 1}-м пролете (в дБ): ");
                        attenuationList.Add(double.Parse(Console.ReadLine()));

                        Console.Write($"Введите усиление {i + 1}-го усилителя (в дБ): ");
                        gainList.Add(double.Parse(Console.ReadLine()));

                        Console.Write($"Введите шум {i + 1}-го усилителя (в дБм): ");
                        noiseList.Add(double.Parse(Console.ReadLine()));
                    }
                    uncorrectInput = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Неверный ввод команды.");
                }
            }

            Console.Write("Введите мощность на выходе передатчика (в дБм): ");
            double powerInput = double.Parse(Console.ReadLine());

            double signalPower = DbmToMilliwatts(powerInput);                       // Мощность сигнала на выходе передатчика
            double noisePower = 0;                                                  // Начальный шум при передаче

            for (int i = 0; i < numberOfLine; i++)
            {
                signalPower /= Math.Pow(10, attenuationList[i] / 10);               // Ослабление сигнала на пролете
                signalPower *= Math.Pow(10, gainList[i] / 10);                      // Усиление сигнала в усилителе
                double currentNoisePower = DbmToMilliwatts(noiseList[i]);           // Шум от текущего усилителя
                noisePower /= Math.Pow(10, attenuationList[i] / 10);                // Ослабление шума на пролете
                noisePower += currentNoisePower;                                    // Добавление шума от усилителя
                noisePower *= Math.Pow(10, gainList[i] / 10);                       // Усиление шума в усилителе
            }

            double powerOutput = MilliwattsToDbm(signalPower);
            double nPowerOutput = MilliwattsToDbm(noisePower);

            double osnr = powerOutput - nPowerOutput;

            Console.WriteLine($"Мощность на выходе линии: {powerOutput:F2} дБм");
            Console.WriteLine($"Мощность накопленного шума: {nPowerOutput:F2} дБм");
            Console.WriteLine($"OSNR: {osnr:F2} дБ");
        }
    }
}
