using System;

namespace AdapterUtils
{
    public class AdapterParams
    {
        public string MeasId { get; set; } = "AC-91";
        public DateTime FromTime { get; set; } = new DateTime(2019,12,1);
        public DateTime ToTime { get; set; } = new DateTime(2019, 12, 2);
        public string RequestType { get; set; } = "data";
        public bool IncludeQuality { get; set; } = false;

        public static AdapterParams ParseArgs(string[] args)
        {
            // read arguments for meas_id, from_time, to_time, host, port, path, username, password, ref_meas_id

            AdapterParams ap = new AdapterParams();

            for (int argIter = 0; argIter < args.Length; argIter++)
            {
                // Console.WriteLine(args[argIter]);
                if (args[argIter] == "--meas_id")
                {
                    ap.MeasId = args[argIter + 1];
                }
                else if (args[argIter] == "--from_time")
                {
                    ap.FromTime = TimeUtils.FromMillisecondsSinceUnixEpoch(long.Parse(args[argIter + 1]));
                }
                else if (args[argIter] == "--to_time")
                {
                    ap.ToTime = TimeUtils.FromMillisecondsSinceUnixEpoch(long.Parse(args[argIter + 1]));
                }
                else if (args[argIter] == "--show_meas_picker")
                {
                    ap.RequestType = RequestTypes.PickMeas;
                }
                else if (args[argIter] == "--config_adapter")
                {
                    ap.RequestType = RequestTypes.Config;
                }
                else if (args[argIter] == "--include_quality")
                {
                    ap.IncludeQuality = true;
                }
            }
            return ap;
        }
    }
}