## Разбиение на базовые блоки

### Общие сведения

**Определение** *Базовый блок* -- это ...

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

Для построения базовых блоков из необходимо выделить контрольные точки...
