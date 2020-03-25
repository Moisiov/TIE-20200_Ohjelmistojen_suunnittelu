using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Enums;

namespace FJ.NUnitTests.TestData
{
    public static class PersonDummyDataProvider
    {
        public static Person DummyM1 => new Person
        {
            FirstName = "Matti",
            LastName = "Teppo",
            City = "Turku",
            Nationality = "FI",
            YearOfBirth = 1901,
            PersonGender = Gender.Man
        };
        
        public static Person DummyW1 => new Person
        {
            FirstName = "Neiti",
            LastName = "Etsivä",
            City = "Lontoo",
            Nationality = "GB",
            YearOfBirth = 1999,
            PersonGender = Gender.Woman
        };
        
        public static Person Create(int? seed = null)
        {
            var rand = seed.HasValue ? new Random(seed.Value) : new Random();
            
            return new Person
            {
                FirstName = TestUtils.GenerateRandomString(8),
                LastName = TestUtils.GenerateRandomString(16),
                PersonGender = TestUtils.GetRandomEnumValue<Gender>(),
                City = TestUtils.GenerateRandomString(12),
                Nationality = TestUtils.GenerateRandomString(12),
                YearOfBirth = rand.Next(1900, DateTime.Today.Year)
            };
        }
    }
}
