using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LYtest.Optimize.ConstantPropagation;
using LYtest.LinearRepr.Values;
using LYtest.LinearRepr;
using LYtest.BaseBlocks;

namespace LYtest.SymbolicAnalysis
{
    public class SymbolicAnalysis
    {
        // передаточная функция инструкции
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

        private Boolean IsAffineExpressible(IThreeAddressCode s)
        {
            return (s.Operation==Operation.Plus || s.Operation==Operation.Minus);
        }

        private AffineExpr ComputeAffineExpr(IThreeAddressCode s, SymbolicMap m)
        {
            AffineExpr res = new AffineExpr();
            res.constants = new List<int>();
            res.variables = new List<IdentificatorValue>();

            if(s.LeftOperand is NumericValue)            
                res.constants.Add((s.LeftOperand as NumericValue).Value);            
            else
            {
                res.variables.Add(s.LeftOperand as IdentificatorValue);
                
                if (IsAffineExpressible(s))
                if (m.variableTable.ContainsKey(s.LeftOperand as IdentificatorValue))
                if (m.variableTable[s.LeftOperand as IdentificatorValue].value.constants!=null)
                if ((s.LeftOperand as IdentificatorValue).ToString()[0]=='$')
                    foreach(var v in m.variableTable[s.LeftOperand as IdentificatorValue].value.constants)
                        res.constants.Add(v);
            }

            if (s.RightOperand != null)
            {
                if (s.RightOperand is NumericValue)
                    res.constants.Add((s.RightOperand as NumericValue).Value);
                else
                {
                    res.variables.Add(s.RightOperand as IdentificatorValue);

                    if (IsAffineExpressible(s))
                    if(m.variableTable.ContainsKey(s.RightOperand as IdentificatorValue))
                    if (m.variableTable[s.RightOperand as IdentificatorValue].value.constants != null)
                    if ((s.RightOperand as IdentificatorValue).ToString()[0]=='$')
                        foreach (var v in m.variableTable[s.RightOperand as IdentificatorValue].value.constants)
                            res.constants.Add(v); 
                }
            }
            return res;
        }

        public SymbolicMap createMap(BaseBlock b)
        {
            var m = new SymbolicMap();

            foreach (var line in b.Enumerate())
            {
                //if ((line.Operation == Operation.Plus))
                {
                    if (line.RightOperand == null)
                    {
                        //константа
                        if (line.LeftOperand is NumericValue)
                        {
                            VariableValue newValue = new VariableValue();
                            AffineExpr affe = new AffineExpr();
                            affe.value = (line.LeftOperand as NumericValue).Value;
                            newValue.value = affe;
                            newValue.type = VariableValueType.NAA;
                            m.variableTable[line.Destination as IdentificatorValue] = newValue;
                        }
                        //одна переменная в левой части
                        else
                        {
                            m = TransferFunc(line, m);
                        }

                    }
                    // в правой части выражения две переменные
                    else
                    {
                        m = TransferFunc(line, m);
                    }
                }
            }
            
            return m;
        }

        // композиция передаточных функций
        // f1 и f2 определены в терминах входного отображения m
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
            else if (/*TransferFunc(s, f2)*/f2.variableTable[key].type == VariableValueType.AFFINE) // if f2(m)(v) = AFFINE
            {
                if (/*TransferFunc(s, f1)*/f1.variableTable[key].type == VariableValueType.NAA) // if f1(m)(v) == NAA
                {
                    newVal.type = VariableValueType.NAA;
                    res.variableTable[key] = newVal;
                }
                else 
                {
                    // подставляем f1(m)(v) вместо m(v) в f2

                    var affunc = /*TransferFunc(s, f2)*/f2.variableTable[key].value;
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
       
        // степень передаточной функции
        public SymbolicMap SelfComposition(IThreeAddressCode line, SymbolicMap f1)
        {
            return Composition(line, f1, f1);
        }
    }
}
