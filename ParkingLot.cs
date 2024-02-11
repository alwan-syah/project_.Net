using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        ParkingLot parkingLot = null;

        while (true)
        {
            Console.Write("$ ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            string[] args = input.Split(' ');

            switch (args[0])
            {
                case "create_parking_lot":
                    int totalSlots = int.Parse(args[1]);
                    parkingLot = new ParkingLot(totalSlots);
                    Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                    break;

                case "park":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string registrationNumber = args[1];
                    string color = args[2];
                    string vehicleType = args[3];

                    Vehicle vehicle = new Vehicle(registrationNumber, color, vehicleType);

                    int allocatedSlot = parkingLot.ParkVehicle(vehicle);
                    if (allocatedSlot != -1)
                        Console.WriteLine($"Allocated slot number: {allocatedSlot}");
                    else
                        Console.WriteLine("Sorry, parking lot is full");
                    break;

                case "leave":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    int slotToLeave = int.Parse(args[1]);
                    parkingLot.LeaveSlot(slotToLeave);
                    Console.WriteLine($"Slot number {slotToLeave} is free");
                    break;

                case "status":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    Console.WriteLine(parkingLot.GetStatus());
                    break;

                case "type_of_vehicles":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string type = args[1];
                    int count = parkingLot.CountVehiclesByType(type);
                    Console.WriteLine(count);
                    break;

                case "registration_numbers_for_vehicles_with_odd_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string oddPlateNumbers = parkingLot.GetRegistrationNumbersByPlateType("odd");
                    Console.WriteLine(oddPlateNumbers);
                    break;

                case "registration_numbers_for_vehicles_with_even_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string evenPlateNumbers = parkingLot.GetRegistrationNumbersByPlateType("even");
                    Console.WriteLine(evenPlateNumbers);
                    break;

                case "registration_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string colorToFilter = args[1];
                    string registrationNumbersByColor = parkingLot.GetRegistrationNumbersByColor(colorToFilter);
                    Console.WriteLine(registrationNumbersByColor);
                    break;

                case "slot_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string colorForSlot = args[1];
                    string slotNumbersByColor = parkingLot.GetSlotNumbersByColor(colorForSlot);
                    Console.WriteLine(slotNumbersByColor);
                    break;

                case "slot_number_for_registration_number":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                        break;
                    }

                    string regNumberToFind = args[1];
                    int foundSlotNumber = parkingLot.GetSlotNumberByRegistrationNumber(regNumberToFind);
                    if (foundSlotNumber != -1)
                        Console.WriteLine(foundSlotNumber);
                    else
                        Console.WriteLine("Not found");
                    break;

                case "exit":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }
}

class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string registrationNumber, string color, string type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}

class ParkingLot
{
    private readonly int totalSlots;
    private readonly List<Vehicle> parkedVehicles;

    public ParkingLot(int totalSlots)
    {
        this.totalSlots = totalSlots;
        parkedVehicles = new List<Vehicle>();
    }

    public int ParkVehicle(Vehicle vehicle)
    {
        if (parkedVehicles.Count < totalSlots)
        {
            parkedVehicles.Add(vehicle);
            return parkedVehicles.Count;
        }
        return -1; // Parking lot is full
    }

    public void LeaveSlot(int slotNumber)
    {
        if (slotNumber > 0 && slotNumber <= parkedVehicles.Count)
            parkedVehicles.RemoveAt(slotNumber - 1);
    }

    public string GetStatus()
    {
        string status = "Slot\tNo.\tType\tRegistration No\tColour\n";

        for (int i = 0; i < parkedVehicles.Count; i++)
        {
            Vehicle vehicle = parkedVehicles[i];
            status += $"{i + 1}\t{vehicle.RegistrationNumber}\t{vehicle.Type}\t{vehicle.Color}\n";
        }

        return status;
    }

    public int CountVehiclesByType(string type)
    {
        return parkedVehicles.Count(vehicle => vehicle.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    public string GetRegistrationNumbersByPlateType(string plateType)
    {
        var filteredVehicles = plateType.ToLower() == "odd" ? parkedVehicles.Where(IsOdd) : parkedVehicles.Where(IsEven);
        return string.Join(", ", filteredVehicles.Select(vehicle => vehicle.RegistrationNumber));
    }

    private static bool IsOdd(Vehicle vehicle)
    {
        char lastDigit = vehicle.RegistrationNumber.Last();
        return char.IsDigit(lastDigit) && (int)char.GetNumericValue(lastDigit) % 2 != 0;
    }

    private static bool IsEven(Vehicle vehicle)
    {
        char lastDigit = vehicle.RegistrationNumber.Last();
        return char.IsDigit(lastDigit) && (int)char.GetNumericValue(lastDigit) % 2 == 0;
    }

    public string GetRegistrationNumbersByColor(string color)
    {
        var filteredVehicles = parkedVehicles.Where(vehicle => vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase));
        return string.Join(", ", filteredVehicles.Select(vehicle => vehicle.RegistrationNumber));
    }

    public string GetSlotNumbersByColor(string color)
    {
        var filteredSlots = parkedVehicles
            .Where(vehicle => vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(vehicle => parkedVehicles.IndexOf(vehicle) + 1);
        return string.Join(", ", filteredSlots);
    }

    public int GetSlotNumberByRegistrationNumber(string registrationNumber)
    {
        var foundVehicle = parkedVehicles.FirstOrDefault(vehicle =>
            vehicle.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));

        return foundVehicle != null ? parkedVehicles.IndexOf(foundVehicle) + 1 : -1;
    }
}
