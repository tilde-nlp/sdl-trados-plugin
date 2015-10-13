using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace LetsMT.MTProvider
{
    class ReaderQuotaExtensionBehaviour : IEndpointBehavior
    {
        private int _quota = int.MaxValue;
        public ReaderQuotaExtensionBehaviour() { }
        public ReaderQuotaExtensionBehaviour(int quota)
        {
            _quota = quota;
        }
        #region Implementation of IEndpointBehavior
        public void Validate(ServiceEndpoint endpoint) { }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            ModifyDataContractSerializerBehavior(endpoint, _quota);
        }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            ModifyDataContractSerializerBehavior(endpoint, _quota);
        }
        #endregion

        public static void ModifyDataContractSerializerBehavior(ServiceEndpoint endpoint, int quota = int.MaxValue)
        {
            foreach (OperationDescription operation in endpoint.Contract.Operations)
            {
                var behavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                behavior.MaxItemsInObjectGraph = quota;
            }
        }
    }
}
