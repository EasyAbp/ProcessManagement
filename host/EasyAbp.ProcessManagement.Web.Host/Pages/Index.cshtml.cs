using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace EasyAbp.ProcessManagement.Pages;

public class IndexModel : ProcessManagementPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
