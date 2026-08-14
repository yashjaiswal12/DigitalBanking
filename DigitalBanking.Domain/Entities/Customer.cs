using DigitalBanking.Domain.Common;
using DigitalBanking.Domain.Enums.Security;
using DigitalBanking.Domain.Exceptions;

namespace DigitalBanking.Domain.Entities
{
    public class Customer : AuditableEntity
    {
        private const int MaxFailedLoginAttempts = 5;

        #region Properties

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public Roles Role { get; private set; } = Roles.Customer;
        public string SecurityStamp { get; private set; } = Guid.NewGuid().ToString();
        public int TokenVersion { get; private set; } = 1;
        public int FailedLoginAttempts { get; private set; } = 0;
        public bool IsLocked { get; private set; } = false;
        public DateTime? LastLoginAt { get; private set; }
        public DateTime? LastFailedLoginAt { get; private set; }
        public byte[] RowVersion { get; private set; } = [];

        #endregion

        #region Constructors

        // required by ef core (making it private to make sure no data inconsistency)
        private Customer()
        { 
        }

        // to make sure we've all the correct data for customer(no partial data allowed)
        private Customer(string firstName, string lastName, string email, string phoneNumber, string passwordHash)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            PasswordHash = passwordHash;
            IsActive = true;
        }

        #endregion

        #region Behaviors

        public void UpdateTokenVersion()
        {
            TokenVersion++;
        }

        public void RecordSuccessfulLogin()
        {
            FailedLoginAttempts = 0;
            LastLoginAt = DateTime.UtcNow;
        }

        public void RecordFailedLogin()
        {
            FailedLoginAttempts++;
            LastFailedLoginAt = DateTime.UtcNow;

            if (FailedLoginAttempts >= MaxFailedLoginAttempts)
                LockAccount();
        }

        public void LockAccount()
        {
            IsLocked = true;
        }

        public void UnlockAccount()
        {
            IsLocked = false;
            FailedLoginAttempts = 0;
        }
        
        public void UpdateSecurityStamp()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public void Activate()
        {
            if (IsDeleted)
                return;

            IsActive = true;
        }

        public void Deactivate()
        {
            if (IsDeleted)
                return;

            IsActive = false;
        }

        public void Delete()
        {
            if (IsDeleted)
                return;

            IsDeleted = true;
        }

        public void UpdateProfile(string firstName, string lastName, string email, string phone)
        {
            if (IsDeleted)
                throw new DomainException("Customer already deleted.");

            ValidateFirstName(firstName);  
            FirstName = firstName;

            ValidateLastName(lastName);
            LastName = lastName;

            ValidateEmail(email);
            Email = NormalizeEmail(email);

            PhoneNumber = NormalizePhone(phone);
        }

        public void ChangePassword(string updatedHash)
        {
            if (IsDeleted)
                throw new DomainException("Customer already deleted.");

            ValidatePassword(updatedHash);
            PasswordHash = updatedHash;
            
            UpdateSecurityStamp();
            UpdateTokenVersion();
        }

        public static Customer Create(string firstName, string lastName, string email, string phone, string passwordHash)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            email = NormalizeEmail(email);
            ValidatePhone(phone);
            phone = NormalizePhone(phone);
            ValidatePassword(passwordHash);

            return new Customer(firstName, lastName, email, phone, passwordHash);
        }

        private static string NormalizeEmail(string email)
        {
            return email.Trim().ToLowerInvariant();
        }

        private static string NormalizePhone(string phone)
        {
            return phone.Replace(" ","").Replace("+91","").Replace("-","");
        }

        private static void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name is required");
        }

        private static void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is required");
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email is required");
        }

        private static void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new DomainException("Phone number is required");
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password is required");
        }

        #endregion
    }
}
