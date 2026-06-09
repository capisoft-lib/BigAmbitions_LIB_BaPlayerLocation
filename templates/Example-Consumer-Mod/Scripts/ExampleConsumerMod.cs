using System;
using System.Threading.Tasks;
using BaPlayerLocation.Subscriber;
using BAModAPI;
using UnityEngine;

[assembly: RegisterModClass(typeof(ExampleConsumer.ExampleConsumerMod))]

namespace ExampleConsumer
{
    [ModEntryOnCityLoad]
    public sealed class ExampleConsumerMod : IModBigAmbitions
    {
        private IDisposable _locationSubscription;

        public string[] RelativeAssetBundlePaths => Array.Empty<string>();

        public Task OnLoadAsync(ModContext context)
        {
            _ = context;

            _locationSubscription = PlayerLocationSubscriber.SubscribeWhenActive(OnPlayerLocationChanged);
            return Task.CompletedTask;
        }

        public Task OnUnloadAsync()
        {
            _locationSubscription?.Dispose();
            _locationSubscription = null;
            return Task.CompletedTask;
        }

        private void OnPlayerLocationChanged(PlayerLocationSnapshot snapshot)
        {
            if (!snapshot.IsAvailable)
                return;

            Debug.Log(
                $"[ExampleConsumer] {MovementKindLabels.ToLabel(snapshot.MovementKind)} " +
                $"pos={snapshot.Position} heading={snapshot.HeadingDeg:F0} place={snapshot.Place}");
        }
    }
}
