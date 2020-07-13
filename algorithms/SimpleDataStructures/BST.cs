using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.SimpleDataStructures
{
    public class Node
    {
        public int Key { get; set; }
        public Node Parent { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node(int key)
        {
            Key = key;
        }
    }
    public class BST
    {
        public Node Root { get; set; }

        public Node Search(Node node, int key)
        {
            while (node != null && key != node.Key)
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

        public Node Minimum(Node node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }

        public Node Maximum(Node node)
        {
            while (node.Right != null)
            {
                node = node.Right;
            }
            return node;
        }

        public Node Successor(Node node)
        {
            if (node.Right != null)
            {
                return Minimum(node.Right);
            }

            var prevnode = node.Parent;

            while (prevnode != null && node != prevnode.Right)
            {
                node = prevnode;
                prevnode = prevnode.Parent;
            }
            return prevnode;
        }

        public Node Predeccessor(Node node)
        {
            if (node.Left != null)
            {
                return Maximum(node.Left);
            }

            var prevnode = node.Parent;

            while (prevnode != null && node != prevnode.Left)
            {
                node = prevnode;
                prevnode = prevnode.Parent;
            }
            return prevnode;
        }

        public void Insert(int key)
        {
            var nodeToInsert = new Node(key);
            Node prevnode = null;
            var node = Root;
            if (node == null)
            {
                Root = nodeToInsert;
                return;
            }
            while (node != null)
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

            if (nodeToInsert.Key < prevnode.Key)
            {
                prevnode.Left = nodeToInsert;
            }
            else
            {
                prevnode.Right = nodeToInsert;
            }
        }

        private void Transplant(Node nodeToReplace, Node replacedNode)
        {
            if (nodeToReplace.Parent == null)
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

            if (replacedNode != null)
            {
                replacedNode.Parent = nodeToReplace.Parent;
            }
        }

        public void Delete(int key)
        {
            var nodeToDelete = Search(Root, key);

            if (nodeToDelete.Left == null)
            {
                Transplant(nodeToDelete, nodeToDelete.Right);
            }
            else if (nodeToDelete.Right == null)
            {
                Transplant(nodeToDelete, nodeToDelete.Left);
            }
            else
            {
                var nextNode = Successor(nodeToDelete);

                if (nextNode.Parent != nodeToDelete)
                {
                    Transplant(nextNode, nextNode.Right);

                    nextNode.Right = nodeToDelete.Right;
                    nextNode.Right.Parent = nextNode;
                }

                Transplant(nodeToDelete, nextNode);

                nextNode.Left = nodeToDelete.Left;
                nextNode.Left.Parent = nextNode;
            }
        }
    }
}
