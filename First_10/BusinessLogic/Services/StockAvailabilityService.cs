using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.Shared;
using First_10.BusinessLogic.Repositories;
using First_10.DataAccess.Models;
using First_10.Queries;
using First_10.ViewModels.Models;
using First_10.ViewModels.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace First_10.BusinessLogic.Services
{
    public class StockAvailabilityService
    {
        private readonly StockAvailabilityRepository _stockAvailabilityRepository;
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public StockAvailabilityService(StockAvailabilityRepository stockAvailabilityRepository,
                                        IDbContextFactory<ApplicationDbContext> dbContextFactory,
                                        IMapper mapper)
        {
            _stockAvailabilityRepository = stockAvailabilityRepository;
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public ICollection<StockAvailabilityViewModel> GetAll()
        {
            var products = _mapper.Map<List<StockAvailabilityViewModel>>(_stockAvailabilityRepository.GetAll());

            return products;
        }

        public void Add(StockAvailabilityViewModel model) =>
            _stockAvailabilityRepository.Add(_mapper.Map<StockAvailability>(model));

        public void Update(StockAvailabilityViewModel model) =>
            _stockAvailabilityRepository.Update(_mapper.Map<StockAvailability>(model));

        public void Delete(int id) => _stockAvailabilityRepository.Remove(id);
    }
}
