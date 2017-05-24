## Живые переменные - итерационный алгоритм

#### Постановка задачи

Определить какие переменные являются активными(живыми) для каждого базового блока.

```csharp
 public sealed class ActiveVarsIterAlg : IterativeCommonAlg<HashSet<LabelValue>>
  {
        private readonly DefUseBuilder DefUse;
        public ActiveVarsIterAlg(CFGraph g) : base(g)
  }
```

#### Реализация алгоритма
Сначала получаем *Def*/*Use* информацию. Затем выполняем итерационный алгоритм.

```csharp
 public ActiveVarsIterAlg(CFGraph g) : base(g)
 {
	 DefUse = new DefUseBuilder(g.Blocks);
     ReverseRun();
 }
```

```csharp
public virtual void ReverseRun()
{
	foreach (var b in graph.GetVertices().Reverse())
		In[b] = Top;

	var nodes = new HashSet<CFGNode>(graph.GetVertices().Reverse());

	var cont = true;

	while (cont)
	{
		cont = false;
        foreach (var node in nodes)
        {
	        var childNodes = new List<CFGNode>();
            if (node.directChild != null)
            {
	            childNodes.Add(node.directChild);
            }
	        Out[node] = MeetOp(childNodes);
	        var prevIn = In[node];
            var newIn = In[node] = TransferFunc(node);

			if (ContCond(prevIn, newIn))
				cont = true;
		}
	}
}
```


#### Пример использования
```csharp
	var cfg = ListBlocksToCFG.Build(blocks);
	var DefUse = new ActiveVarsIterAlg(cfg);
```


