﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph graph = new Graph("graph");
        GraphO g = new GraphO();
        string placedir = "";
        string routedir = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e) // TextBox str
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                PlaceDir.Text = filename;
                placedir = filename;
            }
        }

        private void btnOpenFile2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                RouteDir.Text = filename;
                routedir = filename;
            }
        }

        private int strToInt(string s)
        {
            int res = 0;
            for(int i=0; i < s.Length; i++)
            {
                if (s[i] - '0' > 9 || s[i] - '0' < 0)
                    return -999;
                res *= 10;
                res += s[i] - '0';
            }
            return res;
        }

        private bool isEdgeExist(Edge a, List<Edge> b)
        {
            for (int j = 0; j < b.Count; j++)
            {
                if (a.getSourceNode() == b[j].getSourceNode() && a.getDestNode() == b[j].getDestNode())
                {
                    return true;
                }
            }
            return false;
        }


        private void simulateGraph_click(object sender, RoutedEventArgs e)
        {
            //this.gViewer1.Graph = null;
            for(int i = 0; i < 1500; i++)
            {
                this.gViewer1.Undo();
            }
            //this.gViewer1 = null;
            GraphO g = new GraphO();
            Graph graph = new Graph("graph");
            int day = Convert.ToInt32(InfectTime.Text);
            if(day != -999)
            {
                Keterangan.Text = "Waiting...";
                loadGraph(ref g, placedir, routedir);
                g.BFS(day);
                List<Node> nodes = new List<Node>();
                nodes = g.getListNode();
                List<Edge> edges = new List<Edge>();
                edges = g.getListEdge();
                List<Edge> path = new List<Edge>();
                path = g.getPath();


                for (int i = 0; i < path.Count; i++)
                {
                    Keterangan.Text += (i+1).ToString()+". " +path[i].getSourceNode() + "->" + path[i].getDestNode() + "\n";
                }

                //g.resetGraph();
                
                for(int i = 0; i < edges.Count; i++)
                {
                    if (isEdgeExist(edges[i], path)) {
                        graph.AddEdge(edges[i].getSourceNode(), edges[i].getDestNode()).Attr.Color = Microsoft.Msagl.Drawing.Color.Chocolate;
                    } else {
                        graph.AddEdge(edges[i].getSourceNode(), edges[i].getDestNode());
                    }
                }
                this.gViewer1.Graph = graph;
                g.resetGraph();

            }  
        }/**/

        static void loadGraph(ref GraphO g, string pdir, string rdir)
        {
            int numNodes = 0; int numEdges = 0;
            string startingCity = "";
            Parser.readNodes(pdir, ref numNodes, ref startingCity, ref g);
            Parser.readEdges(rdir, ref numEdges, ref g);
            g.setStartNode(startingCity);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
