using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace ClassLibrary1
{   
    public abstract class Instruccion
    {
        public abstract object Evaluate(Entorno entorno);
    }


    public enum CustomExceptionType
    {
        NullReference,
        UnknownValue,
       
    }

    //  clase de excepción personalizada que hereda de Exception
    public class CustomException : Exception
    {  
        public CustomExceptionType Tipo { get; set; }

        public CustomException(string message, CustomExceptionType tipo)
            : base(message)
        {
            this.Tipo = tipo;
        }
    }

    public class CountFunction : Instruccion
    {
        public Identifier secuence { get; }

        public CountFunction(Identifier secuence)
        {
            this.secuence = secuence;
        }

        public override object Evaluate(Entorno entorno)
        {
            


            var secuenceValue = (SecuenceFigure<Instruccion>)this.secuence.Evaluate(entorno);
            return secuenceValue.secuenceFigure.Count();




        }
    }

    public class Color : Instruccion
    {
        public string color { get; }

        public Color(string color)
        {
            this.color = color;
        }
        public override object Evaluate(Entorno entorno)
        {
            entorno.ActualColor.Push(color);
            return null;
        }
    }

    public class Restore : Instruccion
    {



        public override object Evaluate(Entorno entorno)
        {
            if (entorno.ActualColor.Count() == 0)

            {
                return null;
            }
            entorno.TrashColor.Push(entorno.ActualColor.Pop());

            return null;
        }
    }
    public class OperacionAritmetica : Instruccion
    {
        public Instruccion OperandoIzquierdo { get; }
        public Instruccion OperandoDerecho { get; }
        public string Operador { get; }

        public OperacionAritmetica(Instruccion operandoIzquierdo, Instruccion operandoDerecho, string operador)
        {
            OperandoIzquierdo = operandoIzquierdo;
            OperandoDerecho = operandoDerecho;
            Operador = operador;
        }

        public override object Evaluate(Entorno entorno)
        {
            var left = OperandoIzquierdo.Evaluate(entorno);
            var right = OperandoDerecho.Evaluate(entorno);

            if (left.GetType() != right.GetType())
            {
                throw new CustomException($" Error type :  {CustomExceptionType.NullReference} . Can 't operate type {left.GetType()} with {right.GetType() }  .", CustomExceptionType.NullReference);
            }

            if (left is SecuenceFigure<Instruccion> && right is SecuenceFigure<Instruccion> && Operador.Equals("+"))
            {
                SecuenceFigure<Instruccion> leftSecuence = (SecuenceFigure<Instruccion>)left;
                SecuenceFigure<Instruccion> rightSecuence = (SecuenceFigure<Instruccion>)right;
                List<Instruccion> SecuenceSum = new List<Instruccion>(leftSecuence.secuenceFigure.Concat(rightSecuence.secuenceFigure));


                return new SecuenceFigure<Instruccion>(SecuenceSum);
            }

            else
            {
                double valorIzquierdo = Convert.ToDouble(left);
                double valorDerecho = Convert.ToDouble(right);




                switch (Operador)
                {
                    case "+":
                        return valorIzquierdo + valorDerecho;
                    case "-":
                        return valorIzquierdo - valorDerecho;
                    case "*":
                        return valorIzquierdo * valorDerecho;
                    case "%":
                        return valorIzquierdo % valorDerecho;
                    case "^":
                        return Math.Pow(valorIzquierdo, valorDerecho);
                    case "/":
                        if (valorDerecho != 0)
                        {
                            return valorIzquierdo / valorDerecho;
                        }
                        else
                        {
                            throw new Exception("Error: División por cero.");
                        }
                    default:
                        throw new Exception($"Error: Operador desconocido '{Operador}'.");
                }

            }
        }
    }

    public class OperadorLogico : Instruccion
{   


    public Instruccion OperandoIzquierdo { get; }
    public Instruccion OperandoDerecho { get; }
    public string Operador { get; }

    public OperadorLogico(Instruccion operandoIzquierdo, Instruccion operandoDerecho, string operador)
    {
        OperandoIzquierdo = operandoIzquierdo;
        OperandoDerecho = operandoDerecho;
        Operador = operador;
    }

    public override object Evaluate(Entorno entorno)
{
   
    var valorIzquierdo = OperandoIzquierdo.Evaluate(entorno);
    var valorDerecho = OperandoDerecho.Evaluate(entorno);

   
    switch (Operador)
    {
        case "==":
            return Convert.ToDouble(valorIzquierdo) == Convert.ToDouble(valorDerecho);
        case "!=":
            return Convert.ToDouble(valorIzquierdo) != Convert.ToDouble(valorDerecho);;
        case ">":
            return Convert.ToDouble(valorIzquierdo) > Convert.ToDouble(valorDerecho);
        case "<":
            return Convert.ToDouble(valorIzquierdo) < Convert.ToDouble(valorDerecho);
        case "&&":
            return Convert.ToBoolean(valorIzquierdo) && Convert.ToBoolean(valorDerecho);
        case "||":
            return Convert.ToBoolean(valorIzquierdo) || Convert.ToBoolean(valorDerecho);
        default:
            throw new Exception($"Error: Operador desconocido '{Operador}'.");
    }
}

}


  





public class Negacion : Instruccion
{
    private Instruccion subexpresion;

    public Negacion(Instruccion subexpresion)
    {
        this.subexpresion = subexpresion;
    }

    public override object Evaluate(Entorno entorno)
{
    
    object valor = subexpresion.Evaluate(entorno);

    
    if (valor is int)
    {
       
        return - Convert.ToDouble(valor);
    }
    else if (valor is double)
    {
        
        return - (double)valor;
    }
    else
    {
      
        throw new Exception("Error: se intentó negar un valor no numérico.");
    }
}
}

    public class Number : Instruccion
{
    public int Value { get; }

    public Number(int value)
    {
        Value = value;
    }

   

        public override object Evaluate(Entorno entorno)
        {
            return Value;
        }
    }

    public class Let : Instruccion
    {

        public List<Instruccion> InstruccionsLet { get; }
        public Instruccion InExpresion { get; }

        public Let(Instruccion InExpresion, List<Instruccion> InstruccionsLet)
        {
            this.InstruccionsLet = InstruccionsLet;
            this.InExpresion = InExpresion;
        }





        public override object Evaluate(Entorno entorno)
        {

            Entorno entornoLocal = entorno.DeepCopy();



            // Ejecutar cada instrucción en InstruccionsLet en el entorno local.
            foreach (var instruccion in InstruccionsLet)
            {
                instruccion.Evaluate(entornoLocal);
            }

            Entorno entornoLocal2 = entornoLocal.DeepCopy();

            object result = InExpresion.Evaluate(entornoLocal2);
            Entorno entornoDiference = entornoLocal2.Difference(entornoLocal);
            entorno.ApplyDifference(entornoDiference);
            return result;

            //, evaluar y devolver el resultado de InExpresion en el entorno local.




        }
    }
}

