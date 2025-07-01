using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config;
using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations
{
    public class BaseContactDtoValidation
    {
        public readonly Random RandomId;
        public BaseContactDtoValidation()
        {
            RandomId = new Random();
        }

        public static List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }

        [Fact]
        public void ContractDtoValidationFormatSuccess()
        {
            var basicContact = ClientFixtures.CreateFakeContactDto(RandomId.Next(999999999));

            var validationResults = ValidateModel(basicContact);

            Assert.Empty(validationResults);
        }

        public static IEnumerable<object[]> GetBasicContactInvalidData()
        {
            yield return new object[] { ClientFixtures.CreateContractDtoInvalidName() };
            yield return new object[] { ClientFixtures.CreateContractDtoInvalidEmail() };
            yield return new object[] { ClientFixtures.CreateContractDtoInvalidPhoneNumber() };
            yield return new object[] { ClientFixtures.CreateContractDtoInvalidAreaCode() };
        }

        [Theory]
        [MemberData(nameof(GetBasicContactInvalidData))]
        public void ContractDtoValidationEachDataFaill(ClientDto basicContact)
        {
            var validationResults = ValidateModel(basicContact);

            Assert.NotEmpty(validationResults);
        }
    }
}
