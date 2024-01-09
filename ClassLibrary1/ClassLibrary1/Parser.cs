using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class AnalizadorSintáctico
    {
        private List<Token> tokens;

        private int indice;
        private Token tokenActual;
        private Token ultimoToken;


        private static List<string> Nombrefunciones = new List<string>();

        public AnalizadorSintáctico(List<Token> tokens)
        {
            this.tokens = tokens;
            indice = 0;
            if (tokens.Count != 0)
            {


                tokenActual = tokens[0];
                ultimoToken = tokens[tokens.Count - 1];

            }


        }

        private void siguienteToken()
        {
            if (indice < tokens.Count - 1)
            {
                indice++;
                tokenActual = tokens[indice];
            }
        }

        private Token NextToken()
        {
            Token valor = tokens[indice];
            indice++;

            return valor;
        }

        private Token Peek(int offset)
        {
            var index = indice + offset;

            if (offset >= tokens.Count)
            {
                return tokens[tokens.Count - 1];
            }
            return tokens[index];

        }


        List<Instruccion> instructions = new List<Instruccion>();

        public List<Instruccion> ParseInstructions(List<Instruccion> instructions)
        {
            while (tokenActual != ultimoToken)
            {
                Instruccion line = Analize();

                instructions.Add(line);
                siguienteToken();
                if (tokenActual.Tipo == TipoToken.inOfLet)
                {
                    break;
                }

            }

            return instructions;
        }
        private Instruccion Analize()
        {

            if (tokenActual.Tipo == TipoToken.Identificador)
            {
                int index_copy = indice;

                while (tokens[index_copy].Tipo == TipoToken.Identificador || tokens[index_copy].Tipo == TipoToken.Coma)
                {
                    index_copy++;
                }

                if (tokens[index_copy].Tipo == TipoToken.OperadorAsignación)
                {
                    return AnalizeAsignation();
                }

            }
            else if (tokenActual.Tipo == TipoToken.IfKeyWord)
            {
                return AnalizarIfElse();
            }
            else if (tokenActual.Tipo == TipoToken.color)
            {
                return AnalizeColor();


            }
            else if (tokenActual.Tipo == TipoToken.restore)
            {
                return AnalizeRestore();


            }
            Instruccion nodo = AnalizeExpresion();
            return nodo;
        }

        private Instruccion AnalizeExpresion()
        {
            if (tokenActual.Tipo == TipoToken.Print)
            {
                return AnalizePrint();
            }
            else if (tokenActual.Tipo == TipoToken.RandomsFunction)
            {
                return AnalizeRandomsFunction();
            }

            else if (tokenActual.Tipo == TipoToken.PalabraReservada && tokenActual.Valor == "import")
            {
                return AnalizeImport();
            }




            else if (tokenActual.Tipo == TipoToken.SamplesFunction)
            {
                return AnalizeSamples();
            }

            else if (tokenActual.Tipo == TipoToken.PointsFunction)
            {
                return AnalizePointsFunction();
            }

            else if (tokenActual.Tipo == TipoToken.Point && Peek(1).Tipo == TipoToken.Sequence)
            {
                return AnalizePointSequence();
            }

            else if (tokenActual.Tipo == TipoToken.Point)
            {
                return AnalizePoint();
            }

            else if (tokenActual.Tipo == TipoToken.Line && Peek(1).Tipo == TipoToken.Sequence)
            {
                return AnalizeLineSequence();
            }


            else if (tokenActual.Tipo == TipoToken.Line)
            {
                return AnalizeLine();
            }

            else if (tokenActual.Tipo == TipoToken.PalabraReservada && tokenActual.Valor == "draw")
            {
                return AnalizeDraw();
            }
            else if (tokenActual.Tipo == TipoToken.Circle)
            {
                return AnalizeCircle();
            }
            else if (tokenActual.Tipo == TipoToken.Ray)
            {
                return AnalizeRay();
            }
            else if (tokenActual.Tipo == TipoToken.Segment)
            {
                return AnalizeSegment();
            }

            else if (tokenActual.Tipo == TipoToken.LineFunction)
            {
                return AnalizeLineFunction();
            }

            else if (tokenActual.Tipo == TipoToken.CircleFunction)
            {
                return AnalizeCircleFunction();
            }

            else if (tokenActual.Tipo == TipoToken.SegmentFunction)
            {
                return AnalizeSegmentFunction();
            }
            else if (tokenActual.Tipo == TipoToken.RayFunction)
            {
                return AnalizeRayFunction();
            }
            else if (tokenActual.Tipo == TipoToken.ArcFunction)
            {
                return AnalizeArcFunction();
            }
            else if (tokenActual.Tipo == TipoToken.MeasureFunction)
            {
                return AnalizeMeasure();
            }


            else if (tokenActual.Tipo == TipoToken.Cadena)
            {
                return AnalizeCadena();
            }
            else if (tokenActual.Tipo == TipoToken.RandomsFunction)
            {
                return AnalizeRandomsFunction();
            }
            else if (tokenActual.Tipo == TipoToken.IntersectFunction)
            {
                return AnalizeIntersection();
            }




















            Instruccion result = AnalizeConector();



            return result;





        }

        private Instruccion AnalizeRandomsFunction()
        {
            siguienteToken();
            siguienteToken();
            siguienteToken();


            return new RandomSequence();

        }

        private Instruccion AnalizeConector()
        {
            Instruccion result = AnalizeOperation();

            while (tokenActual != null && tokenActual.Tipo == TipoToken.LogicConector)
            {
                Token operador = tokenActual;
                siguienteToken();
                Instruccion derecho = AnalizeOperation();

                result = new LogicConector(result, derecho, operador.Valor);
            }
            return result;
        }

        private Instruccion AnalizeOperation()
        {
            Instruccion result = AnalizeArithmeticOperation();

            while (tokenActual != null && tokenActual.Tipo == TipoToken.OperadorLógico)
            {
                Token operador = tokenActual;
                siguienteToken();
                Instruccion derecho = AnalizeArithmeticOperation();

                result = new OperadorLogico(result, derecho, operador.Valor);
            }
            return result;
        }

        private Instruccion AnalizeArithmeticOperation()
        {
            Instruccion result = AnalizeTerm();
            while (tokenActual != null && (tokenActual.Valor == "+" || tokenActual.Valor == "-"))
            {
                Token operador = tokenActual;
                siguienteToken();
                Instruccion derecho = AnalizeTerm();
                result = new OperacionAritmetica(result, derecho, operador.Valor);
            }
            return result;
        }

        private Instruccion AnalizeTerm()
        {
            Instruccion result = AnalizeFactor();
            while (tokenActual != null && (tokenActual.Valor == "*" || tokenActual.Valor == "/" || tokenActual.Valor == "%"))
            {
                Token operador = tokenActual;
                siguienteToken();
                Instruccion derecho = AnalizeFactor();
                result = new OperacionAritmetica(result, derecho, operador.Valor);
            }
            return result;
        }

        private Instruccion AnalizeFactor()
        {
            Instruccion result = AnalizeExponent();
            while (tokenActual != null && tokenActual.Valor == "^")
            {
                Token operador = tokenActual;
                siguienteToken();
                Instruccion derecho = AnalizeExponent();
                result = new OperacionAritmetica(result, derecho, operador.Valor);
            }
            return result;
        }


        private Instruccion AnalizeExponent()
        {


            if (tokenActual.Tipo == TipoToken.Numero)
            {
                int valor = int.Parse(tokenActual.Valor);
                siguienteToken();
                Instruccion nodo = new Number(valor);


                return nodo;
            }
            else if (tokenActual.Tipo == TipoToken.Let)
            {

                Instruccion nodo = AnalizeLet();
                return nodo;


            }

            else if (tokenActual.Tipo == TipoToken.OpeningBrace && Peek(2).Tipo == TipoToken.Punto)
            {

                Instruccion nodo = AnalizeInfiniteSequence(); ;
                return nodo;

            }

            else if (tokenActual.Tipo == TipoToken.OpeningBrace)
            {
                Instruccion nodo = AnalizeSecuence();
                return nodo;


            }
            else if (tokenActual.Tipo == TipoToken.DeclaratedFunction && tokenActual.Valor == "count")
            {

                Instruccion nodo = AnalizeCountFunction();




                return nodo;


            }
            else if (tokenActual.Tipo == TipoToken.DeclaratedFunction && Nombrefunciones.Contains(tokenActual.Valor))
            {

                Instruccion nodo = AnalizeFunctionCall();




                return nodo;


            }

            else if (tokenActual.Tipo == TipoToken.DeclaratedFunction)
            {

                Instruccion nodo = AnalizeFunctionDeclaration();
                return nodo;


            }




            else if (tokenActual.Tipo == TipoToken.Identificador)
            {

                Instruccion nodo = AnalizeIdentifier();


                return nodo;




            }





            else if (tokenActual.Tipo == TipoToken.OpeningParenthesis)
            {
                siguienteToken();
                Instruccion nodo = Analize();

                if (tokenActual.Tipo != TipoToken.ClosingParenthesis)
                {
                    throw new CustomException("Missing ClosingParenthesis ", CustomExceptionType.UnknownChar);
                }


                siguienteToken();
                return nodo;
            }

            else if (tokenActual.Tipo == TipoToken.Undefined)
            {
                Instruccion nodo = AnalizeUndefined();


                return nodo;
            }

            else if (tokenActual.Valor == "-")
            {
                siguienteToken();
                Instruccion nodo = AnalizeFactor();
                Instruccion negacion = new Negacion(nodo);
                return negacion;



            }




            else
            {
                throw new CustomException("Unknown char at " + tokenActual.Valor + " " + indice, CustomExceptionType.UnknownChar);
            }
        }











        private IntersectExpression AnalizeIntersection()
        {
            siguienteToken();
            siguienteToken();



            Instruccion p1 = AnalizeExpresion();
            siguienteToken();
            Instruccion p2 = AnalizeExpresion();
            siguienteToken();

            return new IntersectExpression(p1, p2);

        }


        private Instruccion AnalizePrint()
        {
            siguienteToken();


          


            Instruccion expression = AnalizeExpresion();






            return new PrintExpression(expression);

        }
        private Instruccion AnalizeRestore()
        {
            siguienteToken();


            return new Restore();
        }

        private Instruccion AnalizePointsFunction()
        {
            siguienteToken();
            if (tokenActual.Tipo != TipoToken.OpeningParenthesis)
            {
                throw new CustomException("OpeningParenthesis expected ", CustomExceptionType.UnknownValue);
            }
            siguienteToken();

            Instruccion figure = AnalizeExpresion();



            if (tokenActual.Tipo != TipoToken.ClosingParenthesis)
            {
                throw new CustomException("ClosingParenthesis expected ", CustomExceptionType.UnknownValue);
            }

            siguienteToken();




            return new PointsFunction(figure);
        }

        private Samples AnalizeSamples()
        {
            siguienteToken();
            siguienteToken();
            siguienteToken();

            return new Samples();
        }

        public Import AnalizeImport()
        {
            siguienteToken();
            string ruta = tokenActual.Valor;
            siguienteToken(); 
            return new Import(ruta);

        }

        private Instruccion AnalizeUndefined()
        {
            siguienteToken();
            return new Undefined();

        }

        private Instruccion AnalizeLineSequence()
        {
            siguienteToken();
            siguienteToken();

            string id = tokenActual.Valor;

            siguienteToken();

            return new LineSequence(id);
        }

        private Instruccion AnalizePointSequence()
        {
            siguienteToken();
            siguienteToken();

            string id = tokenActual.Valor;

            siguienteToken();

            return new PointSequence(id);

        }


       


        private Instruccion AnalizeColor()
        {
            siguienteToken();
            string color = tokenActual.Valor;
            siguienteToken();
            return new Color(color);
        }
        private Instruccion AnalizeLet()
        {
            siguienteToken();
            List<Instruccion> InstruccionsAux = new List<Instruccion>();
            List<Instruccion> InstruccionsLet = ParseInstructions(InstruccionsAux);



            if (tokenActual.Tipo != TipoToken.inOfLet)
            {
                throw new Exception("'in' KeyWord mssing ");
            }
            siguienteToken();
            Instruccion LetInstruction = Analize();



            return new Let(LetInstruction, InstruccionsLet);



        }

        private Instruccion AnalizeIdentifier()
        {
            string Idvalue = tokenActual.Valor;
            siguienteToken();

            return new Identifier(Idvalue);



        }

        private Instruccion AnalizeMeasure()
        {

            siguienteToken();
            siguienteToken();

            if (tokenActual.Tipo != TipoToken.Identificador)
            {
                throw new CustomException("Unknown char  " + tokenActual.Valor, CustomExceptionType.UnknownChar);
            }

            Identifier p1 = (Identifier)AnalizeIdentifier();
            siguienteToken();
            Identifier p2 = (Identifier)AnalizeIdentifier();
            siguienteToken();

            return new MeasureFunction(p1, p2);
        }

        private Instruccion AnalizeInfiniteSequence()
        {
            // Asume que el token actual es la llave de apertura '{'
            siguienteToken();

            // El siguiente token debe ser un número que representa el inicio de la secuencia
            if (tokenActual.Tipo != TipoToken.Numero)
            {
                throw new ArgumentException("Se esperaba un número al inicio de la secuencia.");
            }

            int start = int.Parse(tokenActual.Valor);
            siguienteToken();

            // Si el siguiente token es '...', la secuencia puede ser infinita
            if (tokenActual.Tipo != TipoToken.Punto)
            {
                throw new ArgumentException("Se esperaba '...' ");
            }
            siguienteToken();

            // Si el siguiente token es '}', la secuencia es infinita y comienza con 'start'
            if (tokenActual.Tipo == TipoToken.ClosingBrace)
            {
                siguienteToken();
                return new InfiniteSequence(start);
            }

            // Si el siguiente token es un número, la secuencia va de 'start' a este número
            else if (tokenActual.Tipo == TipoToken.Numero)
            {
                int end = int.Parse(tokenActual.Valor);
                siguienteToken();

                // El siguiente token debe ser '}'
                if (tokenActual.Tipo != TipoToken.ClosingBrace)
                {
                    throw new ArgumentException("Se esperaba '}' al final de la secuencia.");
                }

                siguienteToken();

                return new InfiniteSequence(start, end);
            }

            else
            {
                throw new ArgumentException("Se esperaba un número o '}' después de '...'.");
            }
        }






        private Instruccion AnalizeDraw()
        {


            siguienteToken();
            Instruccion ToDraw = AnalizeExpresion();
            string etiqueta = " ";
            if (tokenActual.Tipo == TipoToken.Cadena)
            {
                etiqueta = tokenActual.Valor;
                siguienteToken();
            }

            return new Draw(ToDraw, etiqueta);

        }

        private Instruccion AnalizeAsignation()
        {
            List<string> nombres = new List<string>();

            while (tokenActual.Tipo != TipoToken.OperadorAsignación)
            {
                nombres.Add(tokenActual.Valor);
                siguienteToken();
                if (tokenActual.Tipo == TipoToken.Coma)
                {
                    siguienteToken();
                }
            }

            siguienteToken();

            Instruccion asignation = Analize();





            return new AnalizeAsignation(nombres, asignation);

        }

       


        private Instruccion AnalizeSecuence()
        {
            List<Instruccion> SecuencesId = new List<Instruccion>();

            siguienteToken();


            while (tokenActual.Tipo != TipoToken.ClosingBrace)
            {
                SecuencesId.Add(AnalizeExpresion());

                if (tokenActual.Tipo == TipoToken.Coma)
                {
                    siguienteToken();
                }

            }

            siguienteToken();

            return new SecuenceFigure<Instruccion>(SecuencesId);
        }


        private IfElseExpression AnalizarIfElse()

        {

            siguienteToken();



            Instruccion IfExpression = Analize();


            if (tokenActual.Tipo != TipoToken.ThenKeyWord)
            {
                throw new Exception("Error:Exprected KeyWord ¨then¨." + tokenActual.Valor);

            }
            siguienteToken();


            Instruccion ThenExpression = Analize();


            if (tokenActual.Tipo != TipoToken.ElseKeyWord)
            {
                throw new Exception("Error: Se esperaba la expresion ¨Else¨.");

            }

            siguienteToken();

            Instruccion ElseExpression = Analize();




            return new IfElseExpression(IfExpression, ThenExpression, ElseExpression);
        }



        private ArcFunction AnalizeArcFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();
            siguienteToken();

            Identifier p2 = (Identifier)AnalizeIdentifier();
            siguienteToken();

            Identifier p3 = (Identifier)AnalizeIdentifier();
            siguienteToken();


            Identifier m = (Identifier)AnalizeIdentifier();

            siguienteToken();
            
            return new ArcFunction(p1, p2, p3, m);

        }

        private FunctionLine AnalizeLineFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeExpresion();

            siguienteToken();


            Identifier p2 = (Identifier)AnalizeIdentifier();

            siguienteToken();






            return new FunctionLine(p1, p2);

        }

        private Point AnalizePoint()
        {
            siguienteToken();
            string id = tokenActual.Valor;

            siguienteToken();

            return new Point(id);
        }

        private Circle AnalizeCircle()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Circle(id);
        }



        private Ray AnalizeRay()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Ray(id);
        }

        private CircleFunction AnalizeCircleFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();

            siguienteToken();

            Instruccion m = AnalizeExpresion();

            siguienteToken();

            return new CircleFunction(p1, m);

        }

        private Cadena AnalizeCadena()
        {



            string expresion = tokenActual.Valor;
            siguienteToken();
            return new Cadena(expresion);


        }

        private SegmentFunction AnalizeSegmentFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();

            siguienteToken();

            Identifier p2 = (Identifier)AnalizeIdentifier();

            siguienteToken();


            return new SegmentFunction(p1, p2);
        }
        private RayFunction AnalizeRayFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();

            siguienteToken();


            Identifier p2 = (Identifier)AnalizeIdentifier();

            siguienteToken();


            return new RayFunction(p1, p2);

        }

        private Line AnalizeLine()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Line(id);
        }

        private Segment AnalizeSegment()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Segment(id);
        }

        private Instruccion AnalizeCountFunction()
        {
            siguienteToken();
            if (tokenActual.Tipo != TipoToken.OpeningParenthesis)
            {
                throw new CustomException("OpeningParenthesis expected ", CustomExceptionType.UnknownValue);
            }
            siguienteToken();

            Instruccion secuence = AnalizeExpresion();



            if (tokenActual.Tipo != TipoToken.ClosingParenthesis)
            {
                throw new CustomException("ClosingParenthesis expected ", CustomExceptionType.UnknownValue);
            }

            siguienteToken();




            return new CountFunction(secuence);

        }

        private Instruccion AnalizeFunctionDeclaration()
        {


            string nombreFuncion = tokenActual.Valor;
            Nombrefunciones.Add(nombreFuncion);
            siguienteToken();

            if (tokenActual.Tipo != TipoToken.OpeningParenthesis)
                throw new CustomException("Unknown char  " + tokenActual.Valor, CustomExceptionType.UnknownChar);

            siguienteToken();

            List<string> parametros = new List<string>();
            while (tokenActual.Tipo != TipoToken.ClosingParenthesis)
            {
                if (tokenActual.Tipo != TipoToken.Identificador)

                {
                    Console.WriteLine(tokenActual.Valor);
                    throw new Exception("Error: Id expected.");
                }

                parametros.Add(tokenActual.Valor);

                siguienteToken();

                if (tokenActual.Tipo == TipoToken.Coma)
                {
                    siguienteToken();
                }





            }

            siguienteToken();

            if (tokenActual.Tipo != TipoToken.OperadorAsignación)
                throw new Exception("Error:  = Expected.");

            siguienteToken();

            Instruccion cuerpo = Analize();

            return new FunctionDeclaration(nombreFuncion, parametros, cuerpo);
        }

        private Instruccion AnalizeFunctionCall()
        {
            string nombreFuncion = tokenActual.Valor;
            siguienteToken();

            if (tokenActual.Tipo != TipoToken.OpeningParenthesis)
                throw new CustomException("Unknown char  " + tokenActual.Valor, CustomExceptionType.UnknownChar);

            siguienteToken();

            List<Instruccion> argumentos = new List<Instruccion>();
            while (tokenActual.Tipo != TipoToken.ClosingParenthesis)
            {
                Instruccion argumento = AnalizeExpresion();
                argumentos.Add(argumento);

                if (tokenActual.Tipo == TipoToken.Coma)
                {

                    siguienteToken();
                }

            }

            siguienteToken();




            return new FunctionCall(nombreFuncion, argumentos);
        }

    }

}










