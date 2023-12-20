using System;
using System.Collections.Generic;
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

        public void siguienteToken()
        {
            if (indice < tokens.Count - 1)
            {
                indice++;
                tokenActual = tokens[indice];
            }
        }

        public Token NextToken()
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
        public Instruccion Analize()
        {    

            if (tokenActual.Tipo == TipoToken.Identificador && Peek(1).Tipo==TipoToken.OperadorAsignación )
            {                                   
                 return AnalizeAsignation();
            }
            
            else if (tokenActual.Tipo == TipoToken.IfKeyWord )
            {                                  
                 return AnalizarIfElse();
            }
            else if (tokenActual.Tipo == TipoToken.color)
            {
                return AnalizeColor();


            }
            else  if (tokenActual.Tipo == TipoToken.restore)
            {
                return AnalizeRestore();


            }
            Instruccion nodo = AnalizeExpresion();
            return nodo;
        }

        private Instruccion AnalizeExpresion()
        {
           
            if (tokenActual.Tipo == TipoToken.Point)
            {
                return AnalizePoint();
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
                return AnalizeLine();
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
            else if(tokenActual.Tipo==TipoToken.MeasureFunction)
            {
                return AnalizeMeasure();
            }
            else if (tokenActual.Tipo == TipoToken.SequenceToken && Peek(1).Tipo == TipoToken.Numero && Peek(5).Tipo == TipoToken.Numero)
            {
                return AnalizeRangeSequence();
            }
            else if(tokenActual.Tipo==TipoToken.SequenceToken && Peek(1).Tipo==TipoToken.Numero)
            {
                return AnalizeInfiniteSequence();
            }
            else if(tokenActual.Tipo==TipoToken.Cadena)
            {
                return AnalizeCadena();
            }
            
            else if (tokenActual.Tipo == TipoToken.OpeningBrace)
            {   
                return AnalizeSecuence();
            }


         



           










            Instruccion result = AnalizeOperation();


    
            return result;


          


        }

       

        private Instruccion AnalizeOperation()
        {
        Instruccion result = AnalizeArithmeticOperation();

        while (tokenActual != null && esOperadorLogico(tokenActual.Valor))
        {
        Token operador = tokenActual;
        siguienteToken();
        Instruccion derecho = AnalizeArithmeticOperation();
        siguienteToken();
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
        Instruccion nodo = AnalizeExpresion();
        
        if (tokenActual.Tipo != TipoToken.ClosingParenthesis)
            throw new Exception("Error: Se esperaba un paréntesis cerrado.");
            
        siguienteToken();
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
    throw new Exception($"Error: Token inesperado '{tokenActual.Valor}' '{indice}.");
    }
        }
        


        private bool esOperadorLogico(string valor)
{   
    return valor == "==" || valor == "!=" || valor == ">" || valor == "<" || valor == "&&" || valor == "||";
}

        private Instruccion AnalizeRestore()
        {
            siguienteToken();
           
            
            return new Restore();
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
            Instruccion LetInstruction = AnalizeExpresion();



            return new Let(LetInstruction, InstruccionsLet);



        }

        private Instruccion AnalizeIdentifier()
        {   
           string Idvalue = tokenActual.Valor;
           siguienteToken();
           
           return new Identifier (Idvalue );
            
            
           
        }

        private Funcion AnalizeMeasure()
        {

            siguienteToken();
            siguienteToken();

            if(tokenActual.Tipo!=TipoToken.Identificador)
            {
                throw new Exception($"Tipo no soportador {indice}");
            }
            
            Identifier p1 = (Identifier)AnalizeIdentifier();
            siguienteToken();
            Identifier p2 = (Identifier)AnalizeIdentifier();
            siguienteToken();

            return new MeasureFunction(p1, p2);
        }

        private Instruccion AnalizeInfiniteSequence()
        {
            siguienteToken();
            int start = int.Parse(NextToken().Valor);
            siguienteToken();
            siguienteToken();
            siguienteToken();
            siguienteToken();

            return new InfiniteSequence(start);



        }

        private Instruccion AnalizeRangeSequence()
        {
            siguienteToken();
            int start = int.Parse(NextToken().Valor);
            siguienteToken();
            siguienteToken();
            siguienteToken();
            int end  = int.Parse(NextToken().Valor);
            siguienteToken();

            return new FiniteSequence(start, end);

        }
        private Instruccion AnalizeDraw()
        {

            string etiqueta;

            siguienteToken();
           
            Instruccion ToDraw = AnalizeExpresion();


           if(tokenActual.Tipo != TipoToken.Cadena)
            {
                etiqueta = "";

            }
           else
            {
                etiqueta = tokenActual.Valor;
            }
            siguienteToken();
           
            return new Draw(ToDraw , etiqueta);

        }

      private Instruccion AnalizeAsignation()
        {   List<string> nombres = new List<string>(); 
       
            while(tokenActual.Tipo != TipoToken.OperadorAsignación)
            {
                  nombres.Add(tokenActual.Valor);
                  siguienteToken(); 
                  if (tokenActual.Tipo == TipoToken.Coma)
                  {
                    siguienteToken();
                  }
            }

            siguienteToken();
        
           Instruccion asignation = AnalizeExpresion();

           


             
            return new AnalizeAsignation(nombres,asignation);
            
        }

         public Instruccion AnalizeNumber()

         {  
            int value = int.Parse(tokenActual.Valor);
            siguienteToken();
            return new Number(value);

         }

        public IfElseExpression AnalizarIfElse()
        {
            
            siguienteToken();


             
            Instruccion IfExpression = AnalizeExpresion();
            
            
            if (tokenActual.Tipo != TipoToken.ThenKeyWord)
            {
                throw new Exception("Error: Se esperaba la expresion ¨then¨.");

            }
            siguienteToken();
             

            Instruccion ThenExpression = AnalizeExpresion();
           
            
            if (tokenActual.Tipo != TipoToken.ElseKeyWord)
            {
                throw new Exception("Error: Se esperaba la expresion ¨Else¨.");

            }

             siguienteToken();
            
            Instruccion ElseExpression = AnalizeExpresion();




            return new IfElseExpression(IfExpression, ThenExpression, ElseExpression);
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




        public ArcFunction AnalizeArcFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();
            siguienteToken();
         
            Identifier p2 = (Identifier)AnalizeIdentifier();
            siguienteToken();
           
            Identifier p3 = (Identifier)AnalizeIdentifier();
            siguienteToken();
           

            double m = double.Parse(NextToken().Valor);

            siguienteToken();





            return new ArcFunction(p1, p2, p3, m);

        }

        public FunctionLine AnalizeLineFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeExpresion();

            siguienteToken();
          

            Identifier p2 = (Identifier)AnalizeIdentifier();

            siguienteToken();
           





            return new FunctionLine(p1, p2);

        }

        public Point AnalizePoint()
        {
            siguienteToken();
            string id = tokenActual.Valor;
           
            siguienteToken();
            
            return new Point(id);
        }

        public Circle AnalizeCircle()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Circle(id);
        }



        public Ray AnalizeRay()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Ray(id);
        }

        public CircleFunction AnalizeCircleFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();

            siguienteToken();

            Identifier m = (Identifier)AnalizeIdentifier();

            siguienteToken();

            return new CircleFunction(p1, m);

        }

        public Cadena AnalizeCadena()
        {

           

            string expresion = tokenActual.Valor;

            return new Cadena(expresion);


        }

        public SegmentFunction AnalizeSegmentFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();

            siguienteToken();
           
            Identifier p2 = (Identifier)AnalizeIdentifier();

            siguienteToken();
            

            return new SegmentFunction(p1, p2);
        }
        public RayFunction AnalizeRayFunction()
        {
            siguienteToken();
            siguienteToken();

            Identifier p1 = (Identifier)AnalizeIdentifier();

            siguienteToken();
           

            Identifier p2 = (Identifier) AnalizeIdentifier();

            siguienteToken();
            

            return new RayFunction(p1, p2);

        }

        public Line AnalizeLine()
        {
            siguienteToken();
            string id = tokenActual.Valor;
            siguienteToken();
            return new Line(id);
        }

        public Segment AnalizeSegment()
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
            if (tokenActual.Tipo != TipoToken.Identificador)
            {
                throw new CustomException("Identtifier expected ", CustomExceptionType.UnknownValue);
            }

            Identifier secuence = (Identifier)AnalizeIdentifier();



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
                throw new Exception("Error: Se esperaba un paréntesis abierto.");

            siguienteToken();

            List<string> parametros = new List<string>();
            while (tokenActual.Tipo != TipoToken.ClosingParenthesis)
            {
                if (tokenActual.Tipo != TipoToken.Identificador)

                {
                    throw new Exception("Error: Se esperaba un identificador.");
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
                throw new Exception("Error: Se esperaba una =.");

            siguienteToken();

            Instruccion cuerpo = Analize();

            return new FunctionDeclaration(nombreFuncion, parametros, cuerpo);
        }

        private Instruccion AnalizeFunctionCall()
        {
            string nombreFuncion = tokenActual.Valor;
            siguienteToken();

            if (tokenActual.Tipo != TipoToken.OpeningParenthesis)
                throw new Exception("Error: Se esperaba un paréntesis abierto.");

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










