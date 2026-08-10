using DigitalBanking.Domain.Common;

namespace DigitalBanking.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        #region Properties

        public Guid CustomerId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public DateTime? ExpiresOn { get; private set; }
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; private set; }
        public bool IsRevoked => RevokedOn.HasValue;

        #endregion

        #region Constructors

        private RefreshToken()
        { 
        }

        private RefreshToken(Guid customerId, string token, DateTime expiresOn, DateTime createdOn)
        {
            CustomerId = customerId;
            Token = token;
            ExpiresOn = expiresOn;
            CreatedOn = createdOn;
        }

        #endregion

        #region Behaviors

        public void Revoke(DateTime currentDateTime) 
        {
            if (IsRevoked)
                return;
            RevokedOn = currentDateTime;
        }

        public bool IsExpired(DateTime currentDateTime)
        {
            return currentDateTime >= ExpiresOn ? true : false;
        }

        public bool IsValid(DateTime currentDateTime)
        {
            return !IsRevoked && !IsExpired(currentDateTime);
        }

        public static RefreshToken Create(Guid customerId, string token, DateTime expiresOn, DateTime createdOn)
        {
            return new RefreshToken(customerId, token, expiresOn, createdOn);
        }

        public void UpdateRefreshToken(string token, DateTime? expiresOn)
        {
            Token = token;
            ExpiresOn = expiresOn;
        }

        #endregion
    }
}
