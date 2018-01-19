using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public class MachineTypeService : IMachineTypeService
    {
        private readonly PlatformDb db;

        public MachineTypeService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async ValueTask<IMachineType> GetAsync(long id)
        {
            var typeId = new MachineTypeIdInfo { Value = id };

            if (typeId.ProviderId == 2) // aws
            {
                return AwsInstanceType.Get(id);
            }
            else
            {
                return await db.MachineTypes.FindAsync(id);
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    internal struct MachineTypeIdInfo
    {
        [FieldOffset(4)]
        public int ProviderId;

        [FieldOffset(0)]
        public long Value;
    }
}
