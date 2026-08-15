using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Beneficiaries.Commands.RemoveBeneficiary
{
    public class RemoveBeneficiaryCommandHandler : IRequestHandler<RemoveBeneficiaryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ILogger<RemoveBeneficiaryCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public RemoveBeneficiaryCommandHandler(IUnitOfWork unitOfWork, IBeneficiaryRepository beneficiaryRepository, 
            ICurrentUserService currentUserService, ILogger<RemoveBeneficiaryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _beneficiaryRepository = beneficiaryRepository;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Handle(RemoveBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(request.BeneficiaryId, _currentUserService.UserId, cancellationToken);
            
            if (beneficiary == null)
            {
                _logger.Log(LogLevel.Error, "Beneficiary with given id is not found for user = {useid}", _currentUserService.UserId);
                throw new BeneficiaryNotFoundException();
            }

            beneficiary.Remove();
            _beneficiaryRepository.RemoveBeneficiary(beneficiary);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Beneficiary removed for customer = {customerId}", _currentUserService.UserId.ToString());
        }
    }
}
