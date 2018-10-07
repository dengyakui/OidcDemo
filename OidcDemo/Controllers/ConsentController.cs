using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using OidcDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace OidcDemo.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;


        public ConsentController(IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }
        private async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl)
        {
            var resquest = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (resquest == null) return null;
            var client = await _clientStore.FindEnabledClientByIdAsync(resquest.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(resquest.ScopesRequested);

            var vm = CreateConsentViewModel(resquest, client, resources);
            vm.ReturnUrl = returnUrl;
            return vm;
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientId = client.ClientId;
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;

            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i));
            vm.ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(i => CreateScopeViewModel(i));
            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                Checked = identityResource.Required,
                Required = identityResource.Required,
                Description = identityResource.Description,
                Emphasize = identityResource.Emphasize,
                DisplayName = identityResource.DisplayName
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                Checked = scope.Required,
                Required = scope.Required,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                DisplayName = scope.DisplayName
            };
        }

        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsentViewModelAsync(returnUrl);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm]InputConsentViewModel inputModel)
        {
            ConsentResponse consentResponse = null;
            if(inputModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if(inputModel.Button == "yes")
            {
                if (inputModel.ScopesConsented?.Any()??false)
                {
                    consentResponse = new ConsentResponse
                    {
                        ScopesConsented = inputModel.ScopesConsented,
                        RememberConsent = inputModel.RememberConsent
                    };
                }
            }

            if (consentResponse != null)
            {
                var request =await _identityServerInteractionService.GetAuthorizationContextAsync(inputModel.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);
                return Redirect(inputModel.ReturnUrl);
            }
            throw new Exception("error");
        }
    }
}