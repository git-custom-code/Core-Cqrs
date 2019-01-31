namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using System;

    /// <summary>
    /// A value type that represents a strongly typed (tcp/udp) port number.
    /// </summary>
    public struct Port : IEquatable<Port>, IComparable, IComparable<Port>
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="Port"/> type.
        /// </summary>
        /// <param name="portNumber"> The port's number (ranging from 0 to 65536). </param>
        public Port(ushort portNumber)
        {
            PortNumber = portNumber;
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets a flag indicating whether or not the port is a well-known port number.
        /// </summary>
        public bool IsWellKnownPort
        {
            get { return PortNumber < 1024; }
        }

        /// <summary>
        /// Gets a flag indicating whether or not the port is registered port number.
        /// </summary>
        public bool IsRegisterdPort
        {
            get { return PortNumber >= 1024 && PortNumber <= 49151; }
        }

        /// <summary>
        /// Gets a flag indicating whether or not the port is a public port number.
        /// </summary>
        public bool IsPublicPort
        {
            get { return PortNumber >= 49151; }
        }

        /// <summary>
        /// Gets the port's number (ranging from 0 to 65536).
        /// </summary>
        private ushort PortNumber { get; }
        
        #endregion

        #region Logic

        /// <summary>
        /// Compares two <see cref="Port"/>s for equality.
        /// </summary>
        /// <param name="left"> The operator's left hand side argument. </param>
        /// <param name="right"> The operator's right hand side argument. </param>
        /// <returns> True if both <see cref="Port"/>s are equal (i.e. have the same port number), false otherwise. </returns>
        public static bool operator ==(Port left, Port right)
        {
            return left.PortNumber == right.PortNumber;
        }

        /// <summary>
        /// Compares two <see cref="Port"/>s for inequality.
        /// </summary>
        /// <param name="left"> The operator's left hand side argument. </param>
        /// <param name="right"> The operator's right hand side argument. </param>
        /// <returns> True if both <see cref="Port"/>s are inequal (i.e. have different port numbers), false otherwise. </returns>
        public static bool operator !=(Port left, Port right)
        {
            return left.PortNumber != right.PortNumber;
        }

        /// <summary>
        /// Explicitely cast an <see cref="ushort"/> port number to a <see cref="Port"/>.
        /// </summary>
        /// <param name="portNumber"> The port number to be converted to a <see cref="Port"/>. </param>
        public static explicit operator Port(ushort portNumber)
        {
            return new Port(portNumber);
        }

        /// <summary>
        /// Explicitely cast a <see cref="Port"/> to an <see cref="ushort"/> port number.
        /// </summary>
        /// <param name="port"> The <see cref="Port"/> to be converted to a port number. </param>
        public static explicit operator ushort(Port port)
        {
            return port.PortNumber;
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is Port other)
            {
                return CompareTo(other);
            }
            return 1;
        }

        /// <inheritdoc />
        public int CompareTo(Port other)
        {
            if (PortNumber > other.PortNumber)
            {
                return 1;
            }
            else if (PortNumber < other.PortNumber)
            {
                return -1;
            }
            return 0;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Port other)
            {
                return PortNumber == other.PortNumber;
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(Port other)
        {
            return PortNumber == other.PortNumber;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return PortNumber;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{PortNumber}";
        }

        #endregion
    }
}