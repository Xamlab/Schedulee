using System.Collections.Generic;

namespace Schedulee.UI.ViewModels.Base
{
    public interface IStaleMonitor
    {
        bool IsStale { get; }
        IEnumerable<string> Properties { get; }
        IEnumerable<string> Collections { get; }
        void StartMonitoring();
        void CaptureProperties(params string[] properties);
        void CaptureCollectionProperties(params string[] properties);
        bool ArePropertiesStale(params string[] properties);
        bool AreCollectionsStale(params string[] properties);
    }
}