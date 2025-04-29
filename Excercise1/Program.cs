using System;

namespace Excercise1
{
    struct MyFrac
    {
        public long nom, denom;
        public MyFrac(long nom, long denom)
        {
            this.nom = nom;
            this.denom = denom;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            long num = Input("Введіть чисельник 1 числа: ");
            
            long denom1 = Input("Введіть знаменник 1 числа: ");
            
            if(CheckDenom(denom1)) return;

            MyFrac f = new MyFrac(num, denom1);

            Console.WriteLine($"""
            Введене число: {MyFracToString(f)}
            Normalized: {MyFracToString(Normalize(f))}
            З цілою частиною: {ToStringWithIntPart(f)}
            Дійсне значення: {DoubleValue(f)}

            """);
            long num2 = Input("Введіть чисельник 2 числа: ");

            long denom2 = Input("Введіть знаменник 2 числа: ");

            if (CheckDenom(denom2)) return;

            MyFrac f2 = new MyFrac(num2, denom2);
            Console.WriteLine("Sum: " + MyFracToString(Normalize(Plus(f, f2))));
            Console.WriteLine("Diff: " + MyFracToString(Normalize(Minus(f, f2))));
            Console.WriteLine("Mult: " + MyFracToString(Normalize(Multiply(f, f2))));
            Console.WriteLine("Div: " + MyFracToString(Normalize(Divide(f, f2))));

            Console.WriteLine("Expr1 (" + n + "): " + MyFracToString(Normalize(CalcExpr1(n))));
            Console.WriteLine("Expr2 (" + n + "): " + MyFracToString(Normalize(CalcExpr2(n))));
        }

        static string MyFracToString(MyFrac f) => $"{f.nom} / {f.denom}";

        static MyFrac Normalize(MyFrac f)
        {
            long nsd = NSD(Math.Abs(f.nom), Math.Abs(f.denom));
            long nom = f.nom / nsd;
            long denom = f.denom / nsd;
            if (denom < 0)
            {
                nom = -nom;
                denom = -denom;
            }
            return new MyFrac(nom, denom);
        }

        static long NSD(long a, long b)
        {
            while (b != 0)
            {
                long t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        static string ToStringWithIntPart(MyFrac f)
        {
            f = Normalize(f);
            long whole = f.nom / f.denom;
            long rem = Math.Abs(f.nom % f.denom);
            if (rem == 0)
                return $"{whole}";
            if (whole == 0 && f.nom < 0)
                return $"-({whole}+{rem}/{f.denom})";
            return f.nom < 0
                ? $"-({Math.Abs(whole)}+{rem}/{f.denom})"
                : $"({whole}+{rem}/{f.denom})";
        }

        static double DoubleValue(MyFrac f) => (double)f.nom / f.denom;

        static MyFrac Plus(MyFrac f1, MyFrac f2) => new MyFrac(f1.nom * f2.denom + f2.nom * f1.denom, f1.denom * f2.denom);

        static MyFrac Minus(MyFrac f1, MyFrac f2) => new MyFrac(f1.nom * f2.denom - f2.nom * f1.denom, f1.denom * f2.denom);

        static MyFrac Multiply(MyFrac f1, MyFrac f2) => new MyFrac(f1.nom * f2.nom, f1.denom * f2.denom);

        static MyFrac Divide(MyFrac f1, MyFrac f2) => new MyFrac(f1.nom * f2.denom, f1.denom * f2.nom);

        static MyFrac CalcExpr1(long n)
        {
            MyFrac result = new MyFrac(0, 1);
            for (int i = 1; i <= n; i++)
            {
                MyFrac term = new MyFrac(1, i * (i + 1));
                result = Plus(result, term);
                result = Normalize(result);
            }
            return result;
        }

        static MyFrac CalcExpr2(long n)
        {
            MyFrac result = new MyFrac(1, 1);
            for (int i = 2; i <= n; i++)
            {
                MyFrac one = new MyFrac(1, 1);
                MyFrac term = Minus(one, new MyFrac(1, i * i));
                result = Multiply(result, term);
                result = Normalize(result);
            }
            return result;
        }
    }
}
