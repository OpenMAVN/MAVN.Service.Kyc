using JetBrains.Annotations;

namespace MAVN.Service.Kyc.Client
{
    /// <summary>
    /// Kyc client interface.
    /// </summary>
    [PublicAPI]
    public interface IKycClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IKycApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application KycApi interface</summary>
        IKycApi KycApi { get; }
    }
}
