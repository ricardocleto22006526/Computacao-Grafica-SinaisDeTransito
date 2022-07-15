using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_OpenCV
{
    public partial class HistGray : Form
    {
        public HistGray(int[] histArray)
        {
            InitializeComponent();
            
            ZedGraph.GraphPane myPane = zedGraphControl1.GraphPane;

            // Titulo da Janela
            myPane.Title.Text = "Histograma de cinzentos";

            ZedGraph.PointPairList lista1 = new ZedGraph.PointPairList();

            for (int i = 0; i < 256; i++)
            {
                lista1.Add(i,histArray[i]);
            }

            myPane.AddCurve("Cinzentos", lista1, Color.Black, ZedGraph.SymbolType.Diamond);

            zedGraphControl1.AxisChange();
        }

        public HistGray(int[,] histRGB)
        {
            InitializeComponent();

            ZedGraph.GraphPane myPane = zedGraphControl1.GraphPane;

            // Titulo da Janela
            myPane.Title.Text = "Histograma de RGB";

            ZedGraph.PointPairList lista1 = new ZedGraph.PointPairList();
            ZedGraph.PointPairList lista2 = new ZedGraph.PointPairList();
            ZedGraph.PointPairList lista3 = new ZedGraph.PointPairList();

            for (int i = 0; i < 256; i++)
            {
                for (int k = 0; k < 256; k++)
                {
                    lista1.Add(i, histRGB[k,i]);
                    lista2.Add(i, histRGB[k,i]);
                    lista3.Add(i, histRGB[k,i]);
                }
                
            }

            myPane.AddCurve("RGB", lista1, Color.Red, ZedGraph.SymbolType.Diamond);
            myPane.AddCurve("RGB", lista2, Color.Green, ZedGraph.SymbolType.Diamond);
            myPane.AddCurve("RGB", lista3, Color.Blue, ZedGraph.SymbolType.Diamond);
        }

    }
}
