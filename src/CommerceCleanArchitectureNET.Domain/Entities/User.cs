using CommerceCleanArchitectureNET.Domain.Common;
using CommerceCleanArchitectureNET.Domain.Exceptions;

namespace CommerceCleanArchitectureNET.Domain.Entities
{
    public class User : BaseEntity
    {
        private User() { }

        public User(string email, string passwordHash, string firstName, string lastName, string role = "User")
        {
            ValidateEmail(email);
            ValidateName(firstName, nameof(firstName));
            ValidateName(lastName, nameof(lastName));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("Password hash cannot be empty");

            Id = Guid.NewGuid();
            Email = email.Trim().ToLowerInvariant();
            PasswordHash = passwordHash;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Role = role;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Role { get; private set; } = "User";
        public bool IsActive { get; private set; }

        public string FullName => $"{FirstName} {LastName}";

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainException("Password hash cannot be empty");

            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email cannot be empty");

            if (email.Length > 256)
                throw new DomainException("Email cannot exceed 256 characters");

            if (!email.Contains('@'))
                throw new DomainException("Email format is invalid");
        }

        private static void ValidateName(string name, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException($"{fieldName} cannot be empty");

            if (name.Length > 100)
                throw new DomainException($"{fieldName} cannot exceed 100 characters");
        }
    }
}
