using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    internal sealed class BaPlayerLocationSubscriberDriver : MonoBehaviour
    {
        private void Update() => PlayerLocationSubscriber.Tick();
    }
}
