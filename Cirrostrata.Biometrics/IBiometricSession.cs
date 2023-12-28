using System;

namespace Cirrostrata.Biometrics
{
    public interface IBiometricSession : IDisposable
    {
        /// <summary>Cancel a pending wait.</summary>
        void Cancel();

        /// <summary>Closes the session if it's open</summary>
        void Close();

        /// <summary>Blocks the current thread until some fingerprint data is available</summary>
        void Wait();
    }
}