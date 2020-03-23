using System;
using FJ.DomainObjects.Enums;

namespace FJ.DomainObjects
{
    public class Person
    {   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public Gender PersonGender { get; set; }
        public string City { get; set; }
        public string Nationality { get; set; }
        public int? YearOfBirth { get; set; }
    }
}
