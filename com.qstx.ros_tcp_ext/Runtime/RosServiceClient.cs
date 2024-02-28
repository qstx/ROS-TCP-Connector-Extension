using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace QSTX.ROSTCPConnector.Extension
{
    public abstract class RosServiceClient<TRequest, TResponse> : MonoBehaviour, IRosServiceClient
        where TRequest : Message where TResponse : Message, new()
    {
        [SerializeField] private string serviceName;
        public string Name => serviceName;

        public delegate void SrvCallbackDelegate(TResponse srv);
        public SrvCallbackDelegate SrvCallback { protected set; get; } = null;
        
        [SerializeField] private UnityEvent<TResponse> srvEvent = null;
        private UnityEvent<TResponse> SrvEvent => srvEvent;

        public bool IsRegistered { protected set; get; } = false;

        public virtual void Register()
        {
            if (IsRegistered)
            {
                Debug.LogError(
                    $"{typeof(RosServiceClient<TRequest, TResponse>).Name}::Register(): {Name} has been Registered!");
                return;
            }

            ROSConnection.GetOrCreateInstance().RegisterRosService<TRequest, TResponse>(Name);
            IsRegistered = true;
            Debug.Log($"Register Service:{Name}");
        }

        public virtual void Request(TRequest request)
        {
            ROSConnection.GetOrCreateInstance().SendServiceMessage<TResponse>(Name, request, Response);
        }

        private void Response(TResponse response)
        {
            SrvCallback?.Invoke(response);
            SrvEvent?.Invoke(response);
        }
    }
}
