using System;
using System.Collections.Generic;

namespace FeynmanDiagram
{
    public class UndoStack
    {
        private Stack<Action> _stack = new Stack<Action>();

        public static UndoStack Current { get; private set; }

        public void Push(Action action)
        {
            if (action != null)
                throw new ArgumentNullException(nameof(action));
            _stack.Push(action);
        }

        public void Undo() => _stack.Pop().Invoke();
    }
}