public class AnalizeAsignation : Instruccion
{
    public List<string> Nombre { get; }
    public Instruccion Value { get; set; }

    public AnalizeAsignation(List<string> nombre, Instruccion value)
    {
        Nombre = nombre;
        Value = value;
    }

    public override object Evaluate(Entorno entorno)
    {
        
       

            var Value = this.Value.Evaluate(entorno);



            if (Value is SecuenceFigure<Instruccion> secuencia)
            {





                for (int i = 0; i < Nombre.Count; i++)
                {
                    // Si se usa "_" como uno de los nombres de la variable, se ignora la asignación correspondiente
                    if (Nombre[i] == "_")
                        continue;


                    else if (i == Nombre.Count - 1)
                    {
                        var resto = secuencia.secuenceFigure.Skip(i).ToList();





                        entorno.DefinirVariable(new Variable(Nombre[i], new SecuenceFigure<Instruccion>(resto.ToList())));
                        break;








                    }

                    // Si se piden valores inexistentes a una secuencia, se guarda en la variable un tipo undefined
                    else if (i >= secuencia.secuenceFigure.Count())
                    {
                        entorno.DefinirVariable(new Variable(Nombre[i], new Undefined()));
                        continue;
                    }

                    // Si el elemento de Nombres es el último de la lista, se le asigna el resto de la secuencia


                    // Se asigna uno con uno cada variable a cada valor de la secuencia
                    if (secuencia.secuenceFigure[i] is Number)
                    {
                        entorno.DefinirVariable(new Variable(Nombre[i], secuencia.secuenceFigure[i].Evaluate(entorno)));
                    }
                    else
                    {
                        entorno.DefinirVariable(new Variable(Nombre[i], secuencia.secuenceFigure[i]));
                    }
                    ;

                }
            }

            else
            {
                foreach (var nombre in Nombre)
                {

                    entorno.DefinirVariable(new Variable(nombre, Value));
                }
            }





            return null;
        }
    }










