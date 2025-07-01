using Bogus;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config.Helpers;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Config;

public sealed class ClientFixtures : BaseFixtures<Client>
{
    public static Client CreateFakeContact(int id)
    {
        var faker = new Faker<Client>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Document, f => f.Random.ReplaceNumbers("###########")) // CPF/CNPJ fictício
            .RuleFor(u => u.Birth, f => DateOnly.FromDateTime(f.Date.Past(40, DateTime.Today.AddYears(-18)))) // 18-60 anos
            .RuleFor(u => u.Creation, f => f.Date.Recent(10)) // últimos 10 dias
            .RuleFor(u => u.Email, f => f.Person.Email);

        var contact = faker.Generate();

        contact.Id = id;
        return contact;
    }

    public static ClientDto CreateFakeContactDto(int id = 0)
    {
        var faker = new Faker<ClientDto>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Document, f => f.Random.Int(10, 99).ToString())
            .RuleFor(u => u.Birth, f => DateOnly.FromDateTime(f.Date.Past(40, DateTime.Today.AddYears(-18)))) // 18-60 anos
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

    public static ClientDto CreateContractDtoInvalidName()
    {
        var contact = CreateFakeContactDto();
        contact.Name = string.Empty;

        return contact;
    }

    public static ClientDto CreateContractDtoInvalidEmail()
    {
        var contact = CreateFakeContactDto();
        contact.Email = FakerDefault.Random.String2(2, 2);

        return contact;
    }

    public static ClientDto CreateContractDtoInvalidPhoneNumber()
    {
        var contact = CreateFakeContactDto();
        contact.Birth = DateOnly.FromDateTime(FakerDefault.Date.Past(40, DateTime.Today.AddYears(-18)));

        return contact;
    }

    public static ClientDto CreateContractDtoInvalidAreaCode()
    {
        var contact = CreateFakeContactDto();
        contact.Document = FakerDefault.Random.String2(1, 1);

        return contact;
    }
}