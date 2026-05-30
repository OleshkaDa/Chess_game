using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//методы перемещения.
namespace Model.Core.Interfaces
{
    public interface IMovable
    {
        void Move(int toRow, int toCol);
        bool IsValidMove(int toRow, int toCol, IPiece[,] board);
    }
}
