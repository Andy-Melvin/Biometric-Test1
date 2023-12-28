using System;
using System.Security.Principal;

namespace Cirrostrata.Biometrics
{
    public class AuthenticationSuccessEventArgs : EventArgs
    {
        /// <summary>The identity of the user who scanned their fingerprint</summary>
        public IdentityReference IdentityReference { get; set; }

        /// <summary>Which finger they scanned</summary>
        public WinBioFinger Finger { get; set; }

        public AuthenticationSuccessEventArgs(IdentityReference identityReference, WinBioFinger finger)
        {
            IdentityReference = identityReference;
            Finger = finger;
        }
    }
}