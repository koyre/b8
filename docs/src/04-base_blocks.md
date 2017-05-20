## Разбиение на базовые блоки

### Общие сведения

**Определение** *Базовый блок* -- это последовательность инструкций трехадресного кода, имеющая одну точку входа (первая инструкция) и одну точку выхода (последняя инструкция).

Базовый блок реализует интерфейс `IBaseBlock`


```csharp
public interface IBaseBlock
{
    bool InsertAfter(IThreeAddressCode after, IThreeAddressCode newElem);
    void Append(IThreeAddressCode newElem);
    bool Remove(IThreeAddressCode elem);
    IEnumerable<IThreeAddressCode> Enumerate();
    string ToString();
}
```

В реализации для хранения команд трехадресного кода использовался `LinkedList<IThreeAddressCode>` для быстрого удаления(/вставки) из(/на) произвольной позиции в середине списка.

### Алгоритм построения

Для построения базовых блоков из необходимо выделить команды-лидеры.

**Определение** *Команда-лидер* -- это:
1. Первая команда
2. Команда, на которую есть переход
3. Команда, следующая за переходом.

Получаем второе определение базового блока:


**Определение** *Базовый блок* -- это последовательность команд от лидера (включая) до лидера (исключая).

Пользуясь этим определением легко реализовать алгоритм разбиения последовательности команд трехадресного кода на базовые блоки.

Для этого первым проходом по последовательности находим команды лидеры:
```csharp
var controlPoints = new HashSet<LabelValue>();
foreach (var lin in lst)
{
    if (forceAdd)
        controlPoints.Add(lin.Label);
    forceAdd = false;

    var op = lin.Operation;

    if (op == Operation.Goto || op == Operation.CondGoto)
    {
        forceAdd = true;
        controlPoints.Add(lin.Destination as LabelValue);
    }
}
```

После этого еще одним проходом собираем команды между лидерами в блоки. На этом разбиение на базовые блоки закончено.
