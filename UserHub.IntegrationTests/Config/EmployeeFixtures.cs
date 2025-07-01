using Bogus;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config.Helpers;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Config;

public sealed class EmployeeFixtures : BaseFixtures<Employee>
{
    public static Employee CreateFakeContact(int id)
    {
        var faker = new Faker<Employee>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.TypeRole, f => f.PickRandom<int>()) // 👈 Enum corretamente usado aqui
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password());

        var contact = faker.Generate();

        contact.Id = id;
        return contact;
    }

    public static EmployeeDto CreateFakeContactDto(int id = 0)
    {
        var faker = new Faker<EmployeeDto>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.TypeRole, f => f.PickRandom<TypeRole>()) // 👈 Enum corretamente usado aqui
            .RuleFor(u => u.Email, f => f.Internet.Email());

        var contact = faker.Generate();

        contact.Id = id;
        return contact;
    }

    public static ContactCreateDto CreateFakeContactCreateDto()
    {
        var faker = new Faker<ContactCreateDto>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.AreaCode, f => f.Random.Int(10, 99).ToString())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(u => u.Email, f => f.Person.Email);

        var contact = faker.Generate();

        return contact;
    }

    public static EmployeeDto CreateEmployeeDtoInvalidName()
    {
        var contact = CreateFakeContactDto();
        contact.Name = string.Empty;

        return contact;
    }

    public static EmployeeDto CreateEmployeeDtoInvalidEmail()
    {
        var contact = CreateFakeContactDto();
        contact.Email = FakerDefault.Random.String2(2, 2);

        return contact;
    }

    //public static EmployeeDto CreateEmployeeDtoInvalidRole()
    //{
    //    var contact = CreateFakeContactDto();
    //    contact.TypeRole = 99;

    //    return contact;
    //}    
}