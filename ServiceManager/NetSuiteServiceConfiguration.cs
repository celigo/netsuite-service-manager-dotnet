using System;
using com.celigo.net.ServiceManager.Properties;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager
{
    /// <summary>
    /// Represents a ServieManager configuration.
    /// </summary>
    public class NetSuiteServiceConfiguration
    {
#if !FIRSTBUILD
        /// <summary>Gets or sets the number of retries in case of a recoverable failure.</summary>
        /// <value>The retry count.</value>
        public int RetryCount { get; set; }

        /// <summary>Gets or sets the interval between successive retries.</summary>
        /// <value>The retry interval.</value>
        public int RetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the size of the search page.
        /// </summary>
        /// <value>The size of the search page.</value>
        [Obsolete("Use NetSuiteServiceConfiguration.SearchPerferences.pageSize instead")] 
        public int SearchPageSize
        {
            get { return SearchPreferences.pageSize; }
            set
            {
                SearchPreferences.pageSize = value;
                SearchPreferences.pageSizeSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the Webservices End Point.
        /// </summary>
        /// <value>The URL of the Webservices End Point.</value>
        public string EndPointUrl { get; set; }

        /// <summary>
        /// Gets or sets the NetSuite Application Id.
        /// </summary>
        /// <value>The application id.</value>
        public string ApplicationId { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the search affects body fields only.
        /// </summary>
        /// <value><c>true</c> if the search should affect body fields only; 
        /// otherwise, <c>false</c>.</value>
        [Obsolete("Use NetSuiteServiceConfiguration.SearchPerferences.bodyFieldsOnly instead")]
        public bool BodyFieldsOnly
        {
            get { return SearchPreferences.bodyFieldsOnly; }
            set { SearchPreferences.bodyFieldsOnly = value; }
        }

        /// <summary>Gets or sets the size of the Update request.</summary>
        /// <value>The size of the Update request.</value>
        public int UpdateRequestSize { get; set; }

        /// <summary>Gets or sets the size of the Add request.</summary>
        /// <value>The size of the Add request.</value>
        public int AddRequestSize { get; set; }

        /// <summary>Gets or sets the size of the Delete request.</summary>
        /// <value>The size of the Delete request.</value>
        public int DeleteRequestSize { get; set; }

        /// <summary>
        /// Gets or sets the search preferences.
        /// </summary>
        /// <value>The search preferences.</value>
        public SearchPreferences SearchPreferences { get; set; }

        /// <summary>Gets or sets SuiteTalk API preferences.</summary>
        public Preferences Preferences { get; set; }

        /// <summary>
        /// Gets or sets whether the read-only fields are ignored.
        /// </summary>
        [Obsolete("Use NetSuiteServiceConfiguration.Preferences.ignoreReadOnlyFields instead")]
        public bool IgnoreReadonlyFields
        {
            get
            {
                return Preferences.ignoreReadOnlyFields;
            }
            set
            {
                Preferences.ignoreReadOnlyFields = value;
                Preferences.ignoreReadOnlyFieldsSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets whether NetSuite warnings should be treated as errors.
        /// </summary>
        [Obsolete("Use NetSuiteServiceConfiguration.Preferences.warningAsError instead")]
        public bool TreatWarningsAsErrors
        {

            get { return Preferences.warningAsError; }
            set
            {
                Preferences.warningAsError = value;
                Preferences.warningAsErrorSpecified = true;
            }
        }

        /// <summary>Gets or sets a value indicating whether an Advanced Search should return only the search columns.</summary>
        [Obsolete("Use NetSuiteServiceConfiguration.SearchPerferences.returnSearchColumns instead")]
        public bool ReturnSearchColumnsOnly
        {
            get { return SearchPreferences.returnSearchColumns; }
            set
            {
                SearchPreferences.returnSearchColumns = value;
            }
        }

        internal void Configure(INetSuiteService service)
        {
            Configure(service, SearchPreferences);
        }

        internal void Configure(INetSuiteService service, SearchPreferences searchPref)
        {
            service.Timeout = System.Threading.Timeout.Infinite;
            service.Url = EndPointUrl;

            service.preferences = Preferences;
            service.searchPreferences = searchPref ?? SearchPreferences;

            if (service.CookieContainer == null)
                service.CookieContainer = new System.Net.CookieContainer();

            if (service.applicationInfo != null)
            {
                service.applicationInfo.applicationId = ApplicationId;
            }
            else if (ApplicationId != null)
            {
                service.applicationInfo = new ApplicationInfo() { applicationId = ApplicationId };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetSuiteServiceConfiguration"/> class.
        /// </summary>
        public NetSuiteServiceConfiguration()
        {
            AddRequestSize    = 10;
            DeleteRequestSize = 10;
            UpdateRequestSize = 10;

            RetryCount    = 3;
            RetryInterval = 5;

            SearchPreferences = new SearchPreferences();
            SearchPreferences.pageSize = 1000;
            SearchPreferences.pageSizeSpecified = true;
            SearchPreferences.bodyFieldsOnly = false;

            Preferences = new Preferences();
            EndPointUrl = Settings.Default.ServiceManager_SuiteTalk_NetSuiteService;
        }
#endif
    }
}