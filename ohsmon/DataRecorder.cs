using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ohsmon
{
    public class DataRecorder
    {
        private string _filename;
        public DataRecorder(string filename)
        {
            _filename = filename;
        }

        public bool AppendData(string data)
        {
            try
            {
                FileStream fs = new FileStream(_filename, FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(data);
                sw.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR - " + e.Message);
            }
          
            return true;
        }


    }
}
