using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.Optimize.ConstantPropagation;
using LYtest.LinearRepr.Values;
using LYtest.LinearRepr;
using LYtest.BaseBlocks;

namespace LYtest.Optimize.ConstantPropagation
{
    public class ConstantPropagation
    {

        public ConstantPropagation()
        {
            /* Empty */
        }

        
        // Передаточная функция для распространения констант
        private VariableValue CalculateTransmitionFunc(IThreeAddressCode line, VariableConstantMap currentTable)
        {
            VariableValue newValue = new VariableValue();

            // если присваиваем константу
            if (line.LeftOperand is NumericValue)
            {
                newValue.type = VariableValueType.CONSTANT;
                newValue.value = (line.LeftOperand as NumericValue).Value;
            }
            // присваиваем переменную
            else
            {
                // если в таблице нет такой переменной, говорим что она NAC
                if (!currentTable.variableTable.ContainsKey(line.LeftOperand as IdentificatorValue))
                {
                    newValue.type = VariableValueType.NAC;
                    return newValue;
                }

                //если переменная уже есть в таблице, то говорим что она константа и присваиваем значение
                VariableValue x = currentTable.variableTable[line.LeftOperand as IdentificatorValue];
                if (x.type.Equals(VariableValueType.CONSTANT))
                {
                    newValue.type = VariableValueType.CONSTANT;
                    newValue.value = x.value;
                }
                //если NAC то оставляем
                else if (x.type.Equals(VariableValueType.NAC))
                {
                    newValue.type = VariableValueType.NAC;
                }
                //если UNDEF то оставляем
                else
                {
                    newValue.type = VariableValueType.UNDEF;
                }
            }

            if (line.RightOperand == null || newValue.type == VariableValueType.NAC)
            {
                return newValue;
            }

            //если правая часть - константа, вычисляем ее
            if (line.RightOperand is NumericValue)
            {
                newValue.value = CalculateConstant(line.Operation, newValue.value, (line.RightOperand as NumericValue).Value);
                return newValue;
            }
            // если в таблице нет такой переменной правой части, говорим что она NAC
            if (!currentTable.variableTable.ContainsKey(line.RightOperand as IdentificatorValue))
            {
                newValue.type = VariableValueType.NAC;
                return newValue;
            }

            VariableValue y = currentTable.variableTable[line.RightOperand as IdentificatorValue];

            //если переменная уже есть в таблице, то говорим что она константа и присваиваем значение
            if (y.type.Equals(VariableValueType.CONSTANT))
            {
                newValue.value = CalculateConstant(line.Operation, newValue.value, y.value);
                return newValue;
            }

            if (y.type.Equals(VariableValueType.NAC))
            {
                newValue.type = VariableValueType.NAC;
            }
            else
            {
                newValue.type = VariableValueType.UNDEF;
            }
            return newValue;
        }

        public VariableConstantMap UpdateMap(BaseBlock baseBlock, VariableConstantMap map)
        {

            VariableConstantMap newM = map;

            foreach (var line in baseBlock.Enumerate())
            {
                if (isAssignment(line.Operation))
                {
                    if (line.RightOperand == null)
                    {
                        //константа
                        if (line.LeftOperand is NumericValue)
                        {
                            VariableValue newValue = new VariableValue();
                            newValue.type = VariableValueType.CONSTANT;
                            newValue.value = (line.LeftOperand as NumericValue).Value;
                            newM.variableTable[line.Destination as IdentificatorValue] = newValue;
                        }
                        //одна переменная в левой части
                        else
                        {
                            newM.variableTable[line.Destination as IdentificatorValue] =
                            CalculateTransmitionFunc(line, map);
                        }

                    }
                    // в правой части выражения две переменные
                    else
                    {
                        newM.variableTable[line.Destination as IdentificatorValue] =
                            CalculateTransmitionFunc(line, map);
                    }
                }
            }
            return newM;
        }


        private bool isAssignment(Operation op)
        {
            return (op.Equals(Operation.Assign)) || (op.Equals(Operation.Plus)) ||
                (op.Equals(Operation.Minus)) || (op.Equals(Operation.Mult)) || (op.Equals(Operation.Div));
        }

        public BaseBlock OptimizeBlock(BaseBlock baseBlock)
        {
            VariableConstantMap blockMap = new VariableConstantMap();
            blockMap = UpdateMap(baseBlock, blockMap);
            IdentificatorValue identificatorValue;

            for (int i = 0; i < baseBlock.Enumerate().Count(); ++i)
            {
                var line = baseBlock.Enumerate().ElementAt(i);

                if (isAssignment(line.Operation))
                {
                    identificatorValue = line.Destination as IdentificatorValue;
                    if (blockMap.variableTable.ContainsKey(identificatorValue) && blockMap.variableTable[identificatorValue].type == VariableValueType.CONSTANT)
                    {
                        line.LeftOperand = new NumericValue(blockMap.variableTable[identificatorValue].value);
                        line.RightOperand = null;
                        line.Operation = Operation.Assign;
                    }
                }

            }
            return baseBlock;
        }

        public static int CalculateConstant(Operation op, int x, int y)
        {
            switch (op)
            {
                case Operation.Plus:
                    return x + y;
                case Operation.Minus:
                    return x - y;
                case Operation.Mult:
                    return x * y;
                case Operation.Div:
                    return x / y;
                default:
                    return 0;
            }
        }

    }
}
