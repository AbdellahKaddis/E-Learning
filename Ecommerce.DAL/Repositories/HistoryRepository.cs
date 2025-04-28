//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ecommerce.DAL.Db;
//using Ecommerce.Models.DTOs;
//using Ecommerce.Models.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Ecommerce.DAL.Repositories
//{
//    public class HistoryRepository
//    {
//        private readonly AppDbContext _context;

//        public HistoryRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public List<HistoryDto> GetAllHistorys()
//        {

//            return _context.History
//                .Include(c => c.Course)
//                .Select(History => new HistoryDto
//                {
//                    HistoryId = History.HistoryId,
//                    titre = History.titre,
//                    URL = History.URL,
//                    Duration = History.Duration,
//                    CourseName = History.Course.CourseName,

//                })
//                .ToList();
//        }

//        public History GetHistoryId(int id)
//        {
//            return _context.History.FirstOrDefault(u => u.HistoryId == id);
//        }

//        public History AddHistory(createHistoryDto dto)
//        {
//            var History = new History
//            {
//                URL = dto.URL,
//                Duration = dto.Duration,
//                titre = dto.titre,
//                CourseId = dto.CourseId
//            };

//            _context.History.Add(History);
//            _context.SaveChanges();
//            return History;
//        }

//        public History UpdateHistory(int id, updateHistoryDto l)
//        {
//            var History = _context.History.FirstOrDefault(les => les.HistoryId == id);
//            if (History == null)
//            {
//                return null;
//            }

//            History.titre = l.titre;
//            History.URL = l.URL;
//            History.Duration = l.Duration;
//            History.CourseId = l.CourseId;

//            _context.SaveChanges();
//            return History;
//        }
//        public bool DeleteHistory(int id)
//        {
//            var History = _context.History.Find(id);
//            if (History != null)
//            {
//                _context.History.Remove(History);
//                _context.SaveChanges();
//                return true;
//            }
//            return false;
//        }
//    }
//}

