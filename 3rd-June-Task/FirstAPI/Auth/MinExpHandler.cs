// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FirstAPI.Auth;

// This class contains logic for determining whether MinimumExperienceRequirements in authorization
// policies are satisfied or not
internal class MinExpAuthorizationHandler : AuthorizationHandler<MinExpRequirement>
{
    private readonly ILogger<MinExpAuthorizationHandler> _logger;

    public MinExpAuthorizationHandler(ILogger<MinExpAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    // Check whether a given MinExpRequirement is satisfied or not for a particular context
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinExpRequirement requirement)
    {
        // Log as a warning so that it's very clear in sample output which authorization policies
        // (and requirements/handlers) are in use
        _logger.LogWarning("Evaluating authorization requirement for experience >= {experience}", requirement.Experience);

        // Check the user's experience
        var experienceClaim = context.User.FindFirst(c => c.Type == "Experience");
        if (experienceClaim != null)
        {
            // If the user has an experience claim, check their experience
            var experience = Convert.ToInt32(experienceClaim.Value, CultureInfo.InvariantCulture);

            // If the user meets the experience criterion, mark the authorization requirement succeeded
            if (experience >= requirement.Experience)
            {
                _logger.LogInformation("Minimum experience authorization requirement {experience} satisfied", requirement.Experience);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Current user's Experience claim ({experience}) does not satisfy the minimum experience authorization requirement {experience}",
                    experienceClaim.Value,
                    requirement.Experience);
            }
        }
        else
        {
            _logger.LogInformation("No Experience claim present");
        }

        return Task.CompletedTask;
    }
}
