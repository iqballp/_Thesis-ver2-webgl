using System;
using System.Collections.Generic;

public class Node
{
    public List<Bilangan> listBilangan = new List<Bilangan>();
    public string sumber;
    public Node parentNode;
    public List<Node> child = new List<Node>();
};