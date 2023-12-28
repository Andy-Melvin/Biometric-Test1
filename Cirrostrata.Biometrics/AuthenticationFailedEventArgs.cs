using System;

namespace Cirrostrata.Biometrics
{
    public class AuthenticationFailedEventArgs : EventArgs
    {
        /// <summary>Reason the fingerprint was not identified</summary>
        public WinBioError Error { get; set; }

        /// <summary>Additional detailed data related to the failure (skewed read, too fast, too slow, etc.)</summary>
        public WinBioRejectDetail RejectDetail { get; set; }

        public AuthenticationFailedEventArgs(WinBioError error, WinBioRejectDetail rejectDetail)
        {
            Error = error;
            RejectDetail = rejectDetail;
        }
    }
}