using System;

namespace GlassPyramid
{
    class Program
    {
        static void Main(string[] args)
        {
            int row;
            int glass;

            // Inmatning: Vilken rad och vilket glas
            Console.WriteLine("Skriv in rad:");
            string rowInput = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Skriv in glas:");
            string glassInput = Console.ReadLine() ?? string.Empty;

            // Validera inmatningen
            if (!int.TryParse(rowInput, out row) || row < 2 || row > 50 ||
                !int.TryParse(glassInput, out glass) || glass < 1 || glass > row)
            {
                Console.WriteLine("Ogiltig inmatning, vänligen försök igen.");
                return;
            }

            // Beräkna tiden det tar för det angivna glaset att svämma över
            double timeToOverflow = CalculateTimeToOverflow(row, glass);

            // Skriv ut resultatet
            Console.WriteLine($"Rad ? {row}");
            Console.WriteLine($"Glas ? {glass}");
            Console.WriteLine($"Det tar {timeToOverflow:F6} sekunder.");
        }

        // Metod för att beräkna tiden det tar för ett specifikt glas att svämma över
        static double CalculateTimeToOverflow(int row, int glass)
        {
            // Skapa en array för att hålla koll på mängden vatten i varje glas
            double[][] glasses = new double[row + 1][];
            for (int i = 0; i <= row; i++)
            {
                glasses[i] = new double[i + 1];
            }

            // Initiera tiden och flödeshastigheten
            double time = 0;
            double pourRate = 1.0 / 10.0; // Vatten fyller 1 glas på 10 sekunder

            // Fyll det översta glaset och starta tiden
            glasses[0][0] += 1.0; // Fyll första glaset
            time += 10.0;

            // Överflöde från första glaset till rad 2
            glasses[1][0] += 0.5;
            glasses[1][1] += 0.5;
            time += 20.0; // 20 sekunder för att fylla rad 2

            // Om det specifika glaset är på rad 2, returnera tiden direkt
            if (row == 2)
            {
                return time;
            }

            // Simulera flödet för rader 3 och framåt
            while (glasses[row - 1][glass - 1] < 1)
            {
                // Kaskadera översvämningen nerför pyramiden
                for (int r = 0; r < row; r++)
                {
                    for (int g = 0; g <= r; g++)
                    {
                        if (glasses[r][g] > 1)
                        {
                            double overflow = glasses[r][g] - 1;
                            glasses[r][g] = 1;

                            // Fördela översvämningen till glasen nedanför
                            if (r + 1 <= row)
                            {
                                if (r == 0) // Första raden
                                {
                                    glasses[r + 1][g] += overflow / 2;
                                    glasses[r + 1][g + 1] += overflow / 2;
                                }
                                else if (r == 1) // Andra raden
                                {
                                    glasses[r + 1][g] += overflow / 2;
                                    glasses[r + 1][g + 1] += overflow / 2;
                                }
                                else // Rad 3 och vidare
                                {
                                    if (g == 0 || g == r) // Kantglas
                                    {
                                        glasses[r + 1][g] += overflow / 2;
                                    }
                                    else // Mellanglas
                                    {
                                        glasses[r + 1][g] += overflow / 4;
                                        glasses[r + 1][g + 1] += overflow / 4;
                                        glasses[r + 1][g - 1] += overflow / 4;
                                        glasses[r + 1][g + 1] += overflow / 4;
                                    }
                                }
                            }
                        }
                    }
                }

                // Kontrollera om målglaset svämmar över
                if (glasses[row - 1][glass - 1] >= 1)
                {
                    break;
                }

                // Öka tiden
                time += 0.1; // Öka tiden med 0.1 sekunder per iteration

                // Tillsätt vatten till det översta glaset igen
                glasses[0][0] += pourRate * 0.1; // Öka mängden vatten i det översta glaset


            }

            return time; // Returnera den totala tiden
        }
    }
}
