using Pulumi.AzureNative.Resources;
using Pulumi.AzureAD;

return await Pulumi.Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroup("pulumi-test-RG", new ResourceGroupArgs
    {
        Location = "North Europe"
    });

    var config = new Pulumi.Config();
    var myOwnId = config.Get("myOwnId") ?? "";
    var applicationName = $"pulumi-test-application";
    var application = new Application(applicationName, new ApplicationArgs
    {
        DisplayName = applicationName,
        Owners = { myOwnId },
    });

    var federatednName = $"pulumi-test-federatedCredential";
    var federatedCredential = new ApplicationFederatedIdentityCredential(federatednName, new ApplicationFederatedIdentityCredentialArgs
    {
        ApplicationId = application.Id,
        Audiences = { "api://AzureADTokenExchange" },
        DisplayName = federatednName,
        Issuer = "https://token.actions.githubusercontent.com",
        Subject = "repo:eriktoger/pulumi_test:environment:Production",
    });
});