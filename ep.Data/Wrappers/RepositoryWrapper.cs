﻿namespace ep.Data.Wrappers
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly EPDbContext _context;
        private IBusinessRepository? _business;
        private ICustomerRepository? _customer;
        private IMessageRepository? _message;
        private IUnitOfWork? _unitOfWork;
        private IUserRepository? _user;
        private IUserTokenRepository? _userToken;

        public RepositoryWrapper(EPDbContext context)
        {
            _context = context;
        }

        public IBusinessRepository Business
            => _business is null ? _business = new BusinessRepository(_context) : _business;

        public ICustomerRepository Customer
            => _customer is null ? _customer = new CustomerRepository(_context) : _customer;

        public IMessageRepository Message 
            => _message is null ? _message = new MessageRepository(_context) : _message;

        public IUnitOfWork UnitOfWork 
            => _unitOfWork is null ? _unitOfWork = new UnitOfWork(_context) : _unitOfWork;

        public IUserRepository User
            => _user is null ? _user = new UserRepository(_context) : _user;

        public IUserTokenRepository UserToken
            => _userToken is null ? _userToken = new UserTokenRepository(_context) : _userToken;
    }
}
