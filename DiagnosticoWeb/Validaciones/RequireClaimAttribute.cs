using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace DiagnosticoWeb.Validaciones
{
    public class RequireClaimAttribute : AuthorizeAttribute
    {
        public string Type { get; }
        public string Value { get; set; }

        public RequireClaimAttribute(string type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type.Length == 0) throw new ArgumentException("Claim Type cannot be empty", nameof(type));
            Type = type;
            base.Policy = Policy;
        }

        public new string Policy { get; } = PolicyName;

        public static readonly string PolicyName = typeof(RequireClaimAttribute).AssemblyQualifiedName;
    }

    public class RequireClaimAuthorizationHandler : AuthorizationHandler<RequireClaimAuthorizationHandler>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireClaimAuthorizationHandler requirement)
        {
            if (context.User != null
                && context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext
                && mvcContext.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
            {
                var controllerClaims = actionDescriptor.ControllerTypeInfo.CustomAttributes.Where(cad => cad.AttributeType == typeof(RequireClaimAttribute));
                var actionClaims = actionDescriptor.MethodInfo.CustomAttributes.Where(cad => cad.AttributeType == typeof(RequireClaimAttribute));
                var actualClaims = context.User.Claims;
                var ids = context.User.Identities;
                bool satisfiesControllerClaims = controllerClaims.All(c => actualClaims.Any(a => a.Satisfies(c)));
                bool satisfiesActionClaims = actionClaims.All(c => actualClaims.Any(a => a.Satisfies(c)));
                if (satisfiesControllerClaims && satisfiesActionClaims) context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public static class RequireClaimAuthorizationExtensions
    {
        public static void AddRequireClaimAttributeAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(o => { o.AddPolicy(RequireClaimAttribute.PolicyName, p => p.AddRequirements(new RequireClaimAuthorizationHandler())); });
        }

        public static bool EqualsTypeValue(this Claim left, Claim right)
        {
            return left.Type == right.Type && left.Value == right.Value;
        }

        public static bool Satisfies(this Claim left, RequireClaimAttribute right)
        {
            return left.Type == right.Type && left.Value == right.Value;
        }
        public static bool Satisfies(this Claim left, CustomAttributeData right)
        {
            if (right.AttributeType != typeof(RequireClaimAttribute)) return false;
            var type = right.ConstructorArguments.First().Value as String;
            var value = right.NamedArguments.First(a => a.MemberName == "Value").TypedValue.Value as String;
            return left.Type == type && left.Value == value;
        }

        public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Contains(predicate);
        }
    }
}