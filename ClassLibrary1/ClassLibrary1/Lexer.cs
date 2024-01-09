using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public enum TipoToken
    {
        Point,
        Line,
        Ray,

        Segment,



        Punto,

        Circle,

        PalabraReservada,

        TokenComentario,

        SegmentFunction,
        ArcFunction,
        RayFunction,
        MeasureFunction,
        CircleFunction,
        IntersectFunction,
        RandomsFunction,
        PointsFunction,
        SamplesFunction,
        Print,
        Funcion,
        Identificador,
        Numero,
        OperadorAritmético,
        OperadorLógico,
        OperadorAsignación,
        OpeningParenthesis,
        ClosingParenthesis,
        OpeningBrace,
        ClosingBrace,

        LogicConector,

        InfiniteSequence,
        RangeSequence,



        Math,
        Comillas,
        PuntoComa,
        Coma,

        Sequence,

        Undefined,

        Cadena,
        DeclaratedFunction,
        Concatenador,
        PointSecuence,
        LineFunction,
        IfKeyWord,
        ElseKeyWord,
        ThenKeyWord,
        Arc,
        Let,
        inOfLet,
        blue,
        red,
        yellow,
        green,
        cyan,
        magenta,
        white,
        gray,
        black,
        color,
        restore,
        Unknown,
    }





    public class Token
    {
        



        public Token(TipoToken tipo, string valor)
        {
            Tipo = tipo;
            Valor = valor;
        }
        public TipoToken Tipo { get; }
        public string Valor { get; }


    }



    public class AnalizadorLéxico
    {
        string codigoFuente;
        int indice;

        Dictionary<string, TipoToken> palabrasReservadas = new Dictionary<string, TipoToken>
        {

            { "function", TipoToken.PalabraReservada },
            { "let", TipoToken.Let },
            { "in", TipoToken.inOfLet },
            { "if", TipoToken.IfKeyWord},
            { "else", TipoToken.ElseKeyWord },
             { "then", TipoToken.ThenKeyWord },
            { "draw", TipoToken.PalabraReservada },
            { "color", TipoToken.color },
            { "restore", TipoToken.restore },
            { "import", TipoToken.PalabraReservada },
            { "circle", TipoToken.Circle },
            { "line", TipoToken.Line },
            { "segment", TipoToken.Segment },
            { "ray", TipoToken.Ray },
            { "arc", TipoToken.Arc },
            { "randoms", TipoToken.PalabraReservada},
            { "point", TipoToken.Point },
            { "samples", TipoToken.PalabraReservada },
             { "and", TipoToken.LogicConector },
              { "or", TipoToken.LogicConector },
                { "undefined", TipoToken.Undefined },
                { "sequence", TipoToken.Sequence},
                { "print", TipoToken.Print },

        };

        Dictionary<string, TipoToken> functionTokens = new Dictionary<string, TipoToken>
        {    { "intersect" , TipoToken.IntersectFunction },
            { "circle", TipoToken.CircleFunction },
            { "line", TipoToken.LineFunction },
            { "segment", TipoToken.SegmentFunction },
            { "ray", TipoToken.RayFunction},
            { "arc", TipoToken.ArcFunction },
            { "measure" , TipoToken.MeasureFunction },
            { "points" , TipoToken.PointsFunction },
            { "randoms" , TipoToken.RandomsFunction },
            { "samples" , TipoToken.SamplesFunction },
        };


        Dictionary<string, TipoToken> Math = new Dictionary<string, TipoToken>
        {
            { "sin", TipoToken.Math},
            { "cos", TipoToken.Math},
            { "log", TipoToken.Math},
            { "PI", TipoToken.Math},


        };




        public AnalizadorLéxico(string codigoFuente)
        {
            this.codigoFuente = codigoFuente;
            indice = 0;





        }



        public List<Token> ObtenerTokens()
        {
            List<Token> tokens = new List<Token>();

            while (indice < codigoFuente.Length)
            {
                char caracterActual = codigoFuente[indice];
                string _caracterActual = codigoFuente[indice].ToString();


                if (caracterActual == ' ' || caracterActual == '\n')
                {
                    indice++;
                    continue;
                }


                if (caracterActual == '(')
                {
                    tokens.Add(new Token(TipoToken.OpeningParenthesis, caracterActual.ToString()));
                    indice++;
                    continue;
                }
                if (caracterActual == ')')
                {
                    tokens.Add(new Token(TipoToken.ClosingParenthesis, caracterActual.ToString()));
                    indice++;
                    continue;
                }
                if (caracterActual == '{')
                {

                    tokens.Add(new Token(TipoToken.OpeningBrace, caracterActual.ToString()));
                    indice++;
                    continue;



                }
                if (caracterActual == '}')
                {
                    tokens.Add(new Token(TipoToken.ClosingBrace, caracterActual.ToString()));
                    indice++;
                    continue;
                }
                if (caracterActual == '"')
                {
                    StringBuilder cadena = new StringBuilder();
                    indice++;

                    while (indice < codigoFuente.Length && codigoFuente[indice] != '"')
                    {
                        cadena.Append(codigoFuente[indice]);
                        indice++;
                    }

                    if (indice == codigoFuente.Length)
                    {
                        throw new Exception("Error: Se esperaba una comilla de cierre.");
                    }

                    indice++;

                    tokens.Add(new Token(TipoToken.Cadena, cadena.ToString()));
                    continue;
                }


                if (caracterActual == ';')
                {
                    tokens.Add(new Token(TipoToken.PuntoComa, caracterActual.ToString()));
                    indice++;
                    continue;
                }
                if (caracterActual == ',')
                {
                    tokens.Add(new Token(TipoToken.Coma, caracterActual.ToString()));
                    indice++;
                    continue;
                }


                if (char.IsDigit(caracterActual))
                {
                    string numero = "";
                    while (indice < codigoFuente.Length && char.IsDigit(codigoFuente[indice]))
                    {
                        numero += codigoFuente[indice];
                        indice++;
                    }
                    tokens.Add(new Token(TipoToken.Numero, numero));
                    continue;
                }


                if (char.IsLetter(caracterActual) || caracterActual == '_')
                {
                    string palabra = "";
                    while (indice < codigoFuente.Length && (char.IsLetterOrDigit(codigoFuente[indice]) || codigoFuente[indice] == '_'))
                    {
                        palabra += codigoFuente[indice];
                        indice++;
                    }

                    while (codigoFuente[indice] == ' ')
                    {
                        indice++;
                    }

                    if (functionTokens.ContainsKey(palabra) && codigoFuente[indice] == '(')
                    {
                        tokens.Add(new Token(functionTokens[palabra], palabra));
                        continue;

                    }

                    else if (palabrasReservadas.ContainsKey(palabra))
                    {
                        tokens.Add(new Token(palabrasReservadas[palabra], palabra));
                        continue;

                    }
                    else if (codigoFuente[indice] == '(')
                    {
                        tokens.Add(new Token(TipoToken.DeclaratedFunction, palabra));
                        continue;

                    }
                    else if (Math.ContainsKey(palabra))
                    {
                        tokens.Add(new Token(Math[palabra], palabra));
                        continue;

                    }



                    tokens.Add(new Token(TipoToken.Identificador, palabra));
                    continue;


                }

                if (caracterActual == '+' || caracterActual == '-' || caracterActual == '*'
                        || caracterActual == '^' || caracterActual == '%'
                        || caracterActual == '/')
                {
                    tokens.Add(new Token(TipoToken.OperadorAritmético, caracterActual.ToString()));
                    indice++;
                    continue;
                }

                if (caracterActual == '&')
                {
                    if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '&')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, "&&"));
                        indice += 2;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TipoToken.Unknown, caracterActual.ToString()));

                        throw new Exception("Caracter desconocido en : " + indice);

                    }

                }

                
                if (caracterActual == '|')
                {
                    if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '|')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, "||"));
                        indice += 2;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TipoToken.Unknown, caracterActual.ToString()));

                        throw new Exception("Caracter desconocido en : " + indice);

                    }

                }



                if (caracterActual == '>')
                {
                    if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '=')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, ">="));
                        indice += 2;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, caracterActual.ToString()));
                        indice++;
                        continue;
                    }

                }

                if (
                        caracterActual == '<')
                {
                    if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '=')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, "<="));
                        indice += 2;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, caracterActual.ToString()));
                        indice++;
                        continue;
                    }

                }

                if
                     (caracterActual == '@')
                {
                    tokens.Add(new Token(TipoToken.Concatenador, caracterActual.ToString()));
                    indice++;
                    continue;
                }







                if (caracterActual == '=')
                {
                    if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '=')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, "=="));
                        indice += 2;
                        continue;
                    }


                    else
                    {
                        tokens.Add(new Token(TipoToken.OperadorAsignación, "="));
                        indice++;
                        continue;
                    }
                }
                if (indice + 2 < codigoFuente.Length && codigoFuente[indice] == '.' && codigoFuente[indice + 1] == '.' && codigoFuente[indice + 2] == '.')
                {
                    tokens.Add(new Token(TipoToken.Punto, "..."));
                    indice += 3;
                    continue;
                }


                if (caracterActual == '!')
                {
                    if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '=')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, "!="));
                        indice += 2;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TipoToken.Unknown, caracterActual.ToString()));

                        throw new Exception("Caracter desconocido en : " + indice);

                    }
                }
                tokens.Add(new Token(TipoToken.Unknown, caracterActual.ToString()));

                throw new Exception("Caracter desconocido en : " + indice);





            }
            return tokens;

        }

    }
}




