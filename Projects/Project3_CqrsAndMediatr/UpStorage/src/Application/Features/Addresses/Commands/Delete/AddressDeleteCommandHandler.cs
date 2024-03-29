﻿using Application.Common.Interfaces;
using Application.Features.Addresses.Commands.Update;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.Delete
{
    public class AddressDeleteCommandHandler : IRequestHandler<AddressDeleteCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AddressDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<Guid>> Handle(AddressDeleteCommand request, CancellationToken cancellationToken)
        {
            var address = await _applicationDbContext.Addresses
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (address == null)
            {
                throw new ArgumentException(nameof(request.Id));
            }

            address.DeletedOn = DateTimeOffset.Now;
            address.DeletedByUserId = null;
            address.IsDeleted = true;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"The address named \"{address.Name}\" was marked as deleted.(isDeleted = {address.IsDeleted} )", address.Id);
        }
    }
}
