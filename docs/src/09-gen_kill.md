## Достигающие определения: вычисление `gen` и `kill`

**Определение** *Множеством `gen_B`* называется множество определений, генерируемых в базовом блоке `B`. *Множеством `kill_B`* называется множество всех прочих определений тех же переменный во всей остальной программе.

Эти множества входят в передаточную функцию для задачи о достигающих определениях, поэтому перед запуском итерационного алгоритма для нее необходимо вычислить эти множества для каждого блока.

Определения хранятся в виде пары, где первый элемент -- метка команды с определением, вторая -- имя определяемой переменной.

```csharp
using Definition = Tuple<LabelValue, IdentificatorValue>;

public readonly Dictionary<IBaseBlock, List<Definition>> Gen;

public readonly Dictionary<IBaseBlock, List<Definition>> Kill;
```
Сначала реализуем функцию для выбора всех определений из блока:

```csharp
static IEnumerable<Tuple<LabelValue, IdentificatorValue>> CalcGen(IBaseBlock block)
{
    return block.Enumerate()
        .Where(IsDefinition)
        .Select(t => Tuple.Create(t.Label, t.AsDefinition()));
}

```

Тогда алгоритм построения множеств `gen` и `kill` будет прямо следовать из определений:

```csharp
foreach (var block in blocks)
{
    var gen = CalcGen(block).ToList();

    var vars = new HashSet<IdentificatorValue>(gen.Select(e => e.Item2));
    var kill = blocks.Where(b => b != block).SelectMany(CalcGen).Where(e => vars.Contains(e.Item2));

    Gen[block] = gen.ToList();
    Kill[block] = kill.ToList();
}

```
