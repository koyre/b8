## Доступные выражения между базовыми блоками - множества e_gen(b) и e_kill(b)

### Постановка задачи
Необходимо произвести анализ выражений для каждого блока с целью
построения множеств E_Gen и E_Kill

### Описание алгортима
Мы проходим по списку всех блоков и для каждого собираем множества. Результирующие множества для каждого блока записываем в словарь множеств, где ключом выступает имя блока.
Каждый блок обрабатывается процедурой, которая хранит ассоциативный массив переменных и списков выражений, их использующих. 

### Входные данные
Базовый блок

### Выходные данные 

Множество выражений

### Реализация алгоритма
E_Gen - множество выражений, генерируемых в базовом блоке.
E_Kill - множество выражений, уничтожаемых в базовом блоке

Вызывается ```genKills = new GenKillExprs(cfg);```
и после этого в ```genKills.Gen[block]``` содержатся выражения, которые генерируются для этого блока
В ```genKills.Remove[block]``` содержатся выражения, которые удаляются в этом блоке

```
public GenKillExprs(CFGraph cfg)
        {
            var blocks = cfg.Blocks;

            foreach (var block in blocks)
            {
                BlockDefs[block] = new List<StringValue>();
                Gen[block] = new List<Expression>();
                Remove[block] = new List<Expression>();

                var countOfElems = block.Enumerate().Count();

                foreach (var elem in block.Enumerate().Reverse())
                {
                    if (elem.IsBinOp())
                    {
                        BlockDefs[block].Add(elem.Destination);

                        if (elem.Operation != Operation.NoOperation)
                        {
                            var expr = new Expression(elem.Operation, elem.LeftOperand, elem.RightOperand);

                            var hasThisExpr = AllExpressions.Any(iexpr => iexpr.Equals(expr));
                            if (!hasThisExpr)
                            {
                                AllExpressions.Add(expr);
                            }

                            if (!BlockDefs[block].Contains(elem.LeftOperand) && !BlockDefs[block].Contains(elem.RightOperand))
                            {
                                Gen[block].Add(expr);
                            }
                        }

                    }
                }

            }
```
----------


