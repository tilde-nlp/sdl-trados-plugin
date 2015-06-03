using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace LetsMT.MTProvider
{
    class HeaderManagerMessageInspector : IClientMessageInspector
    {
        public string HeaderName { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// Create a new CookieManagerMessageInspector instance.
        /// </summary>
        /// <param name="cookie">The cookie to append to inspected messages.</param>
        public HeaderManagerMessageInspector(string headerName, string value)
        {
            this.HeaderName = headerName;
            this.Value = value;
        }
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequest;

            // The HTTP request object is made available in the outgoing message only
            // when the Visual Studio Debugger is attacched to the running process
            if (!request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                request.Properties.Add(HttpRequestMessageProperty.Name,
                    new HttpRequestMessageProperty());
            }

            httpRequest = (HttpRequestMessageProperty)
                request.Properties[HttpRequestMessageProperty.Name];
            httpRequest.Headers.Add(HeaderName, Value);

            return null;

        }
    }
}
