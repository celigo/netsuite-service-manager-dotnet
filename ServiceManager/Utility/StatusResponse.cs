using System;
using System.Collections.Generic;
using System.Text;
using com.celigo.net.ServiceManager.Exceptions;
using com.celigo.net.ServiceManager.SuiteTalk;

namespace com.celigo.net.ServiceManager.Utility
{
    /// <summary>
    /// Holds utility functions related to the Service Manager library
    /// </summary>
    public static class StatusResponse
    {
        /// <summary>Gets the first error detail contained in the given <paramref name="status"/>.</summary>
        /// <param name="status">The status.</param>
        /// <returns>The first error detail found in the given <paramref name="status"/></returns>
        public static StatusDetail GetStatusError(Status status)
        {
            StatusDetail[] details = GetStatusDetails(status, StatusDetailType.ERROR);
            if (details.Length > 0)
                return details[0];
            else
                return null;
        }

        /// <summary>
        /// Gets the status details of <paramref name="detailType"/> contained in the <paramref name="status"/>.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="detailType">Type of the detail.</param>
        /// <returns>The status details of <paramref name="detailType"/>.</returns>
        public static StatusDetail[] GetStatusDetails(Status status, StatusDetailType detailType)
        {
            List<StatusDetail> retVal = new List<StatusDetail>(status.statusDetail.Length);
            for (int i = 0; i < status.statusDetail.Length; i++)
                if (status.statusDetail[i].type == detailType)
                {
                    retVal.Add(status.statusDetail[i]);
                }
            return retVal.ToArray();
        }

        /// <summary>Gets the status detail messages concatenated to a single string.</summary>
        /// <param name="details">The status details containing the messages.</param>
        /// <param name="detailsSeperator">
        /// The separator string that would separate adjacent messages when concatenating.
        /// </param>
        /// <returns>A string containing the status messages contained in the specified list of
        /// status details.</returns>
        public static string GetStatusDetailMessages(StatusDetail[] details, string detailsSeperator)
        {
            StringBuilder buffer = new StringBuilder(details.Length * 20);
            for (int i = 0; i < details.Length; i++)
                if (details[i] != null)
                {
                    buffer.Append(details[i].message);
                    buffer.Append(detailsSeperator);
                }
            return buffer.ToString();
        }

        /// <summary>
        /// Generates an exception based on the given failed NetSuite Status response.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static StatusErrorException ConvertToException(Status status)
        {
            if (status.statusDetail == null)
                throw new InvalidOperationException("Cannot create an exception when the Status.statusDetail is null");
            else
            {
                return new StatusErrorException(GetStatusDetailMessages(status.statusDetail, "\n"))
                        {
                            Details = status.statusDetail,
                        };            
            }
        }

        /// <summary>
        /// Determines whether the specified status contains the given error code.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="statusDetailCodeType">Type of the status detail code.</param>
        /// <returns>
        ///   <c>true</c> if the specified status contains the error code; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasError(Status status, StatusDetailCodeType statusDetailCodeType)
        {
            if (status.isSuccess || status.statusDetail == null)
                return true;
            else
            {
                return null != Array.Find(status.statusDetail, s => s.code == statusDetailCodeType);
            }
        }
    }
}