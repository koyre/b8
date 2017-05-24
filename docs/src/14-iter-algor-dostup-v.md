##  Доступные выражения, итерационный алгоритм

### Постановка задачи
Найти доступные выражения внутри базовых блоков.

### Описание алгортима
```
OUT [ВХОД] = 0; 
for (каждый базовый блок В, отличный от входного) OUT [В] = U; 
while (внесены изменения в OUT) 
for (каждый базовый блок В, отличный от входного) { 
IN [В] = Пр-предшественник В OUT [P]; 
OUT [В] = e_genB U (IN [В] - е_кШв); 
} 
```
### Входные данные
Вход: граф потока, у которого для каждого блока В вычислены e_killB и e_genB. 

### Выходные данные 
Словари ins и outs, содержашие в себе множества для каждого блока, получившиеся на последней итерации алгоритма.
```
InBlocks[vertce.Value] = UnionNodes(InBlocks[vertce.Value], OutBlocks[parent.Value]);

OutBlocks[vertce.Value] = new List<Expression>(genKills.AllExpressions);
```

### Реализация алгоритма
```
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
```
----------


