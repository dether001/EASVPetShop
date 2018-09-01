using Easv.PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Easv.PetShop.Core.Application_Service.Service;
using Easv.PetShop.Core.Application_Service;
using Easv.PetShop.Infrastructure.Data.Repositories;

namespace Easv.PetShop.Infrastructure.Data
{
    public class FakeDB
    {
        public static int PetId = 1;
        private static PetRepository petRepository = new PetRepository();
        private static IPetService petService = new PetService(petRepository);
        public static IEnumerable<Pet> ListOfPets = new List<Pet>();
        public static void InitDB()
        {
            petService.NewPet("Dingo", "Dog", new DateTime(2000, 10, 10), new DateTime(2010, 10, 10), "White", "Hans", 500.5);
            petService.NewPet("Buller", "Dog", new DateTime(2005, 03, 07), new DateTime(2011, 10, 10), "Black", "Martin", 1500.7);
            petService.NewPet("Wuff", "Dog", new DateTime(2006, 05, 05), new DateTime(2015, 10, 10), "Grey", "Lukas", 800.5);
        }
    }

}
