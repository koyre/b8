### Описание задачи: Символьный анализ: передаточная функция одной инструкции, композиция передаточных функций, степень передаточной функции

### Теоретическая сводка:
**Определение** *Передаточная функция инструкции* интепретирует семантику присваивания чтобы определить, возможно ли выразить его в виде афинного выражения. Функция f инструкции *s* присваивания переменной *x* символьного отображения *m* определяется следующим образом:
<img src="https://github.com/Done12/dumpster/blob/master/func.png" alt="">

**Определение** *Композиция передаточных функций.*  
Для вычисления **f<SUB>2</SUB>**∘**f<SUB>1</SUB>**, где **f<SUB>2</SUB>** и **f<SUB>1</SUB>** определены в терминах входного отображения *m* достаточно подставить вместо m(v<SUB>i</SUB>) в определении **f<SUB>2</SUB>** определение **f<SUB>1</SUB>(m)(v<SUB>i</SUB>)**. Все операторы над **NAA** заменяются **NAA**. Таким образом,
<img src="https://github.com/Done12/dumpster/blob/master/comf.png" alt="">

**Определение** *Степень передаточной функции* определяется как композиция с собой.
  
### Входные данные:
Для передаточной функции и степени: инструкция s(трехадресный код), символьное отображение m  
Для композиции: инструкция s(трехадресный код), 2 символьных отображения f1 и f2  
Для построения отображения: набор инструкций(базовый блок b)  

#### Выходные данные:
Объект класса SymbolicMap, который хранит таблицу переменных, и их значений - афинных выражений

### Описание алгоритмов
Передаточная функция:

```csharp
public SymbolicMap TransferFunc(IThreeAddressCode s, SymbolicMap m)
{
  VariableValue newVal = new VariableValue();
  //compute affine expression even if instruction is NAA
  newVal.value = ComputeAffineExpr(s, m);//to keep track of variables

  var key = s.Destination as IdentificatorValue;
  
  if (IsAffineExpressible(s))    
  {
    newVal.type = VariableValueType.AFFINE;
  }
  else
  {
    newVal.type = VariableValueType.NAA;
  }
  m.variableTable[key] = newVal;

  return m;
}
```

Композиция функций:

```csharp
public SymbolicMap Composition(IThreeAddressCode s, SymbolicMap f1, SymbolicMap f2)
{
  VariableValue newVal = new VariableValue();
  SymbolicMap res = f2;

  var key = s.Destination as IdentificatorValue;

  if (/*TransferFunc(s, f2)*/f2.variableTable[key].type == VariableValueType.NAA) // if f2(m)(v) = NAA
  {
    newVal.type = VariableValueType.NAA;
    res.variableTable[key] = newVal;
  }
  else if (f2.variableTable[key].type == VariableValueType.AFFINE) // if f2(m)(v) = AFFINE
  {
    if (f1.variableTable[key].type == VariableValueType.NAA) // if f1(m)(v) == NAA
    {
      newVal.type = VariableValueType.NAA;
      res.variableTable[key] = newVal;
    }
    else 
    {
      // подставляем f1(m)(v) вместо m(v) в f2

      var affunc = f2.variableTable[key].value;
      var appliedf1 = TransferFunc(s, f1);

      newVal.type = VariableValueType.AFFINE;
      newVal.value.constants = affunc.constants;               
      newVal.value.variables = appliedf1.variableTable[key].value.variables;

      res.variableTable[key] = newVal;
    }
  }
  else
  {
    newVal.type = VariableValueType.UNDEF;
    res.variableTable[key] = newVal;
  }

  return res;
}
```

Степень передаточной функции:

```csharp
public SymbolicMap SelfComposition(IThreeAddressCode line, SymbolicMap f1)
{
  return Composition(line, f1, f1);
}
```

#### Пример использования:

```csharp
SymbolicAnalysis syman = new SymbolicAnalysis();
var map = syman.createMap(blocks[0] as BaseBlock);
```
