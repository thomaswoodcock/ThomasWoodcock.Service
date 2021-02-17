using System;

using ThomasWoodcock.Service.Domain.Accounts;
using ThomasWoodcock.Service.Domain.Accounts.FailureReasons;
using ThomasWoodcock.Service.Domain.SharedKernel.Results;

using Xunit;

namespace ThomasWoodcock.Service.Domain.UnitTests.Accounts
{
    public sealed class AccountTests
    {
        public sealed class Create : IClassFixture<Fixture>
        {
            private readonly Fixture _fixture;

            public Create(Fixture fixture)
            {
                this._fixture = fixture;
            }

            [Fact]
            public void EmptyId_Create_ReturnsFailedResult()
            {
                // Arrange Act
                IResult<Account> result = Account.Create(Guid.Empty, "Test Name", "test@test.com", "TestPassword123");

                // Assert
                Assert.True(result.IsFailed);
                Assert.False(result.IsSuccessful);
                Assert.IsType<InvalidAccountIdFailure>(result.FailureReason);
                Assert.Null(result.Value);
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            public void EmptyName_Create_ReturnsFailedResult(string name)
            {
                // Arrange Act
                IResult<Account> result =
                    Account.Create(this._fixture.AccountId, name, "test@test.com", "TestPassword123");

                // Assert
                Assert.True(result.IsFailed);
                Assert.False(result.IsSuccessful);
                Assert.IsType<InvalidAccountNameFailure>(result.FailureReason);
                Assert.Null(result.Value);
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("test@test.")]
            [InlineData("www.test.com")]
            [InlineData("test^test.com")]
            public void InvalidEmailAddress_Create_ReturnsFailedResult(string emailAddress)
            {
                // Arrange Act
                IResult<Account> result =
                    Account.Create(this._fixture.AccountId, "Test Name", emailAddress, "TestPassword123");

                // Assert
                Assert.True(result.IsFailed);
                Assert.False(result.IsSuccessful);
                Assert.IsType<InvalidAccountEmailFailure>(result.FailureReason);
                Assert.Null(result.Value);
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            public void EmptyPassword_Create_ReturnsFailedResult(string password)
            {
                // Arrange Act
                IResult<Account> result =
                    Account.Create(this._fixture.AccountId, "Test Name", "test@test.com", password);

                // Assert
                Assert.True(result.IsFailed);
                Assert.False(result.IsSuccessful);
                Assert.IsType<InvalidAccountPasswordFailure>(result.FailureReason);
                Assert.Null(result.Value);
            }

            [Fact]
            public void ValidArguments_Create_ReturnsSuccessfulResult()
            {
                // Arrange Act
                IResult<Account> result = Account.Create(this._fixture.AccountId, "Test Name", "test@test.com",
                    "TestPassword123");

                // Assert
                Assert.True(result.IsSuccessful);
                Assert.False(result.IsFailed);
                Assert.Null(result.FailureReason);
                Assert.Equal(this._fixture.AccountId, result.Value.Id);
                Assert.Equal("Test Name", result.Value.Name);
                Assert.Equal("test@test.com", result.Value.EmailAddress.ToString());
                Assert.Equal("TestPassword123", result.Value.Password);
            }
        }

        public sealed class Fixture
        {
            internal readonly Guid AccountId = new("E27DDA41-E981-466C-A998-3DB885521630");
        }
    }
}
