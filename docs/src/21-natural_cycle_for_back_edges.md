## Для каждого обратного ребра найти естественный цикл

### Общие сведения

Ребро (a -> b) является обратным, если b доминирует над a.
Для обратного ребра (a -> b) естественный цикл - множество узлов, которые могут достичь a, не проходя через b.

### Постановка задачи
Для каждого обратного ребра необходимо найти его естественный цикл

### Входные данные
Для решения используются граф потока управления и его обратные ребра

### Выходные данные
Функция возвращает естественные циклы для всех обратных ребер графа

### Решение задачи
Для каждого обратного ребра (a -> b) запустим алгоритм DFS, начиная с узла a по обратному графу , причём изначально b помечается уже посещенный, чтобы через него не проходил поиск
 
В класс графа потока управления (CFGRaph) добавлена функция, находящая все обратные ребра, и определяющая для них естественные циклы
```cs
public List<List<CFGNode>> getNaturalCyclesForBackwardEdges()
{
	return EdgeTypes
	  .Where(edgeType => edgeType.Value == EdgeType.Retreating)
      .Select(edgeType => edgeType.Key)
      .Where(edge => isBackwardEdge(edge))
      .Select(edge => 
         naturalCycleGraph.findBetween(edge.Source, edge.Target))
      .ToList();
}
```
Для определения естественного цикла, был реализован алгоритм DFS, который запоминает все пройденные узлы

```cs
public List<CFGNode> findBetween(CFGNode from, CFGNode to)
{
    visitedNodes.Clear();
    visitedNodes.Add(to);

    backDFS(from);

    return visitedNodes;
}
private void backDFS(CFGNode currentNode)
{
    if (!visitedNodes.Contains(currentNode))
    visitedNodes.Add(currentNode);
    
    getNodes(currentNode).ToList()
        .Where(node => !visitedNodes.Contains(node)).ToList()
        .ForEach(node => backDFS(node));
}
```