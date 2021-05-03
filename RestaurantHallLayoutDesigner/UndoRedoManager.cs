using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorLayoutDesigner
{
    /// <MetaDataID>{ad52ef61-b4d9-4c6f-b15b-65678f770b35}</MetaDataID>
    public class UndoRedoManager
    {
        public static void ReplaceCommandTarget(ICommandTargetObject commandTargetObject)
        {
            foreach (var cmd in Commands)
                cmd.ReplaceCommandTarget(commandTargetObject);
        }


        static int CurrentCommandIndex = -1;
        static List<UndoRedoCommand> Commands = new List<UndoRedoCommand>();


        public static void Clear()
        {
            Commands.Clear();
        }

        public static UndoRedoCommand NewCommand()
        {
            var undoRedoCommand = new UndoRedoCommand();
            foreach (var cmd in (from command in Commands
                                 where Commands.IndexOf(command) > CurrentCommandIndex
                                 select command).ToList())
            {
                Commands.Remove(cmd);
            }
            Commands.Add(undoRedoCommand);
            CurrentCommandIndex = Commands.IndexOf(undoRedoCommand);
            return undoRedoCommand;
        }

        public static bool HasUndoCommands
        {
            get
            {

                return CurrentCommandIndex >= 0;
            }

        }

        public static void Undo()
        {
            if (CurrentCommandIndex >= 0)
                Commands[CurrentCommandIndex--].Undo();
        }

        public static bool HasRedoCommands
        {
            get
            {
                return CurrentCommandIndex + 1 < Commands.Count;
            }

        }

        public static void Redo()
        {
            if (CurrentCommandIndex + 1 < Commands.Count)
                Commands[++CurrentCommandIndex].Redo();
        }



    }


}
