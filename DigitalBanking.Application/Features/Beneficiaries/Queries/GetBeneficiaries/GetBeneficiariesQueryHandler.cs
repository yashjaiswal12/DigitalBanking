using DigitalBanking.Application.Features.Beneficiaries.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaries
{
    public class GetBeneficiariesQueryHandler : IRequestHandler<GetBeneficiariesQuery, List<BeneficiaryDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ILogger<GetBeneficiariesQueryHandler> _logger;

        public GetBeneficiariesQueryHandler(ICurrentUserService currentUserService, IBeneficiaryRepository beneficiaryRepository,
            ILogger<GetBeneficiariesQueryHandler> logger)
        {
            _currentUserService = currentUserService;
            _beneficiaryRepository = beneficiaryRepository;
            _logger = logger;
        }

        public async Task<List<BeneficiaryDto>> Handle(GetBeneficiariesQuery request, CancellationToken cancellationToken)
        {
            var beneficiaries = await _beneficiaryRepository.GetBeneficiariesByCustomerIdAsync(_currentUserService.UserId, cancellationToken);

            if (beneficiaries.Count == 0)
            {
                _logger.Log(LogLevel.Error, "Beneficiaries not found for user = {useid}", _currentUserService.UserId);
                throw new BeneficiaryNotFoundException();
            }

            _logger.Log(LogLevel.Information, "Beneficiaries list for user = {useid}", _currentUserService.UserId);

            return beneficiaries.Select(beneficiary => new BeneficiaryDto
            {
                Id = beneficiary.Id,
                AccountNumber = beneficiary.BeneficiaryAccountNumber,
                BeneficiaryName = beneficiary.BeneficiaryName,
                BankName = beneficiary.BeneficiaryBankName,
                BankCode = beneficiary.BeneficiaryBankCode,
                Status = beneficiary.Status,
                IsVerified = beneficiary.VerifiedAt.HasValue,
                VerifiedAt = beneficiary.VerifiedAt

            }).ToList<BeneficiaryDto>();
        }
    }
}
