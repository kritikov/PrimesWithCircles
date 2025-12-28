using System.ComponentModel;

namespace PrimesWithCircles.Infrastucture.Enums
{
    public enum PresentationMode
    {
        [Description("One circle")]
        OneCircle,

        [Description("Two circles")]
        TwoCircles,

        [Description("Seek primes")]
        SeekPrimes
    }

}
