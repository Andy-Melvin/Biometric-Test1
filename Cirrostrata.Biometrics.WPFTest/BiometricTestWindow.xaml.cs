using System;
using System.Windows.Threading;

namespace Cirrostrata.Biometrics.WPFTest
{
    public partial class BiometricTestWindow
    {
        private readonly Biometrics _biometrics;
        private IBiometricSession _session;

        public BiometricTestWindow()
        {
            InitializeComponent();

            _biometrics = new Biometrics();

            _biometrics.IdentifyFailed += _biometrics_IdentifyFailed;
            _biometrics.IdentifySuccess += _biometrics_IdentifySuccess;

            _session = _biometrics.OpenSession();
        }

        void _biometrics_IdentifySuccess(object sender, AuthenticationSuccessEventArgs e)
        {
            // Must be dispatched! Because the session is only good for a single result,
            // we need to close it and reopen another one for a subsequent scan. Closing
            // and reopening the session must be done on the same thread that opened the
            // session. Thus, we need to asynchronously dispatch back to the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                new Action(() =>
                           {
                               Status.Text = "Success";
                               UserName.Text = e.IdentityReference.ToString();
                               _session.Close();
                               _session = _biometrics.OpenSession();
                           }));
        }

        void _biometrics_IdentifyFailed(object sender, AuthenticationFailedEventArgs e)
        {
            // See comment above...
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                new Action(() =>
                           {
                               Status.Text = "Failed";
                               UserName.Text = String.Empty;
                               _session.Close();
                               _session = _biometrics.OpenSession();
                           }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _session.Close();
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
