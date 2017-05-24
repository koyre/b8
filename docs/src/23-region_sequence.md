
## Формирование поседовательности областей R1,...,Rn в восходящем порядке

### Описание задачи

Необходимо построить восходящую последовательность областей R1,...,Rn, где первые k областей R1,...,Rk - области-листья, представляющие собой один узел графа. Затем естественные циклы упорядочиваются изнутри наружу, т.е. начиная с наиболее внутренних циклов. В итоге все естественные циклы сводятся к отельным узлам.

### Входные данные
Приводимый граф потока.
### Выходные данные
Список областей графа потока
### Описание алгоритма
1. Построить список листьев-областей, состоящий из отдельных узлов графа.
2. Неоднократно выбрать естественный цикл L, такой, что если существуют либые естественные циклы, содержащиеся в L, то тела и области этих цклов уже внесены в список. Добавить область тела, состоящую из тела L (без обратных ребер к заголовку L). Затем добавить область цикла L (содержит обратные ребра к заголовку L).
3. Если весь граф не представляет собой естественный цикл, добавляем в конец списка область, состоящую из всего графа потока целиком.

### Описание реализации

Для решения задачи был создан класс `RegionSequence` со следующим интерфейсом

```csharp
public class RegionSequence
{
    List<Region> regions;
    public List<Region> Regions { get { return regions; } }
    public RegionSequence(CFGraph cfg) { ... }
    ...
}
```
Построение последовательности происходит в конструкторе класса, на вход которому подается приведенный граф управления.

На первом этапе строятся области-листья для каждого узла графа.

```csharp
List<CFGNode> allNodes = cfg.GetVertices().ToList();
List<Edge<CFGNode>> edges = cfg.EdgeTypes.Select(e => e.Key).ToList();
foreach (var node in cfg.GetVertices())
{
    var edgesFromNode = edges.FindAll(e => e.Target == node);
    regions.Add(new LeafRegion(node, edges, NextName()));
}
```

Затем ищутся все естествеенные циклы с помощью встроенного метода графа (`getNaturalCyclesForBackwardEdges()`). Также создаются множества `cyclesHeaders` (все заголовки найденных циклов) и `addedCyclesHeaders` (заголовки циклов, которые внесены в список областей) для удобства учета обработанных циклов.

```csharp
var nc = cfg.getNaturalCyclesForBackwardEdges();
List<Edge<CFGNode>> edges = cfg.EdgeTypes.Select(e => e.Key).ToList();
HashSet<CFGNode> cyclesHeaders = new HashSet<CFGNode>(nc.Select(c => c[0]));
HashSet <CFGNode> addedCyclesHeaders = new HashSet<CFGNode>();
```

После чего, пока есть циклы, не добавленные в список областей:
1. Выбираются циклы, подходящие для обработки (заголовок принадлежит множеству заголовков добавленных циклов)
2. Для каждого найденного цикла в список областей добавляется область тела, а затем бласть цикла.
3. Обработанные циклы удаляются из списка недобавленных.

```csharp
while (nc.Count > 0)
{
    List<List<CFGNode>> cyclesToAdd = nc.FindAll(c => c.Skip(1).All(node =>
    {    
        return cyclesHeaders.Contains(node) ? addedCyclesHeaders.Contains(node) : true ;
    }));
    foreach (var cycle in cyclesToAdd)
    {
        var nodes = new HashSet<CFGNode>(cycle);
        AddCycle(cycle, edges, nodes);
        addedCyclesHeaders.Add(cycle[0]);
    }
    nc.RemoveAll(c => addedCyclesHeaders.Contains(c[0]));
}
```

Последний шаг: если граф не представляется собой естественный цикл, то внести в список область, состаящую из всего графа (все узлы и ребра).

```csharp
bool cfgIsNaturalCycle = cyclesHeaders.Contains(allNodes[0]);
if (!cfgIsNaturalCycle)
  regions.Add(new BodyRegion(allNodes[0], allNodes, edges, NextName()));
```

### Пример использования

Для создания последовательности областей нужно создать объект `RegionSequence` и передать на вход граф потока данных.

```csharp
var rs = new RegionSequence(cfg);
```
