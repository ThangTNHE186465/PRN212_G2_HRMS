using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly PositionDAO _positionDAO;

        public PositionRepository(PositionDAO positionDAO)
        {
            _positionDAO = positionDAO;
        }

        public List<Position> GetPositions()
        {
            return _positionDAO.GetPositions();
        }

        public Position GetPosition(int positionId)
        {
            return _positionDAO.GetPosition(positionId);
        }

        public void AddPosition(Position position)
        {
            _positionDAO.AddPosition(position);
        }

        public void UpdatePosition(Position position)
        {
            _positionDAO.UpdatePosition(position);
        }

    }
}
