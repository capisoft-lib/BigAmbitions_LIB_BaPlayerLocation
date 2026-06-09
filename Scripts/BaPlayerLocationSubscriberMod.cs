using System.Threading.Tasks;
using BAModAPI;
using UnityEngine;

[assembly: RegisterModClass(typeof(BaPlayerLocation.Subscriber.BaPlayerLocationSubscriberMod))]

namespace BaPlayerLocation.Subscriber
{
    [ModEntryOnCityLoad]
    public sealed class BaPlayerLocationSubscriberMod : IModBigAmbitions
    {
        private GameObject _driverObject;

        public string[] RelativeAssetBundlePaths => System.Array.Empty<string>();

        public Task OnLoadAsync(ModContext context)
        {
            ModStoragePaths.Initialize(context);
            ModLog.Info(
                "Loading subscriber library | mod_root=" + ModStoragePaths.ModRootDirectory);

            SubscriberConfig.Load().Apply();
            PlayerLocationSubscriber.Initialize();

            _driverObject = new GameObject("BaPlayerLocation_Subscriber_Driver");
            Object.DontDestroyOnLoad(_driverObject);
            _driverObject.AddComponent<BaPlayerLocationSubscriberDriver>();

            ModLog.Info("Subscriber library loaded.");
            return Task.CompletedTask;
        }

        public Task OnUnloadAsync()
        {
            ModLog.Info("Unloading subscriber library.");

            if (_driverObject != null)
            {
                Object.Destroy(_driverObject);
                _driverObject = null;
            }

            PlayerLocationSubscriber.Shutdown();
            SubscriberThresholds.ResetToDefaults();
            ModStoragePaths.Shutdown();
            ModLog.Info("Subscriber library unloaded.");
            return Task.CompletedTask;
        }
    }
}
