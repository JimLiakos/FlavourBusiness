using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorLayoutDesigner
{

    /// <MetaDataID>{9dfb5f32-386a-4e1c-b30e-771b430017a7}</MetaDataID>
    public class CommandTarget
    {
        public ICommandTargetObject CommandTargetObject;

        public object Data;

    }
    /// <MetaDataID>{21e65aa1-e124-4a34-b017-53209e090164}</MetaDataID>
    public class UndoRedoCommand
    {
        public List<CommandTarget> UndoData = new List<CommandTarget>();

        internal void ReplaceCommandTarget(ICommandTargetObject commandTargetObject)
        {
            foreach (var commandTarget in UndoData.Where(x => x.CommandTargetObject.ID == commandTargetObject.ID))
                commandTarget.CommandTargetObject = commandTargetObject;

            foreach (var commandTarget in RedoData.Where(x => x.CommandTargetObject.ID == commandTargetObject.ID))
                commandTarget.CommandTargetObject = commandTargetObject;

        }

        public List<CommandTarget> RedoData = new List<CommandTarget>();

        internal void Undo()
        {
            foreach (var commandTargetObject in UndoData.Select(x => x.CommandTargetObject))
                commandTargetObject.Undo(this);
        }

        internal void Redo()
        {
            foreach (var commandTargetObject in RedoData.Select(x => x.CommandTargetObject))
                commandTargetObject.Redo(this);
        }
    }

    /// <MetaDataID>{f0a8f746-e00f-4b1c-a503-9ff376b62e30}</MetaDataID>
    public interface ICommandTargetObject
    {
        Guid ID { get; }
        void Undo(UndoRedoCommand command);
        void Redo(UndoRedoCommand command);

        void MarkUndo(UndoRedoCommand command);
        void MarkRedo(UndoRedoCommand command);

    }
}
