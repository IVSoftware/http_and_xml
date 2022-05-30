using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace http_and_xml
{
    class Program
    {
        static void Main(string[] args)
        {
            string text;
            using (StreamReader sr = new StreamReader(
                new WebClient().OpenRead("http://ergast.com/api/f1/2022")))
            {
                text = sr.ReadToEnd();
            }
            var json =  JsonConvert.SerializeXNode(
                    XElement.Parse(text), 
                    Formatting.Indented); // Pleasing to the eye
            var wrapper = (Wrapper)JsonConvert.DeserializeObject<Wrapper>(json);

            Console.WriteLine($"Races for the {wrapper.MRData.RaceTable.Season} season.");
            Console.WriteLine($"Found {wrapper.MRData.RaceTable.Races.Length} races");
            var race0 = wrapper.MRData.RaceTable.Races[0];
            Console.WriteLine($"{race0.RaceName} is taking place at {race0.Circuit.CircuitName}");
        }
	}

    public class Wrapper
    {
        public MRData MRData { get; set; }
    }

    public class MRData
	{
        public RaceTable RaceTable{ get; set; }
    }
    public class RaceTable
    {
        [JsonProperty(propertyName:"@season")]
        public string Season { get; set; }

        [JsonProperty(propertyName: "Race")]
        public Race[] Races { get; set; }
    }

    #region O P    C l a s s e s    u s e d    a s - i s
    public class Race
    {
        public string RaceName { set; get; }
        public Circuit Circuit { set; get; }
        public string Date;
        public string Time;
        public StepDateTime FirstPractice { set; get; }
        public StepDateTime SecondPractice { set; get; }
        public StepDateTime ThirdPractice { set; get; }
        public StepDateTime Qualifying { set; get; }
        public StepDateTime Sprint { set; get; }
    }
    public class Circuit
    {
        public string CircuitName { set; get; }
        public Location Location { set; get; }
    }
    public class Location
    {
        public double lat { set; get; }
        [XmlAttribute("long")]
        public double Long { set; get; }
        public string Locality { set; get; }
        public string Country { set; get; }
    }
    public class StepDateTime
    {
        public string Date { set; get; }
        public string Time { set; get; }
    }
    #endregion O P    C l a s s e s    u s e d    a s - i s
}
