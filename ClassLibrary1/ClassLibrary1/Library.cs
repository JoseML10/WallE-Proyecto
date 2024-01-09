using ClassLibrary1;
using System;
using System.Collections;
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
        InvalidOperation,
        UnknownChar

    }

    public interface IDrawable
    {
        void GetDrawable(Entorno entorno, string label);
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

    public class Import : Instruccion
    {
        public string Ruta { get; }

        public Import(string ruta)
        {
            Ruta = ruta;
        }

        public override object Evaluate(Entorno entorno)
        {
            return this;

        }

    }

    public sealed class PointsFunction : Instruccion, Isecuenciable, IEnumerator<Point>
    {
        private Random random;
        private Instruccion figura;
        private Figura Evaluated_figura;
       


        public int? SecuenceCount => null;

        public Point Current
        {
            get
            {

                return  InternalPoint(Evaluated_figura); 
                
               
                
            }
        }

        object IEnumerator.Current => Current;

        public PointsFunction(Instruccion figura)
        {
            this.random = new Random();
            this.figura = figura;



        }

        public override object Evaluate(Entorno entorno)
        {
            this.Evaluated_figura = (Figura)figura.Evaluate(entorno);
            return this;
        }

        public object GetElement(int i, Entorno entorno)
        {
           
            return Current;
        }

        public object SubSecuence(int i)
        {
            return this;
        }

        public bool MoveNext()
        {
           
            return true;
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private Point InternalPoint(Figura figura )
        {
            if (figura is Circle c)
            {
                double angle = random.NextDouble() * Math.PI * 2;
                double radius = Math.Sqrt(random.NextDouble()) * c.Measure * 4 / 5; // Ajuste aquí
                double x = c.Centro.Coord.X + radius * Math.Cos(angle);
                double y = c.Centro.Coord.Y + radius * Math.Sin(angle);
                return new Point(new Coord(x, y));
            }

            else if (figura is Line l)
            {
                double t = random.NextDouble(); // Genera un número aleatorio entre 0 y 1
                double x = l.Punto1.Coord.X + t * (l.Punto2.Coord.X - l.Punto1.Coord.X);
                double y = l.Punto1.Coord.Y + t * (l.Punto2.Coord.Y - l.Punto1.Coord.Y);
                return new Point(new Coord(x, y));
            }
            else if (figura is Ray r)
            {
                double t = random.NextDouble(); // Genera un número aleatorio entre 0 y 1
                                                // Asegura que el punto esté en la dirección correcta para el rayo
                double x = r.Punto1.Coord.X + t * (r.Punto2.Coord.X - r.Punto1.Coord.X);
                double y = r.Punto1.Coord.Y + t * (r.Punto2.Coord.Y - r.Punto1.Coord.Y);
                return new Point(new Coord(x, y));
            }
            else if (figura is Segment s)
            {
                double t = random.NextDouble(); // Genera un número aleatorio entre 0 y 1
                                                // Asegura que el punto esté dentro del segmento
                double x = s.Punto1.Coord.X + t * (s.Punto2.Coord.X - s.Punto1.Coord.X);
                double y = s.Punto1.Coord.Y + t * (s.Punto2.Coord.Y - s.Punto1.Coord.Y);
                return new Point(new Coord(x, y));
            }




            else
            {
                throw new ArgumentException("Figura desconocida");
            }
        }
    }

    public sealed class Samples : Instruccion, Isecuenciable, IEnumerator<Point>
    {
        private Random random;


        public int? SecuenceCount => null;

        public Point Current => new Point(new Coord(random.Next(1, 500), random.Next(1, 500)));

        object IEnumerator.Current => Current;

        public Samples()
        {
            this.random = new Random();

        }




        public override object Evaluate(Entorno entorno)
        {
            return this;

        }

        public object GetElement(int i, Entorno entorno)
        {
            return Current;
        }

        public object SubSecuence(int i)
        {
            return this;
        }



        public bool MoveNext()
        {
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    public class PrintExpression : Instruccion
    {
        public static List<object> Printeables = new List<object>();
        public Instruccion Expresion { get; }

        public PrintExpression(Instruccion expresion)
        {
            Expresion = expresion;
        }

        public override object Evaluate(Entorno entorno)
        {

            var ToPrint = Expresion.Evaluate(entorno);

            if (ToPrint is Number num)
            {
                Printeables.Add(num.Value.ToString());
                return null;
            }

            else if (ToPrint is Isecuenciable secuence)
            {

                for (int i = 0; i < secuence.SecuenceCount; i++)
                {
                    if (secuence.GetElement(i, entorno) is Number n)
                    {
                        Printeables.Add(n.Value.ToString());

                    }
                    else
                    {
                        Printeables.Add(secuence.GetElement(i, entorno));


                    }

                }
                return null;
            }


            else
            {
                Printeables.Add(ToPrint.ToString());
                return null;
            }




        }
    }


    public class CountFunction : Instruccion
    {
        public Instruccion secuence { get; }

        public CountFunction(Instruccion secuence)
        {
            this.secuence = secuence;
        }

        public override object Evaluate(Entorno entorno)
        {
            Isecuenciable secuence = (Isecuenciable)this.secuence.Evaluate(entorno);
            if (secuence.SecuenceCount is null)
            {
                return new Undefined();
            }

            else
            {


                return secuence.SecuenceCount;
            }





        }
    }

    public sealed class PointSequence : Instruccion
    {
        private Random random;

        public string Id { get; }
        public int? SecuenceCount => null;

        public SecuenceFigure<Point> pointSequence { get; private set; }


        public PointSequence(string Id)
        {
            this.Id = Id;
            this.random = new Random();

            List<Point> PointsList = new List<Point>();
            for (var i = 0; i < 100; i++)
            {

                PointsList.Add(new Point(new Coord(random.Next(1, 500), random.Next(1, 500))));
            }
            this.pointSequence = new SecuenceFigure<Point>(PointsList);

        }




        public override object Evaluate(Entorno entorno)


        {



            var point = new Variable(Id, pointSequence);
            entorno.DefinirVariable(point);

            return this.pointSequence;



        }


    }

    public sealed class LineSequence : Instruccion
    {
        private Random random;

        public string Id { get; }
        public int? SecuenceCount => null;

        public SecuenceFigure<Line> lineSequence { get; private set; }


        public LineSequence(string Id)
        {
            this.Id = Id;
            this.random = new Random();

            List<Line> LineList = new List<Line>();
            for (var i = 0; i < 100; i++)
            {

                LineList.Add(new Line(new Point(new Coord(random.Next(1, 500), random.Next(1, 500))), new Point(new Coord(random.Next(1, 500), random.Next(1, 500)))));
            }
            this.lineSequence = new SecuenceFigure<Line>(LineList);

        }




        public override object Evaluate(Entorno entorno)


        {



            var point = new Variable(Id, lineSequence);
            entorno.DefinirVariable(point);

            return this.lineSequence;



        }


    }


    public sealed class Undefined : Instruccion
    {


        public Undefined()
        {
            nombre = "undefined";
        }

        public string nombre { get; set; }



        public override object Evaluate(Entorno entorno)
        {
            return this;
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








            if (left is Isecuenciable LeftSec && right is Isecuenciable RightSec && Operador.Equals("+"))
            {
                List<Instruccion> SumList = new List<Instruccion>();
                for (var i = 0; i < LeftSec.SecuenceCount; i++)
                {
                    SumList.Add((Instruccion)LeftSec.GetElement(i, entorno));
                }
                for (var i = 0; i < RightSec.SecuenceCount; i++)
                {
                    SumList.Add((Instruccion)RightSec.GetElement(i, entorno));
                }

                return new SecuenceFigure<Instruccion>(SumList);




            }

            else if (left is Undefined && right is Isecuenciable && Operador.Equals("+"))
            {

                return new Undefined();

            }


            else if (left is Isecuenciable && right is Undefined && Operador.Equals("+"))
            {

                return left;

            }

            else if (left.GetType() != right.GetType())
            {
                throw new CustomException($" Error type :  {CustomExceptionType.InvalidOperation} . Can 't operate type {left.GetType()} with {right.GetType()}  .", CustomExceptionType.InvalidOperation);
            }


            else
            {
                int valorIzquierdo = ((Number)left).Value;
                int valorDerecho = ((Number)right).Value;




                switch (Operador)
                {
                    case "+":
                        return new Number(valorIzquierdo + valorDerecho);
                    case "-":
                        return new Number(valorIzquierdo - valorDerecho);
                    case "*":
                        return new Number(valorIzquierdo * valorDerecho);
                    case "%":
                        return new Number(valorIzquierdo % valorDerecho);
                    case "^":
                        return new Number((int)Math.Pow(valorIzquierdo, valorDerecho));
                    case "/":
                        if (valorDerecho != 0)
                        {
                            return new Number(valorIzquierdo / valorDerecho);
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


    public class LogicConector : Instruccion
    {


        public Instruccion OperandoIzquierdo { get; }
        public Instruccion OperandoDerecho { get; }
        public string Operador { get; }

        public LogicConector(Instruccion operandoIzquierdo, Instruccion operandoDerecho, string operador)
        {
            OperandoIzquierdo = operandoIzquierdo;
            OperandoDerecho = operandoDerecho;
            Operador = operador;
        }

        public bool isFalseBoolean(object boolean)
        {

            if (boolean is Isecuenciable SecCondition && SecCondition.SecuenceCount == 0 ||
                boolean is Number number && number.Value.Equals(0) ||
               boolean is Undefined || boolean is false
               )


            {
                return false;

            }

            return true;

        }

        public override object Evaluate(Entorno entorno)
        {

            bool valorIzquierdo = isFalseBoolean(OperandoIzquierdo.Evaluate(entorno));
            bool valorDerecho = isFalseBoolean(OperandoDerecho.Evaluate(entorno));


            switch (Operador)
            {

                case "and":

                    return valorDerecho && valorIzquierdo;


                case "or":
                    return valorDerecho || valorIzquierdo;
                default:
                    throw new Exception($"Error: Unknown operator '{Operador}'.");
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

            Number valorIzquierdo = (Number)OperandoIzquierdo.Evaluate(entorno);
            Number valorDerecho = (Number)OperandoDerecho.Evaluate(entorno);


            switch (Operador)
            {
                case "==":
                    return (valorIzquierdo.Value) == (valorDerecho.Value);
                case "!=":
                    return (valorIzquierdo.Value) != (valorDerecho.Value);
                case ">":
                    return (valorIzquierdo.Value) > (valorDerecho.Value);
                case ">=":
                    return (valorIzquierdo.Value) >= (valorDerecho.Value);

                case "<":
                    return (valorIzquierdo.Value) < (valorDerecho.Value);

                case "<=":
                    return (valorIzquierdo.Value) <= (valorDerecho.Value);

                default:
                    throw new Exception($"Error: Unknown operator '{Operador}'.");
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


            if (valor is Number number)
            {

                return new Number (-(int)(number.Value));
            }

            else
            {

                throw new Exception("Error: Number expected.");
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
            return this;
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



        Object Value = this.Value.Evaluate(entorno);







        if (Value is Isecuenciable secuencia)
        {





            for (int i = 0; i < Nombre.Count; i++)
            {
                // Si se usa "_" como uno de los nombres de la variable, se ignora la asignación correspondiente
                if (Nombre[i] == "_")
                    continue;


                else if (i == Nombre.Count - 1)
                {

                    entorno.DefinirVariable(new Variable(Nombre[i], secuencia.SubSecuence(i)));



                    break;








                }

                // Si se piden valores inexistentes a una secuencia, se guarda en la variable un tipo undefined
                else if (i >= secuencia.SecuenceCount)
                {
                    entorno.DefinirVariable(new Variable(Nombre[i], new Undefined()));
                    continue;
                }

                // Si el elemento de Nombres es el último de la lista, se le asigna el resto de la secuencia


                // Se asigna uno con uno cada variable a cada valor de la secuencia

                entorno.DefinirVariable(new Variable(Nombre[i], secuencia.GetElement(i, entorno)));

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

public sealed class Draw : Instruccion

{

    public string Etiqueta { get; }
    public Instruccion ToDraw { get; }

    public Type DrawType { get; private set; }

    public Draw(Instruccion ToDraw, string etiqueta)
    {
        this.ToDraw = ToDraw;
        Etiqueta = etiqueta;
        DrawType = ToDraw.GetType();
    }
    public override object Evaluate(Entorno entorno)
    {

        if (!(ToDraw.Evaluate(entorno) is IDrawable))
        {
            throw new CustomException($"Can 't draw {ToDraw.Evaluate(entorno)} type", CustomExceptionType.InvalidOperation);
        }
        else
        {

            ((IDrawable)ToDraw).GetDrawable(entorno, Etiqueta);
            return ToDraw.Evaluate(entorno);

        }





    }
}








public sealed class Identifier : Instruccion, IDrawable
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



    public void GetDrawable(Entorno entorno, string label)
    {
        IDrawable evaluated = (IDrawable)Evaluate(entorno);
        evaluated.GetDrawable(entorno, label);
    }
}




public class Function : Instruccion, IDrawable
{
    public override object Evaluate(Entorno entorno)
    {
        return null;
    }

    public void GetDrawable(Entorno entorno, string label)
    {
        IDrawable evaluated = (IDrawable)Evaluate(entorno);
        evaluated.GetDrawable(entorno, label);
    }
}






public sealed class FunctionLine : Function
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
    public Point Centro { get; }
    public Point Punto2 { get; }
    public Point Punto3 { get; }

    public int Measure { get; }

    public override string nombre { get; set; }
    public Arc(Point punto1, Point punto2, Point punto3, int measure)
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
    public override string nombre { get; set; }

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


public sealed class MeasureFunction : Instruccion
{

    public Identifier P1 { get; }
    public Identifier P2 { get; }

    public int Value { get; set; }

    public MeasureFunction(Identifier P1, Identifier P2)
    {

        this.P1 = P1;
        this.P2 = P2;

    }

    public override object Evaluate(Entorno entorno)
    {
        Point punto1 = (Point)entorno.BuscarVariable(P1.name).Value;
        Point punto2 = (Point)entorno.BuscarVariable(P2.name).Value;

        int distance = (int)Math.Sqrt(Math.Pow((punto1.Coord.X - punto2.Coord.X), 2) + Math.Pow((punto2.Coord.Y - punto1.Coord.Y), 2));
        Value = distance;
        return new Number (distance);
    }

}

public sealed class ArcFunction : Function
{

    

    public Identifier Centro { get; }
    public Identifier Punto2 { get; }
    public Identifier Punto3 { get; }
    public Identifier Measure { get; }

    public ArcFunction(Identifier P1, Identifier P2, Identifier P3, Identifier Measure)
    {

        Centro = P1;
        this.Punto2 = P2;
        this.Punto3 = P3;
        this.Measure = Measure;

    }

    public override object Evaluate(Entorno entorno)
    {
        return new Arc((Point)entorno.BuscarVariable(Centro.name).Value, (Point)entorno.BuscarVariable(Punto2.name).Value, (Point)entorno.BuscarVariable(Punto3.name).Value, (int)entorno.BuscarVariable(Measure.name).Value);
    }


}





public abstract class Figura : Instruccion, IDrawable
{


    public abstract string nombre { get; set; }

    public void GetDrawable(Entorno entorno, string label)
    {
        Entorno.figuras.Add((this, entorno.ActualColor.Peek()));
        Entorno.Label.Add(label);
    }
}







public sealed class CircleFunction : Function
{

    public Identifier P1 { get; }

    public Instruccion Measure { get; }

    

    public CircleFunction(Identifier P1, Instruccion Measure)
    {

        this.P1 = P1;
        this.Measure = Measure;

    }

    public override object Evaluate(Entorno entorno)
    {
        return new Circle((Point)entorno.BuscarVariable(P1.name).Value,(int)Measure.Evaluate(entorno) );
    }
}




public sealed class Samples : Instruccion
{
    public override object Evaluate(Entorno entorno)
    {
        throw new NotImplementedException();
    }
}



public sealed class Coord
{
    public double X { get; set; }
    public double Y { get; set; }

    public Coord(double x, double y)
    {

        X = x;
        Y = y;
    }
}



public sealed class Point : Figura, IDrawable
{
    public Coord Coord { get; set; }

    public override string nombre { get; set; } = "Default_point";

    private static Random r = new Random();

    public Point(string nombre)
    {
        this.nombre = nombre;

        int x = r.Next(1, 300);
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
        var point = new Variable(nombre, this);
        entorno.DefinirVariable(point);
        return this;
    }

    
}



public sealed class Circle : Figura
{
    public Point Centro { get; }
    public int Radio { get; }

    public double Measure { get; }
    public Circle(string nombre)
    {
        Point p1 = new Point("p1");
        Centro = p1;
        Random r = new Random();
        Measure = r.Next(1, 500);
        
        this.nombre = nombre;

    }

    public Circle(Point centro, double measure)
    {
        Centro = centro;
        Measure = measure;
        this.nombre = "Circulo sin nombre";
    }


    public override string nombre { get; set; }

    public override object Evaluate(Entorno entorno)
    {
        var cirlce = new Variable(nombre, this);
        entorno.DefinirVariable(cirlce);
        return null;
    }
}

public sealed class Line : Figura
{
    public Point Punto1 { get; }
    public Point Punto2 { get; }
    public override string nombre { get; set; } = "Default_line";

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

    }

    public override object Evaluate(Entorno entorno)
    {
        var line = new Variable(nombre, this);
        entorno.DefinirVariable(line);
        return this;
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
        var ray = new Variable(nombre, this);
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
        var segment = new Variable(nombre, this);
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

public class RandomSequence : Instruccion, Isecuenciable, IEnumerator<double>
{
    private Random random;
    private static int seed = Environment.TickCount;

    public int? SecuenceCount => null;

    public double Current => random.NextDouble();

    object IEnumerator.Current => Current;

    public RandomSequence()
    {
        this.random = new Random(seed);
    }

    public override object Evaluate(Entorno entorno)
    {
        return this;
    }

    public object GetElement(int i, Entorno entorno)
    {
        return Current;
    }

    public object SubSecuence(int i)
    {
        return this;
    }



    public bool MoveNext()
    {
        return true;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public class InfiniteSequence : Instruccion, IEnumerator<Number>, Isecuenciable
{
    private int start;
    private int? end;
    private int current;

    public InfiniteSequence(int start, int? end = null)
    {
        this.start = start;
        this.end = end;
        current = start;
    }

    public Number Current
    {
        get
        {
            return new Number(current);
        }
    }

    public int? SecuenceCount
    {
        get
        {
            if (end is null)
                return null;
            else
            {
                return end - start + 1;
            }
        }
    }

    object IEnumerator.Current => Current;



    public void Dispose()
    {
        // No hay recursos para liberar
    }

    public override object Evaluate(Entorno entorno)
    {
        return this;
    }

    public bool MoveNext()
    {

        return end == null || current <= end;
    }

    public void Reset()
    {
        current = start;
    }

    public object SubSecuence(int i)
    {

        return new InfiniteSequence(current, end);
    }

    public object GetElement(int i, Entorno entorno)
    {
        if (MoveNext())
        {
            var current_int = Current;
            current++;
            return current_int;

        }
        else
        {
            throw new CustomException("Index out of the range of the secuence", CustomExceptionType.InvalidOperation);
        }

    }
}

public interface Isecuenciable
{
    int? SecuenceCount { get; }
    object GetElement(int i, Entorno entorno);
    object SubSecuence(int i);
}

public class Cadena : Instruccion
{

    public string cadena { get; }

    public Cadena(string expression)
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





        if (auxCond is Isecuenciable SecCondition && SecCondition.SecuenceCount == 0 ||
            auxCond is int number && number.Equals(0) ||
           auxCond is Undefined || auxCond is false
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


public class SecuenceFigure<T> : Instruccion, IDrawable, Isecuenciable where T : Instruccion
{
    public List<T> elements { get; }

    public int? SecuenceCount => elements.Count();

    public SecuenceFigure(List<T> elements)
    {
        this.elements = elements;
    }








    public override object Evaluate(Entorno entorno)
    {
        Type firstElementType = null;

        foreach (Instruccion element in elements)
        {
            // Si es el primer elemento, guarda su tipo
            if (firstElementType == null)
            {
                firstElementType = element.Evaluate(entorno).GetType();
            }
            // Si no es el primer elemento, comprueba que sea del mismo tipo
            else if (element.Evaluate(entorno).GetType() != firstElementType)
            {
                throw new CustomException("All elements should be of the same type.", CustomExceptionType.InvalidOperation);


            }

        }
        return this;

    }

    public void GetDrawable(Entorno entorno, string label)
    {
        foreach (var element in elements)
        {
            if (element is IDrawable drawable)
            {
                drawable.GetDrawable(entorno, label);

            }
        }


    }

    public object GetElement(int i, Entorno entorno)
    {
        return elements[i].Evaluate(entorno);
    }

    public object SubSecuence(int i)
    {
        var resto = elements.Skip(i).ToList();


        return new SecuenceFigure<T>(resto.ToList());



    }

    public bool IsFinte()
    {
        return true;
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

public class FunctionCall : Function
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

        }



        return funcion.Cuerpo.Evaluate(entornoFuncion);
    }
}



public class IntersectExpression : Instruccion
{

    public Instruccion Figure1 { get; }
    public Instruccion Figure2 { get; }
    public IntersectExpression(Instruccion f1, Instruccion f2)
    {
        Figure1 = f1;
        Figure2 = f2;
    }

    public override object Evaluate(Entorno entorno)
    {
        Instruccion figure1 = (Instruccion)Figure1.Evaluate(entorno);
        Instruccion figure2 = (Instruccion)Figure2.Evaluate(entorno); ;


        List<Figura> points = new List<Figura>();

        //Punto, circulo
        if (figure1.GetType() == typeof(Point) && figure2.GetType() == typeof(Circle) || figure1.GetType() == typeof(Circle) && figure2.GetType() == typeof(Point))
        {
            Point p;
            Circle c;
            if (figure1 is Point) { p = (Point)figure1; c = (Circle)figure2; }
            else { c = (Circle)figure1; p = (Point)figure2; }
            // Calcula la distancia entre el centro de la circunferencia y el punto usando la ecuación de la circunferencia
            double distance = Math.Abs(Math.Sqrt(Math.Pow(p.Coord.X - c.Centro.Coord.X, 2) + Math.Pow(p.Coord.Y - c.Centro.Coord.Y, 2)));
            // Si esta es igual al radio de la circunferencia entonces el punto esta contenido 
            if (distance == Math.Abs(c.Measure))
            {
                points.Add(p);
            }
            return new SecuenceFigure<Figura>(points);
        }
        //Linea, circulo
        else if (figure1.GetType() == typeof(Line) && figure2.GetType() == typeof(Circle) || figure1.GetType() == typeof(Circle) && figure2.GetType() == typeof(Line))
        {
            Line l;
            Circle c;
            if (figure1 is Line) { l = (Line)figure1; c = (Circle)figure2; }
            else { c = (Circle)figure1; l = (Line)figure2; }
            points = IntersectCircleLine(c.Centro, c.Measure, l.Punto1, l.Punto2);
            return new SecuenceFigure<Figura>(points);
        }
        //Ray, circulo
        else if (figure1.GetType() == typeof(Ray) && figure2.GetType() == typeof(Circle) || figure1.GetType() == typeof(Circle) && figure2.GetType() == typeof(Ray))
        {
            Ray r;
            Circle c;
            if (figure1 is Ray) { r = (Ray)figure1; c = (Circle)figure2; }
            else { c = (Circle)figure1; r = (Ray)figure2; }
            points = IntersectCircleLine(c.Centro, c.Radio, r.Punto1, r.Punto2);
            double dx = r.Punto2.Coord.X - r.Punto1.Coord.X;
            double dy = r.Punto2.Coord.Y - r.Punto1.Coord.Y;

            if (points.Count != 0)
            {
                Point p = (Point)points[0];
                if (!IntersectPointRay(p, r))
                {
                    points.Remove(p);
                }
            }
            return new SecuenceFigure<Figura>(points);
        }
        //Segmento, circulo
        else if (figure1.GetType() == typeof(Segment) && figure2.GetType() == typeof(Circle) || figure1.GetType() == typeof(Circle) && figure2.GetType() == typeof(Segment))
        {
            Segment s;
            Circle c;
            if (figure1 is Line) { s = (Segment)figure1; c = (Circle)figure2; }
            else { c = (Circle)figure1; s = (Segment)figure2; }
            points = IntersectCircleLine(c.Centro, c.Measure, s.Punto1, s.Punto2);
            double dx = s.Punto2.Coord.X - s.Punto1.Coord.X;
            double dy = s.Punto2.Coord.Y - s.Punto1.Coord.Y;
            // Verifica que el punto pertenezca a la segmento
            if (points.Count != 0)
            {
                Point p = (Point)points[0];
                if (!IntersectPointSegment(p, s))
                {
                    points.Remove(p);
                }
            }
            return new SecuenceFigure<Figura>(points);
        }
        //Arco, circulo
        else if (figure1.GetType() == typeof(Arc) && figure2.GetType() == typeof(Circle) || figure1.GetType() == typeof(Circle) && figure2.GetType() == typeof(Arc))
        {
            Arc a1;
            Circle c;
            if (figure1 is Arc) { a1 = (Arc)figure1; c = (Circle)figure2; }
            else { c = (Circle)figure1; a1 = (Arc)figure2; }

            double inicialAng1 = Math.Atan2(a1.Punto2.Coord.Y - a1.Centro.Coord.Y, a1.Punto2.Coord.X - a1.Centro.Coord.X);
            double endAng1 = Math.Atan2(a1.Punto3.Coord.Y - a1.Centro.Coord.Y, a1.Punto3.Coord.X - a1.Centro.Coord.X);
            points = IntersectCircles(a1.Centro, a1.Measure, c.Centro, c.Measure);
            if (points.Count == 1)
            {
                Point p = (Point)points[0];

                if (!IntersectPointArc(p, a1))
                {
                    points.Remove(p);
                }
            }

            if (points.Count == 2)
            {
                Point p1 = (Point)points[0];
                if (!IntersectPointArc(p1, a1))
                {
                    points.Remove(p1);
                }

                Point p2 = (Point)points[1];
                if (!IntersectPointArc(p2, a1))
                {
                    points.Remove(p2);
                }
            }
            return new SecuenceFigure<Figura>(points);
        }

        //Circulo, circulo
        else if (figure1.GetType() == typeof(Circle) && figure2.GetType() == typeof(Circle))
        {
            Circle c1 = (Circle)figure1;
            Circle c2 = (Circle)figure2;
            points = IntersectCircles(c1.Centro, c1.Measure, c2.Centro, c2.Measure);
            if (points.Count != 0)
                return new SecuenceFigure<Figura>(points);

        }
        //Punto, punto
        else if (figure1.GetType() == typeof(Point) && figure2.GetType() == typeof(Point))
        {
            Point p = (Point)figure1;
            Point s = (Point)figure2;
            if (p.Coord.X == s.Coord.X && p.Coord.Y == s.Coord.Y)
            {
                points.Add(p);
            }
            return new SecuenceFigure<Figura>(points);
        }
        //Punto, linea
        else if (figure1.GetType() == typeof(Point) && figure2.GetType() == typeof(Line) || figure1.GetType() == typeof(Line) && figure2.GetType() == typeof(Point))
        {
            Point p;
            Line l;
            if (figure1 is Point) { p = (Point)figure1; l = (Line)figure2; }
            else { l = (Line)figure1; p = (Point)figure2; }
            if (p.Coord.Y - l.Punto1.Coord.Y == (l.Punto2.Coord.Y - l.Punto1.Coord.Y) / (l.Punto2.Coord.X - l.Punto1.Coord.X) * (p.Coord.X - l.Punto1.Coord.X))
            {
                points.Add(p);
            }
            return new SecuenceFigure<Figura>(points);
        }
        //Punto, ray
        else if (figure1.GetType() == typeof(Point) && figure2.GetType() == typeof(Ray) || figure1.GetType() == typeof(Ray) && figure2.GetType() == typeof(Point))
        {
            Point p;
            Ray r;
            if (figure1 is Point) { p = (Point)figure1; r = (Ray)figure2; }
            else { r = (Ray)figure1; p = (Point)figure2; }
            double dx = r.Punto2.Coord.X - r.Punto1.Coord.X;
            double dy = r.Punto2.Coord.Y - r.Punto1.Coord.Y;
            double intercept = ((p.Coord.X - r.Punto1.Coord.X) * dx + (p.Coord.Y - r.Punto1.Coord.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (intercept >= 0)
            {
                points.Add(p);
            }
            return new SecuenceFigure<Figura>(points);
        }

        //Punto, segmento
        else if (figure1.GetType() == typeof(Point) && figure2.GetType() == typeof(Segment) || figure1.GetType() == typeof(Segment) && figure2.GetType() == typeof(Point))
        {
            Point p;
            Segment s;
            if (figure1 is Segment) { s = (Segment)figure1; p = (Point)figure2; }
            else { p = (Point)figure1; s = (Segment)figure2; }
            double dx = s.Punto2.Coord.X - s.Punto1.Coord.X;
            double dy = s.Punto2.Coord.Y - s.Punto1.Coord.Y;
            // Verifica que el punto pertenezca a la segmento

            double intercept = ((p.Coord.X - s.Punto1.Coord.X) * dx + (p.Coord.Y - s.Punto1.Coord.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
            if (intercept >= 0 && intercept <= 1)
            {
                points.Add(p);
            }
            return new SecuenceFigure<Figura>(points);
        }

        //Punto, arco
        else if (figure1.GetType() == typeof(Point) && figure2.GetType() == typeof(Arc) || figure1.GetType() == typeof(Arc) && figure2.GetType() == typeof(Point))
        {
            Point p;
            Arc c;
            if (figure1 is Point) { p = (Point)figure1; c = (Arc)figure2; }
            else { c = (Arc)figure1; p = (Point)figure2; }
            // Calcula la distancia entre el centro del arco y el punto usando la ecuación de la circunferencia
            double distance = Math.Abs(Math.Sqrt(Math.Pow(p.Coord.X - c.Centro.Coord.X, 2) + Math.Pow(p.Coord.Y - c.Centro.Coord.Y, 2)));
            // Si esta es igual al radio de la circunferencia entonces el punto esta contenido 
            if (distance == Math.Abs(c.Measure))
            {
                if (points.Count == 1)
                {
                    Point p_ = (Point)points[0];

                    if (!IntersectPointArc(p, c))
                    {
                        points.Remove(p);
                    }
                }
                return new SecuenceFigure<Figura>(points);
            }
        }
        //Arco, arco
        else if (figure1.GetType() == typeof(Arc) && figure2.GetType() == typeof(Arc))
        {
            Arc a1 = (Arc)figure1;
            Arc a2 = (Arc)figure2;

            double inicialAng1 = Math.Atan2(a1.Punto2.Coord.Y - a1.Centro.Coord.Y, a1.Punto2.Coord.X - a1.Centro.Coord.X);
            double endAng1 = Math.Atan2(a1.Punto3.Coord.Y - a1.Centro.Coord.Y, a1.Punto3.Coord.X - a1.Centro.Coord.X);

            double inicialAng2 = Math.Atan2(a2.Punto2.Coord.Y - a2.Centro.Coord.Y, a2.Punto2.Coord.X - a2.Centro.Coord.X);
            double endAng2 = Math.Atan2(a2.Punto3.Coord.Y - a2.Centro.Coord.Y, a2.Punto3.Coord.X - a2.Centro.Coord.X);

            points = IntersectCircles(a1.Centro, a1.Measure, a2.Centro, a2.Measure);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                //Angulo del punto de intersección respecto al primer arco
                double angulo = Math.Atan2(p_.Coord.Y - a1.Centro.Coord.Y, p_.Coord.X - a1.Centro.Coord.X);
                if (angulo >= inicialAng1 && angulo <= endAng1)
                {
                    //Angulo del punto de intersección respecto al segundo arco
                    angulo = Math.Atan2(p_.Coord.Y - a2.Centro.Coord.Y, p_.Coord.X - a2.Centro.Coord.X);
                    if (angulo >= inicialAng2 && angulo <= endAng2)
                    {
                        return new SecuenceFigure<Figura>(points);
                    }
                    else
                    {
                        points.Remove(p_);
                    }
                }
                else
                {
                    points.Remove(p_);
                }
            }
            if (points.Count == 2)
            {
                Point p1 = (Point)points[1];
                //Angulo del punto de intersección respecto al primer arco
                double angulo = Math.Atan2(p1.Coord.Y - a1.Centro.Coord.Y, p1.Coord.X - a1.Centro.Coord.X);
                if (angulo >= inicialAng1 && angulo <= endAng1)
                {
                    //Angulo del punto de intersección respecto al segundo arco
                    angulo = Math.Atan2(p1.Coord.Y - a2.Centro.Coord.Y, p1.Coord.X - a2.Centro.Coord.X);
                    if (!(angulo >= inicialAng2) && !(angulo <= endAng2))
                    {
                        points.Remove(p1);
                    }
                }
                else
                {
                    points.Remove(p1);
                }

                Point p2 = (Point)points[0];
                //Angulo del punto de intersección respecto al primer arco
                angulo = Math.Atan2(p2.Coord.Y - a1.Centro.Coord.Y, p2.Coord.X - a1.Centro.Coord.X);
                if (angulo >= inicialAng1 && angulo <= endAng1)
                {
                    //Angulo del punto de intersección respecto al segundo arco
                    angulo = Math.Atan2(p2.Coord.Y - a2.Centro.Coord.Y, p2.Coord.X - a2.Centro.Coord.X);
                    if (angulo >= inicialAng2 && angulo <= endAng2)
                    {
                        return new SecuenceFigure<Figura>(points);
                    }
                    else
                    {
                        points.Remove(p2);
                    }
                }
                else
                {
                    points.Remove(p2);
                }
                if (points.Count == 1)
                {
                    return new SecuenceFigure<Figura>(points);
                }

            }

        }
        //Linea, linea 
        else if (figure1.GetType() == typeof(Line) && figure2.GetType() == typeof(Line))
        {
            Line l1 = (Line)figure1;
            Line l2 = (Line)figure2;
            points = IntersectLines(l1.Punto1, l1.Punto2, l2.Punto1, l2.Punto2);
            if (points.Count == 1)
            {
                return new SecuenceFigure<Figura>(points);
            }

            else
            {
            }
        }
        //Linea, segmento
        else if (figure1.GetType() == typeof(Segment) && figure2.GetType() == typeof(Line) || figure1.GetType() == typeof(Line) && figure2.GetType() == typeof(Segment))
        {
            Segment s;
            Line l;
            if (figure1 is Segment) { s = (Segment)figure1; l = (Line)figure2; }
            else { l = (Line)figure1; s = (Segment)figure2; }
            points = IntersectLines(l.Punto1, l.Punto2, s.Punto1, s.Punto2);
            if (points.Count == 1)
            {
                double dx = s.Punto2.Coord.X - s.Punto1.Coord.X;
                double dy = s.Punto2.Coord.Y - s.Punto1.Coord.Y;
                Point p_ = (Point)points[0];
                // Verifica que el punto pertenezca a la segmento
                double intercept = ((p_.Coord.X - s.Punto1.Coord.X) * dx + (p_.Coord.Y - s.Punto1.Coord.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (!(intercept < 0) && !(intercept > 1))
                {
                    points.Remove(p_);
                }
                else return new SecuenceFigure<Figura>(points);
            }
        }
        //Ray, lines
        else if (figure1.GetType() == typeof(Ray) && figure2.GetType() == typeof(Line) || figure1.GetType() == typeof(Line) && figure2.GetType() == typeof(Ray))
        {
            Ray r;
            Line l;
            if (figure1 is Segment) { r = (Ray)figure1; l = (Line)figure2; }
            else { l = (Line)figure1; r = (Ray)figure2; }
            points = IntersectLines(l.Punto1, l.Punto2, r.Punto1, r.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                double dx = r.Punto2.Coord.X - r.Punto1.Coord.X;
                double dy = r.Punto2.Coord.Y - r.Punto1.Coord.Y;
                double intercept = ((p_.Coord.X - r.Punto1.Coord.X) * dx + (p_.Coord.Y - r.Punto1.Coord.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (intercept >= 0)
                {
                    return new SecuenceFigure<Figura>(points);
                }
                else points.Remove(p_);
            }
        }
        //Linea, arco
        else if (figure1.GetType() == typeof(Arc) && figure2.GetType() == typeof(Line) || figure1.GetType() == typeof(Line) && figure2.GetType() == typeof(Arc))
        {
            Arc a;
            Line l;
            if (figure1 is Segment) { a = (Arc)figure1; l = (Line)figure2; }
            else { l = (Line)figure1; a = (Arc)figure2; }
            points = IntersectCircleLine(a.Centro, a.Measure, l.Punto1, l.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                if (IntersectPointArc(p_, a))
                    return new SecuenceFigure<Figura>(points);
                points.Remove(p_);
            }
            if (points.Count == 2)
            {
                Point p1 = (Point)points[0];
                if (!IntersectPointArc(p1, a))
                {
                    points.Remove(p1);
                }
                Point p2 = (Point)points[1];
                if (IntersectPointArc(p2, a))
                    return new SecuenceFigure<Figura>(points);
                else points.Remove(p2);
                if (points.Count == 1)
                    return new SecuenceFigure<Figura>(points);
            }
        }
        //segmento, segmento
        else if (figure1.GetType() == typeof(Segment) && figure2.GetType() == typeof(Segment))
        {
            Segment s1 = (Segment)figure1;
            Segment s2 = (Segment)figure2;
            points = IntersectLines(s1.Punto1, s1.Punto2, s2.Punto1, s2.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                if (IntersectPointSegment(p_, s1) && IntersectPointSegment(p_, s2))
                    return new SecuenceFigure<Figura>(points);
                else points.Remove(p_);
            }
        }
        //Segmento, ray
        else if (figure1.GetType() == typeof(Ray) && figure2.GetType() == typeof(Segment) || figure1.GetType() == typeof(Segment) && figure2.GetType() == typeof(Ray))
        {
            Segment s;
            Ray r;
            if (figure1 is Segment) { s = (Segment)figure1; r = (Ray)figure2; }
            else { r = (Ray)figure1; s = (Segment)figure2; }
            points = IntersectLines(r.Punto1, r.Punto2, s.Punto1, s.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                if (!IntersectPointRay(p_, r) || !IntersectPointSegment(p_, s))
                {
                    points.Remove(p_);
                }

            }
            return new SecuenceFigure<Figura>(points);
        }
        //Segmento, arco
        else if (figure1.GetType() == typeof(Arc) && figure2.GetType() == typeof(Segment) || figure1.GetType() == typeof(Segment) && figure2.GetType() == typeof(Arc))
        {
            Segment s;
            Arc a;
            if (figure1 is Segment) { s = (Segment)figure1; a = (Arc)figure2; }
            else { a = (Arc)figure1; s = (Segment)figure2; }
            points = IntersectCircleLine(a.Centro, a.Measure, s.Punto1, s.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                if (!IntersectPointArc(p_, a) || !IntersectPointSegment(p_, s))
                {
                    points.Remove(p_);
                }
            }
            return new SecuenceFigure<Figura>(points);
        }

        //Ray, ray
        else if (figure1.GetType() == typeof(Ray) && figure2.GetType() == typeof(Ray))
        {
            Ray r1 = (Ray)figure1;
            Ray r2 = (Ray)figure2;
            points = IntersectLines(r1.Punto1, r1.Punto2, r2.Punto1, r2.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                if (!IntersectPointRay(p_, r1) || !IntersectPointRay(p_, r2))
                {
                    points.Remove(p_);
                }
            }
            return new SecuenceFigure<Figura>(points);
        }
        //Ray,Arc
        else if (figure1.GetType() == typeof(Arc) && figure2.GetType() == typeof(Ray) || figure1.GetType() == typeof(Ray) && figure2.GetType() == typeof(Arc))
        {
            Ray r;
            Arc a;
            if (figure1 is Ray) { r = (Ray)figure1; a = (Arc)figure2; }
            else { a = (Arc)figure1; r = (Ray)figure2; }
            points = IntersectCircleLine(a.Centro, a.Measure, r.Punto1, r.Punto2);
            if (points.Count == 1)
            {
                Point p_ = (Point)points[0];
                if (!IntersectPointArc(p_, a) || !IntersectPointRay(p_, r))
                {
                    points.Remove(p_);
                }
            }
            return new SecuenceFigure<Figura>(points);
        }






        throw new Exception($"No es posible calcular la intersección entre " + figure1 + " y " + figure2);

    }
    bool IntersectPointSegment(Point p, Segment s)
    {
        double dx = s.Punto2.Coord.X - s.Punto1.Coord.X;
        double dy = s.Punto2.Coord.Y - s.Punto1.Coord.Y;
        // Verifica que el punto pertenezca a la segmento
        double intercept = ((p.Coord.X - s.Punto1.Coord.X) * dx + (p.Coord.Y - s.Punto1.Coord.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
        if (intercept < 0 || intercept > 1)
        {
            return false;
        }
        else return true;
    }

    bool IntersectPointRay(Point p, Ray r)
    {
        double dx = r.Punto2.Coord.X - r.Punto1.Coord.X;
        double dy = r.Punto2.Coord.Y - r.Punto1.Coord.Y;
        double intercept = ((p.Coord.X - r.Punto1.Coord.X) * dx + (p.Coord.Y - r.Punto1.Coord.Y) * dy) / (Math.Pow(dx, 2) + Math.Pow(dy, 2));
        if (intercept >= 0)
        {
            return true;
        }
        else return false;
    }

    bool IntersectPointArc(Point p, Arc a1)
    {
        double inicialAng1 = Math.Atan2(a1.Punto2.Coord.Y - a1.Centro.Coord.Y, a1.Punto2.Coord.X - a1.Centro.Coord.X);
        double endAng1 = Math.Atan2(a1.Punto2.Coord.Y - a1.Centro.Coord.Y, a1.Punto2.Coord.X - a1.Centro.Coord.X);
        double angulo = Math.Atan2(p.Coord.Y - a1.Centro.Coord.Y, p.Coord.X - a1.Centro.Coord.X);
        if (angulo >= inicialAng1 && angulo <= endAng1)
        {
            return true;
        }
        else return false;
    }

    List<Figura> IntersectCircleLine(Point Center, double Radius, Point P1, Point P2)
    {
        List<Figura> points = new List<Figura>();
        // m es la pendiente de la recta 
        double m = (P2.Coord.Y - P1.Coord.Y) / (P2.Coord.X - P1.Coord.X);
        // ecuación de la recta 
        double b = P1.Coord.Y - m * P1.Coord.X;
        // coordenada x del centro de la circunferencia
        double h = Center.Coord.X;
        // coordenada y del centro de la circunferencia
        double k = Center.Coord.Y;
        // radio de la circunferencia
        double r = Radius;
        // A, B y C son coeficientes de la ecuación cuadrática que se obtiene al sustituir la ecuación de la recta en la ecuación de la circunferencia para encontrar las coordenadas de los puntos de intersección 
        double A = 1 + Math.Pow(m, 2);
        double B = -2 * h + 2 * m * b - 2 * k * m;
        double C = Math.Pow(h, 2) + Math.Pow(b, 2) + Math.Pow(k, 2) - Math.Pow(r, 2) - 2 * b * k;
        double discriminant = Math.Pow(B, 2) - 4.0 * A * C;

        // Si el discriminante es negativo no existe intersección
        // Si el discriminante es cero existe un solo punto de intersección
        if (discriminant == 0)
        {
            double x = -B / (2 * A);
            double y = m * x + b;
            Point intersect = new Point("")
            {
                Coord = new Coord(x, y)

            };
            points.Add(intersect);
        }
        // Si el discriminante es positivo existen dos puntos de intersección
        else
        {
            double x1 = (-B + Math.Sqrt(discriminant)) / (2.0 * A);
            double y1 = m * x1 + b;
            double x2 = (-B - Math.Sqrt(discriminant)) / (2.0 * A);
            double y2 = m * x2 + b;
            Point intersect1 = new Point("")
            {
                Coord = new Coord(x1, y1)
            };
            Point intersect2 = new Point("")
            {
                Coord = new Coord(x2, y2)
            };
            points.Add(intersect1);
            points.Add(intersect2);
        }
        return points;
    }

    List<Figura> IntersectCircles(Point Center1, double Radius1, Point Center2, double Radius2)
    {
        List<Figura> points = new List<Figura>();
        //distancia entre los centros de las circunferencias
        double d = Math.Sqrt(Math.Pow(Center2.Coord.X - Center1.Coord.X, 2) + Math.Pow(Center2.Coord.Y - Center1.Coord.Y, 2));
        //si la distancia es igual a la suma de los radios entonces son tangentes
        if (d == Radius1 + Radius2)
        {
            double x = (Radius1 * Center2.Coord.X - Radius2 * Center1.Coord.X) / (Radius1 - Radius2);
            double y = (Radius1 * Center2.Coord.Y - Radius2 * Center1.Coord.Y) / (Radius1 - Radius2);
            Point p = new Point("")
            {
                Coord = new Coord(x, y)
            };
            points.Add(p);
        }
        //en el primer caso uno de los círculos estaría contenido dentro del otro y en el otro no se intersectan
        else if (d > Math.Abs(Radius1 - Radius2) && d < Radius1 + Radius2)
        {
            double a = (Math.Pow(Radius1, 2) - Math.Pow(Radius2, 2) + Math.Pow(d, 2)) / (2 * d);
            double h = Math.Sqrt(Math.Pow(Radius1, 2) - Math.Pow(a, 2));

            double x2 = Center1.Coord.X + a * (Center2.Coord.X - Center1.Coord.X) / d;
            double y2 = Center1.Coord.Y + a * (Center2.Coord.Y - Center1.Coord.Y) / d;

            Point p1 = new Point("")
            {
                Coord = new Coord(x2 + h * (Center2.Coord.Y - Center1.Coord.Y) / d,
                y2 - h * (Center2.Coord.X - Center1.Coord.X) / d)

            };
            Point p2 = new Point("")
            {
                Coord = new Coord(x2 - h * (Center2.Coord.Y - Center1.Coord.Y) / d,
                y2 + h * (Center2.Coord.X - Center1.Coord.X) / d)


            };
            points.Add(p1);
            points.Add(p2);
        }
        return points;
    }

    List<Figura> IntersectLines(Point firstlinep1, Point firstlinep2, Point secondtlinep1, Point secondtlinep2)
    {
        List<Figura> points = new List<Figura>();
        //Hallar pendiente de la primera linea 
        double m1 = (firstlinep1.Coord.Y - firstlinep2.Coord.Y) / (firstlinep1.Coord.X - firstlinep2.Coord.X);
        //Hallar punto medio de la primera linea 
        //Para coordenadas X 
        double xMiddle1 = (firstlinep1.Coord.X + firstlinep2.Coord.X) / 2;
        //Para coordenadas Y 
        double yMiddle1 = (firstlinep1.Coord.Y + firstlinep2.Coord.Y) / 2;

        //Hallar pendiente de la segunda linea 
        double m2 = (secondtlinep1.Coord.Y - secondtlinep2.Coord.Y) / (secondtlinep1.Coord.X - secondtlinep2.Coord.X);
        //Hallar punto medio de la primera linea 
        //Para coordenadas X 
        double xMiddle2 = (secondtlinep1.Coord.X + secondtlinep2.Coord.X) / 2;
        //Para coordenadas Y 
        double yMiddle2 = (secondtlinep1.Coord.Y + secondtlinep2.Coord.Y) / 2;
        if (m1 != m2)
        {
            // Coordenada x del punto de intersección 
            double x = (yMiddle2 - yMiddle1 + m1 * xMiddle1 - m2 * xMiddle2) / (m1 - m2);
            // Coordenada y del punto de intersección 
            double y = yMiddle1 + m1 * (x - xMiddle1);
            Point intersect = new Point("")
            {
                Coord = new Coord(x, y)
            };
            points.Add(intersect);
        }
        return points;
    }
}



















public class Entorno


{


    public Stack<string> ActualColor = new Stack<string>();
    public Stack<string> TrashColor = new Stack<string>();

    public Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

    public static List<(Figura, string)> figuras = new List<(Figura, string)>();

    public static List<string> Label = new List<string>();





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
        foreach (var function in difference.funciones)
        {
            this.DefineFunction(function.Value);
        }

        // Agrega las figuras de 'difference' a 'this'

    }


    public Entorno DeepCopy()
    {

        Entorno newEntorno = new Entorno();

        // Copia cada variable de this a newEntorno
        foreach (var variable in this.variables)
        {
            newEntorno.variables.Add(variable.Key, variable.Value);
        }
        // Copia cada funcion de this a newEntorno
        foreach (var function in this.funciones)
        {
            newEntorno.funciones.Add(function.Key, function.Value);
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
        foreach (var function in this.funciones)
        {
            if (!other.funciones.ContainsKey(function.Key))
            {
                difference.DefineFunction(function.Value);
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

    public Entorno()
    {
        ActualColor = new Stack<string>();
        ActualColor.Push("black");
    }
}
