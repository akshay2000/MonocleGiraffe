using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinImgur.Interfaces
{
    public struct HttpProgress
    {
        public ulong BytesReceived;
        public ulong BytesSent;
        public uint Retries;
        public HttpProgressStage Stage;
        public ulong? TotalBytesToReceive;
        public ulong? TotalBytesToSend;
    }

    public enum HttpProgressStage
    {
        None,
        DetectingProxy,
        ResolvingName,
        ConnectingToServer,
        NegotiatingSsl,
        SendingHeaders,
        SendingContent,
        WaitingForResponse,
        ReceivingHeaders,
        ReceivingContent
    }
}
