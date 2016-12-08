#if !FIRSTBUILD
using System;
using System.Xml.Serialization;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Represents a NetSuite session created by explicitly login in using the login(..) method.
    /// </summary>
    [Serializable]
    public class UserSession : IUserSession
    {
        /// <summary>
        /// Represents an unsuccessful login attempt.
        /// </summary>
        public static readonly UserSession Unsuccessful = new UserSession() { IsSuccess = false };
        
        internal const string JSESSIONID = "JSESSIONID";

        /// <summary>
        /// Gets value indicating whether the login was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets the logged in user's ID.
        /// </summary>
        /// <value>The user id.</value>
        public RecordRef UserId { get; set; }

        /// <summary>
        /// Gets the roles available to the current user.
        /// </summary>
        /// <value>The roles.</value>
        public WsRole[] Roles { get; set; }

        [NonSerialized]
        private INetSuiteService _serviceProxy;

        /// <summary>
        /// Gets or sets the service proxy maintained by this session.
        /// </summary>
        /// <value>The service proxy.</value>
        [XmlIgnore]
        internal INetSuiteService ServiceProxy
        {
            get { return _serviceProxy; }
            set { _serviceProxy = value; }
        }

        [NonSerialized]
        private NetSuiteCredential _credential;

        /// <summary>
        /// Gets or sets the credentials used to create this session.
        /// </summary>
        /// <value>The credentials.</value>
        [XmlIgnore]
        internal NetSuiteCredential Credentials
        {
            get { return _credential; }
            set { _credential = value; }
        }

        /// <summary>
        /// Gets or sets an internally generated id for the current session.
        /// </summary>
        /// <value>The session id.</value>
        public string SessionId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession"/> class.
        /// </summary>
        public UserSession()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="credential">Credential used to create this session.</param>
        /// <param name="response">The response.</param>
        internal UserSession(INetSuiteService service, NetSuiteCredential credential, SessionResponse response)
        {
            UserId       = response.userId;
            Roles        = response.wsRoleList;

            ServiceProxy = service;
            Credentials  = credential;
            IsSuccess    = response.status.isSuccess;

            if (service.CookieContainer != null)
            {
                var nsUrl = new Uri(service.Url);
                var cookies = service.CookieContainer.GetCookies(nsUrl);
                for (int i = 0; i < cookies.Count; i++)
                {
                    if (cookies[i].Name.Equals(JSESSIONID, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SessionId = cookies[i].Value;
                        break;
                    }
                }
            }
        }
    }
}
#endif