public sealed class Identifier : Instruccion
{
    public string name { get; }

    public Identifier(string name)
    {
        this.name = name;
    }
    public override object Evaluate(Entorno entorno)


    {
        if (entorno.BuscarVariable(name) != null)
        {
            return entorno.BuscarVariable(name).Value;
        }
        else
        {
            throw new CustomException($" Error type :  {CustomExceptionType.NullReference} . Value  {name}  not found .", CustomExceptionType.NullReference);
        }
    }
}


public sealed class Draw : Instruccion

    {   

        public string Etiqueta { get; }
        public Instruccion ToDraw {get;}

        public Draw( Instruccion ToDraw , string etiqueta)
        {
           this.ToDraw = ToDraw;
           Etiqueta = etiqueta;
        }
        public override object Evaluate(Entorno entorno)
        {
           if(entorno.ActualColor.Count==0)
        {

            if (ToDraw.GetType() == typeof(SecuenceFigure<Instruccion>))
            {
                var secuence = (SecuenceFigure<Instruccion>)ToDraw;
                foreach (Identifier id in secuence.secuenceFigure)
                {
                    entorno.figuras.Add(((Figura)id.Evaluate(entorno), "black"));
                }
            }
            else
            {
                Instruccion draw = (Instruccion)ToDraw.Evaluate(entorno);
                if (draw.GetType() == typeof(FunctionLine))
                {
                    Instruccion mdraw = (Figura)draw.Evaluate(entorno);

                    entorno.figuras.Add(((Figura)mdraw, "black"));

                }

                else
                {

                    entorno.figuras.Add(((Figura)draw, "black"));
                }

            }
        }
            else
        {
            if (ToDraw.GetType() == typeof(SecuenceFigure<Instruccion>))
            {
                var secuence = (SecuenceFigure<Instruccion>)ToDraw;
                foreach (Identifier id in secuence.secuenceFigure)
                {
                    entorno.figuras.Add(((Figura)id.Evaluate(entorno), entorno.ActualColor.Peek()));
                }
            }
            else
            {
                Instruccion draw = (Instruccion)ToDraw.Evaluate(entorno);
                if (draw.GetType() == typeof(FunctionLine))
                {
                    Instruccion mdraw = (Figura)draw.Evaluate(entorno);

                    entorno.figuras.Add(((Figura)mdraw, entorno.ActualColor.Peek()));

                }

                else
                {

                    entorno.figuras.Add(((Figura)draw, entorno.ActualColor.Peek()));
                }

            }

        }

            
          
            return null;



        }
    }



    public abstract class Funcion : Instruccion
    {

    }

    public sealed class FunctionLine : Funcion
    {

        public Identifier Punto1 { get; }
        public Identifier Punto2 { get; }



        public FunctionLine(Identifier P1, Identifier P2)
        {

            this.Punto1 = P1;
            this.Punto2 = P2;

        }

        public override object Evaluate(Entorno entorno)
        {

            return new Line((Point)entorno.BuscarVariable(Punto1.name).Value, (Point)entorno.BuscarVariable(Punto2.name).Value);

        }
    }

    public sealed class Arc : Figura
    {
        public Point  Centro { get; }
        public Point Punto2 { get; }
        public Point Punto3 { get; }

        public double Measure { get; }

        public override string nombre { get; set; }
        public Arc(Point punto1,Point punto2 , Point punto3, double measure)
        {
            Centro = punto1;
            Punto2 = punto2;
            Punto3 = punto3;
            Measure = measure;
        }

        public override object Evaluate(Entorno entorno)
        {
            var arc = new Variable(nombre, this);
            entorno.DefinirVariable(arc);
            return null;
        }
    }



    public sealed class SegmentFunction : Figura
    {

        public Identifier Punto1 { get; }
        public Identifier Punto2 { get; }
        public override string nombre { get; set ; }

        public SegmentFunction(Identifier P1, Identifier P2)
        {

            this.Punto1 = P1;
            this.Punto2 = P2;

        }

        public override object Evaluate(Entorno entorno)
        {
            return new Segment((Point)entorno.BuscarVariable(Punto1.name).Value, (Point)entorno.BuscarVariable(Punto2.name).Value);
        }
    }

    public sealed class RayFunction : Figura
    {

        public Identifier Punto1 { get; }
        public Identifier Punto2 { get; }
        public override string nombre { get; set; }

        public RayFunction(Identifier P1, Identifier P2)
        {

            this.Punto1 = P1;
            this.Punto2 = P2;

        }

        public override object Evaluate(Entorno entorno)
        {
            return new Ray((Point)entorno.BuscarVariable(Punto1.name).Value, (Point)entorno.BuscarVariable(Punto2.name).Value);
        }
    }


    public sealed class MeasureFunction : Funcion
    {

        public Identifier P1 { get; }
        public Identifier P2 { get; }

        public MeasureFunction(Identifier P1, Identifier P2)
        {

            this.P1 = P1;
            this.P2 = P2;

        }

    public override object Evaluate(Entorno entorno)
    {
        Point punto1 = (Point)entorno.BuscarVariable(P1.name).Value;
        Point punto2 = (Point)entorno.BuscarVariable(P2.name).Value;

        double distance = Math.Sqrt(Math.Pow((punto1.Coord.X - punto2.Coord.X), 2) + Math.Pow((punto2.Coord.Y - punto1.Coord.Y), 2));

        return distance;
    }

    }

    public sealed class ArcFunction : Figura
    {

        public override string  nombre { get; set ;}

        public Identifier Centro { get; }
        public Identifier Punto2 { get; }
        public Identifier Punto3 { get; }
        public double Measure { get; }

        public ArcFunction(Identifier P1, Identifier P2, Identifier P3, double Measure)
        {

            Centro = P1;
            this.Punto2 = P2;
            this.Punto3 = P3;
            this.Measure = Measure;

        }

        public override object Evaluate(Entorno entorno)
        {
            return new Arc((Point)entorno.BuscarVariable(Centro.name).Value, (Point)entorno.BuscarVariable(Punto2.name).Value, (Point)entorno.BuscarVariable(Punto3.name).Value , Measure);
        }

        
    }

    public sealed class IntersectFunction : Funcion
    {

        public Figura Figure1 { get; }
        public Figura Figure2 { get; }


        public IntersectFunction(Figura Figure1, Figura Figure2)
        {

            this.Figure1 = Figure1;
            this.Figure2 = Figure2;


        }

        public override object Evaluate(Entorno entorno)
        {
            throw new NotImplementedException();
        }
    }

    

    public abstract class Figura : Instruccion
    {


        public abstract string nombre { get; set; }
        

    }

    public sealed class PointsFunction:Instruccion
    {
        Figura Figura { get; }

        public PointsFunction(Figura figura)
        {
        Figura = figura;

        }
        public IEnumerable<Point> Points()
        {
             

            int x = 0;
            int y = 0;

        Coord coord = new Coord(x, y);

            while (true)
            {
                yield return new Point(coord);
                x++;
                y++;
            }
        }

    public override object Evaluate(Entorno entorno)
    {
        throw new NotImplementedException();
    }
}






    public sealed class CircleFunction : Figura
    {

        public Identifier P1 { get; }

        public Identifier Measure { get; }

        public override string nombre { get; set; }

        public CircleFunction(Identifier P1, Identifier Measure)
        {

            this.P1 = P1;
            this.Measure = Measure;

        }

        public override object Evaluate(Entorno entorno)
        {
            return new Circle((Point)entorno.BuscarVariable(P1.name).Value,(double) entorno.BuscarVariable(Measure.name).Value);
        }
    }




    public sealed class Samples : Funcion
    {
        public override object Evaluate(Entorno entorno)
        {
            throw new NotImplementedException();
        }
    }



    public sealed class Coord
    {
         public int X { get; set; }
        public int Y { get; set; }

        public Coord(int x , int y )
        {
             
                X=x;
                Y=y;
        }
    }



    public sealed class Point: Figura
    { 
        public Coord Coord {get;}

        public override string nombre  { get; set; }

        private static Random r = new Random();

       



        public Point(string nombre )
        {
           
           this.nombre= nombre;
            
            int  x = r.Next(1, 300);
            int y = r.Next(1, 300);
            Coord cor = new Coord(x, y);
            Coord = cor;
            
        }

    

    public Point(Coord coordenadas)
    {
        Coord = coordenadas;
        
    }





    public override object Evaluate(Entorno entorno)
        {  
             var point = new Variable(nombre,this);
             entorno.DefinirVariable(point);
             return null;
        }
    }



    public sealed class Circle : Figura
    {
        public Point Centro { get; }
        public int Radio { get; }

        public double Measure { get; }
        public Circle(string nombre)
        {
            Point p1 = new Point("p1" );
            Centro = p1;
            Random r = new Random();
            Measure= r.Next(1, 500);
            this.nombre = nombre;

        }

        public Circle (Point centro, double  measure)
        {
            Centro = centro;
            Measure= measure;
            this.nombre = "Circulo sin nombre";
        }


        public override string nombre { get; set; }

        public override object Evaluate(Entorno entorno)
        {
            var cirlce = new Variable(nombre,this);
             entorno.DefinirVariable(cirlce);
             return null;
        }
    }

    public sealed class Line : Figura
    {
        public Point Punto1 { get; }
        public Point Punto2 { get; }
        public override string nombre { get; set; }

        // Constructor que recibe el nombre como un string
        public Line(string nombre)
        {
            Point p1 = new Point("p1");
            Point p2 = new Point("p2");

            Punto1 = p1;
            Punto2 = p2;
            this.nombre = nombre;
        }

        // Constructor que recibe dos objetos de tipo Point
        public Line(Point punto1, Point punto2)
        {
            Punto1 = punto1;
            Punto2 = punto2;
            this.nombre = "Linea sin nombre";
        }

        public override object Evaluate(Entorno entorno)
        {
            var line = new Variable(nombre, this);
            entorno.DefinirVariable(line);
            return null;
        }
    }

    public sealed class Ray : Figura
    {
        public Point Punto1 { get; }
        public Point Punto2 { get; }

        public Ray(string nombre)
        {
            Point p1 = new Point("p1");
            Point p2 = new Point("p2");

            

            Punto1 = p1;
            Punto2 = p2;

            this.nombre = nombre;

        }

        public Ray(Point punto1, Point punto2)
        {
            Punto1 = punto1;
            Punto2 = punto2;
            this.nombre = "Rayo sin nombre";
        }


        public override string nombre { get; set; }

        public override object Evaluate(Entorno entorno)
        {
            var ray = new Variable(nombre,this);
             entorno.DefinirVariable(ray);
             return null;
        }
    }


    public sealed class Segment : Figura
    {

        public Point Punto1 { get; }
        public Point Punto2 { get; }

        public Segment(string nombre)
        {
            Point p1 = new Point("p1");
            Point p2 = new Point("p2");


            Punto1 = p1;
            Punto2 = p2;

            this.nombre = nombre;

        }

        public Segment(Point punto1, Point punto2)
        {
            Punto1 = punto1;
            Punto2 = punto2;
            this.nombre = "Segmento sin nombre";
        }


        public override string nombre { get; set; }

        public override object Evaluate(Entorno entorno)
        {
           var segment = new Variable(nombre,this);
             entorno.DefinirVariable(segment);
             return null;
        }
    }

   


    public class Variable
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
        }


    }



    

   




