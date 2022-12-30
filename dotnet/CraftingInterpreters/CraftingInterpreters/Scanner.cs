using System.Collections;

namespace CraftingInterpreters;

public class Scanner
{
    private readonly string _source;
    private readonly List<Token> _tokens = new();

    private int Start { get; set; } = 0;
    private int Current { get; set; } = 0;
    private int Line { get; set; } = 1;
    private bool IsAtEnd => Current >= _source.Length;
    
    public Scanner(string source) => _source = source;

    public IEnumerable<Token> ScanTokens()
    {
        while (!IsAtEnd)
        {
            // We are at the beginning of the next lexeme.
            Start = Current;
            ScanToken();
        }
        
        _tokens.Add(new Token(TokenType.EOF, "", null, Line));

        return _tokens;
    }
    
    private static bool IsDigit(char c) => c is >= '0' and <= '9';
    
    private static bool IsAlpha(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    
    private char Advance() => _source[Current++];

    private void AddToken(TokenType type) => AddToken(type, null);

    private bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

    private char PeekNext() => Current + 1 >= _source.Length ? '\0' : _source[Current + 1];

    private char Peek() => IsAtEnd ? '\0' : _source[Current];
    
    private void AddToken(TokenType type, object? literal)
    {
        var text = _source[Start..Current];
        _tokens.Add(new Token(type, text, literal, Line));
    }

    private void ScanToken()
    {
        var c = Advance();

        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            
            case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            
            case '/':
                if (Match('/')) 
                {
                    while (Peek() != '\n' && !IsAtEnd) 
                    {
                        Advance();
                    }
                } 
                else 
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;

            case '\n':
                Line++;
                break;

            case '"': String(); break;

            default:
                if (IsDigit(c)) 
                {
                    Number();
                } 
                else if (IsAlpha(c)) 
                {
                    Identifier();
                } 
                else 
                {
                    Program.Error(Line, "Unexpected character");
                }
                break;
        }
    }

    private bool Match(char expected)
    {
        if (IsAtEnd)
        {
            return false;
        }

        if (_source[Current] != expected)
        {
            return false;
        }

        Current++;
        return true;
    }

    private void Number()
    {
        while (IsDigit(Peek()))
        {
            Advance();
        }
        
        // Look for a factional part.
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            // Consume the "."
            Advance();

            while (IsDigit(Peek()))
            {
                Advance();
            }
        }

        AddToken(TokenType.NUMBER, Convert.ToDouble(_source[Start..Current]));
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek()))
        {
            Advance();
        }

        var text = _source[Start..Current];
        var type = _keywords[text] is TokenType value ? value : TokenType.IDENTIFIER;
        AddToken(type);
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd)
        {
            if (Peek() == '\n')
            {
                Line++;
            }

            Advance();
        }

        if (IsAtEnd)
        {
            Program.Error(Line, "Unterminated string.");
            return;
        }
        
        // The closing ".
        Advance();
        
        // Trim the surrounding quotes
        var value = _source[(Start+1)..(Current-1)];
        AddToken(TokenType.STRING, value);
    }

    private static readonly Hashtable _keywords = new Hashtable
    {
        { "and", TokenType.AND },
        { "class", TokenType.CLASS },
        { "else", TokenType.ELSE },
        { "false", TokenType.FALSE },
        { "for", TokenType.FOR },
        { "fun", TokenType.FUN },
        { "if", TokenType.IF },
        { "nil", TokenType.NIL },
        { "or", TokenType.OR },
        { "print", TokenType.PRINT },
        { "return", TokenType.RETURN },
        { "super", TokenType.SUPER },
        { "this", TokenType.THIS },
        { "true", TokenType.TRUE },
        { "var", TokenType.VAR },
        { "while", TokenType.WHILE },
    };
}