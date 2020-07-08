using System.Text.Json;
using Vendor.VLog;

namespace Vendor.Vhttp
{
    public abstract class HttpRequestContext
    {
        public JsonElement element;

        public string StrGetValue(string name)
        {
            try
            {
                switch (element.GetProperty(name).ValueKind)
                {
                    case JsonValueKind.String:
                        return element.GetProperty(name).GetString();
                    case JsonValueKind.Number:
                        try { return element.GetProperty(name).GetUInt32().ToString(); }
                        catch { return element.GetProperty(name).GetDouble().ToString(); }
                    default:
                        return element.GetProperty(name).GetString();
                }
            }
            catch (System.Exception ex)
            {
                HttpLog.AppGravarLog(ex.Message, $"namespace Vendor.Vhttp :: HttpRequestContext => prop: {name}");
                return string.Empty;
            }     
        }

        public string StrGetValue(string name, JsonElement Nelement)
        {
            try
            {
                switch (Nelement.GetProperty(name).ValueKind)
                {
                    case JsonValueKind.String:
                        return Nelement.GetProperty(name).GetString();
                    case JsonValueKind.Number:
                        try { return Nelement.GetProperty(name).GetUInt32().ToString(); }
                        catch { return Nelement.GetProperty(name).GetDouble().ToString(); }
                    default:
                        return Nelement.GetProperty(name).GetString();
                }
            }
            catch (System.Exception ex)
            {
                HttpLog.AppGravarLog(ex.Message, $"namespace Vendor.Vhttp :: HttpRequestContext => prop: {name}");
                return string.Empty;
            }
        }

        public string StrGetValue(string name, int position, JsonElement Nelement)
        {
            try
            {
                switch (Nelement.GetProperty(name)[position].ValueKind)
                {
                    case JsonValueKind.String:
                        return Nelement.GetProperty(name)[position].GetString();
                    case JsonValueKind.Number:
                        try { return Nelement.GetProperty(name)[position].GetUInt32().ToString(); }
                        catch { return Nelement.GetProperty(name)[position].GetDouble().ToString(); }
                    default:
                        return Nelement.GetProperty(name)[position].GetString();
                }
            }
            catch (System.Exception ex)
            {
                HttpLog.AppGravarLog(ex.Message, $"namespace Vendor.Vhttp :: HttpRequestContext => prop: {name}");
                return string.Empty;
            }
        }
    }
}
