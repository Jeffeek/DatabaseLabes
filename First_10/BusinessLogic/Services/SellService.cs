using System.Collections.Generic;
using AutoMapper;
using First_10.BusinessLogic.Repositories;
using First_10.DataAccess.Models;
using First_10.ViewModels.Models;

namespace First_10.BusinessLogic.Services
{
    public class SellService
    {
        private readonly SellRepository _sellRepository;
        private readonly IMapper _mapper;

        public SellService(SellRepository sellRepository,
                           IMapper mapper)
        {
            _sellRepository = sellRepository;
            _mapper = mapper;
        }

        public ICollection<SellViewModel> GetAll()
        {
            var products = _mapper.Map<List<SellViewModel>>(_sellRepository.GetAll());

            return products;
        }

        public void Add(SellViewModel model) =>
            _sellRepository.Add(_mapper.Map<Sell>(model));

        public void Update(SellViewModel model) =>
            _sellRepository.Update(_mapper.Map<Sell>(model));

        public void Delete(int id) => _sellRepository.Remove(id);
    }
}