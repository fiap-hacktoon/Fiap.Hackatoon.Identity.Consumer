using FIAP.TechChallenge.ContactsConsult.Application.Applications;
using FIAP.TechChallenge.ContactsConsult.Domain.Entities;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Services;
using FIAP.TechChallenge.ContactsConsult.UnitTests;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ContactsConsult.UnitTests.ApplicationTests
{
    public class ContactApplicationTests
    {
        private Mock<IContactService> _contactServiceMock;
        private Mock<ILogger<ContactApplication>> _loggerMock;
        private ContactApplication _contactApplication;

        public ContactApplicationTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _loggerMock = new Mock<ILogger<ContactApplication>>();
            _contactApplication = new ContactApplication(_contactServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllContactsAsyncRepositorySuccess()
        {
            var contacts = CommonTestData.GetContacListObject();

            _contactServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(contacts);

            var result = await _contactApplication.GetAllContactsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _contactServiceMock.Verify(service => service.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllContactsAsyncRepositoryFail()
        {
            _contactServiceMock.Setup(service => service.GetAllAsync()).ThrowsAsync(new Exception("ERRO-SIMULADO"));

            var result = await _contactApplication.GetAllContactsAsync();

            Assert.Null(result);
            _contactServiceMock.Verify(service => service.GetAllAsync(), Times.Once);
            _loggerMock.Equals("Ocorreu um erro na consulta de todos os contatos. Erro: ERRO-SIMULADO");
        }

        [Fact]
        public async Task GetContactByIdAsyncRepositorySuccess()
        {
            var contactId = 1;
            var contact = CommonTestData.GetContactObject();
            _contactServiceMock.Setup(service => service.GetByIdAsync(contactId)).ReturnsAsync(contact);

            var result = await _contactApplication.GetContactByIdAsync(contactId);

            Assert.NotNull(result);
            Assert.Equal(contactId, result.Id);
            _contactServiceMock.Verify(service => service.GetByIdAsync(contact.Id), Times.Once);
        }

        [Fact]
        public async Task GetContactByIdAsyncRepositoryFail()
        {
            var contactId = 1;
            _contactServiceMock.Setup(service => service.GetByIdAsync(contactId)).ReturnsAsync((Contact)null);

            var result = await _contactApplication.GetContactByIdAsync(contactId);

            Assert.Null(result);
            _contactServiceMock.Verify(service => service.GetByIdAsync(contactId), Times.Once);
        }

        [Fact]
        public async Task GetContactsByAreaCodeAsyncRepositorySuccess()
        {
            var areaCode = "11";
            var contactList = CommonTestData.GetContacListObject();

            _contactServiceMock.Setup(service => service.GetByAreaCodeAsync(areaCode)).ReturnsAsync(contactList);

            var result = await _contactApplication.GetContactsByAreaCodeAsync(areaCode);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _contactServiceMock.Verify(service => service.GetByAreaCodeAsync(areaCode), Times.Once);
        }
    }
}
