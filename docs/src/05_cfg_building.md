## Создание из ББЛ CFG

Был создан класс ```CFGraph```, который представляет собой граф потока управления. Он базируется на библиотеке [QuickGraph](https://quickgraph.codeplex.com/), которая предоставляет понятный и многофункциональный интерфейс работы с графами.

```
public class CFGraph
{
    public BidirectionalGraph<CFGNode, Edge<CFGNode>> graph =
        new BidirectionalGraph<CFGNode, Edge<CFGNode>>();
        
    public readonly List<IBaseBlock> Blocks;
        
    public CFGraph(List<IBaseBlock> blocks);
    public CFGNode GetRoot();
    public int NumberOfVertices();
    public IEnumerable<CFGNode> GetVertices();
}
```

Граф потока управления строится из базовых блоков следующим образом:
1. Для каждого базового блока строим узел его содержащий: ```CFGNode```. В отличии от базового блока, узлы хранят ссылка на следующие базовые блоки.
1. Добавляем в граф все узлы
2. Основываясь на связях в узлах, добавляем в граф ребра


```
List<CFGNode> cfg_nodes = new List<CFGNode>(blocks.Count);
for (int i = 0; i < blocks.Count; i++)
{
    cfg_nodes.Add(new CFGNode(blocks[i]));
}
/// Create connections between CFGNode

/// Create graph
graph.AddVertexRange(cfg_nodes);
foreach (var node in cfg_nodes)
{
    if (node.directChild != null)
    {
        graph.AddEdge(new Edge<CFGNode>(node, node.directChild));
    }
    
    if (node.gotoNode != null)
    {
        graph.AddEdge(new Edge<CFGNode>(node, node.gotoNode));
    }
}
```


#### Входные данные

Конструктору класса ```CFGraph``` подаётся на вход список базовых блоков 
трёхадресного кода (`List<IBaseBlock> blocks`).

#### Выходные данные 

Экземпляр класса ```CFGraph``` хранит в себе граф из библиотеки - ```BidirectionalGraph<CFGNode, Edge<CFGNode>> graph```, а также прочую служебную информацию необходимую для дальнейших преобразований.

#### Пример применения  

```
var blocks = LinearToBaseBlock.Build(code);
var cfg = new CFGraph(blocks);
```
