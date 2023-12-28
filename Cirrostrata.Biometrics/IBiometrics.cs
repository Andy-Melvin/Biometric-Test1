using System;

namespace Cirrostrata.Biometrics
{
    public interface IBiometrics
    {
        /// <summary>Opens a new session</summary>
        /// <returns>IBiometricSession</returns>
        IBiometricSession OpenSession();

        /// <summary>Called when the user's identity cannot be determined</summary>
        event EventHandler<AuthenticationFailedEventArgs> IdentifyFailed;

        /// <summary>Called when the user's identity has been confirmed</summary>
        event EventHandler<AuthenticationSuccessEventArgs> IdentifySuccess;
    }
}