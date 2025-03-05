using Bogus;
using FIAP.TechChallenge.ContactsConsult.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.ContactsConsult.Domain.Entities;
using FIAP.TechChallenge.ContactsConsult.IntegrationTest.Config.Helpers;

namespace FIAP.TechChallenge.ContactsConsult.IntegrationTest.Config;

public sealed class ContactFixtures : BaseFixtures<Contact>
{
    public static Contact CreateFakeContact(int id)
    {
        var faker = new Faker<Contact>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.AreaCode, f => f.Random.Int(10, 99).ToString())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(u => u.Email, f => f.Person.Email);

        var contact = faker.Generate();

        contact.Id = id;
        return contact;
    }

    public static ContactDto CreateFakeContactDto(int id = 0)
    {
        var faker = new Faker<ContactDto>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.AreaCode, f => f.Random.Int(10, 99).ToString())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(u => u.Email, f => f.Person.Email);

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

    public static ContactDto CreateContractDtoInvalidName()
    {
        var contact = CreateFakeContactDto();
        contact.Name = string.Empty;

        return contact;
    }

    public static ContactDto CreateContractDtoInvalidEmail()
    {
        var contact = CreateFakeContactDto();
        contact.Email = FakerDefault.Random.String2(2, 2);

        return contact;
    }

    public static ContactDto CreateContractDtoInvalidPhoneNumber()
    {
        var contact = CreateFakeContactDto();
        contact.Phone = FakerDefault.Random.String2(2, 2);

        return contact;
    }

    public static ContactDto CreateContractDtoInvalidAreaCode()
    {
        var contact = CreateFakeContactDto();
        contact.AreaCode = FakerDefault.Random.String2(1, 1);

        return contact;
    }
}