using System;
using System.Collections.Generic;

class program
{
    // Parkeringsplatser
    static List<string>[] parkingSpots = new List<string>[101];

    static void Main(string[] args)
    {
        // Skapar en lista för varje parkeringsruta
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            parkingSpots[i] = new List<string>();
        }

        // Visa meny tills användaren avslutar
        while (true)
        {
            ShowMenu();
            switch (Console.ReadLine())
            {
                case "1":
                    ParkVehicle();
                    break;
                case "2":
                    MoveVehicle();
                    break;
                case "3":
                    RemoveVehicle();
                    break;
                case "4":
                    SearchVehicle();
                    break;
                case "5":
                    ShowParkingStatus();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }

    static void ParkVehicle()
    {
        Console.Write("Ange registreringsnummer (börjar med MC för motorcykel eller CAR för bil): ");
        string Regnummer = Console.ReadLine();

        // Om inget registreringsnummer har angetts
        if (string.IsNullOrWhiteSpace(Regnummer))
        {
            Console.WriteLine("Inget registreringsnummer angivet.");
            return;
        }

        if (Regnummer.StartsWith("MC"))
        {
            Console.WriteLine("Registreringsnumret är giltigt för en MC.");

            // Hitta en plats för MC där det antingen finns en annan MC eller är helt tomt
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                if (parkingSpots[i].Count == 0 ||
                    (parkingSpots[i].Count == 1 && parkingSpots[i][0].StartsWith("MC")))
                {
                    parkingSpots[i].Add(Regnummer);
                    Console.WriteLine($"Motorcykeln har parkerats på plats {i + 1}.");
                    return;
                }
            }

            Console.WriteLine("Det finns ingen ledig parkeringsplats för MC.");
        }
        else if (Regnummer.StartsWith("CAR"))
        {
            Console.WriteLine("Registreringsnumret är giltigt för en bil.");

            // Hitta en ledig plats för att parkera bilen
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                if (parkingSpots[i].Count == 0)
                {
                    parkingSpots[i].Add(Regnummer);
                    Console.WriteLine($"Bilen har parkerats på plats {i + 1}.");
                    return;
                }
            }

            Console.WriteLine("Det finns ingen ledig parkeringsplats för bil.");
        }
        else
        {
            Console.WriteLine("Var vänlig att ge ett giltigt registreringsnummer som börjar med MC eller CAR.");
        }
    }

    static void MoveVehicle()
    {
        Console.Write("Ange registreringsnummer: ");
        string regNumber = Console.ReadLine();

        int currentSpot = FindVehicle(regNumber);
        if (currentSpot == -1)
        {
            Console.WriteLine("Fordonet kunde inte hittas.");
            return;
        }

        Console.Write("Ange ny platsnummer (1-100): ");
        if (int.TryParse(Console.ReadLine(), out int newSpot) && newSpot >= 1 && newSpot <= 100)
        {
            newSpot--;

            if (regNumber.StartsWith("MC"))
            {
                // MC kan dela en plats om det finns en annan MC eller platsen är tom
                if (parkingSpots[newSpot].Count == 0 ||
                    (parkingSpots[newSpot].Count == 1 && parkingSpots[newSpot][0].StartsWith("MC")))
                {
                    parkingSpots[newSpot].Add(regNumber);
                    parkingSpots[currentSpot].Remove(regNumber); // Ta bort fordonet från sin gamla plats
                    Console.WriteLine($"Motorcykeln har flyttats till plats {newSpot + 1}.");
                }
                else
                {
                    Console.WriteLine("Den nya platsen är upptagen och kan inte rymma en MC.");
                }
            }
            else if (regNumber.StartsWith("CAR"))
            {
                // Bilar måste ha en helt tom plats
                if (parkingSpots[newSpot].Count == 0)
                {
                    parkingSpots[newSpot].Add(regNumber);
                    parkingSpots[currentSpot].Remove(regNumber); // Ta bort fordonet från sin gamla plats
                    Console.WriteLine($"Bilen har flyttats till plats {newSpot + 1}.");
                }
                else
                {
                    Console.WriteLine("Den nya platsen är upptagen.");
                }
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt platsnummer.");
        }
    }

    static void RemoveVehicle()
    {
        Console.Write("Ange registreringsnummer att ta bort: ");
        string regNumber = Console.ReadLine();

        int spot = FindVehicle(regNumber);
        if (spot == -1)
        {
            Console.WriteLine("Fordonet kunde inte hittas.");
            return;
        }

        parkingSpots[spot].Remove(regNumber);
        Console.WriteLine($"Fordonet med registreringsnummer {regNumber} har tagits bort från plats {spot + 1}.");
    }

    static void SearchVehicle()
    {
        Console.Write("Ange registreringsnummer att söka: ");
        string regNumber = Console.ReadLine();

        int spot = FindVehicle(regNumber);
        if (spot == -1)
        {
            Console.WriteLine("Fordonet kunde inte hittas.");
        }
        else
        {
            Console.WriteLine($"Fordonet finns på plats {spot + 1}.");
        }
    }

    static int FindVehicle(string regNumber)
    {
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i].Contains(regNumber))
            {
                return i;
            }
        }
        return -1;
    }

    static void ShowParkingStatus()
    {
        Console.WriteLine("\nParkeringsstatus:");
        for (int i = 0; i < parkingSpots.Length; i++)
        {
            if (parkingSpots[i].Count > 0)
            {
                Console.WriteLine($"Plats {i + 1}: {string.Join(", ", parkingSpots[i])}");
            }
            else
            {
                Console.WriteLine($"Plats {i + 1}: [TOM]");
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n--- Parkeringssystem ---");
        Console.WriteLine("Öppet från 07:00 - 00:00, annars straffavgift.");
        Console.WriteLine("1. Parkera fordon");
        Console.WriteLine("2. Flytta fordon");
        Console.WriteLine("3. Ta bort fordon");
        Console.WriteLine("4. Sök efter fordon");
        Console.WriteLine("5. Visa parkeringsstatus");
        Console.WriteLine("6. Avsluta");
        Console.Write("Vänligen välj ett alternativ: ");
    }
}
