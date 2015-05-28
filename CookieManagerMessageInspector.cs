using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace LetsMT.MTProvider
{
    /// <summary>
    /// A message inspector that adds a specified cookie to all outhoing HTTP requests.
    /// </summary>
    public class CookieManagerMessageInspector : IClientMessageInspector
    {
        public string Cookie { get; set; }
        /// <summary>
        /// Create a new CookieManagerMessageInspector instance.
        /// </summary>
        /// <param name="cookie">The cookie to append to inspected messages.</param>
        public CookieManagerMessageInspector(string cookie)
        {
            this.Cookie = cookie;
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
            httpRequest.Headers.Add(HttpRequestHeader.Cookie, this.Cookie);

            return null;

        }
    }
}
