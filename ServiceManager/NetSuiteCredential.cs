using com.celigo.net.ServiceManager.Exceptions;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Contains the NetSuite user Credentials.
    /// </summary>
    [System.Serializable]
    public class NetSuiteCredential
    {
        /// <summary>
        /// Gets or sets the NetSuite account number.
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the user's registered email address.
        /// </summary>
        /// <value>The user's registered email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        /// <value>The  user's password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's role id.
        /// </summary>
        /// <value>The user's role id.</value>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the no of seats for this credential.
        /// </summary>
        /// <value>The no of seats.</value>
        public int NoOfSeats { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteCredential"/> class.
        /// </summary>
        public NetSuiteCredential() : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteCredential"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="account">The account.</param>
        /// <param name="roleId">The role id.</param>
        public NetSuiteCredential(string email, string password, string account, string roleId) :
            this(email, password, account, roleId, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteCredential"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="account">The account.</param>
        /// <param name="roleId">The role id.</param>
        /// <param name="noOfSeats">The no of seats.</param>
        public NetSuiteCredential(string email, string password, string account, string roleId, int noOfSeats)
        {
            Email		= email;
            Password	= password;
            Account		= account;
            RoleId		= roleId;
            NoOfSeats	= noOfSeats;
        }

        [System.NonSerialized]
        private Passport _passport;

        internal Passport GetPassport()
        {
            if (Email == null || Account == null || Password == null)
                throw new InvalidCredentialException("Credentials are incomplete.");
            else if (_passport == null || _passport.email != Email || _passport.account != Account 
                    || _passport.password != Password
                    || (_passport.role == null && RoleId != null)
                    || (_passport.role != null && _passport.role.internalId != RoleId))
            {
                _passport = new Passport()
                                {
                                    account = Account,
                                    email = Email,
                                    password = Password,
                                    role = (RoleId == null ? null : new RecordRef() { internalId = RoleId }),
                                };
            }
            return _passport;
        }
    }
}
