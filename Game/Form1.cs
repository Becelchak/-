using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game
{
    public partial class Form1 : Form
    {
        // TODO
        // 1. 2 балла - Динамическое добавление вершин и ребер + удаление вершин и ребер
        // 2. 2 балла - BFS и DFS - визуализация процесса
        // 3. 2 балла - Поиск кратчайшего пути - визуализация результата
        // 4. 2 балла - drag&drop
        public Point[] points;

        public Form1()
        {
            var graph = Graph.MakeGraph(
                0, 1,
                2, 3,
                4, 5
            );
            var listDeletedNode = new List<Node>();
            var controls = new List<Control>();
            var nodeDiameter = 40;
            var bigRadius = 200;
            var center = new Point(300, 300);
            var angle =
                360.0 / graph.Length;
            points = Enumerable.Range(0, graph.Length)
                .Select(e => new Point(
                    center.X + (int) (bigRadius * Math.Sin(e * Math.PI * angle / 180.0)),
                    center.Y + (int) (bigRadius * Math.Cos(e * Math.PI * angle / 180.0))
                ))
                .ToArray();


            var label = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(ClientSize.Width, 30),
                Text = "Чтобы удалить вершину, наберите ее номер. Чтобы удалить ребро введите через тире номера нодов"
            };
            var box = new TextBox
            {
                Location = new Point(0, label.Bottom),
                Size = label.Size
            };
            var deleted = new Button
            {
                Location = new Point(0, box.Bottom),
                Size = label.Size,
                Text = "Удалить"
            };
            var create = new Button
            {
                Location = new Point(deleted.Right, box.Bottom),
                Size = label.Size,
                Text = "Создать"
            };

            Controls.Add(create);
            Controls.Add(label);
            Controls.Add(box);
            Controls.Add(deleted);
            deleted.Click += (sender, args) =>
            {
                if (box.Text.Contains('-'))
                {
                    var from = Int32.Parse(box.Text.Split("-")[0]);
                    var to = Int32.Parse(box.Text.Split("-")[1]);
                    var edges = new List<Edge>(graph[from].IncidentEdges);
                    foreach (var edge in edges)
                    {
                        if (edge.To.NodeNumber == to)
                            graph.Delete(edge);
                    }
                }
                else
                {
                    var number = Int32.Parse(box.Text);
                    var edges = new List<Edge>(graph[number].IncidentEdges);

                    foreach (var edge in edges)
                    {
                        graph.Delete(edge);
                    }

                    listDeletedNode.Add(graph[number]);
                }

                Invalidate();
            };


            create.Click += (sender, args) =>
            {
                if (box.Text.Contains('-'))
                {
                    var from = Int32.Parse(box.Text.Split("-")[0]);
                    var to = Int32.Parse(box.Text.Split("-")[1]);
                    graph.Connect(from, to);
                }
                else
                {
                    graph.Add(new Node(graph.Nodes.Count()));
                    angle = 360.0 / graph.Length;
                    points = Enumerable.Range(0, graph.Length)
                        .Select(e => new Point(
                            center.X + (int) (bigRadius * Math.Sin(e * Math.PI * angle / 180.0)),
                            center.Y + (int) (bigRadius * Math.Cos(e * Math.PI * angle / 180.0))
                        ))
                        .ToArray();
                }

                Invalidate();
            };

            Paint += (sender, args) =>
            {
                var graphics = CreateGraphics();
                foreach (var node in graph.Nodes)
                {
                    foreach (var incidentNode in node.IncidentNodes)
                    {
                        var point = points[node.NodeNumber];
                        if (incidentNode.NodeNumber >= node.NodeNumber)
                        {
                            continue;
                        }

                        var incidentPoint = points[incidentNode.NodeNumber];
                        graphics.DrawLine(Pens.Green, point, incidentPoint);
                    }
                }


                foreach (var node in graph.Nodes)
                {
                    var point = points[node.NodeNumber];
                    if (listDeletedNode.Contains(node))
                    {
                        Controls.Remove(controls[node.NodeNumber]);
                        continue;
                    }

                    graphics.FillEllipse(
                        Brushes.Blue,
                        new Rectangle(
                            point.X - nodeDiameter / 2,
                            point.Y - nodeDiameter / 2,
                            nodeDiameter,
                            nodeDiameter));
                    if (controls.Count < graph.Nodes.Count())
                    {
                        var label = new Label()
                        {
                            Text = node.NodeNumber.ToString(),
                            Location = new Point(point.X, point.Y),
                            BackColor = Color.Red,
                            Width = 20
                        };
                        Controls.Add(label);
                        controls.Add(label);
                    }
                    else
                    {
                        controls[node.NodeNumber].Location = point;
                    }
                }
            };
            InitializeComponent();
        }
    }
}