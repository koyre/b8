## Генерация трехадресного кода

Интерфейс команды в трехадресном коде
```cs
    public interface IThreeAddressCode
    {
        Operation Operation { get; set; }
        IValue LeftOperand { get; set; }
        IValue RightOperand { get; set; }
        StringValue Destination { get; set; }
        LabelValue Label { get; set; }
    }
```
* Operation - бинарная операция или процедура (Print/Println)
* LeftOperand:
 * Левый операнд бинарной операции
 * Присваиваемое значение в случае операции Assign
* RightOperand
 * Правый операнд бинарной операции
* Destination
 * Переменная, которой присваивается значения
 * Команда, к которой будет осуществлён переход
* Label - метка

Генерация трехадресного кода осуществляется с помощью обхода LinearCodeVisitor 

### Линейный код
```
a = b + (c + 1);
```
переводится в
```
%ulabel1: $const0 := c + 1
%ulabel0: a := b + $const0
```
### Условный оператор
```
if <условие> 
{
    oper1;
}
else
{
    oper2;
}
```

переводится в
```
%ulabel0: IF <условие> THEN GOTO %label0
%ulabel1: oper1
%ulabel2: GOTO %label1
%label0: NOP
%ulabel3: oper2
%label1: NOP
```
### Циклы
```
for i = 1..n {
	a = 1;
}

```

переводится в 
```
%ulabel0: i := 1
%label0: NOP
%ulabel3: $const0 := i <= n
%ulabel4: IF $const0 THEN GOTO %label1
%ulabel5: GOTO %label2
%label1: NOP
%ulabel6: a := 1
%ulabel1: i := i + 1
%ulabel2: GOTO %label0
%label2: NOP
```