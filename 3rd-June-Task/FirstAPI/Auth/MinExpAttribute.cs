// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Auth;

// This attribute derives from the [Authorize] attribute, adding
// the ability for a user to specify an 'age' paratmer. Since authorization
// policies are looked up from the policy provider only by string, this
// authorization attribute creates is policy name based on a constant prefix
// and the user-supplied age parameter. A custom authorization policy provider
// (`MinimumAgePolicyProvider`) can then produce an authorization policy with
// the necessary requirements based on this policy name.
internal class MinExpAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "MinExp";

    public MinExpAttribute(int experience) => Experience = experience;

    // Get or set the Experience property by manipulating the underlying Policy property
    public int Experience
    {
        get
        {
            if (int.TryParse(Policy.AsSpan(POLICY_PREFIX.Length), out var experience))
            {
                return experience;
            }
            return default(int);
        }
        set
        {
            Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
