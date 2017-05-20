## Построение дерева доминаторов

### Общие сведения

**Определение** *Дерево доминаторов* -- это дерево, в котором входной узел является корнем, а каждый узел `d`доминирует только над своими потомками в дереве.

Для каждого узла графа потока управления при помощи итерационного алгоритма для задачи потока данных находятся все *доминаторы* -- такие узлы графа, которые присутствуют в любом пути от начала программы до данного узла. Далее, для каждого узла (кроме корня графа потока управления) выбирается непосредственный доминатор, который и объявляется родителем в дереве доминаторов.

Таким образом, для построения дерева доминаторов на вход алгоритму необходимо подать только граф потока управления.

###Реализация


В реализации данного модуля был использован пакет NuGet QuickGraph. 

Узлами дерева доминаторов являются объекты класса `DominatorTreeNode`:

```csharp
 public class DominatorTreeNode
 {
    public DominatorTreeNode ParentNode;
    public List<DominatorTreeNode> ChildrenNodes;
    public CFGNode CFGNode;
    
    ...
}
```

Сама реализация алгоритма построения дерева выглядит следующим образом:

```csharp
public DominatorTree(CFGraph cfg)
{
	var doms = new DominatorsIterAlg(cfg).Dom; // 1
    var vertices = doms.Keys.Select(x => new DominatorTreeNode(x)).ToList();
    graph.AddVertexRange(vertices); // 2
            
    foreach (var node in vertices) // 3
    {
	    var dominatedBy = doms[node.CFGNode].ToList();
        dominatedBy.Reverse();
        var cfgClosestDominator = dominatedBy.Skip(1).FirstOrDefault(); // 3.1
        if (cfgClosestDominator != null)
        {
	        var domClosestDominator = vertices
		        .FirstOrDefault(x => x.CFGNode == cfgClosestDominator);

            node.ParentNode = domClosestDominator; // 3.2
            domClosestDominator.AddChild(node);

            graph.AddEdge(new Edge<DominatorTreeNode> // 3.3
							(domClosestDominator, node));
		}
    }
}
```
1. Используется итерационный алгоритм нахождения доминаторов.
2. Преобразованные вершины графа потока управления объявляются вершинами дерева доминаторов.
3. Для каждой вершины:
		1) находится ближайший доминатор (непосредственный)
		2) доминатор объявляется предком текущей вершины, а сама вершина -- потомком доминатора
		3) построенная дуга добавляется в дерево доминаторов

### Пример использования
Для использования дерева доминаторов достаточно лишь передать в конструктор класса `DominatorTree` в качестве аргумента граф потока управления программы:
```csharp
var dt = new LYtest.DominatorTree.DominatorTree(cfg);
```

Класс `DominatorTree` предоставляет доступ к корню и вершинам дерева, а также к самой структуре `QuickGraph.BidirectionalGraph`, поэтому можно использовать все реализованные для этого класса алгоритмы. Кроме того, так как информация о предках и потомках узлов дерева хранится в самих узлах, то можно передвигаться по дереву непосредственно.