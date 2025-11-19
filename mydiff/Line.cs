namespace mydiff;

public class Line
{
    public required int NumberLine{get;set;}
    public required string ContentLine{get;set;}
    
    public bool IsLcs{get;set;}
    
    public bool IsFirst { get; set; }
    
    public int CounterLine { get; set; }

    public static void PrintLine(Line line, string sign)
    {
        switch (sign)
        {
            case "minus":
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;
            case "plus":
                Console.ForegroundColor = ConsoleColor.Green;
                break;
        }

        if (sign == "none")
        {
            Console.Write(line.IsFirst ? $"{line.NumberLine} {line.CounterLine}" : $"{line.CounterLine} {line.NumberLine} ");
        }
        else
        {
            Console.Write(line.IsFirst ? $"{line.NumberLine}   " : $"  {line.NumberLine} ");
        }
        switch (sign)
        {
            case "minus":
                Console.Write($"-");
                break;
            case "plus":
                Console.Write($"+");
                break;
            default:
                Console.Write($" ");
                break;
        }
        Console.WriteLine($" {line.ContentLine}");
        Console.ResetColor();
    }
}