public class InfiniteSequence : Instruccion
{
    private int start;

    public InfiniteSequence(int start)
    {
        this.start = start;
    }

    public IEnumerable<int> GetSequence()
    {
        int current = start;
        while (true)
        {
            yield return current;
            current++;
        }
    }

    public IEnumerable<int> GetLimitedSequence()
    {
        return GetSequence().Take(1000);
    }

    public override object Evaluate(Entorno entorno)
    {
        IEnumerable<int> seq =this.GetLimitedSequence();
        return seq;
    }
}


public class SecuenciaPuntos
{


    private IEnumerator<Point> generadorPuntos;

    public SecuenciaPuntos(Func<Point> generador)
    {
        generadorPuntos = GenerarSecuencia(generador).GetEnumerator();
    }

    private IEnumerable<Point> GenerarSecuencia(Func<Point> generador)
    {
        while (true)
        {
            yield return generador();
        }
    }

    public Point SiguientePunto()
    {
        generadorPuntos.MoveNext();
        return generadorPuntos.Current;
    }





}

public class FiniteSequence : Instruccion
{
    private int start;
    private int end;

    public FiniteSequence(int start, int end)
    {
        this.start = start;
        this.end = end;
    }

    public IEnumerable<int> GetSequence()
    {
        for (int current = this.start; current <= this.end; current++)
        {
            yield return current;
        }
    }

