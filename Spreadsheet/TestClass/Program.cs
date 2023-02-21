
namespace Program;
/// <summary>
/// This is the test class for FormulaEvaluator
/// </summary>
public class Program
{

    static IEnumerable<int> Primes()
    {
        yield return 2;
        int p = 3;
        while (true)
        {
            if (IsPrime(p))
                yield return p;
            p += 2;
        }
    }

    static void Main(string[] args)
    {
        //IEnumerable<int> primes = Primes();
        //foreach (int p in primes)
        //Console.WriteLine(p);
        Console.WriteLine("{\"Cells\":{\"a1\":{\"stringForm\":\"a2+3\"},\"a2\":{\"stringForm\":\"2\"}},\"Version\":\"default\"}");
    }

    static bool IsPrime(int p)
    {
        if((p % 2 == 1) && (p % 3 == 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

