namespace CustomCode.Core.Messages
{
    using System;

    /// <summary>
    /// A struct that can be used as a wrapper around a unique identity value.
    /// </summary>
    /// <typeparam name="T"> The type of the identity's unique value. </typeparam>
    public struct Identity<T> : IEquatable<Identity<T>>, IComparable, IComparable<Identity<T>>
        where T : IEquatable<T>, IComparable, IComparable<T>
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="Identity{T}"/> type.
        /// </summary>
        /// <param name="value"> The identity's unique value. </param>
        public Identity(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "An identity cannot have the value <null>");
            }

            Value = value;
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the identity's unique value.
        /// </summary>
        private T Value { get; }

        #endregion

        #region Logic

        /// <summary>
        /// Compares two <see cref="Identity{T}"/>s for equality.
        /// </summary>
        /// <param name="left"> The operator's left hand side argument. </param>
        /// <param name="right"> The operator's right hand side argument. </param>
        /// <returns> True if both <see cref="Identity{T}"/>s are equal, false otherwise. </returns>
        public static bool operator ==(Identity<T> left, Identity<T> right)
        {
            return left.Value.Equals(right.Value);
        }

        /// <summary>
        /// Compares two <see cref="Identity{T}"/>s for inequality.
        /// </summary>
        /// <param name="left"> The operator's left hand side argument. </param>
        /// <param name="right"> The operator's right hand side argument. </param>
        /// <returns> True if both <see cref="Identity{T}"/>s are inequal, false otherwise. </returns>
        public static bool operator !=(Identity<T> left, Identity<T> right)
        {
            return !left.Value.Equals(right.Value);
        }

        /// <summary>
        /// Explicitely cast a unique value of type <typeparamref name="T"/> to an <see cref="Identity{T}"/>.
        /// </summary>
        /// <param name="value"> The unique value to be converted to an <see cref="Identity{T}"/>. </param>
        public static explicit operator Identity<T>(T value)
        {
            return new Identity<T>(value);
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is Identity<T> other)
            {
                return CompareTo(other);
            }
            return 1;
        }

        /// <inheritdoc />
        public int CompareTo(Identity<T> other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Identity<T> other)
            {
                return Value.Equals(other.Value);
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(Identity<T> other)
        {
            return Value.Equals(other.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Value} (Id)";
        }

        #endregion
    }
}