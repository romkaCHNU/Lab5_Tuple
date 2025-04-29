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

            long num1 = Input("Введіть чисельник 1 числа: ");
            
            long denom1 = Input("Введіть знаменник 1 числа: ");
            
            if(CheckDenomForZeroValue(denom1)) return;

            MyFrac f = new MyFrac(num1, denom1);

            Console.WriteLine($"""
            Введене число: {MyFracToString(f)}
            Normalized: {MyFracToString(Normalize(f))}
            З цілою частиною: {ToStringWithIntPart(Normalize(f))}
            Дійсне значення: {DoubleValue(f)}

            """);
            long num2 = Input("Введіть чисельник 2 числа: ");

            long denom2 = Input("Введіть знаменник 2 числа: ");

            if (CheckDenomForZeroValue(denom2)) return;

            MyFrac f2 = new MyFrac(num2, denom2);
            Console.WriteLine($"""

            Сума: {MyFracToString(Normalize(Plus(f, f2)))}
            Різниця: {MyFracToString(Normalize(Minus(f, f2)))}
            Добуток: {MyFracToString(Normalize(Multiply(f, f2)))}
            Частка: {MyFracToString(Normalize(Divide(f, f2)))}

            """);
            
            long n = Input("Введіть число n: ");

            Console.WriteLine("Expr1 (" + n + "): " + MyFracToString(Normalize(CalcExpr1(n))));
            Console.WriteLine("Expr2 (" + n + "): " + MyFracToString(Normalize(CalcExpr2(n))));
        }
        static long Input(string prompt)
        {
            Console.Write(prompt);
            return long.Parse(Console.ReadLine());
        }
        static bool CheckDenomForZeroValue(long denom)
        {
            if (denom == 0)
            {
                Console.WriteLine("Знаменник не може дорівнювати нулю.");
                return true;
            }
            return false;
        }
        static string MyFracToString(MyFrac f) => $"{f.nom} / {f.denom}";

        static MyFrac Normalize(MyFrac f)
        {
            long nsd = NSD(Math.Abs(f.nom), Math.Abs(f.denom));
            long nom = f.nom / nsd;
            long denom = f.denom / nsd;
            if (denom < 0)
                (nom, denom) = (-nom, -denom);

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
            long intPart = f.nom / f.denom;

            if (intPart == 0)
                return MyFracToString(f) + " (Цілу частину було неможливо виділити)";

            long newNom = Math.Abs(f.nom % f.denom);

            if (newNom == 0)
                return $"{intPart}";

            if (intPart == 0 && f.nom < 0)
                return $"-({intPart}+{newNom}/{f.denom})";

            return f.nom < 0 ? $"-({Math.Abs(intPart)}+{newNom}/{f.denom})" : $"({intPart}+{newNom}/{f.denom})";
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
