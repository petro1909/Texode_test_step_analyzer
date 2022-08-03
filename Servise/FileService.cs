using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texode_test_step_analyzer
{
    public class FileService
    {
        public string[] GetAllFilesPathsFromSpecifiedFolder(string folderPath)
        {
            try
            {
                string[] filePaths = Directory.GetFiles(folderPath);
                return filePaths;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return default;
            }
        }

    }
}
