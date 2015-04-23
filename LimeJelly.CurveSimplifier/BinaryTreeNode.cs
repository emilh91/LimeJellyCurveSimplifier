namespace LimeJelly.CurveSimplifier
{
    class BinaryTreeNode<T>
    {
        public T Data { get; private set; }

        public BinaryTreeNode<T> Left { get; set; }

        public BinaryTreeNode<T> Right { get; set; }

        public BinaryTreeNode(T data)
        {
            Data = data;
        }
    }
}
