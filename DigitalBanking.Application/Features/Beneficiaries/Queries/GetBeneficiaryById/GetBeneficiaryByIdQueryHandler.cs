using DigitalBanking.Application.Features.Beneficiaries.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaryById
{
    public class GetBeneficiaryByIdQueryHandler : IRequestHandler<GetBeneficiaryByIdQuery, BeneficiaryDto>
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetBeneficiaryByIdQueryHandler> _logger;

        public GetBeneficiaryByIdQueryHandler(IBeneficiaryRepository beneficiaryRepository, ILogger<GetBeneficiaryByIdQueryHandler> logger,
            ICurrentUserService currentUserService)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<BeneficiaryDto> Handle(GetBeneficiaryByIdQuery request, CancellationToken cancellationToken)
        {
            var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(request.BeneficiaryId, _currentUserService.UserId, cancellationToken);

            if (beneficiary == null)
            {
                _logger.Log(LogLevel.Error, "Beneficiary with given id is not found for user = {useid}", _currentUserService.UserId);
                throw new BeneficiaryNotFoundException();
            }

            _logger.Log(LogLevel.Information, "Beneficiary information retrieved for user = {useid}", _currentUserService.UserId);

            return new BeneficiaryDto
            {
                Id = beneficiary.Id,
                AccountNumber = beneficiary.BeneficiaryAccountNumber,
                BeneficiaryName = beneficiary.BeneficiaryName,
                BankName = beneficiary.BeneficiaryBankName,
                BankCode = beneficiary.BeneficiaryBankCode,
                Status = beneficiary.Status,
                IsVerified = beneficiary.VerifiedAt.HasValue,
                VerifiedAt = beneficiary.VerifiedAt
            };
        }
    }
}
