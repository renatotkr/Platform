using System;
using System.Text;

using Carbon.Extensions;

namespace Carbon.Platform.Computing
{
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

            sb.Append('.');

            sb.Append(GetSizeName(a.MachineSize));

            return sb.ToString();
        }

        public static MachineTypeId GetId(string name)
        {
            // cg1.4xlarge
            var id = new MachineTypeId();

            var parts = name.Split(Seperators.Period);

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
            switch (name)
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
                case "9xlarge"  : return 9;
                case "10xlarge":  return 10;
                case "16xlarge" : return 16;
                case "18xlarge" : return 18;
                case "32xlarge" : return 32;
                default         : throw new Exception("unknown size: " + name);
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
                default : return id.ToString() + "xlarge";
            }
        }
    }
}