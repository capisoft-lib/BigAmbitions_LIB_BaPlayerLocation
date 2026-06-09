using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    /// <summary>
    /// Public API for other mods: probe player location and subscribe to changes only.
    /// </summary>
    public static class PlayerLocationSubscriber
    {
        private const float ProbeIntervalSeconds = 0.25f;

        private static readonly List<Subscription> Subscriptions = new List<Subscription>();
        private static readonly List<Action> ActiveCallbacks = new List<Action>();

        private static float _nextProbeTime;
        private static PlayerLocationSnapshot _current;
        private static PlayerLocationSnapshot _lastNotified;
        private static bool _hasCurrent;
        private static bool _hasNotified;

        public static event Action<PlayerLocationSnapshot> Changed;

        /// <summary>True after the subscriber mod loaded its city driver until unload.</summary>
        public static bool IsActive { get; private set; }

        public static bool HasCurrent => _hasCurrent;

        public static bool TryGetCurrent(out PlayerLocationSnapshot snapshot)
        {
            snapshot = _current;
            return _hasCurrent;
        }

        public static IDisposable Subscribe(Action<PlayerLocationSnapshot> onChanged)
        {
            if (onChanged == null)
                throw new ArgumentNullException(nameof(onChanged));

            var subscription = new Subscription(onChanged);
            Subscriptions.Add(subscription);
            NotifyCurrentIfKnown(onChanged);
            return subscription;
        }

        /// <summary>
        /// Runs <paramref name="callback"/> immediately when the library is already active;
        /// otherwise once after this mod loads in the city (handles consumer mod load order).
        /// </summary>
        public static void OnActive(Action callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (IsActive)
            {
                InvokeActiveCallback(callback);
                return;
            }

            ActiveCallbacks.Add(callback);
        }

        /// <summary>
        /// Subscribes when the library is ready. Prefer this over manual <see cref="OnActive"/>
        /// + <see cref="Subscribe"/> when your mod may load before LIB_BaPlayerLocation.
        /// </summary>
        public static IDisposable SubscribeWhenActive(Action<PlayerLocationSnapshot> onChanged)
        {
            if (onChanged == null)
                throw new ArgumentNullException(nameof(onChanged));

            IDisposable inner = null;
            Action attach = () => inner = Subscribe(onChanged);

            if (IsActive)
                attach();
            else
                ActiveCallbacks.Add(attach);

            return new DeferredSubscription(attach, () => inner?.Dispose());
        }

        internal static void Initialize()
        {
            Reset();
            IsActive = true;
            FlushActiveCallbacks();
            ModLog.Info("PlayerLocationSubscriber ready (LIB_BaPlayerLocation v0.11).");
        }

        internal static void Shutdown()
        {
            Reset();
            IsActive = false;
            ActiveCallbacks.Clear();
            Subscriptions.Clear();
            Changed = null;
            ModLog.Info("PlayerLocationSubscriber shut down.");
        }

        internal static void Tick()
        {
            var now = Time.unscaledTime;
            if (now < _nextProbeTime)
                return;

            _nextProbeTime = now + ProbeIntervalSeconds;

            if (!PlayerLocationProbe.TryProbe(out var snapshot))
            {
                if (_hasCurrent || _hasNotified)
                    NotifyUnavailable();
                return;
            }

            _current = snapshot;
            _hasCurrent = true;

            if (!PlayerLocationProbe.HasSignificantChange(snapshot, _lastNotified, _hasNotified))
                return;

            _lastNotified = snapshot;
            _hasNotified = true;
            NotifyChanged(snapshot);
        }

        private static void NotifyUnavailable()
        {
            _hasCurrent = false;
            _hasNotified = false;
            _current = default;
            _lastNotified = default;

            var unavailable = new PlayerLocationSnapshot(
                MovementKind.Unavailable,
                default,
                0f,
                0f,
                string.Empty);

            NotifyChanged(unavailable);
        }

        private static void NotifyChanged(PlayerLocationSnapshot snapshot)
        {
            Changed?.Invoke(snapshot);

            for (var i = Subscriptions.Count - 1; i >= 0; i--)
            {
                var subscription = Subscriptions[i];
                if (subscription.IsDisposed)
                {
                    Subscriptions.RemoveAt(i);
                    continue;
                }

                try
                {
                    subscription.Handler(snapshot);
                }
                catch (Exception ex)
                {
                    ModLog.Error("PlayerLocationSubscriber callback failed", ex);
                }
            }
        }

        private static void FlushActiveCallbacks()
        {
            if (ActiveCallbacks.Count == 0)
                return;

            var pending = ActiveCallbacks.ToArray();
            ActiveCallbacks.Clear();

            foreach (var callback in pending)
                InvokeActiveCallback(callback);
        }

        private static void InvokeActiveCallback(Action callback)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                ModLog.Error("PlayerLocationSubscriber active callback failed", ex);
            }
        }

        private static void NotifyCurrentIfKnown(Action<PlayerLocationSnapshot> onChanged)
        {
            if (!_hasCurrent)
                return;

            try
            {
                onChanged(_current);
            }
            catch (Exception ex)
            {
                ModLog.Error("PlayerLocationSubscriber callback failed on subscribe", ex);
            }
        }

        private static void Reset()
        {
            _nextProbeTime = 0f;
            _current = default;
            _lastNotified = default;
            _hasCurrent = false;
            _hasNotified = false;
        }

        private sealed class DeferredSubscription : IDisposable
        {
            private readonly Action _attach;
            private readonly Action _disposeInner;
            private bool _isDisposed;

            internal DeferredSubscription(Action attach, Action disposeInner)
            {
                _attach = attach;
                _disposeInner = disposeInner;
            }

            public void Dispose()
            {
                if (_isDisposed)
                    return;

                _isDisposed = true;
                ActiveCallbacks.Remove(_attach);
                _disposeInner();
            }
        }

        private sealed class Subscription : IDisposable
        {
            internal readonly Action<PlayerLocationSnapshot> Handler;
            internal bool IsDisposed { get; private set; }

            internal Subscription(Action<PlayerLocationSnapshot> handler)
            {
                Handler = handler;
            }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }
    }
}
