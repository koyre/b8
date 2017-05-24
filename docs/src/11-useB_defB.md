## Живые переменные между ББЛ, множества use_B и def_B

#### Описание задачи

Построение def-use и use-def цепочки для блока.

#### Описание алгоритма

Для каждого ББЛ:
1. берем левый операнд, если он не содержится в def - добавляем его в use.
2. Аналогично для правого операнда.
3. Добавляем в def переменную, если идет присваивание значения.


####Реализация алгоритма

```csharp
 public DefUseBuilder(List<IBaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                var defsB = new List<Definition>();
                var usesB = new List<Definition>();

                foreach (var t in block.Enumerate())
                {
                    var leftOperand = t.LeftOperand as IdentificatorValue;
                    var rightOperand = t.RightOperand as IdentificatorValue;

                    if (leftOperand != null && !defsB.Select(e => e.Item2).Contains(leftOperand))
                    {
                        usesB.Add(Tuple.Create(t.Label, leftOperand));
                    }
                    if (rightOperand != null && !defsB.Select(e => e.Item2).Contains(rightOperand))
                    {
                        usesB.Add(Tuple.Create(t.Label, rightOperand));
                    }

                    var def = t.AsDefinition();
                    if (def != null)
                    {
                        defsB.Add(Tuple.Create(t.Label, def));
                    }
                }

                Use[block] = usesB;
                Def[block] = defsB;
            }
        }
```



#### Пример использования

```csharp
	DefUse = new DefUseBuilder(g.Blocks);
```