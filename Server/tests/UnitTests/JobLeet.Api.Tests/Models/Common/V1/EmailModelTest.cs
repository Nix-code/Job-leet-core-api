﻿using JobLeet.WebApi.JobLeet.Api.Models.Common.V1;

namespace UnitTests.JobLeet.Api.Tests.Models.Common.V1
{
    public class EmailModelTest
    {
        [Fact]
        public void EmailModel_ShouldInstantiateSuccessfully ()
        {
            // Arrange
            EmailModel emailModel = new EmailModel
            {
                EmailType = EmailCategory.Personal,
                EmailAddress = "helloworld@email.com"
            };

            // Assert
            Assert.Equal(emailModel.EmailType, EmailCategory.Personal);
            Assert.Equal(emailModel.EmailAddress, "helloworld@email.com");

        }

        [Fact]
        public void EmailModel_ShouldValidateAnnotations()
        {
            EmailModel emailModel = new EmailModel
            {
                EmailType = EmailCategory.Personal,
                EmailAddress = null
            };

            var ValidateResults = ValidateAnnotationHelper.ValidateModel(emailModel);

            Assert.NotNull(emailModel.EmailType);
            Assert.Null(emailModel.EmailAddress);
            Assert.False(ValidateResults.Any(v => v.MemberNames.Contains("EmailType") && v.ErrorMessage.Contains("Email Type is required")), "Email Type should be marked as required");
            Assert.True(ValidateResults.Any(v => v.MemberNames.Contains("EmailAddress") && v.ErrorMessage.Contains("Email Address is required")), "Email Address should be marked as required");
        }
    }
}
