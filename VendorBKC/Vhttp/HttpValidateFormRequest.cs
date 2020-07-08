using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Vendor.Vhttp
{
    public class HttpValidateFormRequest : Attribute, IAuthorizationFilter
    {

        public Dictionary<string, string> VForm = new Dictionary<string, string>()
        {
            { "nome","required" }
        };

        public Dictionary<string, string> VMessenger = new Dictionary<string, string>()
        {
            { "nome", "campo é obrigatório!"}
        };



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
            {
                var request = reader.ReadToEndAsync();
                string result = request.Result;
                var jObject = JObject.Parse(result);

                if (Validate(jObject))
                {
                    context.Result = new ObjectResult(new { message = "Usuário não tem Permissão de Acesso!" }) { StatusCode = 403 };
                }
            }
        }

        public bool Validate(JObject jObject)
        {
            foreach (var jForm in jObject)
            {
                foreach (var form in VForm)
                {
                    if (jForm.Key == form.Key)
                    {
                        if (string.IsNullOrWhiteSpace(jForm.Value.ToString()))
                        {
                            return false;
                        }
                    }
                }
            }

           
            return true;
        }
    }
}
