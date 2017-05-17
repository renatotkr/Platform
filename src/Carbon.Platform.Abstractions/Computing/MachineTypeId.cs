using System;
using System.Runtime.InteropServices;
using System.Text;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct MachineTypeId
    {
        // e.g. C, T, M, X, I               | Basic | Standard |  
        [FieldOffset(0)]
        internal byte ClassId1;

        // e.g. I, S
        [FieldOffset(1)]
        internal byte ClassId2;

        // 1, 2, 3, 4, 5, ...
        [FieldOffset(2)]
        internal byte Generation;

        [FieldOffset(3)]
        internal byte MachineSize;

        [FieldOffset(4)]
        internal int ProviderId;

        [FieldOffset(0)]
        public long Value;

        public static implicit operator long(MachineTypeId id) => id.Value;
    }

    public class AwsMachineType : IMachineType
    {
        internal AwsMachineType(long id, string name)
        {
            Id   = id;
            Name = name;
        }

        public string Name { get; }

        public long Id { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion
    }

    public static class AwsInstanceType
    {
        public static IMachineType Get(long id)
        {
            return new AwsMachineType(id, GetName(id));
        }

        public static IMachineType Get(string name)
        {
            return new AwsMachineType(GetId(name), name);
        }

        public static string GetName(long id)
        {
            var a = new MachineTypeId { Value = id };

            var sb = new StringBuilder();

            if (a.ClassId1 != 0)
            {
                sb.Append(alphabet[a.ClassId1 - 1]);
            }

            if (a.ClassId2 != 0)
            {
                sb.Append(alphabet[a.ClassId2 - 1]);
            }

            if (a.Generation != 0)
            {
                sb.Append(a.Generation);
            }

            sb.Append(".");

            sb.Append(GetSizeName(a.MachineSize));

            return sb.ToString();
        }

        public static MachineTypeId GetId(string name)
        {
            // cg1.4xlarge
            var id = new MachineTypeId();

            var parts = name.Split('.');

            var a = parts[0];
            var b = parts[1];

            if (a.Length == 3)
            {
                id.ClassId1 = GetLetterId(a[0]);
                id.ClassId2 = GetLetterId(a[1]);
                id.Generation = byte.Parse(a[2].ToString());
            }
            else
            {
                id.ClassId1 = GetLetterId(a[0]);
                id.Generation = byte.Parse(a[1].ToString());
            }

            id.MachineSize = GetSizeId(b);
            id.ProviderId = 1;

            return id;
        }

        private static readonly char[] alphabet = {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        // hi1, hs1

        private static byte GetLetterId(char letter)
        {
            return (byte)(Array.IndexOf(alphabet, char.ToLower(letter)) + 1);
        }

        private static byte GetSizeId(string name)
        {
            switch(name)
            {
                case "nano"     : return 0; // 0.25
                case "micro"    : return 1; // 0.5
                case "small"    : return 2; // 1
                case "medium"   : return 3; // 2
                case "large"    : return 4;  
                case "xlarge"   : return 5;
                case "2xlarge"  : return 6;
                case "4xlarge"  : return 7;
                case "8xlarge"  : return 8;
                case "16xlarge" : return 16;
                case "32xlarge" : return 32;
                default         : throw new Exception("unknown name: " + name);
            }
        }

        private static string GetSizeName(byte id)
        {
            switch (id)
            {

                case 0  : return "nano";
                case 1  : return "micro"; 
                case 2  : return "small";
                case 3  : return "medium";
                case 4  : return "large";
                case 5  : return "xlarge";
                case 6  : return "2xlarge";
                case 7  : return "4xlarge";
                case 8  : return "8xlarge";
                case 16 : return "16xlarge";
                case 32 : return "32xlarge";
                default : throw new Exception("unknown id:" + id);
            }
        }
    }
}