    public override object Evaluate(Entorno entorno)
    {
        IEnumerable<int> seq=this.GetSequence();
        return seq;
    }
}

public class Cadena : Instruccion
{

    public string cadena { get; }
    
    public Cadena (string expression)
    {
        cadena = expression;
    }

    public override object Evaluate(Entorno entorno)
    {
        return cadena;
    }



}

public class IfElseExpression : Instruccion
    {
        public Instruccion Condicion { get; }
        public Instruccion ExpresionIf { get; }
        public Instruccion ExpresionElse { get; }

        public IfElseExpression(Instruccion condicion, Instruccion expresionIf, Instruccion expresionElse)
        {
            Condicion = condicion;
            ExpresionIf = expresionIf;
            ExpresionElse = expresionElse;
        }

        public override object Evaluate(Entorno entorno)
        {    
          
              object auxCond = this.Condicion.Evaluate(entorno); 
           

           
           
           
            if (  auxCond is SecuenceFigure<Instruccion> SecCondition && SecCondition.secuenceFigure.Count() == 0  || 
                auxCond is int number  && number.Equals(0) ||
               auxCond is Undefined
               )
            {   
                return ExpresionElse.Evaluate(entorno);
                
            }




            else
            {
                return ExpresionIf.Evaluate(entorno);
            }
        }
    }

