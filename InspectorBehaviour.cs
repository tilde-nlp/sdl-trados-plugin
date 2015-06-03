using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace LetsMT.MTProvider
{
    class InspectorBehaviour : IEndpointBehavior
    {
        public IClientMessageInspector Inspector { get; set; }
        /// <summary>
        /// Create a new <see cref="CookieManagementBehaviour"/> instance.
        /// </summary>
        /// <param name="inspector">The message inspector that is applied to client behaviour.</param>
        public InspectorBehaviour(IClientMessageInspector inspector)
        {
            this.Inspector = inspector;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(Inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
