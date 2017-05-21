using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using LYtest.CFG;

namespace LYtest.Optimize.AvailableExprAnalyzer
{
    class AvailableExprAnalyzer

    {
        public readonly Dictionary<IBaseBlock, List<Expression>> InBlocks = new Dictionary<IBaseBlock, List<Expression>>();
        public readonly Dictionary<IBaseBlock, List<Expression>> OutBlocks = new Dictionary<IBaseBlock, List<Expression>>();
        public readonly GenKillExprs genKills;

        protected readonly CFGraph cfg;

        public AvailableExprAnalyzer(CFGraph _cfg)
        {
            
            cfg = _cfg;
            // Генерируем e_gen и e_kills
            genKills = new GenKillExprs(cfg);

            // Инициализируем для всех ББЛ пустые списки с входными выражениями
            foreach (var vertice in cfg.GetVertices())
            {
                InBlocks.Add(vertice.Value, new List<Expression>());
            }
        }

        public void analyze()
        {
            var firstBlock = cfg.GetVertices().First();
            var vertces = cfg.GetVertices();
            // Инициализируем для кажлого ББЛ кроме прервого список выходных выражений равный U (множество всех выражений)
            foreach (var vertce in vertces)
            {
                if (firstBlock != vertce.Value)
                {
                    OutBlocks[vertce.Value] = new List<Expression>(genKills.AllExpressions);
                }
            }

            bool hasChanges = true;

            while (hasChanges)
            {
                hasChanges = false;

                foreach (var vertce in vertces)
                {
                    if (firstBlock != vertce.Value)
                    {
                        // Добавляем во входные выражения все выражения, которые выходят из родительских блоков
                        foreach (var parent in vertce.ParentsNodes)
                        {
                            InBlocks[vertce.Value] = UnionNodes(InBlocks[vertce.Value], OutBlocks[parent.Value]);
                        }

                        //Копируем старый список выходных выражений для сравнения в будущем
                        var oldOut = new List<Expression>(OutBlocks[vertce.Value]);

                        // IN[B] - e_kill_B - убиваем в списке входных выражений те, которые будут убиты в текущем базовом блоке
                        var resNodes = ResidualNodes(InBlocks[vertce.Value], genKills.Remove[vertce.Value]);

                        // объединяем оставшиеся IN[B] с e_gen_B, т.е. все что осталось после кровавой резни между IN и e_kill объединяем с тем, что родилось в данном ББЛ 
                        // Cохраняем это все как выходные выражения для текущего базового блока
                        OutBlocks[vertce.Value] = UnionNodes(genKills.Gen[vertce.Value], resNodes);

                        // Если мы поменяли список выходных вырожений, то мы обязаны снова пройти по всем блокам и пересчитать вход/выход, ибо наш вход опирается на выходы других блоков, которые могли быть изменены
                        if (!EqualNodes(OutBlocks[vertce.Value], oldOut))
                        {
                            hasChanges = true;
                        }
                    }
                }
            }
        }

        // Дальше кому-то может быть больно в различных частях тела, преимущественно в глазках
        // Т.к. мои знания c# примерно равны моему зачету по компиляторам (P.S. его у меня нет)
        // То возможно я изобрел велосипед, по этому простите и если есть смысл - поясните как поправить



        // Объединяем node1 с node2
        public List<Expression> UnionNodes(List<Expression> node1, List<Expression> node2)
        {
            var result = new List<Expression>(node1);
        
            foreach (var expr in node2)
            {
                bool existInResult = node1.Any(item => item.Equals(expr));

                if (!existInResult)
                {
                    result.Add(expr);
                }
            }

            return result;
        }
        
        // Здесь начинается самое интересное
        // После безудержного веселья, компашка выражений под названием node1 возвращалась домой, но у подъезда их уже ждали они...
        // Killers, издали увидев приближение node1 спяртались в подъезде и стали дожидаться пока те войдут
        // Как только это случилось, они набросились на node1 и в цикле стали убивать
        // Жуткая резня, много крови, но как ни странно - node1 смогли дать отпор и потеряли лишь по одному бойцу на каждого killer'a
        
        // Короче, здесь мы можем нанять киллеров чтобы убить некоторых членов node1
        public List<Expression> ResidualNodes(List<Expression> node1, List<Expression> killers)
        {
            var result = new List<Expression>();

            foreach (var node in node1)
            {
                bool hasKiller = killers.Any(item => item.Equals(node));

                if (!hasKiller)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        // Очень странный метод, да, скорее всего кривой
        // Проверяет эквивалентность node1 к node2
        public bool EqualNodes(List<Expression> node1, List<Expression> node2)
        {
            var newNode2 = new List<Expression>(node2);

            if (node1.Count == node2.Count)
            {
                var countOfElements = node1.Count;
                var equalsCount = 0;

                foreach (var node in node1)
                {
                    bool containsItem = newNode2.Any(item => item.Equals(node));
                    newNode2.Remove(node);

                    if (containsItem)
                    {
                        equalsCount++;
                    }
                }

                return countOfElements == equalsCount;
            }
            else
            {
                return false;
            }
        }
    }    
}
