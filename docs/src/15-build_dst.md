## Построить на CFG глубинное остовное дерево, перенумеровав элементы в порядке обратном посфиксному

Был создан класс ```DepthSpanningTree```, который представляет собой остовное дерево CFG. Он базируется на библиотеке [QuickGraph](https://quickgraph.codeplex.com/), которая предоставляет понятный и многофункциональный интерфейс работы с графами.

```cs
  public class DepthSpanningTree
  {
      public Dictionary<CFGNode, int> Numbers { get; }
      public BidirectionalGraph<CFGNode, Edge<CFGNode>> Tree { get; }
      public DepthSpanningTree(CFGraph cfg);
      
      // Finds back path from source to target, true if it is.
      public bool FindBackwardPath(CFGNode source, CFGNode target);

      public override string ToString();
   }
}
```

Остовное дерево CFG строится из ```CFGraph``` следующим образом:
узлы графа потока управления обходятся в прямом порядке, в процессе прохода они нумеруются.

Конструктор остова запускает рекурсивную функцию от корня ```CFGraph```:
```cs
private void BuildTree(CFGNode текущий_узел, ref int счетчик)
{
	1. Пометить текущий узел как просмотренный
	2. Если нет потомков, то занумеровать и выйти
	3. Добавить в остов текущий узел
	4. Для каждого потомка:
		1. Если он не посещен:
			1. Если его нет в остове, добавить его
			2. Добавить ребро к потомку
			3. BuildTree(потомок, счетчик)
		2. Занумеровать потомка
}
```


#### Входные данные

Конструктору класса ```DepthSpanningTree``` подаётся на вход ```CFGraph```

#### Выходные данные 

Экземпляр класса ```DepthSpanningTree``` хранит в себе граф из библиотеки - ```BidirectionalGraph<CFGNode, Edge<CFGNode>> Tree```, а также прочую служебную информацию необходимую для дальнейших преобразований.

#### Пример применения  

```
var blocks = LinearToBaseBlock.Build(code);
var cfg = new CFGraph(blocks);
var dst = new DepthSpanningTree(cfg);
```
