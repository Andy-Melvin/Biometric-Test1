namespace Cirrostrata.Biometrics
{
    public enum WinBioRejectDetail
    {
        None = 0,
        TooHigh = 1,
        TooLow = 2,
        TooLeft = 3,
        TooRight = 4,
        TooFast = 5,
        TooSlow = 6,
        PoorQuality = 7,
        TooSkewed = 8,
        TooShort = 9,
        MergeFailure = 10
    }
}