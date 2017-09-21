## Построение фронта доминирования (DF) и итерационного фронта доминирования (IDF)

### Постановка задачи:
  Узел d графа потока доминирует над узлом n *(d dom n)*, если любой путь от входного узла графа потока к n проходит через d. При таком определении каждый узел доминирует над самим собой.

#### Фронт доминирования узла n

DF(n) - множество узлов m:

-  n доминирует над p *(n dom p)*, где p - предшественник m
-  n не доминирует над m *(n !dom m)* или n совпадает с m *(n = m)*

<img src="https://github.com/dmitriybulanov/MemoryPuzzle/blob/master/1.png" alt="">
 
#### Итерационный фронт доминирования

S - множество узлов

IDF(S) = U DF(x), где x - узел из S

IDF<SUB>1</SUB>(S) = DF(S)

IDF<SUB>i+1</SUB>(S) = DF(S U DF<SUB>i</SUB>(S))

IDF(S)=Lim<SUB>n->∞</SUB>DF<SUB>n</SUB>(S)

```csharp
 public class DominanceFrontier
 {
        public Dictionary<IBaseBlock, HashSet<IBaseBlock>> DF = new Dictionary<IBaseBlock, HashSet<IBaseBlock>>();
        public Dictionary<IBaseBlock, HashSet<IBaseBlock>> IDF = new Dictionary<IBaseBlock, HashSet<IBaseBlock>>();
        private List<IBaseBlock> blocks;

        public DominanceFrontier(List<IBaseBlock> blocks)
 }
```

#### Описание алгоритма

 Вычисление DF[x] для всех x (из лекций):

	foreach n - узел
	{
		if n имеет больше одного предшественника
		foreach p - предшественник n
		{
			r = p
			while r != IDom(n) // где, IDom(n) - непосредственный доминатор n
			{
				DF[r] += n
				r = IDom(r)
			}
		}
	}

#### Входные данные:

Список базовых блоков.

#### Выходные данные:

Объект класса DominanceFrontier, который хранит для каждого базового блока информацию об его фронте доминирования.

#### Пример использования:

```csharp
IBaseBlock block = new IBaseBlock();
List<IBaseBlock> blocks = new List<IBaseBlock>();
var dFront = new DominanceFrontier(blocks);
// где dFront.DF - Словарь, ключ - имя блока, значение - список узлов, которые входят во фронт доминирования блока.
var IDF = new HashSet<IBaseBlock>();
IDF = dFront.ComputeIDF(block); // для блока.
IDF = dFront.ComputeIDF(blocks); // для множества блоков.
```
