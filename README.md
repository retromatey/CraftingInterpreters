## Grammar for Lox Expressions

- Literals: numbers, strings, Booleans, and nil
- Unary Expressions: a prefix ! to perform a logical not, and - to negate a number
- Binary Expressions: the infix arithmetic (+, -, *, /) and logic operators (==, !=, <, <=, >, <=)
- Parentheses: A pair of ( and ) wrapped around an expression

```
expression → literal
           | unary
           | binary
           | grouping ;

literal    → NUMBER | STRING | "true" | "false" | "nil" ;
grouping   → "(" expression ")" ;
unary      → ( "-" | "!" ) expression ;
binary     → expression operator expression ;
operator   → "==" | "!=" | "<" | "<=" | ">" | ">=" | "+" | "-" | "*" | "/" ;
```

