using System;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        public static class GCore
        {
            public static readonly Location Amsterdam       = Create(1,  "am2");
            public static readonly Location Frankfurt       = Create(2,  "fr2"); // fr5
            public static readonly Location London          = Create(3,  "tl");
            public static readonly Location Luxembourg      = Create(4,  "ed");
            public static readonly Location Madrid          = Create(5,  "cm");
            // public static readonly Location Milan        = Create(6,  "?");
            public static readonly Location Paris           = Create(7,  "cp");
            public static readonly Location Prague          = Create(8,  "cec");
            public static readonly Location Stockholm       = Create(9,  "ts");
            public static readonly Location Warsaw          = Create(10, "pl1");                // PL
            // public static readonly Location Barnaul      = Create(11, "?");                  // RU
            public static readonly Location Ekaterinburg    = Create(12, "dh");                 // RU (aka Yekaterinburg)
            public static readonly Location Kazan           = Create(13, "bl");                 // RU
            public static readonly Location Khabarovsk      = Create(14, "rc");                 // RU
            public static readonly Location Kiev            = Create(15, "gn");                 // _
            public static readonly Location Krasnoyarsk     = Create(16, "ct");                 // RU
            public static readonly Location Minsk           = Create(17, "blt");                // _
            public static readonly Location Moscow          = Create(18, "m9");                 // RU
            // public static readonly Location Novosibirsk  = Create(19, "?");                  // RU
            // public static readonly Location Orel         = Create(20, "?");                  // RU
            public static readonly Location Pavlodar        = Create(21, "kt" );
            // public static readonly Location RostovonDon  = Create(22, "?");                  // RU
            public static readonly Location Ufa             = Create(23, "ufn");
            // public static readonly Location Vladivostok  = Create(24, "?");                  // RU
            public static readonly Location Bishkek         = Create(25, "ky");

            // 2017-03-06 (Aquired Skypark CDN) 
            // - announced Seoul, Sydney, Vienna, São Paulo, Chicago, Seattle, and Los Angeles PoPs

            public static readonly Location Sydney          = Create(26, "sy4");                // AU
            public static readonly Location Ashburn         = Create(27, "dc3");                // US
            public static readonly Location Dallas          = Create(28, "td");                 // US
            public static readonly Location Miami           = Create(29, "mi1");                // US
            public static readonly Location SanJose         = Create(30, "sv4");                // US
            public static readonly Location Sunnyvale       = Create(31, "sv5");                // US
            public static readonly Location Chicago         = Create(32, "ch1");                // US
            public static readonly Location Seattle         = Create(33, "tse");                // US
            public static readonly Location HongKong        = Create(34, "hk2"); // 2017-06-22
            public static readonly Location Tokyo           = Create(35, "cc1"); // 2017-06-22
            public static readonly Location Seoul           = Create(36, "kx");  // 2017-07-08
            public static readonly Location Singapore       = Create(37, "sg1"); // ???
            public static readonly Location TelAviv         = Create(38, "bzi"); // 2017-08-17 | 37th
            public static readonly Location SaintPetersburg = Create(39, "k12"); // 2017-12-11 | 11th in Russia

            // Coming...
            // - Denver
            // - Los Angeles
            // - Toronto
            // - São Paulo
            // - Vienna

            // tii (TR/Istanbul)
            // ttkb (RU/Yekaterinburg)

            // tde (Omaha?)

            public static Location Get(int id)
            {
                foreach (var location in All)
                {
                    if (location.Id == id) return location;
                }

                throw ResourceError.NotFound(ResourceProvider.GCore, ResourceTypes.Location, id.ToString());
            }

            public static Location Get(string name)
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                switch (name)
                { 
                    case "am2"  : return Amsterdam;
                    case "bl"   : return Kazan;
                    case "blt"  : return Minsk;
                    case "bzi"  : return TelAviv;
                    case "cc1"  : return Tokyo;
                    case "cec"  : return Prague;    
                    case "ch1"  : return Chicago;
                    case "cm"   : return Madrid;  
                    case "cp"   : return Paris;
                    case "ct"   : return Krasnoyarsk;
                    case "dc3"  : return Ashburn;
                    case "dc11" : return Ashburn;
                    case "dh"   : return Ekaterinburg; 
                    case "dt"   : return Moscow;
                    case "ed"   : return Luxembourg;  
                    case "fr2"  : return Frankfurt;
                    case "fr5"  : return Frankfurt;
                    case "gn"   : return Kiev;
                    case "hk2"  : return HongKong;
                    case "kt"   : return Pavlodar;
                    case "k12"  : return SaintPetersburg;
                    case "kx"   : return Seoul;
                    case "ky"   : return Bishkek;
                    case "m9"   : return Moscow;
                    case "mts"  : return Moscow; // ???
                    case "mi1"  : return Miami;
                    case "nkf"  : return Amsterdam;
                    case "pl1"  : return Warsaw;
                    case "rc"   : return Khabarovsk;
                    case "sg1"  : return Singapore;
                    case "sg2"  : return Singapore;
                    case "sv4"  : return SanJose;    // San Jose
                    case "sv5"  : return Sunnyvale;  
                    case "sy4"  : return Sydney;
                    case "td"   : return Dallas;
                    case "tl"   : return London;
                    case "ts"   : return Stockholm;
                    case "tse"  : return Seattle;
                    case "ttkb" : return Ekaterinburg;
                    case "ufn"  : return Ufa;                   
                }

                throw ResourceError.NotFound(ResourceProvider.GCore, ResourceTypes.Location, name);
            }

            private static Location Create(int number, string name)
            {
                var locationId = LocationId.Create(ResourceProvider.GCore, (ushort)number, 0);

                return new Location(locationId, name);
            }

            public static readonly Location[] All = new[] {
                Amsterdam,
                Frankfurt,
                London,
                Luxembourg,
                Madrid,
                // Milan,
                Paris,
                Prague,
                Stockholm,
                Warsaw,
                // Barnaul,
                Ekaterinburg,
                Kazan,
                Khabarovsk,
                Kiev,
                Krasnoyarsk,
                Minsk,
                Moscow,
                // Novosibirsk,
                // Orel,
                Pavlodar,
                // RostovonDon,
                Ufa,
                // Vladivostok,
                Bishkek,
                Sydney,
                Ashburn,
                Dallas,
                Miami,
                SanJose,
                Sunnyvale,
                Chicago,
                Seattle,
                HongKong,
                Tokyo,
                Seoul,
                Singapore,
                TelAviv,
                SaintPetersburg
            };
        }
    }  
}

// https://lg.gcore.lu/lg.cgi