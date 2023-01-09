namespace CraftingInterpreters.Test;

[TestClass]
public class AstPrinterTests
{
    [TestMethod]
    public void Test_AstPrinter_Returns_Correct_Representation()
    {
        // Arrange
        var astPrinter = new AstPrinter();
        var expression = new Binary(
            new Unary(new Token(TokenType.MINUS, "-", null, 1), new Literal(123)),
            new Token(TokenType.STAR, "*", null, 1), new Grouping(new Literal(45.67)));
        
        // Act
        var result = astPrinter.Print(expression);

        // Assert
        Assert.AreEqual("(* (- 123) (group 45.67))", result);
    }
}