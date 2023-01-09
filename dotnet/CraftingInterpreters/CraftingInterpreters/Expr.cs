namespace CraftingInterpreters;

public interface IVisitor<out T> {
    T VisitBinaryExpr(Binary expr);
    T VisitGroupingExpr(Grouping expr);
    T VisitLiteralExpr(Literal expr);
    T VisitUnaryExpr(Unary expr);
}

public abstract record Expr()
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}

public record Binary(Expr Left, Token Operator, Expr Right) : Expr
{
    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this);
}

public record Grouping(Expr Expression) : Expr
{
    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);
}

public record Literal(object Value) : Expr
{
    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);
}

public record Unary(Token Operator, Expr Right) : Expr
{
    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this);
}