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




        Circle,

        PalabraReservada,

        SegmentFunction,
        ArcFunction,
        RayFunction,
        MeasureFunction,
        CircleFunction,
        IntersectFunction,

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


        Math,
        Comillas,
        PuntoComa,
        Coma,

        Desconocido,

        Flecha,

        Cadena,

        Concatenador,
        PointSecuence,
        LineFunction,
        IfKeyWord,
        ElseKeyWord,
        ThenKeyWord,
        Arc,
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
            { "print", TipoToken.PalabraReservada },
            { "function", TipoToken.PalabraReservada },
            { "let", TipoToken.PalabraReservada },
            { "in", TipoToken.PalabraReservada },
            { "if", TipoToken.IfKeyWord},
            { "else", TipoToken.ElseKeyWord },
             { "then", TipoToken.ThenKeyWord },
            { "draw", TipoToken.PalabraReservada },
            { "measure", TipoToken.PalabraReservada },
            { "color", TipoToken.PalabraReservada },
            { "restore", TipoToken.PalabraReservada },
            { "import", TipoToken.PalabraReservada },
            { "circle", TipoToken.Circle },
            { "line", TipoToken.Line },
            { "segment", TipoToken.Segment },
            { "ray", TipoToken.Ray },
            { "arc", TipoToken.Arc },
            { "randoms", TipoToken.PalabraReservada},
            { "point", TipoToken.Point },
            { "samples", TipoToken.PalabraReservada },


        };

        Dictionary<string, TipoToken> functionTokens = new Dictionary<string, TipoToken>
        {

            { "circle", TipoToken.CircleFunction },
            { "line", TipoToken.LineFunction },
            { "segment", TipoToken.SegmentFunction },
            { "ray", TipoToken.RayFunction},
            { "arc", TipoToken.ArcFunction },


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
                    else if (Math.ContainsKey(palabra))
                    {
                        tokens.Add(new Token(Math[palabra], palabra));
                        continue;

                    }
                    else if (palabrasReservadas.ContainsKey(palabra))
                    {
                        tokens.Add(new Token(palabrasReservadas[palabra], palabra));
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

                    if (
                            caracterActual == '>' || caracterActual == '<')
                    {
                        tokens.Add(new Token(TipoToken.OperadorLógico, caracterActual.ToString()));
                        indice++;
                        continue;
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

                        else if (indice + 1 < codigoFuente.Length && codigoFuente[indice + 1] == '>')
                        {
                            tokens.Add(new Token(TipoToken.Flecha, "=>"));
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
                            tokens.Add(new Token(TipoToken.Desconocido, caracterActual.ToString()));

                            throw new Exception("Caracter desconocido en : " + indice);

                        }
                    }
                    tokens.Add(new Token(TipoToken.Desconocido, caracterActual.ToString()));

                    throw new Exception("Caracter desconocido en : " + indice);





                }
                return tokens;

            }

        }
    }




