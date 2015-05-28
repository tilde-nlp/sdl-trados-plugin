using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;

namespace LetsMT.MTProvider
{
    /// <summary>
    /// A management behaviour that applies <see cref="CookieManagerMessageInspector"/> to client behaviour.
    /// </summary>
    class CookieManagementBehaviour : IEndpointBehavior
    {
        public string Cookie { get; set; }
        /// <summary>
        /// Create a new <see cref="CookieManagementBehaviour"/> instance.
        /// </summary>
        /// <param name="cookie">The cookie that is used to create the <see cref="CookieManagerMessageInspector"/> that is added to client behaviour.</param>
        public CookieManagementBehaviour(string cookie)
        {
            this.Cookie = cookie;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            CookieManagerMessageInspector inspector = new CookieManagerMessageInspector(this.Cookie);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
