using System.Globalization;
using System.Text.RegularExpressions;

namespace GestionValesRdz
{
    public static class NumerosALetras
    {
        private static string[] UNIDADES =
        {
            "",
            "uno",
            "dos",
            "tres",
            "cuatro",
            "cinco",
            "seis",
            "siete",
            "ocho",
            "nueve"
        };

        private static string[] DECENAS =
        {
            "diez",
            "once",
            "doce",
            "trece",
            "catorce",
            "quince",
            "diecises",
            "diecisiete",
            "dieciocho",
            "diecinueve",
            "veinte",
            "treinta",
            "cuarenta",
            "cincuenta",
            "sesenta",
            "setenta",
            "ochenta",
            "noventa"
        };

        private static string[] CENTENAS =
        {
            "",
            "ciento",
            "doscientos",
            "trescientos",
            "cuatrocientos",
            "quinientos",
            "seiscientos",
            "setecientos",
            "ochocientos",
            "novecientos"           
        };

        private static Regex r;

        public static string Convertir(string numero, bool mayusculas)
        {
            string literal = "";
            string parte_decimal;

            numero = (decimal.Parse(numero, NumberStyles.Currency)).ToString();
            numero = numero.Replace(".", ",");

            if (numero.IndexOf(",") == -1)
                numero = numero + ",00";

            r = new Regex(@"\d{1,9},\d{1,2}");

            MatchCollection mc = r.Matches(numero);
            if (mc.Count > 0)
            {
                string[] num = numero.Split(',');
                parte_decimal = string.Format(" PESOS {0}/100 M.N", num[1]);
                if (int.Parse(num[0]) == 0)
                    literal = "cero";
                else if (int.Parse(num[0]) > 999999)
                    literal = getMillones(num[0]);
                else if (int.Parse(num[0]) > 999)
                    literal = getMiles(num[0]);
                else if (int.Parse(num[0]) > 99)
                    literal = getCentenas(num[0]);
                else if (int.Parse(num[0]) > 9)
                    literal = getDecenas(num[0]);
                else
                    literal = getUnidades(num[0]);

                if (mayusculas)
                    return (literal + parte_decimal).ToUpper();
                else
                    return (literal + parte_decimal);
            }
            else
                return null;
        }

        private static string getMillones(string numero)
        {
            string miles = numero.Substring(numero.Length - 6);
            string millon = numero.Substring(0, numero.Length - 6);
            string n = "";
            if (millon.Length > 1)
                n = getCentenas(millon) + " millones ";
            else
                n = getUnidades(millon) + " millon ";
            return n + getMiles(miles);
        }

        private static string getMiles(string num)
        {
            string c = num.Substring(num.Length - 3);
            string m = num.Substring(0, num.Length - 3);
            string n = "";
            if (int.Parse(m) > 1)
            {
                n = getCentenas(m);
                return string.Format("{0} mil {1}", n, getCentenas(c));
            }
            else if(int.Parse(m) == 1)
            {
                n = getCentenas(m);
                return string.Format("mil {1}", n, getCentenas(c));
            }
            else
                return "" + getCentenas(c);
        }

        private static string getCentenas(string num)
        {
            if (int.Parse(num) > 99)
            {
                if (int.Parse(num) == 100)
                    return " cien ";
                else
                    return string.Format("{0} {1}", CENTENAS[int.Parse(num.Substring(0, 1))], getDecenas(num.Substring(1)));
            }
            else
                return getDecenas(int.Parse(num) + "");
        }

        private static string getDecenas(string num)
        {
            int n = int.Parse(num);
            if (n < 10)
                return getUnidades(num);
            else if (n > 19)
            {
                string u = getUnidades(num);
                if (u.Equals(""))
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 8];
                else
                    return string.Format("{0} y {1} ", DECENAS[int.Parse(num.Substring(0, 1)) + 8], u);
            }
            else
                return DECENAS[n - 10];
        }

        private static string getUnidades(string numero)
        {
            string num = numero.Substring(numero.Length - 1);
            return UNIDADES[int.Parse(num)];
        }
    }
}
