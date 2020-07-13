using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.SimpleDataStructures
{
    public enum Color : byte
    {
        Red,
        Black
    }
    public static class ColorExtensions
    {
        public static bool IsBlack(this Color color)
        {
            return color == Color.Black;
        }
        public static bool IsRed(this Color color)
        {
            return color == Color.Red;
        }
        public static void SetRed(this RBTNode node)
        {
            node.Color = Color.Red;
        }
        public static void SetBlack(this RBTNode node)
        {
            node.Color = Color.Black;
        }
    }
    public class RBTNode
    {
        public int Key { get; set; }
        public Color Color { get; set; }
        public RBTNode Parent { get; set; }
        public RBTNode Left { get; set; }
        public RBTNode Right { get; set; }
        public RBTNode() { }
        public RBTNode(int key)
        {
            Key = key;
            Color = Color.Red;
        }
    }

    public class RBT
    {
        private readonly static RBTNode NIL = new RBTNode { Color = Color.Black };

        public RBTNode Root { get; set; }

        public RBTNode Search(RBTNode node, int key)
        {
            while (node != NIL && key != node.Key)
            {
                if (key < node.Key)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }
            return node;
        }

        public RBTNode Minimum(RBTNode node)
        {
            while (node.Left != NIL)
            {
                node = node.Left;
            }
            return node;
        }

        public RBTNode Maximum(RBTNode node)
        {
            while (node.Right != NIL)
            {
                node = node.Right;
            }
            return node;
        }

        public RBTNode Successor(RBTNode node)
        {
            if (node.Right != NIL)
            {
                return Minimum(node.Right);
            }

            var prevnode = node.Parent;

            while (prevnode != NIL && node != prevnode.Right)
            {
                node = prevnode;
                prevnode = prevnode.Parent;
            }
            return prevnode;
        }

        public RBTNode Predeccessor(RBTNode node)
        {
            if (node.Left != NIL)
            {
                return Maximum(node.Left);
            }

            var prevnode = node.Parent;

            while (prevnode != NIL && node != prevnode.Left)
            {
                node = prevnode;
                prevnode = prevnode.Parent;
            }
            return prevnode;
        }

        private void Transplant(RBTNode nodeToReplace, RBTNode replacedNode)
        {
            if (nodeToReplace.Parent == NIL)
            {
                Root = replacedNode;
            }
            else if (nodeToReplace == nodeToReplace.Parent.Left)
            {
                nodeToReplace.Parent.Left = replacedNode;
            }
            else
            {
                nodeToReplace.Parent.Right = replacedNode;
            }

            replacedNode.Parent = nodeToReplace.Parent;
        }

        private void LeftRotate(RBTNode node)
        {
            var rightNode = node.Right;
            rightNode.Right = node.Left;

            if (rightNode.Left != NIL)
            {
                rightNode.Left.Parent = node;
            }
            
            rightNode.Parent = node.Parent;

            if (node.Parent == NIL)
            {
                Root = rightNode;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = rightNode;
            }
            else
            {
                node.Parent.Right = rightNode;
            }

            rightNode.Left = node;
            node.Parent = rightNode;
        }

        private void RightRotate(RBTNode node)
        {
            var leftNode = node.Left;
            leftNode.Right = node.Right;

            if (leftNode.Right != NIL)
            {
                leftNode.Right.Parent = node;
            }

            leftNode.Parent = node.Parent;

            if (node.Parent == NIL)
            {
                Root = leftNode;
            }
            else if (node == node.Parent.Right)
            {
                node.Parent.Right = leftNode;
            }
            else
            {
                node.Parent.Left = leftNode;
            }

            leftNode.Right = node;
            node.Parent = leftNode;
        }

        private void InsertFixup(RBTNode node)
        {
            while (node.Parent.Color.IsRed())
            {
                if (node.Parent==node.Parent.Parent.Left)
                {
                    var uncleNode = node.Parent.Parent.Right;

                    if (uncleNode.Color.IsRed())
                    {
                        node.Parent.SetBlack();
                        uncleNode.SetBlack();
                        node.Parent.Parent.SetRed();
                        node = node.Parent.Parent;
                    }
                    else 
                    {
                        if (node == node.Parent.Right)
                        {
                            node = node.Parent;
                            LeftRotate(node);
                        }

                        node.Parent.SetBlack();
                        node.Parent.Parent.SetRed();
                        RightRotate(node.Parent.Parent);
                    }
                }
                else
                {
                    var uncleNode = node.Parent.Parent.Left;

                    if (uncleNode.Color.IsRed())
                    {
                        node.Parent.SetBlack();
                        uncleNode.SetBlack();
                        node.Parent.Parent.SetRed();
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RightRotate(node);
                        }

                        node.Parent.SetBlack();
                        node.Parent.Parent.SetRed();
                        LeftRotate(node.Parent.Parent);
                    }
                }
                Root.SetBlack();
            }
        }

        public void Insert(int key)
        {
            var nodeToInsert = new RBTNode(key);
            RBTNode prevnode = NIL;
            var node = Root;
            
            while (node != NIL)
            {
                prevnode = node;

                if (nodeToInsert.Key < node.Key)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            nodeToInsert.Parent = prevnode;

            if (prevnode == NIL)
            {
                Root = nodeToInsert;
            }
            else if (nodeToInsert.Key < prevnode.Key)
            {
                prevnode.Left = nodeToInsert;
            }
            else
            {
                prevnode.Right = nodeToInsert;
            }

            nodeToInsert.Left = NIL;
            nodeToInsert.Right = NIL;

            InsertFixup(nodeToInsert);
        }

        private void DeleteFixup(RBTNode badNode)
        {
            RBTNode brotherNode;

            while (badNode != Root && badNode.Color.IsBlack())
            {
                if (badNode == badNode.Parent.Left)
                {
                    brotherNode = badNode.Parent.Right;

                    if (brotherNode.Color.IsRed())
                    {
                        brotherNode.SetBlack();
                        badNode.Parent.SetRed();
                        LeftRotate(badNode.Parent);
                        brotherNode = badNode.Parent.Right;
                    }

                    if (brotherNode.Left.Color.IsBlack() && brotherNode.Right.Color.IsBlack())
                    {
                        brotherNode.SetRed();
                        badNode = badNode.Parent;
                    }
                    else
                    {
                        if (brotherNode.Right.Color.IsBlack())
                        {
                            brotherNode.Left.SetBlack();
                            brotherNode.SetRed();
                            RightRotate(brotherNode);
                            brotherNode = badNode.Parent.Right;
                        }

                        brotherNode.Color = badNode.Parent.Color;
                        badNode.Parent.SetBlack();
                        LeftRotate(badNode.Parent);
                        badNode = Root;
                    }
                }
                else
                {
                    brotherNode = badNode.Parent.Left;

                    if (brotherNode.Color.IsRed())
                    {
                        brotherNode.SetBlack();
                        badNode.Parent.SetRed();
                        RightRotate(badNode.Parent);
                        brotherNode = badNode.Parent.Left;
                    }

                    if (brotherNode.Left.Color.IsBlack() && brotherNode.Right.Color.IsBlack())
                    {
                        brotherNode.SetRed();
                        badNode = badNode.Parent;
                    }
                    else
                    {
                        if (brotherNode.Right.Color.IsBlack())
                        {
                            brotherNode.Right.SetBlack();
                            brotherNode.SetRed();
                            LeftRotate(brotherNode);
                            brotherNode = badNode.Parent.Left;
                        }

                        brotherNode.Color = badNode.Parent.Color;
                        badNode.Parent.SetBlack();
                        RightRotate(badNode.Parent);
                        badNode = Root;
                    }
                }
            }
            badNode.SetBlack();
        }

        public void Delete(int key)
        {
            var nodeToDelete = Search(Root, key);

            RBTNode badNode;

            var nextNode = nodeToDelete;
            var originalColor = nextNode.Color;

            if (nodeToDelete.Left == NIL)
            {
                badNode = nodeToDelete.Right;
                Transplant(nodeToDelete, nodeToDelete.Right);
            }
            else if (nodeToDelete.Right == NIL)
            {
                badNode = nodeToDelete.Left;
                Transplant(nodeToDelete, nodeToDelete.Left);
            }
            else
            {
                nextNode = Successor(nodeToDelete.Right);
                originalColor = nextNode.Color;
                badNode = nextNode.Right;

                if (nextNode.Parent == nodeToDelete)
                {
                    badNode.Parent = nextNode;
                }
                else
                {
                    Transplant(nextNode, nextNode.Right);
                    nextNode.Right = nodeToDelete.Right;
                    nextNode.Right.Parent = nextNode;
                }

                Transplant(nodeToDelete, nextNode);
                nextNode.Left = nodeToDelete.Left;
                nextNode.Left.Parent = nextNode;
                nextNode.Color = nodeToDelete.Color;
            }

            if (originalColor.IsBlack())
            {
                DeleteFixup(badNode);
            }
        }
    }
}
