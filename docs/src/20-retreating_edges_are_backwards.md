## Являются ли все ли отступающие ребра обратными

### Общие сведения

Ребро (a -> b) является обратным, если b доминирует над a.
Узел a доминирует над узлом b, если любой путь от входного узла графа к b проходит через a.

### Постановка задачи
Необходимо было проверить, являются ли все отступающие рёбра обратными

### Входные данные
Для решения используются отступающие рёбра графа (классификация была проведена в другой задаче) и дерево доминаторов (было построено в другой задаче)

### Выходные данные
Функция возвращает True, если все отступающие ребра обратные, иначе False

### Решение задачи
Из определений следует, что для решения задачи необходимо проверить, что для всех отступающих ребер (a -> b) узел b доминирует над узлом a.

Для решения в класс графа управления (CFGraph) был добавлен метод allRetreatingEdgesAreBackwards()
```cs
        public bool allRetreatingEdgesAreBackwards()
        {
            return EdgeTypes.Where(
            edgeType => edgeType.Value == EdgeType.Retreating)
                .Select(edgeType => edgeType.Key).ToList()
                .All(edge => isDominate(edge.Target, edge.Source));
        }
```

Для определения, является ли узел a доминирующим по отношению к узлу b,  используется дерево доминаторов. Если существует путь от узла a к узлу b, то a доминирует над b.

Для поиска пути в дереве доминаторов был реализован алгоритм DFS.

```cs
        private bool isWayExists(DominatorTreeNode from, DominatorTreeNode to)
        {
            if (from.Equals(to))
                return true;

            visited.Add(from);

            var nodes = graph.Edges.Where(dtn => dtn.Source.Equals(from))
    .Select(dtn => dtn.Target)
    .Where(dtn => !visited.Contains(dtn)).ToList();

            foreach (var node in nodes)
                if (isWayExists(node, to))
                    return true;
            return false;
        }
```