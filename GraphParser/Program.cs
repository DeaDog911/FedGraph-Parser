using GraphParser;
using System.Net.Http.Headers;

RedactorConfig rconfig = Parsing.parse("graph.graph");
Config[] configs = new Config[] {
    new Config(),
    new Config(),
    new Config()
};

List<CVertex>[] serversVertices = new List<CVertex>[] {
    new List<CVertex>(),
    new List<CVertex>(),
    new List<CVertex>()
};

List<CEdge>[] serversEdges = new List<CEdge>[] {
    new List<CEdge>(),
    new List<CEdge>(),
    new List<CEdge>()
};

Dictionary<int, List<RedactorEdge>> adj_lists = new Dictionary<int, List<RedactorEdge>>();

/*
 * 1. Парсим .graph
 * 2. Составляем список из вершин
 * 3. Составляем список из рёбер (А лучше список смежности по первой вершине ребра)
 * 4. Проходимся по рёбрам:
 * 4.1. Для первого ребра добавляем обе вершины в список вершин для первого подграфа
 * 4.2. Далее находим все инцидентные ребра для вершин, если они не гранчиащие
 *      и добавляем вторую вершину ребра в список для первого графа
 * 4.3. Если не граничащие вершины закончились, переходим к граничащим.
 * 4.4. Проделываем то же самое, только уже для второго подграфа.
 */

// Создаем списки смежности
foreach (RedactorEdge rv in rconfig.edges)
{
    if (adj_lists.ContainsKey(rv.vertex1))
    {
        adj_lists[rv.vertex1].Add(rv);
    }
    else
    {
        adj_lists.Add(rv.vertex1, new List<RedactorEdge> { rv });
    }
}

int server_i = 0;
foreach (RedactorEdge rv in rconfig.edges)
{
    serversVertices[server_i].Add(new CVertex(rv.vertex1));
    serversVertices[server_i].Add(new CVertex(rv.vertex2));
    
}
