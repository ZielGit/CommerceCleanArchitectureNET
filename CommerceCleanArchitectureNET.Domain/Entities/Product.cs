using CommerceCleanArchitectureNET.Domain.Common;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Entities
{
    public class Product : BaseEntity
    {
        private Product() { } // Para EF Core

        public Product(string name, Money price, int stock)
        {
            ValidateName(name);
            ValidatePrice(price);
            ValidateStock(stock);

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
            CreatedAt = DateTime.UtcNow;
        }

        public string Name { get; private set; }
        public Money Price { get; private set; }
        public int Stock { get; private set; }
        public bool IsActive { get; private set; } = true;

        public void UpdateStock(int quantity)
        {
            ValidateStock(quantity);
            Stock = quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty");

            if (name.Length > 200)
                throw new DomainException("Product name cannot exceed 200 characters");
        }

        private static void ValidatePrice(Money price)
        {
            if (price.Amount <= 0)
                throw new DomainException("Product price must be greater than zero");
        }

        private static void ValidateStock(int stock)
        {
            if (stock < 0)
                throw new DomainException("Product stock cannot be negative");
        }
    }
}
