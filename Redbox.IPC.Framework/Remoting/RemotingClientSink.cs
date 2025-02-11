using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace Redbox.IPC.Framework.Remoting
{
    public class RemotingClientSink :
        BaseChannelSinkWithProperties,
        IClientChannelSink,
        IChannelSinkBase
    {
        public RemotingClientSink(IClientChannelSink nextSink, object remoteChannelData)
        {
            NextChannelSink = nextSink;
            var setMethod = NextChannelSink.GetType().GetProperty("Item").GetSetMethod();
            setMethod.Invoke(NextChannelSink, new object[2]
            {
                "timeout",
                remoteChannelData
            });
            setMethod.Invoke(NextChannelSink, new string[2]
            {
                "connectiongroupname",
                remoteChannelData.ToString()
            });
        }

        public void ProcessMessage(
            IMessage msg,
            ITransportHeaders requestHeaders,
            Stream requestStream,
            out ITransportHeaders responseHeaders,
            out Stream responseStream)
        {
            NextChannelSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
        }

        public void AsyncProcessRequest(
            IClientChannelSinkStack sinkStack,
            IMessage msg,
            ITransportHeaders headers,
            Stream stream)
        {
            NextChannelSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }

        public void AsyncProcessResponse(
            IClientResponseChannelSinkStack sinkStack,
            object state,
            ITransportHeaders headers,
            Stream stream)
        {
            NextChannelSink.AsyncProcessResponse(sinkStack, state, headers, stream);
        }

        public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
        {
            return null;
        }

        public IClientChannelSink NextChannelSink { get; }
    }
}