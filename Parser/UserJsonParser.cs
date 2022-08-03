using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Texode_test_step_analyzer.Model;

namespace Texode_test_step_analyzer.Parser
{
    public class UserJsonParser
    {
        public void WriteUserToJsonString(User user, string filePath)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(writer))
                    { 
                        serializer.Serialize(jsonWriter, user);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public HashSet<User> GetUsersFromJsonFiles(string[] filesPaths)
        {
            HashSet<User> users = new HashSet<User>();
            if(filesPaths == null) return users;
            
            foreach(string filePath in filesPaths)
            {
                List<DayResult> dayResults = GetDayResultsFromJSon(filePath);
                AddDayResultsToUsers(dayResults, users);
            }
            return users;
        }

        private List<DayResult> GetDayResultsFromJSon(string filePath)
        {
            List<DayResult> userDayResults = new List<DayResult>();
            if (string.IsNullOrEmpty(filePath)) return userDayResults;


            string fileName = "";
            int dayNumber = 0;
            try
            {
                fileName = filePath[(filePath.LastIndexOf("\\") + 1)..];
                Regex regex = new Regex("\\d+");
                dayNumber = int.Parse(regex.Match(fileName).Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            

            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    using (JsonReader jsonReader = new JsonTextReader(reader))
                    {
                        userDayResults = serializer.Deserialize<List<DayResult>>(jsonReader);
                    }
                }
                userDayResults.ForEach(dr => dr.Day = dayNumber);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return userDayResults;
        }

        private void AddDayResultsToUsers(List<DayResult> userDayResults, HashSet<User> users)
        {
            if (userDayResults == null || users == null) return;

            foreach (DayResult dr in userDayResults)
            {
                if (dr.User == null) continue;
                User user = new User(dr.User, dr);

                bool isContains = users.TryGetValue(user, out User actualUser);

                if (isContains)
                {
                    actualUser.AddDayResult(dr);
                }
                else
                {
                    users.Add(user);
                }
            }
        }
    }
}