public class SecuenceFigure<T> : Instruccion 
{
    public List<T> secuenceFigure { get; }

    public SecuenceFigure(List<T> secuenceFigure)
    {
        this.secuenceFigure = secuenceFigure;
    }
   public override object Evaluate(Entorno entorno)
{
    List<T> Evaluatedsecuence = new List<T>();
    Type type = null;
    
    foreach (T instruction in secuenceFigure)

    {   
        T evaluatedInstruction = instruction;
       
       if(instruction is Identifier)
    {
    Instruccion instruccion = instruction as Instruccion;
       
        var evaluated = instruccion.Evaluate(entorno);
    if (evaluated is  int )
    {
        object NumberObject = new Number((int)evaluated) ;
         evaluatedInstruction = (T)NumberObject;
    }
     else
     {  
        object ob = evaluated;
        evaluatedInstruction =(T) ob;
     }
        
    }

        
        if (type == null)
        {
            type = evaluatedInstruction.GetType();
        }
        else if (evaluatedInstruction.GetType() != type)
        {
            throw new Exception($"All elements in the sequence must be of the same type. Cannot insert an element of type {evaluatedInstruction.GetType().Name} into a sequence of type {type.Name}.");
        }
        Evaluatedsecuence.Add(evaluatedInstruction);
    }
    
    SecuenceFigure<T> EvaluatedSecuence = new SecuenceFigure<T>(Evaluatedsecuence);
    return EvaluatedSecuence;
}

}

