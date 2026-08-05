using DigitalBanking.Domain.Common;
using System.Runtime.InteropServices;

namespace DigitalBanking.Domain.Entities
{
    public class Customer : BaseEntity
    {
        #region Properties

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        #endregion

        #region Constructors

        // required by ef core (making it private to make sure no data inconsistency)
        private Customer()
        { 
        }

        // to make sure we've all the correct data for customer(no partial data allowed)
        public Customer(string firstName, string lastName, string email, string phoneNumber, string passwordHash)
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

        public bool Activate()
        {
            return true;
        }

        public bool Deactivate()
        {
            return false;
        }

        public void UpdateProfile(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void ChangePassword(string updatedHash)
        {
            PasswordHash = updatedHash;
        }

        public static Customer Create(string firstName, string lastName, string email, string phoneNumber, string passwordHash)
        {
            return new Customer(firstName, lastName, email, phoneNumber, passwordHash);
        }

        #endregion
    }
}
