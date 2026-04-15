using Application.Common.Results;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Users.Inputs;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;

namespace Application.Memberships.Services;

public sealed class UpdateMembershipService(IMembershipRepository repo, IUserRepository urepo, IBenefitRepository BenefitRepo) : IUpdateMembershipService
{
    public async Task<Result<Membership>> ExecuteAsync(UpdateMembershipInput input, List<UpdateMembershipBenefitInput> benefits, CancellationToken ct = default)
    {
        if (input is null)
            throw new ArgumentException("input must be provided");

        var benefitlist = new List<string>();

        var userlists = new List<string>();

        var user = await urepo.GetUserByUserIdAsync(input.userid, ct);

        var status = input.status;

        if (user != null)
        {
            if (input.status == "Delete")
            {
                var userlist = input.users.ToList();

                userlist.Remove(user.UserId);

                user.UpdateProfile(user.FirstName, user.LastName, user.Phonenumber, user.ProfileImageUri, "0", user.WorkoutsId);

                await urepo.UpdateAsync(user, ct);

                userlists = userlist;

                status = "Active";
            }
            else
            {
                var userlist = input.users.ToList();

                userlist.Add(user.UserId);

                user.UpdateProfile(user.FirstName, user.LastName, user.Phonenumber, user.ProfileImageUri, input.id, user.WorkoutsId);

                await urepo.UpdateAsync(user, ct);

                userlists = userlist;
            }
        }
        else
        {
            userlists = input.users.ToList();
        }

        foreach (var benefit in benefits)
        {
            var updatebenefit = await BenefitRepo.GetByIdAsync(benefit.id);

            if(updatebenefit != null)
            {
                if(updatebenefit.MembershipId == input.id)
                {
                    updatebenefit.UpdateBenefit(updatebenefit.Benefit);

                    benefitlist.Add(updatebenefit.Benefit);

                    await BenefitRepo.UpdateAsync(updatebenefit);
                }
                else
                {
                    benefitlist.Remove(updatebenefit.Benefit);

                    await BenefitRepo.RemoveAsync(updatebenefit);
                }
            }
            else
            {
                var newbenefit = MembershipBenefits.create
                    (
                       benefit: benefit.benefit,
                       membershipId: input.id
                    );

                await BenefitRepo.AddAsync(newbenefit, ct);

                benefitlist.Add(newbenefit.Benefit);
            }

        }

        var membership = await repo.GetByIdAsync(input.id);
        if(membership == null)
        {
            throw new ArgumentException("membership not found");
        }

        membership.Update(input.name, input.description, benefitlist, status, input.type, input.pricing, input.monthlyDuration, input.userid, userlists);


        var result = await repo.UpdateAsync(membership, ct);

        return !result
                ? Result<Membership>.NotFound($"Membership '{membership.Name}' was not found")
                : Result<Membership>.Ok(membership);
    }
}
