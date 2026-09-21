using System;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.Event
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<object>> receiversByType = new Dictionary<Type, List<object>>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            receiversByType.Clear();
        }

        public static void Subscribe<TEvent>(IEventReceiver<TEvent> receiver) where TEvent : IEvent
        {
            if (!receiversByType.TryGetValue(typeof(TEvent), out var receivers))
            {
                receivers = new List<object>();
                receiversByType.Add(typeof(TEvent), receivers);
            }

            if (!receivers.Contains(receiver))
                receivers.Add(receiver);
        }

        public static void Unsubscribe<TEvent>(IEventReceiver<TEvent> receiver) where TEvent : IEvent
        {
            if (!receiversByType.TryGetValue(typeof(TEvent), out var receivers))
                return;

            receivers.Remove(receiver);
            if (receivers.Count == 0)
                receiversByType.Remove(typeof(TEvent));
        }

        public static void Publish<TEvent>(TEvent evt) where TEvent : IEvent
        {
            if (!receiversByType.TryGetValue(typeof(TEvent), out var receivers))
                return;

            // 回调可以订阅或退订；本轮只通知发布时已登记且仍有效的接收者。
            foreach (var receiver in receivers.ToArray())
            {
                if (receiver is UnityEngine.Object unityObject && unityObject == null)
                {
                    receivers.Remove(receiver);
                    continue;
                }

                if (receivers.Contains(receiver))
                    ((IEventReceiver<TEvent>)receiver).OnEvent(evt);
            }

            if (receivers.Count == 0 && receiversByType.TryGetValue(typeof(TEvent), out var current) && current == receivers)
                receiversByType.Remove(typeof(TEvent));
        }
    }
}
