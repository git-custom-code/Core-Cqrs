namespace CustomCode.Core.Messages.Transport.NetMQ
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A value type that represents a strongly typed (ipv4) ip address.
    /// </summary>
    public struct IPAddress : IEquatable<IPAddress>, IComparable, IComparable<IPAddress>
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="IPAddress"/> type.
        /// </summary>
        /// <param name="firstOctet"> The address' first octet (8 bits). </param>
        /// <param name="secondOctet"> The address' second octet (8 bits). </param>
        /// <param name="thirdOctet"> The address' third octet (8 bits). </param>
        /// <param name="fourthOctet"> The address' fourth octet (8 bits). </param>
        public IPAddress(byte firstOctet, byte secondOctet, byte thirdOctet, byte fourthOctet)
        {
            Value = BitConverter.ToUInt32(new[] { firstOctet, secondOctet, thirdOctet, fourthOctet }, 0);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IPAddress"/> type.
        /// </summary>
        /// <param name="address"> The ipv4 address. </param>
        public IPAddress(string address)
        {
            var regex = new Regex(@"^(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])$");
            var match = regex.Match(address);
            if (match.Success && match.Groups.Count == 5)
            {
                var firstOctet = byte.Parse(match.Groups[1].Value);
                var secondOctet = byte.Parse(match.Groups[2].Value);
                var thirdOctet = byte.Parse(match.Groups[3].Value);
                var fourthOctet = byte.Parse(match.Groups[4].Value);
                Value = BitConverter.ToUInt32(new[] { firstOctet, secondOctet, thirdOctet, fourthOctet }, 0);
            }
            else
            {
                throw new ArgumentException("Invalid ip address format", nameof(address));
            }
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the address' unsigned 32-bit value. 
        /// </summary>
        private uint Value { get; }

        #endregion

        #region Logic

        /// <summary>
        /// Compares two <see cref="IPAddress"/>es for equality.
        /// </summary>
        /// <param name="left"> The operator's left hand side argument. </param>
        /// <param name="right"> The operator's right hand side argument. </param>
        /// <returns> True if both <see cref="IPAddress"/>es are equal, false otherwise. </returns>
        public static bool operator ==(IPAddress left, IPAddress right)
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// Compares two <see cref="IPAddress"/>es for inequality.
        /// </summary>
        /// <param name="left"> The operator's left hand side argument. </param>
        /// <param name="right"> The operator's right hand side argument. </param>
        /// <returns> True if both <see cref="IPAddress"/>s are inequal, false otherwise. </returns>
        public static bool operator !=(IPAddress left, IPAddress right)
        {
            return left.Value != right.Value;
        }

        /// <summary>
        /// Explicitely cast an <see cref="string"/> ipv4 address to an <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="address"> The ipv4 address to be converted to an <see cref="IPAddress"/>. </param>
        public static explicit operator IPAddress(string address)
        {
            return new IPAddress(address);
        }

        /// <summary>
        /// Explicitely cast an <see cref="IPAddress"/> to a <see cref="string"/> ipv4 address.
        /// </summary>
        /// <param name="address"> The <see cref="IPAddress"/> to be converted to a ipv4 string address. </param>
        public static explicit operator string(IPAddress address)
        {
            return address.ToString();
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is IPAddress other)
            {
                return CompareTo(other);
            }
            return 1;
        }

        /// <inheritdoc />
        public int CompareTo(IPAddress other)
        {
            if (Value > other.Value)
            {
                return 1;
            }
            else if (Value < other.Value)
            {
                return -1;
            }
            return 0;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is IPAddress other)
            {
                return Value == other.Value;
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(IPAddress other)
        {
            return Value == other.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (int)Value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var octets = BitConverter.GetBytes(Value);
            return $"{octets[0]}.{octets[1]}.{octets[2]}.{octets[3]}";
        }

        #endregion
    }
}