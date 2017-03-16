
%{
    public Node root;
    public MyLanguageParser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%using ProgramTree

%namespace LYtest
%partial

%parsertype MyLanguageParser
%visibility internal
%tokentype Token


%union { 
			public int iVal;
			public string sVal;
			public Node nVal;
			public BlockNode blVal;
			public Operator opVal;
			public List<Node> lnVal;
	   }

%start main

%token <iVal> NUMBER
%token ASSIGN LPAREN RPAREN DDOT WHILE ENDWHILE FOR ENDFOR IF ENDIF BEGIN END SEMICOLON ELSE PRINTLN PRINT COMMA
%token MINUS PLUS MULT DIV  EQ NEQ LT GT LE GE OR AND NOT

%token <sVal> IDENT

%type <nVal> term expr factor statement ifst whilest statement forst id assign cycle arExpr logExpr proc
%type <blVal> stlist block
%type <opVal> addOp mulOp eqOp logOp 
%type <lnVal> explist
%%

main   : stlist EOF { root = $1; }
       ;


id		  : IDENT { $$ = new IdentNode($1); }
          ;

assign	  : id ASSIGN expr { $$ = new AssignNode($1 as IdentNode, $3 as ExprNode); }
          ;

proc : PRINT LPAREN explist RPAREN { $$ = new Procedure(BuildOnProcedure.Print, $3); }
     | PRINTLN LPAREN explist RPAREN { $$ = new Procedure(BuildOnProcedure.Println, $3); }
     ;

explist :  { $$ = new List<Node>(); }
         | expr { var t = new List<Node>();
		          t.Add($1);
				  $$ = t; }
		 | expr COMMA explist { $3.Add($1);
		                        $$ = $3; }
		 ;

statement : assign SEMICOLON { $$ = $1; }
          | proc SEMICOLON { $$ = $1; }
		  | cycle { $$ = $1; }
		  ;

cycle     : ifst { $$ = $1; }
		  | forst { $$ = $1; }
		  | whilest { $$ = $1; }
		  ;

stlist    : statement  { $$ = new BlockNode($1 as StatementNode); }
          | stlist statement
				{ 
					$1.Add($2 as StatementNode); 
					$$ = $1; 
				}
	      ;

forst : FOR id ASSIGN expr DDOT expr stlist ENDFOR SEMICOLON
         { $$ = new ForNode($2 as IdentNode, $4 as ExprNode, $6 as ExprNode, $7 as BlockNode); }
	  ;


whilest : WHILE expr stlist ENDWHILE SEMICOLON{ $$ = new WhileNode($2 as ExprNode, $3); }
        ;
 
ifst :  IF expr stlist ENDIF SEMICOLON{ $$ = new IfNode($2 as ExprNode, $3); }
       |  IF expr stlist ELSE stlist ENDIF SEMICOLON{ $$ = new IfNode($2 as ExprNode, $3, $5); }
     ;

block : BEGIN stlist END { $$ = $2; }
      | BEGIN END { $$ = new BlockNode(); }
      ;

			
addOp : PLUS { $$ = Operator.Plus; }
      | MINUS { $$ = Operator.Minus; }
	  ;

mulOp : MULT { $$ = Operator.Mult; }
      | DIV { $$ = Operator.Div; }
	  ;
	  
eqOp      : LE { $$ = Operator.Le; }
	      | LT { $$ = Operator.Lt; }
		  | GT { $$ = Operator.Gt; }
		  | GE { $$ = Operator.Ge; }
		  | EQ { $$ = Operator.Eq; }
		  | NEQ { $$ = Operator.Neq; }
		  ;
	  
logOp  : OR   { $$ = Operator.Or; }
       | AND  { $$ = Operator.And; }
	   ;

expr : logExpr { $$ = $1; }
      | expr logOp logExpr  { $$ = new BinOp($1 as ExprNode, $3 as ExprNode, $2); }
	  | NOT logExpr { $$ = new UnOp($2 as ExprNode, UnaryOperator.Not); }   
	  ;

logExpr : arExpr { $$ = $1; }
		| logExpr eqOp arExpr  { $$ = new BinOp($1 as ExprNode, $3 as ExprNode, $2); }   
	    ;

arExpr     : term { $$ = $1; }	   
           | arExpr addOp term 	{ $$ = new BinOp($1 as ExprNode, $3 as ExprNode, $2); }
		   ;



factor   : id  { $$ = $1; }
		 | NUMBER { $$ = new Const($1); }
		 | LPAREN expr RPAREN { $$ = $2; }
		 ;


term   : factor      { $$ = $1; }
       | term mulOp factor  { $$ = new BinOp($1 as ExprNode, $3 as ExprNode, $2); }
	   ;

%%