public sealed class Undefined 
    {
   
        
    public Undefined()
    {
        nombre = "undefined";
    }

    public  string nombre { get ;set ; }

    public object Evaluate(Entorno entorno)
        {
            return this;
           
        }
    }

public class FunctionDeclaration : Instruccion
{
    public string Nombre { get; }
    public List<string> Parametros { get; }
    public Instruccion Cuerpo { get; }

    public

    FunctionDeclaration(string nombre, List<string> parametros, Instruccion cuerpo)
    {
        Nombre = nombre;
        Parametros = parametros;
        Cuerpo = cuerpo;

    }
    public override object Evaluate(Entorno entorno)
    {

        entorno.DefineFunction(this);


        return null;
    }

}

public class FunctionCall : Instruccion
{
    public string Nombre { get; }
    public List<Instruccion> Argumentos { get; }

    public FunctionCall(string nombre, List<Instruccion> argumentos)
    {
        Nombre = nombre;
        Argumentos = argumentos;
    }

    public override object Evaluate(Entorno entorno)
    {
        var funcion = entorno.BuscarFuncion(Nombre);

        if (funcion == null)
        {
            throw new Exception($"Error: Función no definida '{Nombre}'.");
        }

        var entornoFuncion = new Entorno();


        foreach (var variable in entorno.variables)
        {
            entornoFuncion.DefinirVariable(variable.Value);
        }
        foreach (var function in entorno.funciones)
        {
            entornoFuncion.DefineFunction(function.Value);
        }

        for (int i = 0; i < Argumentos.Count; i++)
        {
            var valor = Argumentos[i].Evaluate(entorno);
            var variable = new Variable(funcion.Parametros[i], valor);

            entornoFuncion.DefinirVariable(variable);
            entorno.DefinirVariable(variable);
        }



        return funcion.Cuerpo.Evaluate(entornoFuncion);
    }
}




















public class Entorno
{
    public Stack<string> ActualColor = new Stack<string>();
    public Stack<string> TrashColor = new Stack<string>();

    public Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

    public List<(Figura,string) > figuras = new List<(Figura,string) >();

    public void DefinirVariable(Variable variable)
    {
        variables[variable.Name] = variable;
    }



    public Variable BuscarVariable(string nombre)
    {
        if (variables.TryGetValue(nombre, out var variable))
        {
            return variable;
        }
        else
        {
            return null;
        }
    }

    public void ApplyDifference(Entorno difference)
    {
        // Agrega las variables de 'difference' a 'this'
        foreach (var variable in difference.variables)
        {
            this.DefinirVariable(variable.Value);
        }

        // Agrega las figuras de 'difference' a 'this'
        foreach (var figura in difference.figuras)
        {
            this.figuras.Add(figura);
        }
    }


    public Entorno DeepCopy()
    {
        Entorno newEntorno = new Entorno();

        // Copia cada variable de this a newEntorno
        foreach (var variable in this.variables)
        {
            newEntorno.variables.Add(variable.Key, variable.Value);
        }

        // Copia cada figura de this' a newEntorno
        foreach (var figura in this.figuras)
        {
            newEntorno.figuras.Add(figura);
        }

        return newEntorno;
    }

    public Entorno Difference(Entorno other)
    {
        Entorno difference = new Entorno();

        // Agrega las variables de this que no están en 'other' a difference
        foreach (var variable in this.variables)
        {
            if (!other.variables.ContainsKey(variable.Key))
            {
                difference.DefinirVariable(variable.Value);
            }


        }

        foreach (var figura in this.figuras)
        {

            if (!other.figuras.Contains(figura))
            {
                difference.figuras.Add(figura);
            }

        }

        // Agrega las variables de other que no están en this a difference


        return difference;
    }

    public Dictionary<string, FunctionDeclaration> funciones = new Dictionary<string, FunctionDeclaration>();

    public void DefineFunction(FunctionDeclaration funcion)
    {
        funciones[funcion.Nombre] = funcion;
    }

    public FunctionDeclaration BuscarFuncion(string nombre)
    {
        if (funciones.TryGetValue(nombre, out var funcion))
        {
            return funcion;
        }
        else
        {
            return null;
        }
    }
}
