using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igrica
{
    public class Maze
    {
        public Graph<Square> g;
        public int start_node;
        public int end_node;

        Dictionary<String, int> h;

        public Maze()
        {
            h = new Dictionary<string, int>();
        }

        public void generateGraph(int rows, int columns, String[] inp)
        {
            int num_nodes = 0;
            String key;
            char[] charArray, charArray2;

            for (int i = 1; i < rows-1; i++)
            {
                charArray = inp[i].ToCharArray();
                for (int j = 1; j < columns-1; j++)
                {
                    if (charArray[j] != '#')
                    {
                        key = i + "," + j;
                        h.Add(key, num_nodes);
                        if (charArray[j] == 'S')
                        {
                            start_node = num_nodes;
                        }
                        if (charArray[j] == 'E')
                        {
                            end_node = num_nodes;
                        }
                        num_nodes++;
                    }
                }
            }
            g = new Graph<Square>(num_nodes);

            int x, y;

            //Matrica na sosednost
            for (int i = 1; i < rows - 1; i++)
            {
                charArray = inp[i].ToCharArray();
                for (int j = 1; j < columns - 1; j++)
                {
                    if (charArray[j] != '#')
                    {
                        if (charArray[j - 1] != '#')
                        {
                            h.TryGetValue(i + "," + j, out x);
                            h.TryGetValue(i + "," + (j-1), out y);
                            g.addEdge(x, y);
                        }
                        if (charArray[j + 1] != '#')
                        {
                            h.TryGetValue(i + "," + j, out x);
                            h.TryGetValue(i + "," + (j + 1), out y);
                            g.addEdge(x, y);
                        }
                        charArray2 = inp[i - 1].ToCharArray();
                        if (charArray2[j] != '#')
                        {
                            h.TryGetValue(i + "," + j, out x);
                            h.TryGetValue((i-1) + "," + j, out y);
                            g.addEdge(x, y);
                        }
                        charArray2 = inp[i + 1].ToCharArray();
                        if (charArray2[j] != '#')
                        {
                            h.TryGetValue(i + "," + j, out x);
                            h.TryGetValue((i+1) + "," + j, out y);
                            g.addEdge(x, y);
                        }
                    }
                }
            }
        }

        public bool findPath()
        {
            bool[] visited = new bool[g.getNumNodes()];
            bool imaPat = false;
            for (int i = 0; i < g.getNumNodes(); i++)
            {
                visited[i] = false;
            }
            Stack<int> s = new Stack<int>();
            s.Push(start_node);

            int pom, pom1;
            while (s.Count != 0)
            {
                if (s.Peek() == end_node)
                {
                    imaPat = true;
                    break;
                }
                pom = s.Peek();
                pom1 = pom;
                for (int i = 0; i < g.getNumNodes(); i++)
                {
                    if (g.adjacent(pom, i) == 1)
                    {
                        pom1 = i;
                        if (!visited[i]) 
                            break;
                    }
                }
                if (!visited[pom1])
                {
                    visited[pom1] = true;
                    s.Push(pom1);
                }
                else s.Pop();
            }
            return imaPat;
        }
        
    }
}
