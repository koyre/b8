using System;
using System.Collections.Generic;
using LYtest.Visitors;


namespace ProgramTree
{


    public abstract class Node
    {
        public abstract void AcceptVisit(IVisitor visitor);
    }

    public abstract class StatementNode : Node
    {
    }

    public abstract class ExprNode : Node
    {
    }


    public class IfNode : StatementNode
    {
        public ExprNode Cond { get; }
        public BlockNode TrueChild { get; }
        public BlockNode FalseChild { get; }
        public IfNode(ExprNode cond, BlockNode child)
        {
            TrueChild = child;
            FalseChild = null;
            Cond = cond;
        }
        public IfNode(ExprNode cond, BlockNode onTrue, BlockNode onFalse)
        {
            TrueChild = onTrue;
            FalseChild = onFalse;
            Cond = cond;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitIf(this);
        }
    }



    public class ForNode : StatementNode
    {
        public IdentNode ForVar { get; }
        public ExprNode Beg { get; }

        public ExprNode End { get; }
    
        public BlockNode St { get; }

        public ForNode(IdentNode id, ExprNode beg, ExprNode end, BlockNode st)
        {
            ForVar = id;
            Beg = beg;
            End = end;
            St = st;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitFor(this);
        }
    }

    public class AssignNode : StatementNode
    {
        public IdentNode Id { get; }
        public ExprNode Expr { get; }
        public AssignNode(IdentNode id, ExprNode expr)
        {
            Id = id;
            Expr = expr;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitAssign(this);
        }
    }

    public class BlockNode : StatementNode
    {
        private readonly List<StatementNode> _sts = new List<StatementNode>();
        public IEnumerable<StatementNode> Childs => _sts;

        public BlockNode()
        {
        }

        public BlockNode(StatementNode node)
        {
            _sts.Add(node);
        }


        public void Add(StatementNode node)
        {
            _sts.Add(node);
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitBlock(this);
        }
    }

    public enum BuildOnProcedure { Print, Println }

    public class Procedure : StatementNode
    {
        public BuildOnProcedure Proc { get; }

        public List<ExprNode> Operands { get { return _operands; } }
         
        private List<ExprNode> _operands = new List<ExprNode>();
        public Procedure(BuildOnProcedure p)
        {
            Proc = p;
        }
        public Procedure(BuildOnProcedure p, List<Node> lst )
        {
            Proc = p;
            foreach (var node in lst)
            {
                var t = node as ExprNode;
                if (t == null)
                    throw new Exception("Illegal node type");
                _operands.Add(t);
            }
        }
        public void Add(ExprNode node)
        {
            _operands.Add(node);
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitProc(this);
        }
    }

    public class WhileNode : StatementNode
    {
        public ExprNode Cond { get; }
        public Node Child { get; }
        public WhileNode(ExprNode cond, BlockNode child)
        {
            Child = child;
            Cond = cond;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitWhile(this);
        }
    }

    public class Const : ExprNode
    {
        public int Val { get; }
        public Const(int val)
        {
            Val = val;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitConst(this);
        }
    }


    public class IdentNode : ExprNode
    {
        public string Name { get; }
        public IdentNode(string name)
        {
            Name = name;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitId(this);
        }
    }

    public enum Operator { Plus, Minus, Mult, Div, Or, And, Lt, Le, Gt, Ge, Eq, Neq }
    public class BinOp : ExprNode
    {
        public ExprNode Lhs { get; }
        public ExprNode Rhs { get; }
        public Operator Op { get; }
        public BinOp(ExprNode a, ExprNode b, Operator op)
        {
            Op = op;
            Lhs = a;
            Rhs = b;
        }

        public override void AcceptVisit(IVisitor visitor)
        {
            visitor.VisitBinOp(this);
        }
    }
}