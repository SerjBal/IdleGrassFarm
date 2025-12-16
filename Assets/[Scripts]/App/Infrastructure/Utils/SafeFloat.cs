using System;
using System.Security.Cryptography;

namespace Serjbal.Utils
{
    public struct SafeFloat
    {
        private int _value;
        private int _salt;
        private byte[] _checksum;
        private static readonly Random _random = new Random();

        public SafeFloat(float value)
        {
            _salt = _random.Next();
            int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            _value = intValue ^ _salt;
            _checksum = CalculateChecksum(intValue, _salt);
        }

        public bool IsTampered()
        {
            int decryptedValue = _salt ^ _value;
            
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

        public override bool Equals(object obj) => (float)this == (float)obj;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => ((float)this).ToString();

        public static implicit operator float(SafeFloat safeFloat)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(safeFloat._salt ^ safeFloat._value), 0);
        }

        public static implicit operator SafeFloat(float normalFloat) => new SafeFloat(normalFloat);
        
        public static bool operator ==(SafeFloat left, SafeFloat right) => (float)left == (float)right;
        public static bool operator !=(SafeFloat left, SafeFloat right) => (float)left != (float)right;
        public static SafeFloat operator +(SafeFloat left, SafeFloat right) => new SafeFloat((float)left + (float)right);
        public static SafeFloat operator -(SafeFloat left, SafeFloat right) => new SafeFloat((float)left - (float)right);
        public static SafeFloat operator *(SafeFloat left, SafeFloat right) => new SafeFloat((float)left * (float)right);
        public static SafeFloat operator /(SafeFloat left, SafeFloat right) => new SafeFloat((float)left / (float)right);
    }
}