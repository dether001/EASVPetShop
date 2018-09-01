using Easv.PetShop.Core.Application_Service.Service;
using Easv.PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easv.PetShop.ConsoleApp
{
    class Printer : IPrinter
    {
        private IPetService _petService;

        

        public Printer(IPetService petService)
        {
            _petService = petService;
        }

        #region Data Init region, plan B if FakeDB fails
        private void InitData()
        {
           Pet pet1 = new Pet
           {
               Name = "Brandy",
               Type = "Dog",
               Birthday = new DateTime(2011, 04, 22),
               SoldDate = new DateTime(2015, 12, 20),
               Color = "Brown",
               PreviousOwner = "From Animal Shelter",
               Price = 12
           };
            _petService.AddPet(pet1);
            Pet pet2 = new Pet
            {
                Name = "Hudini",
                Type = "Hamster",
                Birthday = new DateTime(2008, 04, 26),
                SoldDate = new DateTime(2008, 05, 13),
                Color = "White",
                PreviousOwner = "Johny Bravo",
                Price = 4
            };
            _petService.AddPet(pet2);
            Pet pet3 = new Pet
            {
                Name = "Betsy",
                Type = "Goat",
                Birthday = new DateTime(2013, 11, 20).Date,
                SoldDate = new DateTime(2018, 08, 18).Date,
                Color = "Black",
                PreviousOwner = "Attila Reiner",
                Price = 850
            };
            _petService.AddPet(pet3);
            Pet pet4 = new Pet
            {
                Name = "LitBoi",
                Type = "Phoenix",
                Birthday = new DateTime(2001, 09, 07).Date,
                SoldDate = new DateTime(2010, 12, 11).Date,
                Color = "fire",
                PreviousOwner = "Albus Percival Wulfrick Brian Dumbledore",
                Price = 1337
            };
            _petService.AddPet(pet4);
        }
        #endregion

        #region UI Init
        public void StartUI()
        {
            InitData();
            string[] menu = new string[]
        {
            "Show list of all pets",
            "Search by type",
            "Create new pet",
            "Delete a pet",
            "Update a pet",
            "Sort pets by price",
            "Get 5 cheapest available pets",
            "Exit menu"
        };


            Console.WriteLine("Please select a menu item: \n");
            DisplayMenu(menu);
            var menuSelected = GetUserInputInt("\nYou selected: ", 1, 8);

            while (menuSelected != 8)
            {
                switch (menuSelected)
                {
                    case 1:
                        PrintPets(_petService.GetPets());
                        break;
                    case 2:
                        var petsType = SearchByType(GetUserInput("Please choose a type: "));
                        PrintPets(petsType);
                        break;
                    case 3:
                        var name = GetUserInput("Name:");
                        var type = GetUserInput("Type:");
                        var birthday = GetUserInputDate("Birth date: ");
                        var soldDate = GetUserInputDate("Sold date: ");
                        var color = GetUserInput("Color:");
                        var prevOwner = GetUserInput("Previous owner:");
                        var price = GetUserInputDouble("Price:");
                        CreatePet(name, type, birthday, soldDate, color, prevOwner, price);
                        break;
                    case 4:
                        DeletePet(GetIdBorders("Id:"));
                        break;
                    case 5: 
                        var updateId = GetIdBorders("Id:");
                        updatePet(updateId);           
                        break;
                    case 6:
                        Console.WriteLine("Sorted list of pets by price: \n");
                        PrintPets(SortByPrice());
                        break;
                    case 7:
                        Console.WriteLine("The 5 cheapest pets: \n");
                        PrintPets(Get5CheapestPets());
                        break;
                    default:
                        break;
                }
                Console.WriteLine("Please select a menu item: \n");
                DisplayMenu(menu);
                menuSelected = GetUserInputInt("\nYour selected:", 1, 8);
            }
            Console.WriteLine("\nBye-bye!");
        }
        #endregion

        void PrintPets(List<Pet> pets)
        {
            if (pets.Count != 0)
            {
                foreach (var pet in pets)
                {
                    Console.WriteLine($"Id: {pet.Id}\n    Name: {pet.Name}\n    Type: {pet.Type}\n    Birthday: {pet.Birthday}\n    SoldDate: {pet.SoldDate}\n    Color: {pet.Color}\n    Previous owner: {pet.PreviousOwner}\n    Price: {pet.Price} \n");
                }
            }
            else
            {
                Console.WriteLine("There are no pets in our database \n");
            }
            
        }

        List<Pet> SearchByType(string type)
        {
            var foundPets = _petService.SearchByType(type.ToLower());
            if (foundPets.Count == 0)
            {
                Console.WriteLine("\n We couldn't find any pets matching your type. \n");
            }
            return foundPets;
        }

        void CreatePet(string name, string type, DateTime birthday, DateTime soldDate, string color, string previousOwner, double price)
        {
            var petToCreate = _petService.NewPet(name, type, birthday, soldDate, color, previousOwner, price);
            if (_petService.AddPet(petToCreate) != null)
            {
                Console.WriteLine("Data succesfully registered. \n");
            }
        }

        void DeletePet(int id)
        {
            if (_petService.DeletePet(id) == null)
            {
                Console.WriteLine("There's no pet with such ID.\n");
            }
            else
            {
                Console.WriteLine("Data succesfully deleted. \n");
            }
        }

        private void updatePet(int idFound)
        {
            if (idFound != 0)
            {
                var updateName = GetUserInput("Name:");
                var updateType = GetUserInput("Type:");
                var updateBirthday = GetUserInputDate("Birthday: ");
                var updateSoldDate = GetUserInputDate("Sold date: ");
                var updateColor = GetUserInput("Color:");
                var updatePrevOwner = GetUserInput("Previous owner:");
                var updatePrice = GetUserInputDouble("Price:");
                var updatedPet = _petService.NewPet(updateName, updateType, updateBirthday, updateSoldDate, updateColor, updatePrevOwner, updatePrice);
                updatedPet.Id = idFound;
                if (_petService.UpdatePet(updatedPet) != null)
                {
                    Console.WriteLine("The data has been updated. \n");
                }
            }
        }

        List<Pet> SortByPrice()
        {
            var sortedByPrice = _petService.SortByPrice();
            return sortedByPrice;
        }

        List<Pet> Get5CheapestPets()
        {
            var cheapest5Pets = _petService.Get5CheapestPets();
            return cheapest5Pets;
        }

        int GetIdBorders(string text)
        {
            var pets = _petService.GetPets();
            if (pets.Count == 0)
            {
                Console.WriteLine("There are no pets in our database.\n");
                return 0;

            }
            var lowestPetId = pets.FirstOrDefault().Id;
            var highestPetId = pets.OrderByDescending(pet => pet.Id).FirstOrDefault().Id;
            var selection = GetUserInputInt(text, lowestPetId, highestPetId);

            return selection;
        }

        void DisplayMenu(string[] menu)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {menu[i]}");
            }
        }

        DateTime GetUserInputDate(string toInput)
        {
            DateTime date;
            Console.WriteLine(toInput);
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Please type in a valid date (Format: YYYY/MM/DD)!");
            }
            return date;
        }

        string GetUserInput(string toInput)
        {
            Console.WriteLine(toInput);
            var input = Console.ReadLine();
            while (input.Length == 0)
            {
                Console.WriteLine("Please enter at least one character!");
                Console.WriteLine(toInput);
                input = Console.ReadLine();
            }
            return input;
        }

        double GetUserInputDouble(string toInput)
        {
            double selection;
            Console.WriteLine(toInput);
            while (!double.TryParse(Console.ReadLine(), out selection))
            {
                Console.WriteLine("Please enter a number!");
            }
            return selection;
        }

        int GetUserInputInt(string toInput, int lowerBorder, int upperBorder)
        {
            int selection;
            Console.WriteLine(toInput);
            while (!int.TryParse(Console.ReadLine(), out selection) || selection < lowerBorder || selection > upperBorder)
            {
                Console.WriteLine($"\nPlease enter a number between {lowerBorder} and {upperBorder}!\n");
                Console.WriteLine(toInput);
            }
            return selection;
        }
    }
}
