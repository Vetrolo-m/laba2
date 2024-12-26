using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AVLNode<T>
{
    public T Value { get; set; }
    public AVLNode<T> Left { get; set; }
    public AVLNode<T> Right { get; set; }
    public int Height { get; set; }

    public AVLNode(T value)
    {
        Value = value;
        Height = 1;
    }
}

public class AVLTree<T> where T : IComparable<T>
{
    private AVLNode<T> root;

    public void Insert(T value)
    {
        root = Insert(root, value);
    }

    private AVLNode<T> Insert(AVLNode<T> node, T value)
    {
        if (node == null) return new AVLNode<T>(value);

        if (value.CompareTo(node.Value) < 0)
            node.Left = Insert(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            node.Right = Insert(node.Right, value);
        else
            return node;

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

        return Balance(node);
    }

    public void Delete(T value)
    {
        root = Delete(root, value);
    }

    private AVLNode<T> Delete(AVLNode<T> node, T value)
    {
        if (node == null) return node;

        if (value.CompareTo(node.Value) < 0)
            node.Left = Delete(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            node.Right = Delete(node.Right, value);
        else
        {
            if (node.Left == null)
                return node.Right;
            else if (node.Right == null)
                return node.Left;

            node.Value = MinValue(node.Right);

            node.Right = Delete(node.Right, node.Value);
        }

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

        return Balance(node);
    }

    private T MinValue(AVLNode<T> node)
    {
        T minv = node.Value;
        while (node.Left != null)
        {
            minv = node.Left.Value;
            node = node.Left;
        }
        return minv;
    }

    public AVLNode<T> Find(T value)
    {
        return Find(root, value);
    }

    private AVLNode<T> Find(AVLNode<T> node, T value)
    {
        while (node != null)
        {
            if (value.CompareTo(node.Value) == 0)
                return node;
            node = value.CompareTo(node.Value) < 0 ? node.Left : node.Right;
        }
        return null;
    }

    public void PrintInOrder()
    {
        PrintInOrder(root);
        Console.WriteLine();
    }

    private void PrintInOrder(AVLNode<T> node)
    {
        if (node != null)
        {
            PrintInOrder(node.Left);
            Console.Write(node.Value + " ");
            PrintInOrder(node.Right);
        }
    }

    private int GetHeight(AVLNode<T> node)
    {
        return node?.Height ?? 0;
    }

    private int GetBalance(AVLNode<T> node)
    {
        if (node == null) return 0;
        return GetHeight(node.Left) - GetHeight(node.Right);
    }

    private AVLNode<T> Balance(AVLNode<T> node)
    {
        int balance = GetBalance(node);

        if (balance > 1 && GetBalance(node.Left) >= 0)
            return RotateRight(node);

        if (balance < -1 && GetBalance(node.Right) <= 0)
            return RotateLeft(node);

        if (balance > 1 && GetBalance(node.Left) < 0)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        if (balance < -1 && GetBalance(node.Right) > 0)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    private AVLNode<T> RotateRight(AVLNode<T> y)
    {
        AVLNode<T> x = y.Left;
        AVLNode<T> T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

        return x;
    }

    private AVLNode<T> RotateLeft(AVLNode<T> x)
    {
        AVLNode<T> y = x.Right;
        AVLNode<T> T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

        return y;
    }
}

public class Program
{
    public static void Main()
    {
        AVLTree<int> avlTree = new AVLTree<int>();
        avlTree.Insert(30);
        avlTree.Insert(20);
        avlTree.Insert(40);
        avlTree.Insert(10);
        avlTree.Insert(25);
        avlTree.Insert(35);
        avlTree.Insert(50);

        Console.WriteLine("Содержимое AVL-дерева в порядке обхода:");
        avlTree.PrintInOrder();

        Console.WriteLine("Удаление 20");
        avlTree.Delete(20);
        avlTree.PrintInOrder();

        Console.WriteLine("Поиск 25: " + (avlTree.Find(25) != null ? "Найден" : "Не найден"));
        Console.WriteLine("Поиск 20: " + (avlTree.Find(20) != null ? "Найден" : "Не найден"));
    }
}
