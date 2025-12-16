using System;
using System.Text;

namespace Serjbal.Utils
{
    public struct SafeString
    {
        private byte[] _encryptedValue;
        private int _salt;
        private byte[] _checksum;
        private static readonly Random _random = new Random();

        public SafeString(string value)
        {
            _salt = _random.Next();

            if (string.IsNullOrEmpty(value))
            {
                _encryptedValue = null;
                _checksum = null;
                return;
            }

            byte[] stringBytes = Encoding.UTF8.GetBytes(value);
            _encryptedValue = new byte[stringBytes.Length];

            for (int i = 0; i < stringBytes.Length; i++)
            {
                _encryptedValue[i] = (byte)(stringBytes[i] ^ (_salt >> ((i % 4) * 8)) & 0xFF);
            }

            _checksum = CalculateChecksum(value, _salt);
        }

        public bool IsTampered()
        {
            string currentValue = DecryptInternal();
            byte[] currentChecksum = CalculateChecksum(currentValue, _salt);
            
            if (_checksum == null) return currentChecksum != null;
            if (currentChecksum == null) return true;
            
            for (int i = 0; i < Math.Min(4, _checksum.Length); i++)
            {
                if (_checksum[i] != currentChecksum[i])
                    return true;
            }
            
            return false;
        }

        private static byte[] CalculateChecksum(string value, int salt)
        {
            if (string.IsNullOrEmpty(value)) return null;
            
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] saltBytes = BitConverter.GetBytes(salt);
            
            byte[] checksum = new byte[8];
            
            for (int i = 0; i < checksum.Length; i++)
            {
                int valueIndex = i % valueBytes.Length;
                int saltIndex = i % saltBytes.Length;
                checksum[i] = (byte)(valueBytes[valueIndex] ^ saltBytes[saltIndex] ^ i);
            }
            
            return checksum;
        }

        private string DecryptInternal()
        {
            if (_encryptedValue == null || _encryptedValue.Length == 0)
                return string.Empty;

            byte[] decryptedBytes = new byte[_encryptedValue.Length];

            for (int i = 0; i < _encryptedValue.Length; i++)
            {
                decryptedBytes[i] = (byte)(_encryptedValue[i] ^ (_salt >> ((i % 4) * 8)) & 0xFF);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public override bool Equals(object obj) => (string)this == (string)obj;
        public override int GetHashCode() => ((string)this).GetHashCode();
        public override string ToString() => (string)this;

        public static implicit operator string(SafeString safeString)
        {
            return safeString.DecryptInternal();
        }

        public static implicit operator SafeString(string normalString) => new SafeString(normalString);
        public static bool operator ==(SafeString a, SafeString b) => (string)a == (string)b;
        public static bool operator !=(SafeString a, SafeString b) => (string)a != (string)b;
    }
}