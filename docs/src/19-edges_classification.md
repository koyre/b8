## Классификация ребер графа: наступающие, отступающие, поперечные

Задача заключается в том, чтобы классифицировать все ребра ```CFGraph```. 

Ребра бывают трех типов:
1. Наступающее ребро: если в [DST](https://github.com/DeKoyre/b8/blob/master/docs/src/15-build_dst.md) существует такое же ребро
2. Отступающее ребро: если [DST](https://github.com/DeKoyre/b8/blob/master/docs/src/15-build_dst.md) существует прямой путь из target в source
3. Поперечное ребро в иных случаях


Для решения этой задачи были созданы следующие классы:
```cs
public enum EdgeType
{
   Coming = 1,
   Retreating = 2,
   Cross = 3
}

public class EdgeTypes : Dictionary<Edge<CFGNode>, EdgeType>
{
   public override string ToString();
}
```

Класс ```CFGraph``` был расширен:
```cs
public class CFGraph
{
	...
	public EdgeTypes EdgeTypes { get; }
	...
}
```

Классификация ребер  ```CFGraph``` происходит следующим образом:
1. Строим глубинное островное дерево для ```CFGraph```
2. Проходим по всем ребрам в ```CFGraph``` и классифицируем их согласно определению

Она вызывается в конструкторе CFGraph.


#### Входные данные

Конструктору класса ```CFGraph``` подаётся на вход список базовых блоков 
трёхадресного кода (`List<IBaseBlock> blocks`).

#### Выходные данные 

Экземпляр класса ```CFGraph``` хранит в себе граф из библиотеки - ```BidirectionalGraph<CFGNode, Edge<CFGNode>> graph```, а также прочую служебную информацию необходимую для дальнейших преобразований. В том числе поле ```EdgeTypes```, содержащее словарь с классификацией ребер.

#### Пример применения  

```cs
var blocks = LinearToBaseBlock.Build(code);
var cfg = new CFGraph(blocks);
Console.WriteLine(cfg.EdgeTypes.ToString());
