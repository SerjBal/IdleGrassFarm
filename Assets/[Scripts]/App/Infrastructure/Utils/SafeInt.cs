using System;
using System.Security.Cryptography;

namespace Serjbal.Utils
{
    public struct SafeInt
    {
        private int _value;
        private int _salt;
        private byte[] _checksum;
        private static readonly Random _random = new Random();

        public SafeInt(int value)
        {
            _salt = _random.Next();
            _value = value ^ _salt;
            _checksum = CalculateChecksum(value, _salt);
        }

        public bool IsTampered()
        {
            int decryptedValue = _value ^ _salt;

            byte[] currentChecksum = CalculateChecksum(decryptedValue, _salt);

            if (_checksum == null || currentChecksum == null)
                return true;

            if (_checksum.Length != currentChecksum.Length)
                return true;

            for (int i = 0; i < _checksum.Length; i++)
            {
                if (_checksum[i] != currentChecksum[i])
                    return true;
            }

            return false;
        }

        private static byte[] CalculateChecksum(int value, int salt)
        {
            using (var sha = SHA256.Create())
            {
                byte[] data = new byte[8];
                byte[] valueBytes = BitConverter.GetBytes(value);
                byte[] saltBytes = BitConverter.GetBytes(salt);

                Buffer.BlockCopy(valueBytes, 0, data, 0, 4);
                Buffer.BlockCopy(saltBytes, 0, data, 4, 4);

                return sha.ComputeHash(data);
            }
        }

        public override bool Equals(object obj) => (int)this == (int)obj;

        public override int GetHashCode() => ((int)this).GetHashCode();

        public override string ToString() => ((int)this).ToString();

        public static implicit operator int(SafeInt safeInt) => safeInt._value ^ safeInt._salt;

        public static implicit operator SafeInt(int normalInt) => new SafeInt(normalInt);

        public static bool operator ==(SafeInt left, SafeInt right) => (int)left == (int)right;
        public static bool operator !=(SafeInt left, SafeInt right) => (int)left != (int)right;
        public static SafeInt operator +(SafeInt left, SafeInt right) => new SafeInt((int)left + (int)right);
        public static SafeInt operator -(SafeInt left, SafeInt right) => new SafeInt((int)left - (int)right);
        public static SafeInt operator *(SafeInt left, SafeInt right) => new SafeInt((int)left * (int)right);
        public static SafeInt operator /(SafeInt left, SafeInt right) => new SafeInt((int)left / (int)right);
    }
}