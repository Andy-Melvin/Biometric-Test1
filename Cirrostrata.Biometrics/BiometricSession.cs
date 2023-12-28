using System.ComponentModel;
using PInvoker.Marshal;
using WinBio;

namespace Cirrostrata.Biometrics
{
    public class BiometricSession : IBiometricSession
    {
        private readonly PWINBIO_IDENTIFY_CALLBACK _identifyCallback;
        private readonly ArrayPtr<WINBIO_SESSION_HANDLE> _sessionHandle;
        private bool _isOpen;

        public BiometricSession(PWINBIO_IDENTIFY_CALLBACK callback)
        {
            _identifyCallback = callback;
            _sessionHandle = new ArrayPtr<WINBIO_SESSION_HANDLE>();

            HRESULT hr = NativeFunctions.WinBioOpenSession(NativeConstants.WINBIO_TYPE_FINGERPRINT,
                                                           NativeConstants.WINBIO_POOL_SYSTEM,
                                                           NativeConstants.WINBIO_FLAG_DEFAULT, 
                                                           null, 0, null,
                                                           _sessionHandle);

            if (hr != 0)
                throw new Win32Exception(hr, "WinBioOpenSession failed");

            hr = NativeFunctions.WinBioIdentifyWithCallback(_sessionHandle[0], _identifyCallback, null);

            if (hr != 0)
                throw new Win32Exception(hr, "WinBioIdentifyWithCallback failed");

            _isOpen = true;
        }

        /// <summary>
        /// Blocks the current thread until some fingerprint data is available
        /// </summary>
        public void Wait()
        {
            HRESULT hr = NativeFunctions.WinBioWait(_sessionHandle[0]);

            if (hr != 0)
                throw new Win32Exception(hr, "WinBioWait failed");
        }

        /// <summary>Cancel a pending wait.</summary>
        public void Cancel()
        {
            HRESULT hr = NativeFunctions.WinBioCancel(_sessionHandle[0]);

            if (hr != 0)
                throw new Win32Exception(hr, "WinBioCancel failed");
        }
        
        /// <summary>Closes the session if it's open</summary>
        public void Close()
        {
            if (!_isOpen)
                return;

            HRESULT hr = NativeFunctions.WinBioCloseSession(_sessionHandle[0]);

            if (hr != 0)
                throw new Win32Exception(hr, "WinBioCloseSession failed");

            _isOpen = false;
        }

        /// <summary>Closes the session if it's open</summary>
        public void Dispose()
        {
            Close();
        }
    }
}