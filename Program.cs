using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = null;

            while (true)
            {
                string input = Console.ReadLine();
                string[] tokens = input.Split(' ');

                switch (tokens[0])
                {
                    case "create_parking_lot":
                        int totalSlots = int.Parse(tokens[1]);
                        parkingLot = new ParkingLot(totalSlots);
                        Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                        break;

                    case "park":
                        if (parkingLot != null && tokens.Length == 4)
                        {
                            string registrationNumber = tokens[1];
                            string color = tokens[2];
                            string vehicleType = tokens[3];

                            Vehicle vehicle = new Vehicle(registrationNumber, color, vehicleType);

                            int slotNumber = parkingLot.ParkVehicle(vehicle);
                            if (slotNumber == -1)
                                Console.WriteLine("Sorry, parking lot is full");
                            else
                                Console.WriteLine($"Allocated slot number: {slotNumber}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                        }
                        break;

                    case "leave":
                        if (parkingLot != null && tokens.Length == 2)
                        {
                            int slotNumber = int.Parse(tokens[1]);
                            parkingLot.Leave(slotNumber);
                            Console.WriteLine($"Slot number {slotNumber} is free");
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                        }
                        break;

                    case "status":
                        if (parkingLot != null)
                            parkingLot.PrintStatus();
                        else
                            Console.WriteLine("Parking lot has not been created yet");
                        break;

                    case "type_of_vehicles":
                        if (parkingLot != null && tokens.Length == 2)
                        {
                            string vehicleType = tokens[1];
                            int count = parkingLot.GetVehicleCountByType(vehicleType);
                            Console.WriteLine(count);
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                        }
                        break;

                    case "registration_numbers_for_vehicles_with_odd_plate":
                        if (parkingLot != null)
                            parkingLot.PrintRegistrationNumbersByPlateType("odd");
                        else
                            Console.WriteLine("Parking lot has not been created yet");
                        break;

                    case "registration_numbers_for_vehicles_with_even_plate":
                        if (parkingLot != null)
                            parkingLot.PrintRegistrationNumbersByPlateType("even");
                        else
                            Console.WriteLine("Parking lot has not been created yet");
                        break;

                    case "registration_numbers_for_vehicles_with_colour":
                        if (parkingLot != null && tokens.Length == 2)
                        {
                            string color = tokens[1];
                            parkingLot.PrintRegistrationNumbersByColor(color);
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                        }
                        break;

                    case "slot_numbers_for_vehicles_with_colour":
                        if (parkingLot != null && tokens.Length == 2)
                        {
                            string color = tokens[1];
                            parkingLot.PrintSlotNumbersByColor(color);
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                        }
                        break;

                    case "slot_number_for_registration_number":
                        if (parkingLot != null && tokens.Length == 2)
                        {
                            string registrationNumber = tokens[1];
                            int slotNumber = parkingLot.GetSlotNumberByRegistrationNumber(registrationNumber);
                            if (slotNumber != -1)
                                Console.WriteLine(slotNumber);
                            else
                                Console.WriteLine("Not found");
                        }
                        else
                        {
                            Console.WriteLine("Invalid command");
                        }
                        break;

                    case "exit":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }

    class ParkingLot
    {
        private int totalSlots;
        private Dictionary<int, Vehicle> parkedVehicles;

        public ParkingLot(int totalSlots)
        {
            this.totalSlots = totalSlots;
            this.parkedVehicles = new Dictionary<int, Vehicle>();
        }

        public int ParkVehicle(Vehicle vehicle)
        {
            for (int i = 1; i <= totalSlots; i++)
            {
                if (!parkedVehicles.ContainsKey(i))
                {
                    parkedVehicles[i] = vehicle;
                    return i;
                }
            }
            return -1;
        }

        public void Leave(int slotNumber)
        {
            if (parkedVehicles.ContainsKey(slotNumber))
                parkedVehicles.Remove(slotNumber);
        }

        public void PrintStatus()
        {
            Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
            foreach (var entry in parkedVehicles.OrderBy(entry => entry.Key))
            {
                Console.WriteLine($"{entry.Key}\t{entry.Value.RegistrationNumber}\t{entry.Value.VehicleType}\t{entry.Value.Color}");
            }
        }

        public int GetVehicleCountByType(string vehicleType)
        {
            return parkedVehicles.Count(vehicle => vehicle.Value.VehicleType.Equals(vehicleType, StringComparison.OrdinalIgnoreCase));
        }

        public void PrintRegistrationNumbersByPlateType(string plateType)
        {
            foreach (var entry in parkedVehicles.OrderBy(entry => entry.Key))
            {
                string registrationNumber = entry.Value.RegistrationNumber;
                char lastChar = registrationNumber[registrationNumber.Length - 1];
                if ((plateType == "odd" && (lastChar - '0') % 2 != 0) || (plateType == "even" && (lastChar - '0') % 2 == 0))
                    Console.Write($"{registrationNumber}, ");
            }
            Console.WriteLine();
        }

        public void PrintRegistrationNumbersByColor(string color)
        {
            foreach (var entry in parkedVehicles.OrderBy(entry => entry.Key))
            {
                if (entry.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                    Console.Write($"{entry.Value.RegistrationNumber}, ");
            }
            Console.WriteLine();
        }

        public void PrintSlotNumbersByColor(string color)
        {
            foreach (var entry in parkedVehicles.OrderBy(entry => entry.Key))
            {
                if (entry.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                    Console.Write($"{entry.Key}, ");
            }
            Console.WriteLine();
        }

        public int GetSlotNumberByRegistrationNumber(string registrationNumber)
        {
            foreach (var entry in parkedVehicles)
            {
                if (entry.Value.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase))
                    return entry.Key;
            }
            return -1;
        }
    }

    class Vehicle
    {
        public string RegistrationNumber { get; }
        public string Color { get; }
        public string VehicleType { get; }

        public Vehicle(string registrationNumber, string color, string vehicleType)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            VehicleType = vehicleType;
        }
    }
}
