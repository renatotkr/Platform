using System;
using System.IO;

using ProtoBuf;

namespace Carbon.Cloud.Logging
{
    public class RequestLog : IDisposable
    {
        private readonly Stream stream;

        public RequestLog(Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public void Append(Request request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!stream.CanWrite)
                throw new Exception("Stream is readonly");

            #endregion

            using (var requestDataStream = new MemoryStream())
            {
                Serializer.Serialize(requestDataStream, request);

                requestDataStream.Position = 0;

                Varint.Encode((ulong)requestDataStream.Length, stream); // Write the length

                requestDataStream.WriteTo(stream);
            }
        }

        public bool TryRead(out Request request)
        {
            if (IsEof)
            {
                request = null;

                return false;
            }

            int dataSize = (int)Varint.Read(stream);

            var dataBuffer = new byte[dataSize];

            stream.Read(dataBuffer, 0, dataSize);

            using (var ms = new MemoryStream(dataBuffer))
            {
                request = Serializer.Deserialize<Request>(ms);

                return true;
            }
        }

        public bool IsEof => stream.Position >= stream.Length;

        public void Dispose()
        {
            this.stream.Dispose();
        }
    }
}