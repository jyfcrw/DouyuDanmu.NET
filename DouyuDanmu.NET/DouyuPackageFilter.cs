using System;
using System.IO;
using SuperSocket.ProtoBase;

namespace DouyuDanmu
{
    class DouyuPackageFilter : FixedHeaderReceiveFilter<DouyuPackageInfo>
    {
        public DouyuPackageFilter() : base(DouyuPackageInfo.HeaderFullSize)
        {
        }

        protected override int GetBodyLengthFromHeader(IBufferStream stream, int size)
        {
            var reader = new BinaryReader(stream.GetCurrentStream());
            var pSize = reader.ReadInt32();

            return pSize - DouyuPackageInfo.HeaderSize;
        }

        public override DouyuPackageInfo ResolvePackage(IBufferStream stream)
        {
            var reader = new BinaryReader(stream.GetCurrentStream());
            var buffer = reader.ReadBytes((int)stream.Length);
            var package = new DouyuPackageInfo();

            package.Decode(buffer);

            return package;
        }
    }
}
