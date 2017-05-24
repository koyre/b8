## Доступные выражения внутри ББЛ(+оптимизации)

### Постановка задачи
Найти доступные выражения внутри базовых блоков.

### Описание алгортима
Выражение Х+У доступно в точке P, если любой путь от начального узла к Р вычисля-
ет Х+У и после такого вычисления и до достижения Р нет последующих присвоений Х
или У.  Блок генерирует выражение x+y, если он вычисляет x+y и не выполняет последующих переопределений x и y.

![enter image description here](https://pp.userapi.com/c637618/v637618517/55f10/wCXvDbqidx0.jpg)

### Входные данные
Базовый блок

### Выходные данные 

Множество выражений

### Реализация алгоритма
```
public Expression(Operation op, IValue leftOper, IValue rightOper)
        {
            Op = op;
            LeftOper = leftOper;
            RightOper = rightOper;
        }

        public Expression()
        {
            LeftOper = null;
            RightOper = null;
            Op = Operation.NoOperation;
        }

        public override bool Equals(object obj)
        {
            if (obj is Expression)
            {
                Expression Other = (Expression)obj;

                return Other.Op == this.Op &&
                    //Коммутативный случай
                    ((LinearHelper.IsBinOp(this.Op) || this.Op == Operation.Mult || this.Op == Operation.Plus) &&
                    (Other.LeftOper.Equals(this.LeftOper) && Other.RightOper.Equals(this.RightOper) || Other.LeftOper.Equals(this.RightOper) && Other.RightOper.Equals(this.LeftOper)) ||
                    //Некоммутативный случай
                    Other.LeftOper.Equals(this.LeftOper) && Other.RightOper.Equals(this.RightOper));
            }
            else
                return false;
        }
```
----------


