using GraphParser;
using Newtonsoft.Json;

RedactorConfig rconfig = Parsing.parse("graph1.graph");

Config[] configs = new Config[] {
    new Config(),
    new Config(),
    new Config()
};

Dictionary<int, List<RedactorEdge>> adj_lists = new Dictionary<int, List<RedactorEdge>>();

/*
 * 1. Парсим .graph
 * 2. Составляем список из вершин
 * 3. Составляем список из рёбер (А лучше список смежности по первой вершине ребра)
   4. Берём ребро
   5. Добавляем обе вершины ребра в рёбра первого сервера. Создаём список смежности в конфиге
   6. Смотрим на первую вершину
   7. Если она не граничащая, рассматриваем инцидентные с ней рёбра
   8. Если граничащая, переходим ко второй вершине
   9. Повторяем, пока не закончатся рёбра с неграничащими вершинами

 */

// Создаем списки смежности
foreach (RedactorEdge re in rconfig.edges)
{
    if (!adj_lists.ContainsKey(re.vertex1))
    {
        adj_lists[re.vertex1] = new List<RedactorEdge>();
    }
    if (!adj_lists.ContainsKey(re.vertex2))
    {
        adj_lists[re.vertex2] = new List<RedactorEdge>();
    }
    adj_lists[re.vertex1].Add(re);
    RedactorEdge re2 = new RedactorEdge();
    re2.vertex1 = re.vertex2;
    re2.vertex2 = re.vertex1;
    re2.weight = re.weight;
    adj_lists[re.vertex2].Add(re2);
}

foreach (var pair in adj_lists)
{
    Console.Write(pair.Key + ": ");
    foreach (RedactorEdge re in pair.Value)
    {
        Console.Write("(" + re.vertex1 + ", " + re.vertex2 + ", " + re.weight + ")");
    }
    Console.WriteLine();
}

int server_id = 0;
int current_vertexid = 0;
int vertex1, vertex2;
CEdge edge1, edge2;
Queue<int> queue = new Queue<int>();
List<int> marked = new List<int>();
int ci = 0, i = 0;
queue.Enqueue(0);
Dictionary<int, int> ids = new Dictionary<int, int>();
while (queue.Count > 0)
{
    current_vertexid = queue.Dequeue();
    vertex1 = int.Parse(rconfig.vertices[current_vertexid].name);
    if (!marked.Contains(current_vertexid)) {
        configs[server_id].vertexes.Add(new CVertex(vertex1));
        configs[server_id].adj_list.Add(new CAdjVertex(vertex1));
        marked.Add(current_vertexid);
        ids[current_vertexid] = i;
        i++;
    }
    if (adj_lists.ContainsKey(current_vertexid))
    { 
        foreach (RedactorEdge re in adj_lists[current_vertexid])
        {
            vertex2 = int.Parse(rconfig.vertices[re.vertex2].name);
            if (!marked.Contains(re.vertex2))
            {
                configs[server_id].vertexes.Add(new CVertex(vertex2));
                configs[server_id].adj_list.Add(new CAdjVertex(vertex2));
                marked.Add(re.vertex2);
                queue.Enqueue(re.vertex2);
                ids[re.vertex2] = i;
                i++;
            }
            edge1 = new CEdge(vertex2, int.Parse(re.weight));
            configs[server_id].adj_list[ids[current_vertexid]].edges.Add(edge1);
        }
    }
}

using (StreamWriter sw = new StreamWriter("graph.json"))
{
    sw.WriteLine(JsonConvert.SerializeObject(configs[0], Formatting.Indented));
}