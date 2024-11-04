using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface IPositionRepository
    {
        List<Position> GetPositions();
        Position GetPosition(int positionId);
        void AddPosition(Position position); // Thêm phương thức AddPosition
        void UpdatePosition(Position position);

    }
}
