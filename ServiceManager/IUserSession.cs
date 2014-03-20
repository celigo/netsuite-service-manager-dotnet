namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Represents a NetSuite session created by explicitly login in using the login(..) method.
    /// </summary>
    public interface IUserSession
    {
        /// <summary>
        /// Gets value indicating whether the login was successful.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets the roles available to the current user.
        /// </summary>
        /// <value>The roles.</value>
        com.celigo.net.ServiceManager.SuiteTalk.WsRole[] Roles { get; }

        /// <summary>
        /// Gets or sets an internally generated id for the current session.
        /// </summary>
        /// <value>The session id.</value>
        string SessionId { get; }

        /// <summary>
        /// Gets the logged in user's ID.
        /// </summary>
        /// <value>The user id.</value>
        com.celigo.net.ServiceManager.SuiteTalk.RecordRef UserId { get; }
    }
}
