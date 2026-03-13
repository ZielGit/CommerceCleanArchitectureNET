using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Tests.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateMoney()
        {
            // Arrange & Act
            var money = new Money(100.50m, "USD");

            // Assert
            Assert.Equal(100.50m, money.Amount);
            Assert.Equal("USD", money.Currency);
        }

        [Fact]
        public void Constructor_WithNegativeAmount_ShouldThrowDomainException()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(
                () => new Money(-100, "USD"));

            Assert.Equal("Amount cannot be negative", exception.Message);
        }

        [Fact]
        public void Add_WithSameCurrency_ShouldReturnSum()
        {
            // Arrange
            var money1 = new Money(100, "USD");
            var money2 = new Money(50, "USD");

            // Act
            var result = money1.Add(money2);

            // Assert
            Assert.Equal(150, result.Amount);
            Assert.Equal("USD", result.Currency);
        }

        [Fact]
        public void Add_WithDifferentCurrency_ShouldThrowDomainException()
        {
            // Arrange
            var money1 = new Money(100, "USD");
            var money2 = new Money(50, "EUR");

            // Act & Assert
            var exception = Assert.Throws<DomainException>(
                () => money1.Add(money2));

            Assert.Equal("Cannot add different currencies", exception.Message);
        }

        [Fact]
        public void OperatorPlus_ShouldAddMoney()
        {
            // Arrange
            var money1 = new Money(100, "USD");
            var money2 = new Money(50, "USD");

            // Act
            var result = money1 + money2;

            // Assert
            Assert.Equal(150, result.Amount);
        }
    }
}
