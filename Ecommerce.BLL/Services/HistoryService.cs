//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ecommerce.DAL.Repositories;
//using Ecommerce.Models.DTOs;
//using Ecommerce.Models.Entities;

//namespace Ecommerce.BLL.Services
//{
//    public class HistoryService
//    {

//        private readonly HistoryRepository _repo;


//        public HistoryService(HistoryRepository repo)
//        {
//            _repo = repo;
//        }

//        public List<HistoryDto> GetAllHistorys() => _repo.GetAllHistorys();
//        public History GetHistoryId(int id) => _repo.GetHistoryId(id);
//        public History AddHistory(createHistoryDto dto) => _repo.AddHistory(dto);

//        public bool DeleteHistory(int id) => _repo.DeleteHistory(id);
//        public History UpdateHistory(int id, updateHistoryDto l) => _repo.UpdateHistory(id, l);
//    }
//}
