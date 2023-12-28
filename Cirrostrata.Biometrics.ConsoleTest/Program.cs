using System;

namespace Cirrostrata.Biometrics.ConsoleTest
{
    class Program
    {
        private readonly IBiometrics _biometrics;

        public Program(IBiometrics biometrics)
        {
            _biometrics = biometrics;
        }

        public void Run()
        {
            _biometrics.IdentifySuccess += IdentifySuccess;
            _biometrics.IdentifyFailed += IdentifyFailed;

            do
            {
                using (IBiometricSession session = _biometrics.OpenSession())
                    session.Wait();

            } while (true);
        }

        static void IdentifyFailed(object sender, AuthenticationFailedEventArgs args)
        {
            Console.WriteLine("Failed: {0} - {1}", args.Error, args.RejectDetail);
        }

        static void IdentifySuccess(object sender, AuthenticationSuccessEventArgs args)
        {
            Console.WriteLine("Success: {0}", args.IdentityReference);
        }

        static void Main()
        {
            new Program(new Biometrics()).Run();
        }
    }
}
