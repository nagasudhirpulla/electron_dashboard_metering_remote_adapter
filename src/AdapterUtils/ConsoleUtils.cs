using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AdapterUtils
{
    public class ConsoleUtils
    {
        public static void FlushChunks(string outStr, int chunkSize = 50000)
        {
            for (int chunkIter = 0; chunkIter < outStr.Length; chunkIter += chunkSize)
            {
                Console.Write(outStr.Substring(chunkIter, Math.Min(chunkSize, outStr.Length - chunkIter)));
                Console.Out.Flush();
            }
        }

        public static void FlushMeasData(string measId, string measName, string measDescription)
        {
            string outStr = JsonConvert.SerializeObject(new List<string> { measId, measName, measDescription });
            FlushChunks(outStr);
        }
    }
}