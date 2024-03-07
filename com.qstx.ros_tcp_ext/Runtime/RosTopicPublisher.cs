using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace QSTX.ROSTCPConnector.Extension
{
    public abstract class RosTopicPublisher<TMessage>: MonoBehaviour,IRosTopicPublisher
        where TMessage : Message
    {
        [SerializeField] private string topicName;
        public string Name => topicName;
        
        [SerializeField]
        private bool registerOnStart = false;
        protected virtual void Start()
        {
            if (registerOnStart)
                Register();
        }
        public bool IsRegistered { protected set; get; } = false;

        public virtual void Register()
        {
            if (IsRegistered)
            {
                Debug.LogError(
                    $"{typeof(RosTopicPublisher<TMessage>).Name}::Register(): {Name} has been Registered!");
                return;
            }
            ROSConnection.GetOrCreateInstance().RegisterPublisher<TMessage>(Name);
            IsRegistered = true;
            Debug.Log($"Register Publisher:{Name}");
        }

        public virtual void Publish(TMessage msg)
        {
            ROSConnection.GetOrCreateInstance().Publish(Name, msg);
        }
        virtual protected void Start(){}
    }
}