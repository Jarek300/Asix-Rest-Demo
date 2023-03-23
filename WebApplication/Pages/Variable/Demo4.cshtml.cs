using Asix;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Code;

namespace WebApplication.Pages.Variable
{
    public class Demo4Model : PageModel
    {
        public VariableModel VariableModel = new("A110", "Przepływ gazu do pieca", "m³/h");
        public string WriteError = "";

        public string UserName = "";
        public string Password = "";
        public string NewValue = "11";


        public async Task OnGet()
        {
            await ReadVariable();
        }


        public async Task ReadVariable()
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();
                ICollection<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(new string[] { VariableModel.Name });
                VariableValue variableState = variableValues.First();
                VariableModel.SetVariableValue(variableState);
            }
            catch (Exception e)
            {
                VariableModel.SetError(e.Message);
            }
        }


        public async Task OnPost()
        {
            try
            {
                Microsoft.Extensions.Primitives.StringValues userName = Request.Form["userName"];
                Microsoft.Extensions.Primitives.StringValues password = Request.Form["password"];
                Microsoft.Extensions.Primitives.StringValues newValue = Request.Form["newValue"];

                UserName = userName[0];
                Password = password[0];
                NewValue = newValue[0];

                AsixRestClient asixRestClient = AsixRestClient.Create(UserName, Password);

                await asixRestClient.PutVariableValueAsync(VariableModel.Name, NewValue, null, "");
                WriteError = "";
                await ReadVariable();
            }
            catch (ApiException<ErrorMessage> e)
            {
                WriteError = e.Result.Message;
            }
            catch (Exception e)
            {
                WriteError = e.Message;
            }
        }
    }
}
