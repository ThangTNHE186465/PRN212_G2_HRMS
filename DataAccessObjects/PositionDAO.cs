using BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class PositionDAO
    {
        private readonly FuhrmContext _context;
        public PositionDAO(FuhrmContext context)
        {
            _context = context;
        }

        public List<Position> GetPositions()
        {
            return _context.Positions.ToList();
        }

        public Position GetPosition(int positionId)
        {
            return _context.Positions.Find(positionId);
        }

       

        public void UpdatePosition(Position position)
        {
            var existingPosition = _context.Positions.Find(position.PositionId);
            if (existingPosition != null)
            {
                existingPosition.PositionName = position.PositionName;
                _context.SaveChanges();
            }
        }
        public void AddPosition(Position position)
        {
            _context.Positions.Add(position);
            _context.SaveChanges();
        }

     
    }
}
