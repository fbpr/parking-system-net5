using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystemNTT
{
    public class Vehicle
    {
        public string LicensePlate { get; set; }
        public string VehicleType { get; set; }
        public string Colour { get; set; }

        public Vehicle(string licensePlate, string colour, string vehicleType)
        {
            LicensePlate = licensePlate;
            VehicleType = vehicleType;
            Colour = colour;
        }
    }

    public class Lot
    {
        private readonly int _lotsAmount;
        private readonly Dictionary<int, Vehicle> _filledLot = new Dictionary<int, Vehicle>();

        public Lot(int lotsAmount)
        {
            _lotsAmount = lotsAmount;
            Console.WriteLine($"Created a parking lot with {_lotsAmount} slots");
        }

        public void Park(Vehicle vehicle)
        {
            if (_filledLot.Count >= _lotsAmount)
            {
                Console.WriteLine("Sorry, parking lot is full");
                return;
            }
            
            int lot = -1;
            for (int i = 1; i <= _lotsAmount; i++)
            {
                if (!_filledLot.ContainsKey(i))
                {
                    lot = i;
                    break;
                }
            }
            _filledLot[lot] = vehicle;
            Console.WriteLine($"Allocated slot number: {lot}");
        }
        
        public void Leave(int lot)
        {
            if (_filledLot.ContainsKey(lot))
            {
                _filledLot.Remove(lot);
                Console.WriteLine($"Slot number {lot} is free");
            }
            else
            {
                Console.WriteLine($"Not found");
            }
        }
        
        public void Status()
        {
            Console.WriteLine("Slot\t\tNo.\t\tType\tRegistration No\tColour");
            foreach (var slot in _filledLot)
            {
                Console.WriteLine($"{slot.Key}\t\t{slot.Value.LicensePlate}\t{slot.Value.VehicleType}\t{slot.Value.Colour}");
            }
        }
        
        public void TypeOfVehicles(string vehicleType)
        {
            var count = _filledLot.Values.Count(v => v.VehicleType.Equals(vehicleType, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(count > 0 ? count.ToString() : "Not found");
        }
        private bool IsOddPlate(string licensePlate)
        {
            if (int.TryParse(licensePlate.Last(char.IsDigit).ToString(), out int lastDigit))
            {
                return lastDigit % 2 == 1;
            }
            return false;
        }

        private bool IsEvenPlate(string licensePlate)
        {
            if (int.TryParse(new string(licensePlate.Where(char.IsDigit).Last().ToString()), out int lastDigit))
            {
                return lastDigit % 2 == 0;
            }
            return false;
        }

        public void RegistrationNumbersForEvenPlate()
        {
            var evenPlates = _filledLot.Values
                .Where(v => IsEvenPlate(v.LicensePlate))
                .Select(v => v.LicensePlate);

            Console.WriteLine(evenPlates.Any() ? string.Join(", ", evenPlates) : "Not found");
        }

        public void RegistrationNumbersForOddPlate()
        {
            var oddPlates = _filledLot.Values
                .Where(v => IsOddPlate(v.LicensePlate))
                .Select(v => v.LicensePlate);

            Console.WriteLine(oddPlates.Any() ? string.Join(", ", oddPlates) : "Not found");
        }

        public void RegistrationNumbersForColor(string colour)
        {
            Console.WriteLine(string.Join(", ", _filledLot.Values.Where(v => v.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase)).Select(v => v.LicensePlate)));
        }

        public void SlotNumbersForColor(string colour)
        {
            Console.WriteLine(string.Join(", ", _filledLot.Where(s => s.Value.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase)).Select(s => s.Key)));        
        }

        public void SlotNumberForRegistration(string licensePlate)
        {
            var slot = _filledLot.FirstOrDefault(s => s.Value.LicensePlate == licensePlate).Key;
            if (_filledLot.TryGetValue(slot, out var vehicle))
            {
                Console.WriteLine(slot);
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }
    }
    
    public class Program
    {
        private static void Main(string[] args)
        {
            Lot parkingLot = null;
            while (true)
            {
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;
                if (input == "exit") break;
                args = input.Split(' ');

                switch (args[0])
                {
                    case "create_parking_lot":
                        parkingLot = new Lot(int.Parse(args[1]));
                        break;
                    case "park":
                        parkingLot?.Park(new Vehicle(args[1], args[2], args[3]));
                        break;
                    case "leave":
                        parkingLot?.Leave(int.Parse(args[1]));
                        break;
                    case "type_of_vehicles":
                        parkingLot?.TypeOfVehicles(args[1]);
                        break;
                    case "registration_numbers_for_vehicles_with_odd_plate":
                        parkingLot?.RegistrationNumbersForOddPlate();
                        break;
                    case "registration_numbers_for_vehicles_with_even_plate":
                        parkingLot?.RegistrationNumbersForEvenPlate();
                        break;
                    case "registration_numbers_for_vehicles_with_colour":
                        parkingLot?.RegistrationNumbersForColor(args[1]);
                        break;
                    case "slot_numbers_for_vehicles_with_colour":
                        parkingLot?.SlotNumbersForColor(args[1]);
                        break;
                    case "slot_number_for_registration_number":
                        parkingLot?.SlotNumberForRegistration(args[1]);
                        break;
                    case "status":
                        parkingLot?.Status();
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }
}