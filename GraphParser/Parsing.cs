using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GraphParser
{
    public class CVertex
    {
        public int id { get; set; }
        public string info { get; set; }
        public List<int> adj_v { get; set; }
        public CVertex(int id, string info="", List<int> adj_v=null)
        {
            this.id = id;
            this.info = info;
            this.adj_v = adj_v;
        }        
    }
    public class CServer
    {
        public int id { get; set; }
        public string address { get; set; }
    }

    public class CEdge
    {
        public int id { get; set; }
        public int weight { get; set; }
    }
    public class CAdjVertex
    {
        public int id { get; set; }
        public List<CEdge> edges { get; set; }
    }
     public class Config
    {
        public List<CVertex> vertexes { get; set; }
        public List<CServer> servers { get; set; }
        public List<CAdjVertex> adj_list { get; set; }
        public Config()
        {
            vertexes = new List<CVertex>();
            servers = new List<CServer>();
            adj_list = new List<CAdjVertex>();
        }
    }
    class RedactorVertex
    {
        public string name { get; set; }
        public string color { get; set; }
    }
    class RedactorEdge
    {
        public int vertex1 { get; set; }
        public int vertex2 { get; set; }
        public string weight { get; set; }
    }
    class RedactorConfig
    {
        public List<RedactorVertex> vertices { get; set; }
        public List<RedactorEdge> edges { get; set; }
    }
    class Parsing
    {
        public static RedactorConfig parse(string filename)
        {
            RedactorConfig config;
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                config = (RedactorConfig) serializer.Deserialize(file, typeof(RedactorConfig));
            }
            return config;
        }
    }
}
