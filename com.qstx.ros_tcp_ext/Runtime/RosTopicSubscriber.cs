using System;
using UnityEngine;
using UnityEngine.Events;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace QSTX.ROSTCPConnector.Extension
{
    public abstract class RosTopicSubscriber<TMessage> : MonoBehaviour, IRosTopicSubscriber
        where TMessage : Message
    {
        public delegate void MsgCallbackDelegate(TMessage msg);
        public MsgCallbackDelegate MsgCallback { protected set; get; } = null;
        [SerializeField] private UnityEvent<TMessage> msgEvent;
        public UnityEvent<TMessage> MsgEvent => msgEvent;
        [SerializeField] private string topicName;
        public string Name => topicName;

        public bool IsSubscribed { protected set; get; } = false;

        private void Callbacks(TMessage msg)
        {
            MsgCallback?.Invoke(msg);
            MsgEvent?.Invoke(msg);
        }

        public virtual void Subscribe()
        {
            if (IsSubscribed)
            {
                Debug.LogError($"{typeof(RosTopicSubscriber<TMessage>).Name}::Subscribe()::{Name} has been subscribed!");
                return;
            }
            ROSConnection.GetOrCreateInstance().Subscribe<TMessage>(Name, Callbacks);
            IsSubscribed = true;
            Debug.Log($"Register Subscriber:{Name}");
        }

        [Obsolete("方法不安全")]
        public virtual void UnSubscribe()
        {
            if (!IsSubscribed)
            {
                Debug.LogError($"{typeof(RosTopicSubscriber<TMessage>).Name}::UnSubscribe()::{Name} hasn't been subscribed!");
                return;
            }

            ROSConnection.GetOrCreateInstance().Unsubscribe(Name);
            Debug.Log($"UnSubscribe Topic: {Name}");
            IsSubscribed = false;
        }
    }
}
