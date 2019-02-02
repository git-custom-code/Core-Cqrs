namespace CustomCode.Core.Messages.Transport.NetMQ.Endpoints
{
    /// <summary>
    /// Enumeration that defines NetMQ's supported tranport protocols.
    /// </summary>
    /// <remarks> See https://netmq.readthedocs.io/en/latest/transports/ for more details. </remarks>
    public enum Protocol : byte
    {
        /// <summary> Transmission control protocol (see https://en.wikipedia.org/wiki/Transmission_Control_Protocol). </summary>
        Tcp = 0,
        /// <summary> In-Process protocol (see http://api.zeromq.org/2-1:zmq-inproc). </summary>
        InProc = 1,
        /// <summary> Pragmatic general multicast (see https://en.wikipedia.org/wiki/Pragmatic_General_Multicast). </summary>
        Pgm = 2
    }
}