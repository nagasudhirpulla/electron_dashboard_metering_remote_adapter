using System.Collections.Generic;

namespace AdapterUtils
{
    public static class RequestTypes
    {
        public const string Data = "data";
        public const string PickMeas = "pick_meas";
        public const string Config = "config";
        public static List<string> Methods
        {
            get
            {
                return new List<string> { Data, PickMeas, Config };
            }
        }

    }
}