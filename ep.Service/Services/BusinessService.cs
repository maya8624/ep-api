﻿using ep.Contract.ViewModels;
using ep.Data.Wrappers;
using ep.Service.Interfaces;

namespace ep.Service.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public BusinessService(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<BusinessView>> GetBusinessesAsync()
        {
            var businesses = await _repository.Business.GetAllAsync();
            var data = _mapper.Map<IEnumerable<Business>, IEnumerable<BusinessView>>(businesses);
            return data;
        }
    }
}