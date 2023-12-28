using System;
using System.Security.Principal;
using WinBio;

namespace Cirrostrata.Biometrics
{
    public class Biometrics : IBiometrics
    {
        // Constants for interacting with the WinBio API
        private const int S_OK = 0;
        private const int WINBIO_E_UNKNOWN_ID = -2146861053;
        private const int WINBIO_E_BAD_CAPTURE = -2146861048;
        private const int WINBIO_E_CANCELED = -2146861052;

        /// <summary>Called when the user's identity cannot be determined</summary>
        public event EventHandler<AuthenticationFailedEventArgs> IdentifyFailed = (x,y) => { };

        /// <summary>Called when the user's identity has been confirmed</summary>
        public event EventHandler<AuthenticationSuccessEventArgs> IdentifySuccess = (x,y) => { };

        private readonly PWINBIO_IDENTIFY_CALLBACK _identifyCallback;

        public Biometrics()
        {
            _identifyCallback = new PWINBIO_IDENTIFY_CALLBACK(IdentifyCallback);
        }

        /// <summary>
        /// Open a biometric session. Sessions are one-shot, they either succeed or fail and 
        /// must be disposed of after the callback is made. If you want to another callback,
        /// you need to open another session.
        /// </summary>
        /// <returns>IBiometricSession object</returns>
        public IBiometricSession OpenSession()
        {
            return new BiometricSession(_identifyCallback);
        }

        private void IdentifyCallback(PVOID IdentifyCallbackContext, HRESULT OperationStatus, 
            WINBIO_UNIT_ID UnitId, WINBIO_IDENTITY Identity, WINBIO_BIOMETRIC_SUBTYPE SubFactor, 
            WINBIO_REJECT_DETAIL RejectDetail)
        {
            switch (OperationStatus)
            {
                case S_OK:
                {
                    var accountSid = Identity.Value.AccountSid[0];
                    var sid = new SecurityIdentifier(accountSid.Data.ToArray(accountSid.Size), 0);
                    IdentityReference user = sid.Translate(typeof(NTAccount));

                    if (user != null)
                        IdentifySuccess(this, new AuthenticationSuccessEventArgs(user, (WinBioFinger)(int)SubFactor));
                    else
                        IdentifyFailed(this, new AuthenticationFailedEventArgs(WinBioError.NoAccount, WinBioRejectDetail.None));

                    break;
                }

                case WINBIO_E_UNKNOWN_ID:
                {
                    IdentifyFailed(this, new AuthenticationFailedEventArgs(WinBioError.UnknownId, (WinBioRejectDetail)(int)RejectDetail));
                    break;
                }

                case WINBIO_E_BAD_CAPTURE:
                {
                    IdentifyFailed(this, new AuthenticationFailedEventArgs(WinBioError.BadCapture, (WinBioRejectDetail)(int)RejectDetail));
                    break;
                }

                case WINBIO_E_CANCELED: break;

                default:
                {
                    IdentifyFailed(this, new AuthenticationFailedEventArgs(WinBioError.Failed, (WinBioRejectDetail)(int)RejectDetail));
                    break;
                }
            }
        }
    }
}