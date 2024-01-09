using System.Drawing;
using ClassLibrary1;
using System;
using System.Numerics;
using System.Windows.Forms;
using Wall_E;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.IO;
using System.Text.RegularExpressions;

namespace Wall_E
{
    

    public partial class Formulario :Form
    {
      
        private Button botonDibujar;
        private Button botonCompilar;
        private Panel PanelPro;
        private RichTextBox entradaUsuario;
        private RichTextBox errores;
        private PictureBox areaGrafica;
        private List<(Figura,string)> figuras;
        private List<string> etiquetas =Entorno.Label;
        private Panel panelUsuario;
        private Panel panelDibujar;
        private Button botonBorrar;
        private Label labelIngresarCod;
        private Label labelWally;
        private List<string> imports=new List<string>();
        Entorno entorno = new Entorno();




        public Formulario()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.botonDibujar = new System.Windows.Forms.Button();
            this.botonCompilar = new System.Windows.Forms.Button();
            this.entradaUsuario = new System.Windows.Forms.RichTextBox();
            this.errores = new System.Windows.Forms.RichTextBox();
            this.areaGrafica = new System.Windows.Forms.PictureBox();
            this.PanelPro = new System.Windows.Forms.Panel();
            this.panelUsuario = new System.Windows.Forms.Panel();
            this.panelDibujar = new System.Windows.Forms.Panel();
            this.botonBorrar = new System.Windows.Forms.Button();
            this.labelIngresarCod = new System.Windows.Forms.Label();
            this.labelWally = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.areaGrafica)).BeginInit();
            this.panelUsuario.SuspendLayout();
            this.panelDibujar.SuspendLayout();
            this.SuspendLayout();
            // 
            // botonDibujar
            // 
            this.botonDibujar.BackColor = System.Drawing.Color.LightYellow;
            this.botonDibujar.Enabled = false;
            this.botonDibujar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.botonDibujar.Font = new System.Drawing.Font("Ink Free", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonDibujar.Location = new System.Drawing.Point(402, 178);
            this.botonDibujar.Name = "botonDibujar";
            this.botonDibujar.Size = new System.Drawing.Size(96, 32);
            this.botonDibujar.TabIndex = 0;
            this.botonDibujar.Text = "Dibujar";
            this.botonDibujar.UseVisualStyleBackColor = false;
            this.botonDibujar.Click += DrawButton_Click;
            // 
            // botonCompilar
            // 
            this.botonCompilar.BackColor = System.Drawing.Color.LightYellow;
            this.botonCompilar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.botonCompilar.Font = new System.Drawing.Font("Ink Free", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonCompilar.Location = new System.Drawing.Point(402, 120);
            this.botonCompilar.Name = "botonCompilar";
            this.botonCompilar.Size = new System.Drawing.Size(96, 33);
            this.botonCompilar.TabIndex = 1;
            this.botonCompilar.Text = "Compilar";
            this.botonCompilar.UseVisualStyleBackColor = false;
            this.botonCompilar.Click += CompileButton_Click;
            // 
            // entradaUsuario
            // 
            this.entradaUsuario.BackColor = System.Drawing.Color.White;
            this.entradaUsuario.Font = new System.Drawing.Font("Arial", 14F);
            this.entradaUsuario.Location = new System.Drawing.Point(-2, -2);
            this.entradaUsuario.Name = "entradaUsuario";
            this.entradaUsuario.Size = new System.Drawing.Size(10600, 10085);
            this.entradaUsuario.TabIndex = 2;
            this.entradaUsuario.Text = "";
            this.entradaUsuario.TextChanged += new System.EventHandler(this.EntradaUsuario_TextChanged);
            // 
            // errores
            // 
            this.errores.BackColor = System.Drawing.Color.White;
            this.errores.Cursor = System.Windows.Forms.Cursors.Default;
            this.errores.Enabled = false;
            this.errores.Font = new System.Drawing.Font("Ink Free", 11.95F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errores.ForeColor = System.Drawing.SystemColors.InfoText;
            this.errores.Location = new System.Drawing.Point(25, 516);
            this.errores.Name = "errores";
            this.errores.ReadOnly = true;
            this.errores.Size = new System.Drawing.Size(500, 133);
            this.errores.TabIndex = 3;
            this.errores.Text = "Errores de Compilación";
            // 
            // areaGrafica
            // 
            this.areaGrafica.BackColor = System.Drawing.Color.White;
            this.areaGrafica.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.areaGrafica.Location = new System.Drawing.Point(0, 0);
            this.areaGrafica.Name = "areaGrafica";
            this.areaGrafica.Size = new System.Drawing.Size(9866, 10000);
            this.areaGrafica.TabIndex = 4;
            this.areaGrafica.TabStop = false;
            this.areaGrafica.Click += new System.EventHandler(this.areaGrafica_Click);
            // 
            // PanelPro
            // 
            this.PanelPro.Location = new System.Drawing.Point(0, 0);
            this.PanelPro.Name = "PanelPro";
            this.PanelPro.Size = new System.Drawing.Size(200, 100);
            this.PanelPro.TabIndex = 0;
            // 
            // panelUsuario
            // 
            this.panelUsuario.AutoScroll = true;
            this.panelUsuario.BackColor = System.Drawing.Color.White;
            this.panelUsuario.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelUsuario.Controls.Add(this.entradaUsuario);
            this.panelUsuario.Location = new System.Drawing.Point(25, 86);
            this.panelUsuario.Name = "panelUsuario";
            this.panelUsuario.Size = new System.Drawing.Size(371, 394);
            this.panelUsuario.TabIndex = 5;
            // 
            // panelDibujar
            // 
            this.panelDibujar.AutoScroll = true;
            this.panelDibujar.Controls.Add(this.areaGrafica);
            this.panelDibujar.Location = new System.Drawing.Point(566, 86);
            this.panelDibujar.Name = "panelDibujar";
            this.panelDibujar.Size = new System.Drawing.Size(780, 563);
            this.panelDibujar.TabIndex = 6;
            // 
            // botonBorrar
            // 
            this.botonBorrar.BackColor = System.Drawing.Color.LightYellow;
            this.botonBorrar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.botonBorrar.Font = new System.Drawing.Font("Ink Free", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonBorrar.Location = new System.Drawing.Point(402, 235);
            this.botonBorrar.Name = "botonBorrar";
            this.botonBorrar.Size = new System.Drawing.Size(96, 33);
            this.botonBorrar.TabIndex = 7;
            this.botonBorrar.Text = "Borrar";
            this.botonBorrar.UseVisualStyleBackColor = false;
            this.botonBorrar.Click += CleanButton_Click;
            // 
            // labelIngresarCod
            // 
            this.labelIngresarCod.AutoSize = true;
            this.labelIngresarCod.Font = new System.Drawing.Font("Ink Free", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIngresarCod.Location = new System.Drawing.Point(33, 63);
            this.labelIngresarCod.Name = "labelIngresarCod";
            this.labelIngresarCod.Size = new System.Drawing.Size(185, 20);
            this.labelIngresarCod.TabIndex = 8;
            this.labelIngresarCod.Text = "Ingresa tu código aquí";
            // 
            // labelWally
            // 
            this.labelWally.AutoSize = true;
            this.labelWally.Font = new System.Drawing.Font("Ink Free", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWally.ForeColor = System.Drawing.Color.Maroon;
            this.labelWally.Location = new System.Drawing.Point(605, 9);
            this.labelWally.Name = "labelWally";
            this.labelWally.Size = new System.Drawing.Size(143, 34);
            this.labelWally.TabIndex = 9;
            this.labelWally.Text = "Geo Wall-e";
            // 
            // Formulario
            // 
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.labelWally);
            this.Controls.Add(this.labelIngresarCod);
            this.Controls.Add(this.botonBorrar);
            this.Controls.Add(this.panelDibujar);
            this.Controls.Add(this.panelUsuario);
            this.Controls.Add(this.botonDibujar);
            this.Controls.Add(this.botonCompilar);
            this.Controls.Add(this.errores);
            this.Name = "Formulario";
            this.Text = "Mi Formulario";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Formulario_Load);
            ((System.ComponentModel.ISupportInitialize)(this.areaGrafica)).EndInit();
            this.panelUsuario.ResumeLayout(false);
            this.panelDibujar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void EntradaUsuario_TextChanged(object sender, EventArgs e)
        {
            botonDibujar.Enabled = false;
            errores.Text = "Errores de Compilación";
        }
        private void DrawButton_Click(object sender, EventArgs e)
        {

            // AST ast = sintaxis.Analizar();
            //  object resultado = ast.Evaluar(entorno);
            // Aqui el parser debe devolver una List<Figuras> que se le pasa como parametro al metodo
            // DibujarFiguras
            //DibujarFiguras();
            DibujarFiguras(figuras);
            etiquetas.Clear();

        }

        private void CompileButton_Click(object sender, EventArgs e )
        {
            //List < Figura > figuras= new List<Figura>();
            //Coord cor = new Coord(100, 100);
            try
            {
                Graphics g = this.areaGrafica.CreateGraphics();
                g.Clear(this.areaGrafica.BackColor);
                Entorno.figuras = new List<(Figura, string)>();
                //ClassLibrary1.Point punto = new ClassLibrary1.Point("p1");
                //figuras.Add(punto);
                string entrada = ObtenerValorCajaTexto();
                figuras = Compilar(entrada);
                botonDibujar.Enabled = true;
                errores.Text = "Errores de Compilación \n No hay errores de compilacion";
            }
            catch (Exception ex)
            {
                errores.Text = ex.Message;
                botonDibujar.Enabled = false;
            }
        }

        private void CleanButton_Click(object sender, EventArgs e)
        {
            entradaUsuario.Clear();
            areaGrafica.Image = null;
        }

        public string ObtenerValorCajaTexto()
        {
            // Este método retorna el valor de la caja de texto
            return entradaUsuario.Text;
        }

        public List<(Figura,string)> Compilar(string parametro)
        {
           
            AnalizadorLéxico lex = new AnalizadorLéxico(parametro);
            
            List<Token> tokens = lex.ObtenerTokens();
            //foreach (var item in tokens)
            //{
            //    MessageBox.Show($"{ item.Tipo}");
            //}
            AnalizadorSintáctico sintaxis = new AnalizadorSintáctico(tokens);
            List<Instruccion> instruccionsAux = new List<Instruccion>();
            List<Instruccion> instruccions = sintaxis.ParseInstructions(instruccionsAux);
            

            foreach (Instruccion i in instruccions)
            {
                var evaluated = i.Evaluate(entorno);
               
                if (i is Import)
                {
                    MessageBox.Show($"Holis");
                    Import copy = (Import)i;
                    if(imports.Contains(copy.Ruta))
                    {
                        throw new Exception("Ya se importo este archivo , error de circularidad");
                    }
                    imports.Add(copy.Ruta);
                    FileInfo fi = new FileInfo(copy.Ruta);
                    string contenido = File.ReadAllText(fi.FullName);

                    string[] lineas = Regex.Split(entradaUsuario.Text, "(;)");
                    //string[] lineas =entradaUsuario.Text.Split(';');

                   

                    

                    for (int j = 0; j < lineas.Length; j++)
                    {
                        if (lineas[j].Contains("import"))
                        {
                            lineas[j] = "\n" + contenido;
                            lineas[j + 1] = "";
                            //MessageBox.Show($"{lineas[j]}");
                        }
                    }

                    // Reemplaza la línea específica con el contenido del archivo

                    string resultado = "";
                    for (int j = 0; j < lineas.Length; j++)
                    {
                       
                        resultado += lineas[j];
                        
                    }
                    entradaUsuario.Text = resultado;
                   

                    if (entradaUsuario.Text.EndsWith(";;"))
                    {
                        entradaUsuario.Text = entradaUsuario.Text.Substring(0, entradaUsuario.Text.Length - 1);
                    }

                    parametro = entradaUsuario.Text;
                    MessageBox.Show($"{parametro}");

                    Entorno.figuras=new List<(Figura, string)>();
                    Compilar(parametro);
                    break;
                   // MessageBox.Show($"{entorno.figuras.Count}");
                }



               
                
             

                

                
               
                



            }


            
                if (PrintExpression.Printeables != null)
                {   
                    foreach(object printeable in PrintExpression.Printeables)
                    {
                        MessageBox.Show((string)printeable);
                    }

                    PrintExpression.Printeables.Clear();
                }
            
            return Entorno.figuras;
        }

        public  object BrushColor(string color)
        {
            if (color == "red")
            {
                return Brushes.Red;
            }
            else if (color == "blue")
            {
                return Brushes.Blue;
            }
            else if (color == "green")
            {
                return Brushes.Green;
            }
            else if (color == "yellow")
            {
                return Brushes.Yellow;
            }
            else if (color == "cyan")
            {
                return Brushes.Cyan;
            }
            else if (color == "magenta")
            {
                return Brushes.Magenta;
            }
            else if (color == "white")
            {
                return Brushes.White;
            }
            else if (color == "gray")
            {
                return Brushes.Gray;
            }
            else if (color == "black")
            {
                return Brushes.Black;
            }


            return Brushes.Black;

        }

        

        public object PenColor(string color)
        {
            if (color == "red")
            {
                return System.Drawing.Color.Red;
            }
            else if (color == "blue")
            {
                return System.Drawing.Color.Blue;
            }
            else if (color == "green")
            {
                return System.Drawing.Color.Green;
            }
            else if (color == "yellow")
            {
                return System.Drawing.Color.Yellow;
            }
            else if (color == "cyan")
            {
                return System.Drawing.Color.Cyan;
            }
            else if (color == "magenta")
            {
                return System.Drawing.Color.Magenta;
            }
            else if (color == "white")
            {
                return System.Drawing.Color.White;
            }
            else if (color == "gray")
            {
                return System.Drawing.Color.Gray;
            }
            else if (color == "black")
            {
                return System.Drawing.Color.Black;
            }


            return System.Drawing.Color.Black;

        }

        public void DibujarFiguras(List<(Figura, string)> figuras)
        {
            var g = areaGrafica.CreateGraphics();
            Pen p = new Pen(System.Drawing.Color.Black);
            int i = 0;

            //MessageBox.Show($"{entorno.figuras.Count}");
            //MessageBox.Show($"{entorno.variables["l"].Value}");

            foreach (var item in figuras)
            {
                p.Color = (System.Drawing.Color)PenColor(item.Item2);
               
                //CircleFunction c = (CircleFunction)item.Item1;
                //MeasureFunction m=(MeasureFunction)c.Measure
                MessageBox.Show($"{item.Item1.GetType().ToString()}");

                switch (item.Item1.GetType().ToString())
                {
                    case "Point":
                        
                        DibujarPunto(g, ((Point)item.Item1), item.Item2, etiquetas[i]);
                        break;
                    case "Circle":
                        DibujarCirculo(g, ((Circle)item.Item1), item.Item2, etiquetas[i]);
                        break;
                    case "Line":
                        DibujarLinea(g, p, ((Line)item.Item1), etiquetas[i]);
                        break;
                    case "Ray":
                        DibujarRayo(g, p, ((Ray)item.Item1), etiquetas[i]);
                        break;
                    case "Segment":
                        DibujarSegmento(g, p, (Segment)item.Item1, etiquetas[i]);
                        break;
                    case "Arc":
                        DibujarArco(g, p, (Arc)item.Item1, etiquetas[i]);
                        break;
                    default:
                        throw new Exception("No es posible dibujar la figura");
                }

                i++;
            }
        }


        private void CajaTexto_Enter(object sender, EventArgs e )
        {
            // Si el texto de la caja es el predeterminado, lo borra
            if (entradaUsuario.Text == "Escribe aquí tu texto")
            {
                entradaUsuario.Text = "";
            }
        }

        //private void CajaTexto_Leave(object sender, EventArgs e)
        //{
        //    valoringresado = this.entradaUsuario.Text;
        //    MessageBox.Show(valoringresado);
        //}


        public void DibujarPunto(Graphics g, Point p, string color, string etiqueta)
        {
            Brush colorDraw = (System.Drawing.Brush)BrushColor(color);
            g.FillEllipse(colorDraw, (float)p.Coord.X, (float)p.Coord.Y, 7, 7);

            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);

            // Imprime el mensaje al lado del punto usando DrawString
            g.DrawString(etiqueta, drawFont, drawBrush, (float)p.Coord.X + 7 + 10, (float)p.Coord.Y);
            //Punto hola = new Punto(300, 300);

        }

        public void DibujarCirculo(Graphics g, Circle c, string color, string etiqueta)
        {
            //Circle circle = new Circle(100, 100, 100);

            Pen mypen = new Pen((System.Drawing.Color)PenColor(color));

            // Dibuja el círculo
            float x = (float)c.Centro.Coord.X - (float)c.Measure.Value;
            float y = (float)c.Centro.Coord.Y - (float)c.Measure.Value;
            int diameter = (int)c.Measure.Value * 2;

            g.DrawEllipse(mypen, x, y, diameter, diameter);


            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);

            // Imprime el mensaje al lado del punto usando DrawString
            g.DrawString(etiqueta, drawFont, drawBrush, (float)c.Centro.Coord.X + (int)c.Measure.Value * 2, (float)c.Centro.Coord.Y);

            // Dibuja el punto medio del círculo
            //g.FillEllipse(Brushes.Black, circle.X + circle.Radius, circle.Y + circle.Radius, 5, 5);

            //// Dibuja un punto sobre el círculo
            //g.FillEllipse(Brushes.Black, circle.X + circle.Radius, circle.Y, 5, 5);


        }

        public void DibujarArco(Graphics g, Pen pen, Arc arc, string etiqueta)
        {




            double distanciaMaxima = (Math.Sqrt(Math.Pow(areaGrafica.Width - arc.Centro.Coord.X, 2) + Math.Pow(areaGrafica.Height - arc.Centro.Coord.Y, 2)) * 10000);



            //Calcula los ángulos de las semirrectas
            float angulo1 = (float)Math.Atan2(arc.Punto2.Coord.Y - arc.Centro.Coord.Y, arc.Punto2.Coord.X - arc.Centro.Coord.X);
            float angulo2 = (float)Math.Atan2(arc.Punto3.Coord.Y - arc.Centro.Coord.Y, arc.Punto3.Coord.X - arc.Centro.Coord.X);

            //Convierte los ángulos a grados
            angulo1 = angulo1 * 180 / (float)Math.PI;
            angulo2 = angulo2 * 180 / (float)Math.PI;

            //Asegura que el arco se dibuje en dirección contraria a las manecillas del reloj
            if (angulo1 < angulo2)
            {
                angulo1 += 360;
            }

            //Calcula la amplitud del arco
            float amplitud = angulo1 - angulo2;

            MessageBox.Show($"{arc.Measure}");
            Vector2 direccion = new Vector2((float)arc.Punto2.Coord.X - (float)arc.Centro.Coord.X, (float)arc.Punto2.Coord.Y - (float)arc.Centro.Coord.Y);

            System.Drawing.Point punto4 = new System.Drawing.Point((int)(arc.Centro.Coord.X + direccion.X * distanciaMaxima), (int)(arc.Centro.Coord.Y + direccion.Y * distanciaMaxima));

            System.Drawing.Point centro = new System.Drawing.Point((int)arc.Centro.Coord.X, (int)arc.Centro.Coord.Y);
            //Dibuja el rayo desde Punto1 hasta el borde del formulario.
            //g.DrawLine(pen, centro, punto4);

            Vector2 direccion1 = new Vector2((float)arc.Punto3.Coord.X - (float)arc.Centro.Coord.X, (float)arc.Punto3.Coord.Y - (float)arc.Centro.Coord.Y);



            System.Drawing.Point punto5 = new System.Drawing.Point((int)(arc.Centro.Coord.X + direccion1.X * distanciaMaxima), (int)(arc.Centro.Coord.Y + direccion1.Y * distanciaMaxima));

            //Dibuja el rayo desde Punto1 hasta el borde del formulario.
            //g.DrawLine(pen, centro, punto5);

            //DibujarLinea(g, pen, linea1);
            //DibujarLinea(g, pen, linea2);
            //Dibuja el arco
            g.DrawArc(pen, (int)arc.Centro.Coord.X - (int)arc.Measure, (int)arc.Centro.Coord.Y - (int)arc.Measure, (int)arc.Measure * 2, (int)arc.Measure * 2, angulo2, amplitud);

            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);

            // Imprime el mensaje al lado del punto usando DrawString
            g.DrawString(etiqueta, drawFont, drawBrush, (float)arc.Centro.Coord.X + (float)arc.Measure * 2 + 10, (float)arc.Centro.Coord.Y);
        }

        private void DibujarLinea(Graphics g, Pen pencil, Line l,string etiqueta)
        {
            
            // Calcular la pendiente de la l�nea
            double m = (l.Punto2.Coord.Y - l.Punto1.Coord.Y) / (l.Punto2.Coord.X - l.Punto1.Coord.X);

            // Calcular los puntos de intersecci�n con los bordes de la pantalla
            int xLeft = 0;
            int yLeft = (int)(l.Punto1.Coord.Y - m * l.Punto1.Coord.X);
            int xRight = areaGrafica.Width;
            int yRight = (int)(m * (xRight - l.Punto1.Coord.X) + l.Punto1.Coord.Y);
            
            // Dibujar una l�nea desde el borde izquierdo hasta el borde derecho de la pantalla
            g.DrawLine(pencil, xLeft, yLeft, xRight, yRight);
        }
        public void DibujarSegmento(Graphics g, Pen pen, Segment s, string etiqueta)
        {
            System.Drawing.Point punto1 = new System.Drawing.Point((int)s.Punto1.Coord.X, (int)s.Punto1.Coord.Y);
            System.Drawing.Point punto2 = new System.Drawing.Point((int)s.Punto2.Coord.X, (int)s.Punto2.Coord.Y);

            
            // Dibuja el segmento desde Punto1 hasta Punto2.
            g.DrawLine(pen, punto1, punto2);
            g.FillEllipse(Brushes.Black, punto1.X, punto1.Y, 5, 5);

            // Dibuja el punto 2 de la linea
            g.FillEllipse(Brushes.Black, punto2.X, punto2.Y, 5, 5);

            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);

            // Imprime el mensaje al lado del punto usando DrawString
            g.DrawString(etiqueta, drawFont, drawBrush, punto2.X + 10, punto2.Y);
        }




        //public void DibujarRayo(Graphics g, Pen pen, Ray rayo, string etiqueta)
        //{


        //    Vector2 direccion = new Vector2((float)rayo.Punto2.Coord.X - (float)rayo.Punto1.Coord.X, (float)rayo.Punto2.Coord.Y - (float)rayo.Punto1.Coord.Y);
        //    direccion = Vector2.Normalize(direccion);

        //    double distanciaMaxima = (Math.Sqrt(Math.Pow(areaGrafica.Width - rayo.Punto1.Coord.X, 2) + Math.Pow(areaGrafica.Height - rayo.Punto1.Coord.Y, 2)) * 10000);

        //    // Calculate the points on the edges of the drawing area
        //    System.Drawing.Point punto3 = new System.Drawing.Point((int)(rayo.Punto1.Coord.X + direccion.X * distanciaMaxima), (int)(rayo.Punto1.Coord.Y + direccion.Y * distanciaMaxima));

        //    System.Drawing.Point punto1 = new System.Drawing.Point((int)rayo.Punto1.Coord.X, (int)rayo.Punto1.Coord.Y);
        //    System.Drawing.Point punto2 = new System.Drawing.Point((int)rayo.Punto2.Coord.X, (int)rayo.Punto2.Coord.Y);

        //    // Dibuja el rayo desde Punto1 hasta el borde del formulario.
        //    g.DrawLine(pen, punto1, punto3);

        //    g.FillEllipse(Brushes.Black, punto1.X, punto1.Y, 5, 5);

        //    // Dibuja el punto 2 de la linea
        //    g.FillEllipse(Brushes.Black, punto2.X, punto2.Y, 5, 5);

        //    Font drawFont = new Font("Arial", 16);
        //    SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);



        //    // Imprime el mensaje al lado del punto usando DrawString
        //    g.DrawString(etiqueta, drawFont, drawBrush, punto1.X + 10, punto1.Y);
        //}

        private void DibujarRayo(Graphics g, Pen pencil, Ray r , string etiqueta)
        {
           
            // Calcular la pendiente de la l�nea
            double m = (r.Punto2.Coord.Y - r.Punto1.Coord.Y) / (r.Punto2.Coord.X - r.Punto1.Coord.X);

            if (r.Punto1.Coord.Y < r.Punto2.Coord.Y)
            {
                // Si el punto de inicio est� por encima del punto final, dibuja hacia la izquierda
                int xLeft = 0;
                int yLeft = (int)(r.Punto1.Coord.Y - m * r.Punto1.Coord.X);
                g.DrawLine(pencil, (int)r.Punto1.Coord.X, (int)r.Punto1.Coord.Y, xLeft, yLeft);
            }
            else
            {
                // Si el punto de inicio est� por debajo del punto final, dibuja hacia la derecha
                int xRight = areaGrafica.Width;
                int yRight = (int)(m * (xRight - r.Punto1.Coord.X) + r.Punto1.Coord.Y);
                g.DrawLine(pencil, (int)r.Punto1.Coord.X, (int)r.Punto1.Coord.Y, xRight, yRight);
            }
        }


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Formulario());
        }

        private void Formulario_Load(object sender, EventArgs e)
        {

        }

        private void areaGrafica_Click(object sender, EventArgs e)
        {

        }
    }
    
    
}

