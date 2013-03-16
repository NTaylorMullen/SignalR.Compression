
namespace SignalR.Compression.Server
{
    public interface IContractsGenerator
    {
        /// <summary>
        /// Generates and caches contracts for all payloads in the solution.
        /// </summary>
        /// <returns>An object that describes all ways to compress/decompress payloads in the solution.</returns>
        object GenerateContracts();
    }
}
